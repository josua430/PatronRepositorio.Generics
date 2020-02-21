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
    public class ApiKeyController : Controller
    {
        #region Update
        /// <summary>
        /// Get for update
        /// </summary>
        /// <param name="id">id of user</param>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult Update()
        {

            if (Session[Test.Helpers.Constant.USUARIO] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = new Models.changekey();
            return View(model);
        }

        /// <summary>
        /// Save data for update
        /// </summary>
        /// <param name="objKey">object with data for update</param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult Update(Models.changekey objKey)
        {
            try
            {
                ApiKey.clsKey objKeyAPI = new ApiKey.clsKey();

                string strResult = objKeyAPI.changeKey(objKey.DevName, objKey.newKey);
                if (strResult == "Ok")
                {
                    TempData["Success"] = "Update successfull!";
                    return RedirectToAction("Index", "Home");

                }else
                {
                    ViewBag.Error = strResult;
                    return View(objKey);
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.InnerException;
            }
            return View(objKey);
        }

        #endregion
    }
}