using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Helpers;
using taller01.src.models;

namespace taller01.src.Interface
{
    public interface IReceiptRepository
    {
        Task<Receipt> Add(Receipt receipt);
        Task<Receipt> Get(int id);
        Task<(IEnumerable<Receipt> Receipts, int TotalCount)> GetReceiptsAsync(QueryObject query);
        Task<IEnumerable<Receipt>> GetByUser(string rut);
    }
}