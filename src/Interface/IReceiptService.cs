using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taller01.src.models;

namespace taller01.src.Interface
{
    public interface IReceiptService
    {
        public Task<Receipt> CreateReceipt(string rut, string country, string city, string commune, string street);
    }
}