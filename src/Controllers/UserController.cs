using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using taller01.src.Mappers;

namespace api.src.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users
                            .Include(u => u.Role)
                            .ToList().Select(u => u.ToUserDto());

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var user = _context.Users
                            .Include(u => u.Role)
                            .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            var role = _context.Roles
                            .FirstOrDefault(r => r.Id == user.RoleId);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] User user)
        {
            var existingUser = _context.Users
                                    .FirstOrDefault(x => x.Id == id);

            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            var role = _context.Roles
                        .FirstOrDefault(r => r.Id == user.RoleId);

            if (role == null)
            {
                return NotFound("Role not found");
            }

            existingUser.Nombre = user.Nombre;
            existingUser.Correo = user.Correo;
            existingUser.RoleId = user.RoleId;

            _context.Users.Update(existingUser);
            _context.SaveChanges();
            return Ok(existingUser);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var user = _context.Users
                            .Include(u => u.Role)
                            .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("User deleted");
        }

        [HttpPost("withCookie")]
        public IActionResult PostByCookie([FromBody] User user)
        {
            // Verificar si el RoleId proporcionado es válido
            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions
            {
                HttpOnly = true, 
                Expires = DateTime.Now.AddDays(1)
            });

            return Ok(user);
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            if (Request.Cookies.TryGetValue("UserId", out var userId))
            {
                var user = _context.Users.Include(u => u.Role).FirstOrDefault(x => x.Id == int.Parse(userId));
                if (user == null)
                {
                    return NotFound("User not found");
                }
                return Ok(user);
            }

            return NotFound("UserId cookie not found");
        }
    }
}