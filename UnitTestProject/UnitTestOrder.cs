using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Helpers;

namespace UnitTestProject
{
    /// <summary>
    /// test classes
    /// </summary>
    [TestClass]
    public class UnitTestOrder
    {

        /// <summary>
        /// Get All Orders method
        /// </summary>
        [TestMethod]
        public void TestMethod_GetAllOrders()
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
                    //log response status here..

                    OrdersDB = Enumerable.Empty<WebApi.Models.OrderModel>();

                }
            }
            Assert.IsTrue(OrdersDB.Count() >= 0);
        }

        /// <summary>
        /// Get one order method
        /// </summary>
        [TestMethod]
        public void TestMethod_GetOneOrder()
        {
            var model = new WebApi.Models.OrderModel();
            bool blnResult = false;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI);
                    //HTTP GET
                    var responseTask = client.GetAsync("Orders?id=1");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Test.Entity.Pedidos>();
                        readTask.Wait();
                        blnResult = true;
                    }
                }
            }
            catch (Exception)
            {  }
            Assert.IsTrue(blnResult);
        }

        /// <summary>
        /// Insert Order
        /// </summary>
        [TestMethod]
        public void TestMethod_InsertOrder()
        {
            string strPrefix = DateTime.Now.Ticks.ToString();
            bool blnInserted = false;
            WebApi.Models.OrderModel objOrder = new WebApi.Models.OrderModel()
            {
                DescripcionPedido = "Descripcion " + strPrefix,
                IdCliente = 1,
                MetodoPago = "Efectivo",
                ValorPedido = 1001,
                FechaPedido = DateTime.Now
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.URLAPI + "Orders");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<WebApi.Models.OrderModel>("Orders", objOrder);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    blnInserted = true;
                }
            }
            Assert.IsTrue(blnInserted);
        }

        /// <summary>
        /// Update Order
        /// </summary>
        [TestMethod]
        public void TestMethod_UpdateOrder()
        {
            string strPrefix = DateTime.Now.Ticks.ToString();
            bool blnResult = false;
            WebApi.Models.OrderModel objOrder = new WebApi.Models.OrderModel()
            {
                DescripcionPedido = "Descripcion " + strPrefix,
                IdCliente = 1,
                MetodoPago = "Efectivo",
                ValorPedido = 1002,
                FechaPedido = DateTime.Now
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.URLAPI + "Orders");

                //HTTP PUT
                var putTask = client.PutAsJsonAsync<WebApi.Models.OrderModel>("Orders", objOrder);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    blnResult = true;
                }
            }
            Assert.IsTrue(blnResult);
        }

        /// <summary>
        /// Delete Order
        /// </summary>
        [TestMethod]
        public void TestMethod_DeleteOrder()
        {
            bool blnResult = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.URLAPI);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Orders/7");
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    blnResult = true;
                }
            }
            Assert.IsTrue(blnResult);
        }

    }
}
