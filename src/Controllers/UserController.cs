using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.data;
using api.src.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using taller01.src.Dtos;
using taller01.src.Interfaces;
using taller01.src.Mappers;

namespace api.src.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? age = null, [FromQuery] string? gender = null, [FromQuery] string? estado = null)
        {
            var query = _userRepository.GetAsQuery(age, gender, estado);

            if (age.HasValue)
            {
                var birthDate = DateTime.Now.AddYears(-age.Value);
                query = query.Where(x => x.FechaNacimiento.Year == birthDate.Year);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(x => x.Gender.Name == gender);
            }

            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(x => x.Estado.Name == estado);
            }

            var users = query.ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var user = _userRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("rut/{rut}")]
        public IActionResult GetByRut([FromRoute] string rut)
        {
            var user = _userRepository.GetByRut(rut);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto userDto)
        {

            var user = userDto.ToUser();
            await _userRepository.Post(user);
            
            return CreatedAtAction(nameof(GetById), new {id = user.Id}, user.ToUserDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var userModel = await _userRepository.Put(id, updateUserDto);
            if (userModel == null) 
            {
                return NotFound();
            }
            return Ok(userModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userModel = await _userRepository.Delete(id);

            if (userModel == null)
            {
                return NotFound("User not found");
            }            
            return NoContent();
        }

        [HttpPost("withCookie")]
        public async Task<IActionResult> PostByCookie([FromBody] UserDto userDto)
        {
            var user = userDto.ToUser();
            await _userRepository.Post(user);

            Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions
            {
                HttpOnly = true, 
                Expires = DateTime.Now.AddDays(1)
            });

            return Ok(user);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (Request.Cookies.TryGetValue("UserId", out var userId))
            {
                var user = await _userRepository.GetById(int.Parse(userId));
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