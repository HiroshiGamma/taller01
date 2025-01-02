using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;

namespace taller01.src.models
{
    public class Receipt
    {        /// <summary>
        /// Id del recibo
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Usuario al que se le asigna el recibo
        /// </summary>
        public AppUser User { get; set; } = null!;

        /// <summary>
        /// Rut del usuario al que se le asigna el recibo
        /// </summary>
        public string UserRut { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del cliente al que se le asigna el recibo
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Pais del cliente al que se le asigna el recibo
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Ciudad del cliente al que se le asigna el recibo
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Comuna del cliente al que se le asigna el recibo
        /// </summary>
        public string Commune { get; set; } = string.Empty;

        /// <summary>
        /// Calle del cliente al que se le asigna el recibo
        /// </summary>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Fecha del recibo
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Total del recibo
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Lista de items en el recibo
        /// </summary>
        public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
        
    }
}