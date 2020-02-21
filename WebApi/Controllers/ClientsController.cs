using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Entity;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for Clients
    /// </summary>
    public class ClientsController : ApiController
    {

        /// <summary>
        /// // GET: api/Clients
        /// </summary>
        /// <returns>List of clients</returns>
        public IQueryable<ClientModel> GetClientes()
        {

            IQueryable<ClientModel> listClientes = null;

            var ClientRepository = new Repository.ClientDataMapper(new Entity.TestEntities());
            listClientes = ClientRepository.GetAll().Select(a => new ClientModel()
            {
                IdCliente = a.IdCliente,
                Nombres = a.Nombres,
                Apellidos = a.Apellidos,
                NombresCompletos = a.Nombres + " " + a.Apellidos,
                Telefono = a.Telefono,
                Celular = a.Celular,
                Correo = a.Correo
            });

            return listClientes;
        }

        // GET: api/Clients/5
        /// <summary>
        /// Get client by Id
        /// </summary>
        /// <param name="id">Id of client</param>
        /// <returns></returns>
        [ResponseType(typeof(ClientModel))]
        public IHttpActionResult GetClientes(decimal id)
        {
            Clientes MyClient = null;

            var ClientRepository = new Repository.ClientDataMapper(new Entity.TestEntities());
            MyClient = ClientRepository.GetById(id);
            ClientModel MyResult =new ClientModel()
            {
                IdCliente = MyClient.IdCliente,
                Nombres = MyClient.Nombres,
                Apellidos = MyClient.Apellidos,
                NombresCompletos = MyClient.Nombres + " " + MyClient.Apellidos,
                Telefono = MyClient.Telefono,
                Celular = MyClient.Celular,
                Correo = MyClient.Correo
            };

            return Json(MyResult);

        }

        // GET: api/Clients/5
        /// <summary>
        /// Get client more orders
        /// </summary>
        /// <param name="id">Id of client</param>
        /// <returns></returns>
        [ResponseType(typeof(ClientModel))]
        public IHttpActionResult GetClienteMasPedidos(int intMasPedidos)
        {
            Clientes MyClient = null;

            var OrderRepository = new Repository.OrderDataMapper(new Entity.TestEntities());
            var MyResult = OrderRepository.GetAll()
                .GroupBy(x => x.IdCliente)
                .Select(y => new { IdCliente = y.Key, Valor = y.Count() })
                .Max(z => z.Valor);
            var ClientRepository = new Repository.ClientDataMapper(new TestEntities());
            MyClient = ClientRepository.GetBy(x => x.Pedidos.Count() == MyResult).FirstOrDefault();

            ClientModel MyFinalResult = new ClientModel()
            {
                IdCliente = MyClient.IdCliente,
                Nombres = MyClient.Nombres,
                Apellidos = MyClient.Apellidos,
                NombresCompletos = MyClient.Nombres + " " + MyClient.Apellidos,
                Telefono = MyClient.Telefono,
                Celular = MyClient.Celular,
                Correo = MyClient.Correo,
                CantidadPedidos = MyResult
            };

            return Json(MyFinalResult);

        }

        // GET: api/Clients/5
        /// <summary>
        /// Get client order most expensive
        /// </summary>
        /// <param name="id">Id of client</param>
        /// <returns></returns>
        [ResponseType(typeof(ClientModel))]
        public IHttpActionResult GetClientePedidoMasCostoso(int intMasCostoso)
        {
            Clientes MyClient = null;

            var OrderRepository = new Repository.OrderDataMapper(new Entity.TestEntities());
            //max value for a order
            var MyResult = OrderRepository.GetAll()
                .GroupBy(x => x.IdCliente)
                .Select(y => new { IdCliente = y.Key, Valor = y.Max(z => z.ValorPedido) })
                .Max(z => z.Valor);
            //id of client for order
            var MyOrder = OrderRepository.GetBy(x => x.ValorPedido == MyResult).FirstOrDefault();

            var ClientRepository = new Repository.ClientDataMapper(new TestEntities());
            MyClient = ClientRepository.GetBy(x => x.IdCliente == MyOrder.IdCliente).FirstOrDefault();

            ClientModel MyFinalResult = new ClientModel()
            {
                IdCliente = MyClient.IdCliente,
                Nombres = MyClient.Nombres,
                Apellidos = MyClient.Apellidos,
                NombresCompletos = MyClient.Nombres + " " + MyClient.Apellidos,
                Telefono = MyClient.Telefono,
                Celular = MyClient.Celular,
                Correo = MyClient.Correo,
                CuentaMasCostosa = (long)MyResult
            };

            return Json(MyFinalResult);

        }

        /// <summary>
        /// Insert new client
        /// </summary>
        /// <param name="clientes"></param>
        /// <returns></returns>
        // POST: api/Clients
        [ResponseType(typeof(ClientModel))]
        public IHttpActionResult PostClientes(ClientModel RequestClient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Clientes MyClient = new Clientes()
            {
                Nombres = RequestClient.Nombres,
                Apellidos = RequestClient.Apellidos,
                Telefono = RequestClient.Telefono,
                Celular = RequestClient.Celular,
                Correo = RequestClient.Correo
            };

            var ClientRepository = new Repository.ClientDataMapper(new Entity.TestEntities());
            int intResult = ClientRepository.Insert(MyClient);

            return Json(intResult);

        }

        /// <summary>
        /// Update client
        /// </summary>
        /// <param name="clientes">Entity</param>
        /// <returns></returns>
        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClientes(ClientModel RequestClient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Clientes MyClient = new Clientes()
                {
                    IdCliente = RequestClient.IdCliente,
                    Nombres = RequestClient.Nombres,
                    Apellidos = RequestClient.Apellidos,
                    Telefono = RequestClient.Telefono,
                    Celular = RequestClient.Celular,
                    Correo = RequestClient.Correo
                };

                var ClientRepository = new Repository.ClientDataMapper(new Entity.TestEntities());
                bool blnResult = ClientRepository.Update(MyClient.IdCliente, MyClient);
                return Json(blnResult);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Delete Client
        /// </summary>
        /// <param name="id">Id of client</param>
        /// <returns></returns>
        // DELETE: api/Clients/5
        [ResponseType(typeof(Clientes))]
        public IHttpActionResult DeleteClientes(decimal id)
        {
            var ClientRepository = new Repository.ClientDataMapper(new Entity.TestEntities());
            bool blnResult = ClientRepository.Delete(id);

            return Json(blnResult);
        }

    }
}