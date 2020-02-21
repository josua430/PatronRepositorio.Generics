using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    /// <summary>
    /// Modelo para cliente
    /// </summary>
    public class ClientModel
    {
        /// <summary>
        /// ID del cliente
        /// </summary>
        public decimal IdCliente { get; set; }
        /// <summary>
        /// Nombres del cliente
        /// </summary>
        public string Nombres { get; set; }
        /// <summary>
        /// Apellidos del cliente
        /// </summary>
        public string Apellidos { get; set; }
        /// <summary>
        /// Nombres completos
        /// </summary>
        public string NombresCompletos { get; set; }
        /// <summary>
        /// Telefono del cliente
        /// </summary>
        public string Telefono { get; set; }
        /// <summary>
        /// Celular del cliente
        /// </summary>
        public string Celular { get; set; }
        /// <summary>
        /// Correo del cliente
        /// </summary>
        public string Correo { get; set; }
        /// <summary>
        /// Cantidad de pedidos realizada
        /// </summary>
        public long CantidadPedidos { get; set; }
        /// <summary>
        /// Valor cuenta mas costosa
        /// </summary>
        public long CuentaMasCostosa { get; set; }

    }
}