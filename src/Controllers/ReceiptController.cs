using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos.Receipt;
using api.src.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Interface;

namespace taller01.src.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptService _receiptService;
        public ReceiptController(IReceiptRepository receiptRepository, IReceiptService receiptService)
        {
            _receiptRepository = receiptRepository;
            _receiptService = receiptService;
        }

        [HttpPost("CreateReceipt")]
        [Authorize]
        public async Task<IActionResult> CreateReceipt([FromBody] ReceiptDto receiptDto)
        {
            try
            {
                var receipt = await _receiptService.CreateReceipt(receiptDto.UserRut, receiptDto.Country, receiptDto.City, receiptDto.Commune, receiptDto.Street);

                var newreceiptDto = new ReceiptDto
                {
                    Id = receipt.Id,
                    UserRut = receipt.UserRut,
                    Country = receipt.Country,
                    City = receipt.City,
                    Commune = receipt.Commune,
                    Street = receipt.Street,
                    Date = receipt.Date,
                    Items = receipt.Items.Select(i => new ReceiptItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList(),
                    Total = receipt.Total
                };
                return Ok(newreceiptDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetReceipt/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReceipt(int id)
        {
            try
            {
                var receipt = await _receiptRepository.Get(id);

                var receiptDto = new ReceiptDto
                {
                    Id = receipt.Id,
                    UserRut = receipt.UserRut,
                    Country = receipt.Country,
                    City = receipt.City,
                    Commune = receipt.Commune,
                    Street = receipt.Street,
                    Date = receipt.Date,
                    Items = receipt.Items.Select(i => new ReceiptItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList(),
                    Total = receipt.Total
                };
                return Ok(receiptDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("receipts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReceipts([FromQuery] QueryObject query)
        {
            try
            {
                var (receipts, totalCount) = await _receiptRepository.GetReceiptsAsync(query);
            

                return Ok(receipts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}