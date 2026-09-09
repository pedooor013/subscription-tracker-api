using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingSubscriptionTrackerAPI.DTOs;
using StreamingSubscriptionTrackerAPI.Models;
using StreamingSubscriptionTrackerAPI.Services;
using System.Security.Claims;

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("username/{username}")]
        public IActionResult GetByUsername(string username)
        {
            return Ok(_userService.GetByUsername(username));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("email/{email}")]
        public IActionResult GetByEmail(string email)
        {
            return Ok(_userService.GetByEmail(email));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            return Ok(_userService.GetById(id));
        }

        [Authorize(Roles = "Admin")]
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
                long? filterByUserId = DiscoverRole();

                var updatedUser = _userService.Update(id, userDto, filterByUserId);
                return Ok(updatedUser);
        }

        [HttpPut("update/actived/{id}")]
        public IActionResult UpdateActived(long id, [FromBody] UpdateActivedRequestDTO dto)
        {
                long? filterByUserId = DiscoverRole();
                var updatedUser = _userService.UpdateActived(id, dto.Actived, filterByUserId);
                return Ok(updatedUser);
        }
        
        [HttpPut("update/password/{id}")]
        public IActionResult UpdatePassword(long id, [FromBody] UpdatePasswordRequestDTO dto)
        {
                long? filterByUserId = DiscoverRole(); 
                var updatedUser = _userService.UpdatePassword(id, dto.Password, filterByUserId);
                return Ok(updatedUser);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(long id)
        {
                long? filterByUserId = DiscoverRole();
                var deletedUser = _userService.Delete(id, filterByUserId);
                return Ok(deletedUser);
        }

        //Utils
        protected long? DiscoverRole()
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            long? filterByUserId = User.IsInRole("Admin") ? null : userId;
            return filterByUserId;

        }

    }
}
