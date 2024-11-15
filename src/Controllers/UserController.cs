using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using Microsoft.AspNetCore.Mvc;

namespace taller01.src.Controllers
{
    [Route("user")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

    }

    
}