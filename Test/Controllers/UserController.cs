using Test.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Test.Controllers
{
    /// <summary>
    /// Post controller
    /// </summary>
    public class UserController : Controller
    {
        /// <summary>
        /// // GET: User
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
            try
            {
                clsDataBaseMethods objDB = new clsDataBaseMethods();
                return Json(objDB.ListUsers());
            }
            catch (Exception)
            {
                return null;
            }

        }

        #region Insert

        /// <summary>
        /// Get for Insert new User
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Insert()
        {
            var model = new Models.user();
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
        /// Insert data for new User
        /// </summary>
        /// <param name="objUser">object with data to insert</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Insert(Models.user objUser)
        {
            try
            {
                clsDataBaseMethods objDB = new clsDataBaseMethods();
                if (objDB.ExistsLogin(objUser.UserName))
                {
                    TempData["Error"] = "¡Login already exists!";
                    return RedirectToAction("Index", "User");
                }
                objDB.Insert(objUser);

                TempData["Success"] = "¡User Created!";
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(objUser);
        }
        #endregion

        #region Update
        /// <summary>
        /// Get for update
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Update(string id)
        {

            if (Session[Test.Helpers.Constant.USUARIO] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //Se crea modelo 
            var model = new Models.user();
            try
            {
                int IdElemento = int.Parse(id);
                //Se abre conexion al entity framework
                using (var context = new Entity.testRealEntities())
                {
                    var objUser = context.user.FirstOrDefault(t => t.Id == IdElemento);
                    if (objUser == null)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    model.UserName = objUser.login;
                    model.Password = objUser.password;
                    model.Password2 = objUser.password;
                    model.Role = objUser.role;
                    model.Gender = objUser.gender;
                    model.Age = objUser.age == null ? "" : objUser.age.ToString();
                    model.Nationality = objUser.nationality;
                    model.IdUser = (int)objUser.Id;
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
        /// <param name="objUser">object with data for update</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Update(Models.user objUser)
        {
            try
            {
                clsDataBaseMethods objDB = new clsDataBaseMethods();

                objDB.Update(objUser);

                TempData["Success"] = "Update successfull!";
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException;
            }
            return View(objUser);
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
                clsDataBaseMethods objDB = new clsDataBaseMethods();
                if (objDB.ExistsId(id))
                {
                    return Json(new { Message = "User not found", Type = "Error" }, JsonRequestBehavior.AllowGet);
                }

                objDB.Delete(id);

                return Json(new { Message = "User deleted", Type = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Error:" + ex.Message, Type = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion
    }
}