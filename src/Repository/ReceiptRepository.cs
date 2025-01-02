using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.Helpers;
using Microsoft.EntityFrameworkCore;
using taller01.src.Interface;
using taller01.src.models;

namespace taller01.src.Repository
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly ApplicationDBContext _context;

        public ReceiptRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        
        public async Task<Receipt> Add(Receipt receipt)
        {
            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();
            return receipt;
        }

        public async Task<Receipt> Get(int id)
        {
            var receipt = await _context.Receipts.Include(i => i.Items).FirstOrDefaultAsync(i => i.Id == id);
            if (receipt == null)
            {
                throw new KeyNotFoundException($"Receipt not found.");
            }
            return receipt;
        }

        public async Task<IEnumerable<Receipt>> GetByUser(string rut)
        {
            return await _context.Receipts.Include(i => i.Items).Where(i => i.UserRut == rut).ToListAsync();
        }

        public async Task<(IEnumerable<Receipt> Receipts, int TotalCount)> GetReceiptsAsync(QueryObject query)
        {
            var queryable = _context.Receipts.Include(r => r.Items).AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                queryable = queryable.Where(r => r.CustomerName.Contains(query.Name));
            }

            if (query.IsDescending)
            {
                queryable = queryable.OrderByDescending(r => r.Date);
            }
            else
            {
                queryable = queryable.OrderBy(r => r.Date);
            }

            int totalCount = await queryable.CountAsync();

            queryable = queryable
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            var receipts = await queryable.ToListAsync();

            return (receipts, totalCount);
        }
    }
}