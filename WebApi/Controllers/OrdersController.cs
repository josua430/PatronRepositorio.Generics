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
    /// Controller for Orders
    /// </summary>
    public class OrdersController : ApiController
    {

        /// <summary>
        /// // GET: api/Orders
        /// </summary>
        /// <returns>List of Orders</returns>
        public IQueryable<OrderModel> GetOrders()
        {

            IQueryable<OrderModel> listPedidos = null;

            var OrderRepository = new Repository.OrderDataMapper(new Entity.TestEntities());
            listPedidos = OrderRepository.GetAll().Select(a => new OrderModel()
            {
                IdCliente = (decimal)a.IdCliente,
                IdPedido = a.IdPedido,
                NombresCliente= a.Clientes.Nombres + " " + a.Clientes.Apellidos,
                DescripcionPedido = a.DescripcionPedido,
                ValorPedido = a.ValorPedido == null ? 0 : (long)a.ValorPedido,
                FechaPedido = (DateTime)a.FechaPedido,
                MetodoPago = a.MetodoPago
            });

            return listPedidos;
        }

        /// <summary>
        /// Get count of records by IdCliente in last week with card
        /// </summary>
        /// <param name="IdCliente"></param>
        /// <returns></returns>
        public long GetCountBy(decimal IdCliente)
        {

            var OrderRepository = new Repository.OrderDataMapper(new Entity.TestEntities());
            //set 7 days ago
            DateTime dtDateToProcess = DateTime.Now.AddDays(-7);
            //set day to initial hours of day
            DateTime dtInitialDate = new DateTime(dtDateToProcess.Year, dtDateToProcess.Month, dtDateToProcess.Day, 0, 0, 0);
            dtDateToProcess = DateTime.Now;
            //set day to end hours of day
            DateTime dtFinalDate = new DateTime(dtDateToProcess.Year, dtDateToProcess.Month, dtDateToProcess.Day, 23, 59, 59);
            long lngQuantityPedidos = OrderRepository.GetCountBy(x => x.IdCliente == IdCliente && x.MetodoPago == "Tarjeta"
            && x.FechaPedido>=dtInitialDate && x.FechaPedido <= dtFinalDate);

            return lngQuantityPedidos;
        }
        
        // GET: api/Orders/5
        /// <summary>
        /// Get Orders by Id
        /// </summary>
        /// <param name="id">Id of Order</param>
        /// <returns></returns>
        [ResponseType(typeof(OrderModel))]
        public IHttpActionResult GetOrders(decimal id)
        {
            Pedidos MyOrder = null;

            var OrderRepository = new Repository.OrderDataMapper(new Entity.TestEntities());
            MyOrder = OrderRepository.GetById(id);
            OrderModel MyResult =new OrderModel()
            {
                IdCliente = (decimal)MyOrder.IdCliente,
                IdPedido = MyOrder.IdPedido,
                DescripcionPedido = MyOrder.DescripcionPedido,
                MetodoPago = MyOrder.MetodoPago,
                FechaPedido = (DateTime)MyOrder.FechaPedido,
                ValorPedido = MyOrder.ValorPedido == null ? 0 : (long)MyOrder.ValorPedido
            };

            return Json(MyResult);

        }

        /// <summary>
        /// Insert new Orders
        /// </summary>
        /// <param name="RequestOrder"></param>
        /// <returns></returns>
        // POST: api/Orders
        [ResponseType(typeof(OrderModel))]
        public IHttpActionResult PostOrders(OrderModel RequestOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pedidos MyOrder = new Pedidos()
            {
                IdCliente = RequestOrder.IdCliente,                
                DescripcionPedido = RequestOrder.DescripcionPedido,
                MetodoPago = RequestOrder.MetodoPago,
                FechaPedido = DateTime.Now,
                ValorPedido = RequestOrder.ValorPedido
            };

            var OrderRepository = new Repository.OrderDataMapper(new Entity.TestEntities());
            int intResult = OrderRepository.Insert(MyOrder);

            return Json(intResult);

        }

        /// <summary>
        /// Update Orders
        /// </summary>
        /// <param name="RequestOrder">Entity</param>
        /// <returns></returns>
        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrders(OrderModel RequestOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Pedidos MyOrder = new Pedidos()
                {
                    IdCliente = RequestOrder.IdCliente,
                    IdPedido = RequestOrder.IdPedido,
                    DescripcionPedido = RequestOrder.DescripcionPedido,
                    MetodoPago = RequestOrder.MetodoPago,
                    FechaPedido = DateTime.Now,
                    ValorPedido = RequestOrder.ValorPedido
                };

                var OrderRepository = new Repository.OrderDataMapper(new TestEntities());
                bool blnResult = OrderRepository.Update(MyOrder.IdPedido, MyOrder);
                return Json(blnResult);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Delete Orders
        /// </summary>
        /// <param name="id">Id of Orders</param>
        /// <returns></returns>
        // DELETE: api/Orders/5
        [ResponseType(typeof(Pedidos))]
        public IHttpActionResult DeleteOrders(decimal id)
        {
            var OrderRepository = new Repository.OrderDataMapper(new Entity.TestEntities());
            bool blnResult = OrderRepository.Delete(id);

            return Json(blnResult);
        }

    }
}