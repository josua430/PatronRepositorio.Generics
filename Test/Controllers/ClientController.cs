using Test.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace Test.Controllers
{
    /// <summary>
    /// Post controller
    /// </summary>
    public class ClientController : Controller
    {

        /// <summary>
        /// // GET: Client
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Index()
        {
            CultureInfo en = new CultureInfo("es-CO");
            Thread.CurrentThread.CurrentCulture = en;
            if (TempData["Error"] != null && !String.IsNullOrEmpty(TempData["Error"].ToString()))
            {
                ViewBag.Error = TempData["Error"].ToString();
            }
            if (TempData["Success"] != null && !String.IsNullOrEmpty(TempData["Success"].ToString()))
            {
                ViewBag.Success = TempData["Success"].ToString();
            }
            return View();
        }

        /// <summary>
        /// Method to load grid
        /// </summary>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult LoadGrid()
        {
            IEnumerable<WebApi.Models.ClientModel> ClientsDB = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.URLAPI);
                //HTTP GET
                var responseTask = client.GetAsync("Clients");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<WebApi.Models.ClientModel>>();
                    readTask.Wait();

                    ClientsDB = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    ClientsDB = Enumerable.Empty<WebApi.Models.ClientModel>();

                    ViewBag.Error = "Server error. Please contact administrator.";
                }
            }
            return Json(ClientsDB.ToList(), JsonRequestBehavior.AllowGet);

        }

        #region Insert

        /// <summary>
        /// Get for Insert new Client
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Insert()
        {
            var model = new WebApi.Models.ClientModel();
            if (TempData["Error"] != null && !String.IsNullOrEmpty(TempData["Error"].ToString()))
            {
                ViewBag.Error = TempData["Error"].ToString();
            }
            if (TempData["Success"] != null && !String.IsNullOrEmpty(TempData["Success"].ToString()))
            {
                ViewBag.Success = TempData["Success"].ToString();
            }

            if (Session[Test.Helpers.Constant.USUARIO] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(model);
        }

        /// <summary>
        /// Insert data for new client
        /// </summary>
        /// <param name="objCliente">object with data to insert</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Insert(WebApi.Models.ClientModel objCliente)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI + "Clients");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<WebApi.Models.ClientModel>("Clients", objCliente);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "¡Cliente Creado!";
                        return RedirectToAction("Index", "Client");
                    }
                }
                ViewBag.Error = "Error al crear el cliente";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(objCliente);
        }
        #endregion

        #region Update
        /// <summary>
        /// Get for update
        /// </summary>
        /// <param name="id">id of Client</param>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Update(string id)
        {

            if (Session[Test.Helpers.Constant.USUARIO] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Se crea modelo 
            var model = new WebApi.Models.ClientModel();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI);
                    //HTTP GET
                    var responseTask = client.GetAsync("Clients?id=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Entity.Clientes>();
                        readTask.Wait();

                        model.IdCliente = readTask.Result.IdCliente;
                        model.Nombres = readTask.Result.Nombres;
                        model.Apellidos = readTask.Result.Apellidos;
                        model.Telefono = readTask.Result.Telefono;
                        model.Celular = readTask.Result.Celular;
                        model.Correo = readTask.Result.Correo;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(model);
        }

        /// <summary>
        /// Save data for update
        /// </summary>
        /// <param name="objCliente">object with data for update</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Update(WebApi.Models.ClientModel objCliente)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI +  "Clients");

                    //HTTP PUT
                    var putTask = client.PutAsJsonAsync<WebApi.Models.ClientModel>("Clients", objCliente);
                    putTask.Wait();

                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "¡Actualización exitosa!";
                        return RedirectToAction("Index", "Client");
                    }
                }
                ViewBag.Error = "Error al actualizar";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.InnerException;
            }
            return View(objCliente);
        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete method
        /// </summary>
        /// <param name="id">id for delete</param>
        /// <returns>Json with message</returns>
        [HttpGet]
        public JsonResult Delete(string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI);

                    //HTTP DELETE
                    var deleteTask = client.DeleteAsync("Clients/" + id);
                    deleteTask.Wait();

                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return Json(new { Message = "Cliente eliminado", Type = "Exito" }, JsonRequestBehavior.AllowGet);
                    }
                }

                return Json(new { Message = "Error al eliminar el registro", Type = "Error" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Error:" + ex.Message, Type = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region MoreOrders
        /// <summary>
        /// Get for client more orders
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult MoreOrders()
        {

            if (Session[Test.Helpers.Constant.USUARIO] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Se crea modelo 
            var model = new WebApi.Models.ClientModel();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI);
                    //HTTP GET
                    var responseTask = client.GetAsync("Clients?intMasPedidos=0");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<WebApi.Models.ClientModel>();
                        readTask.Wait();

                        model.IdCliente = readTask.Result.IdCliente;
                        model.Nombres = readTask.Result.Nombres;
                        model.Apellidos = readTask.Result.Apellidos;
                        model.Telefono = readTask.Result.Telefono;
                        model.Celular = readTask.Result.Celular;
                        model.Correo = readTask.Result.Correo;
                        model.CantidadPedidos = readTask.Result.CantidadPedidos;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(model);
        }

        #endregion

        #region ExpensiveOrder
        /// <summary>
        /// Get for client with Expensive Order
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult ExpensiveOrder()
        {

            if (Session[Test.Helpers.Constant.USUARIO] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Se crea modelo 
            var model = new WebApi.Models.ClientModel();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI);
                    //HTTP GET
                    var responseTask = client.GetAsync("Clients?intMasCostoso=0");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<WebApi.Models.ClientModel>();
                        readTask.Wait();

                        model.IdCliente = readTask.Result.IdCliente;
                        model.Nombres = readTask.Result.Nombres;
                        model.Apellidos = readTask.Result.Apellidos;
                        model.Telefono = readTask.Result.Telefono;
                        model.Celular = readTask.Result.Celular;
                        model.Correo = readTask.Result.Correo;
                        model.CuentaMasCostosa = readTask.Result.CuentaMasCostosa;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(model);
        }

        #endregion
    }
}