using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Models
{
    /// <summary>
    /// Modelo de pedido
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// Id del pedido
        /// </summary>
        public decimal IdPedido { get; set; }
        /// <summary>
        /// Id del cliente del pedido
        /// </summary>
        public decimal IdCliente { get; set; }
        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string NombresCliente { get; set; }
        /// <summary>
        /// Descripcion del pedido
        /// </summary>
        public string DescripcionPedido { get; set; }
        /// <summary>
        /// Metodo de pago
        /// </summary>
        public string MetodoPago { get; set; }
        /// <summary>
        /// Valor del pedido
        /// </summary>
        public long ValorPedido { get; set; }
        /// <summary>
        /// Fecha de pedido
        /// </summary>
        public DateTime FechaPedido { get; set; }
        /// <summary>
        /// Lista de clientes
        /// </summary>
        public List<SelectListItem> ListaClientes { get; set; }

    }
}