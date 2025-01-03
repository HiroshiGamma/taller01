using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos.Receipt
{
    public class ReceiptDto
    {

        public int Id { get; set; }


        public string UserRut { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        
        public string Commune { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;
        

        public DateTime Date { get; set; }

        public int Total { get; set; }

        public List<ReceiptItemDto> Items { get; set; } = new List<ReceiptItemDto>();
        
    }
}