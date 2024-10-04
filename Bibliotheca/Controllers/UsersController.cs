using Business.Abstract;
using Core.Concrete.Entities;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bibliotheca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("tcno")]
        public async Task<IActionResult> GetByTcNo(string tcNo)
        {
            var result = await _userService.GetByTcNo(tcNo);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _userService.GetByEmail(email);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return StatusCode(result.Status,result);
        }

        [HttpGet("username")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var result = await _userService.GetByUsername(username);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest();
        }



        [HttpPut]
        public async Task<IActionResult> Update(User user)
        {
            var result = await _userService.Update(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

    }

}
