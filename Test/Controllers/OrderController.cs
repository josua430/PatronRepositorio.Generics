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
    public class OrderController : Controller
    {

        /// <summary>
        /// // GET: Order
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
            IEnumerable<WebApi.Models.OrderModel> OrdersDB = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.URLAPI);
                //HTTP GET
                var responseTask = client.GetAsync("Orders");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<WebApi.Models.OrderModel>>();
                    readTask.Wait();

                    OrdersDB = readTask.Result;
                }
                else //web api sent error response 
                {
                    OrdersDB = Enumerable.Empty<WebApi.Models.OrderModel>();

                    ViewBag.Error = "Server error. Please contact administrator.";
                }
            }
            return Json(OrdersDB.ToList(), JsonRequestBehavior.AllowGet);

        }


        private IEnumerable<WebApi.Models.ClientModel> GetListClientsOrders()
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
            }
            return ClientsDB;
        }

        #region Insert

        /// <summary>
        /// Get for Insert new Order
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Insert()
        {
            var model = new WebApi.Models.OrderModel();
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

            ViewBag.Clients = GetListClientsOrders().Select(x => new SelectListItem
            {
                Text = x.NombresCompletos,
                Value = x.IdCliente.ToString()
            });

            return View(model);
        }

        /// <summary>
        /// Insert data for new order
        /// </summary>
        /// <param name="objOrder">object with data to insert</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Insert(WebApi.Models.OrderModel objOrder)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //query for count of orders. It must be until 5 in a week in card
                    long lngCountOrders = 0;
                    if (objOrder.MetodoPago == "Tarjeta")
                    {
                        client.BaseAddress = new Uri(Constant.URLAPI);
                        //HTTP GET
                        var responseTask = client.GetAsync("Orders?IdCliente=" + Convert.ToInt64(objOrder.IdCliente).ToString());
                        responseTask.Wait();
                        var resultCount = responseTask.Result;
                        if (resultCount.IsSuccessStatusCode)
                        {
                            var readTask = resultCount.Content.ReadAsAsync<long>();
                            readTask.Wait();

                            lngCountOrders = readTask.Result;
                        }
                    }

                    if (lngCountOrders<5)
                    {
                        //Insert data
                        client.BaseAddress = new Uri(Constant.URLAPI + "Orders");

                        //HTTP POST
                        var postTask = client.PostAsJsonAsync<WebApi.Models.OrderModel>("Orders", objOrder);
                        postTask.Wait();

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            TempData["Success"] = "¡Pedido Creado!";
                            return RedirectToAction("Index", "Order");
                        }
                    }
                    else
                    {
                        ViewBag.Error = "Error al crear el pedido. El cliente no puede pagar mas de 5 veces con tarjeta";
                    }
                }
                //ViewBag.Error = "Error al crear el pedido";
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            ViewBag.Clients = GetListClientsOrders().Select(x => new SelectListItem
            {
                Text = x.NombresCompletos,
                Value = x.IdCliente.ToString()
            });

            return View(objOrder);
        }
        #endregion

        #region Update
        /// <summary>
        /// Get for update
        /// </summary>
        /// <param name="id">id of order</param>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Update(string id)
        {

            if (Session[Test.Helpers.Constant.USUARIO] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Se crea modelo 
            var model = new WebApi.Models.OrderModel();
            try
            {
                ViewBag.Clients = GetListClientsOrders().Select(x => new SelectListItem
                {
                    Text = x.NombresCompletos,
                    Value = x.IdCliente.ToString()
                });

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI);
                    //HTTP GET
                    var responseTask = client.GetAsync("Orders?id=" + id);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Entity.Pedidos>();
                        readTask.Wait();

                        model.IdCliente = (decimal)readTask.Result.IdCliente;
                        model.DescripcionPedido = readTask.Result.DescripcionPedido;
                        model.MetodoPago = readTask.Result.MetodoPago;
                        model.ValorPedido = (long)readTask.Result.ValorPedido;
                        model.IdPedido = readTask.Result.IdPedido;
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
        /// <param name="objOrder">object with data for update</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Update(WebApi.Models.OrderModel objOrder)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    //query for count of orders. It must be until 5 in a week in card
                    long lngCountOrders = 0;
                    if (objOrder.MetodoPago == "Tarjeta")
                    {
                        client.BaseAddress = new Uri(Constant.URLAPI);
                        //HTTP GET
                        var responseTask = client.GetAsync("Orders?IdCliente=" + Convert.ToInt64(objOrder.IdCliente).ToString());
                        responseTask.Wait();
                        var resultCount = responseTask.Result;
                        if (resultCount.IsSuccessStatusCode)
                        {
                            var readTask = resultCount.Content.ReadAsAsync<long>();
                            readTask.Wait();

                            lngCountOrders = readTask.Result;
                        }
                    }

                    if (lngCountOrders < 5)
                    {

                        client.BaseAddress = new Uri(Constant.URLAPI + "Orders");

                        //HTTP PUT
                        var putTask = client.PutAsJsonAsync<WebApi.Models.OrderModel>("Orders", objOrder);
                        putTask.Wait();

                        var result = putTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            TempData["Success"] = "¡Actualización exitosa!";
                            return RedirectToAction("Index", "Order");
                        }
                    }else
                    {
                        ViewBag.Error = "Error al crear el pedido. El cliente no puede pagar mas de 5 veces con tarjeta";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.InnerException;
            }
            ViewBag.Clients = GetListClientsOrders().Select(x => new SelectListItem
            {
                Text = x.NombresCompletos,
                Value = x.IdCliente.ToString()
            });
            return View(objOrder);
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
                    var deleteTask = client.DeleteAsync("Orders/" + id);
                    deleteTask.Wait();

                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return Json(new { Message = "Pedido Eliminado", Type = "Exito" }, JsonRequestBehavior.AllowGet);
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
    }
}