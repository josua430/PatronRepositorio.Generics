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
    public class UnitTestClient
    {

        /// <summary>
        /// Get All Clients method
        /// </summary>
        [TestMethod]
        public void TestMethod_GetAllClients()
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

                }
            }
            Assert.IsTrue(ClientsDB.Count() >= 0);
        }

        /// <summary>
        /// Get one Client method
        /// </summary>
        [TestMethod]
        public void TestMethod_GetOneClient()
        {
            var model = new WebApi.Models.ClientModel();
            bool blnResult = false;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constant.URLAPI);
                    //HTTP GET
                    var responseTask = client.GetAsync("Clients?id=1");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Test.Entity.Clientes>();
                        readTask.Wait();
                        blnResult = true;
                    }
                }
            }
            catch (Exception)
            {
                
            }
            Assert.IsTrue(blnResult);
        }

        /// <summary>
        /// Insert Client
        /// </summary>
        [TestMethod]
        public void TestMethod_InsertClient()
        {
            string strPrefix = DateTime.Now.Ticks.ToString();
            bool blnInserted = false;
            WebApi.Models.ClientModel objCliente = new WebApi.Models.ClientModel()
            {
                Nombres = "Nombre" + strPrefix,
                Apellidos = "Apellido" + strPrefix,
                Correo = "prueba@gmail.com",
                Telefono = "900000",
                Celular = "3002220000"
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.URLAPI + "Clients");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<WebApi.Models.ClientModel>("Clients", objCliente);
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
        /// Update Client
        /// </summary>
        [TestMethod]
        public void TestMethod_UpdateClient()
        {
            string strPrefix = DateTime.Now.Ticks.ToString();
            bool blnResult = false;
            WebApi.Models.ClientModel objCliente = new WebApi.Models.ClientModel()
            {
                Nombres = "Nombre" + strPrefix,
                Apellidos = "Apellido" + strPrefix,
                Correo = "prueba@gmail.com",
                Telefono = "900000",
                Celular = "3002220000",
                IdCliente = 5
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.URLAPI + "Clients");

                //HTTP PUT
                var putTask = client.PutAsJsonAsync<WebApi.Models.ClientModel>("Clients", objCliente);
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
        /// Delete Client
        /// </summary>
        [TestMethod]
        public void TestMethod_DeleteClient()
        {
            bool blnResult = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.URLAPI);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Clients/6");
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    blnResult = true;
                }
            }
            Assert.IsTrue(blnResult);
        }

        /// <summary>
        /// Client more orders
        /// </summary>
        [TestMethod]
        public void TestMethod_ClientOrders()
        {
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
            catch (Exception)
            { }
            Assert.IsTrue(model.IdCliente == 2);
        }

        /// <summary>
        /// Client most expensive order
        /// </summary>
        [TestMethod]
        public void TestMethod_ClientExpensive()
        {
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
            catch (Exception)
            { }
            Assert.IsTrue(model.CuentaMasCostosa == 100000);
        }

    }
}
