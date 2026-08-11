using Microsoft.AspNetCore.Mvc;
using StreamingSubscriptionTrackerAPI.Models;
using StreamingSubscriptionTrackerAPI.Services;
using StreamingSubscriptionTrackerAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace StreamingSubscriptionTrackerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("username/{username}")]
        public IActionResult GetByUsername(string username)
        {
            return Ok(_userService.GetByUsername(username));
        }

        [HttpGet("email/{email}")]
        public IActionResult GetByEmail(string email)
        {
            return Ok(_userService.GetByEmail(email));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            return Ok(_userService.GetById(id));
        }

        [HttpGet("actived/{actived}")]
        public IActionResult GetByActived(bool actived)
        {
            return Ok(_userService.GetByActived(actived));
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create([FromBody] UserRequestDTO userDto)
        {
            try
            {
                var createdUser = _userService.Create(userDto);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequestDTO loginDto)
        {
            try
            {
                var user = _userService.Login(loginDto.Username, loginDto.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(long id, [FromBody] UserRequestDTO userDto)
        {
            try
            {
                var updatedUser = _userService.Update(id, userDto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/actived/{id}")]
        public IActionResult UpdateActived(long id, [FromBody] UpdateActivedRequestDTO dto)
        {
            try
            {
                var updatedUser = _userService.UpdateActived(id, dto.Actived);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("update/password/{id}")]
        public IActionResult UpdatePassword(long id, [FromBody] UpdatePasswordRequestDTO dto)
        {
            try
            {
                var updatedUser = _userService.UpdatePassword(id, dto.Password);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var deletedUser = _userService.Delete(id);
                return Ok(deletedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
