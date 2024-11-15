using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using Microsoft.AspNetCore.Mvc;
using taller01.src.Mappers;

namespace taller01.src.Controllers
{
    [Route("product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
         private readonly ApplicationDBContext _context;
        public ProductController(ApplicationDBContext context)
        {
            _context = context;
        }
    
    [HttpGet]
    public IActionResult Get()
    {
        var products = _context.Products.ToList().Select(p=>p.ToProductDto());
        return Ok(products);
    }
    

    }
}