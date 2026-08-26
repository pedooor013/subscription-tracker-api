using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StreamingSubscriptionTrackerAPI.DTOs;
using StreamingSubscriptionTrackerAPI.Services;
using System.Security.Claims;

namespace StreamingSubscriptionTrackerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionCategoryController : Controller
    {
        public ISubscriptionCategoryService _subscriptionCategoryService;

        public SubscriptionCategoryController(ISubscriptionCategoryService subscriptionCategoryService)
        {
            _subscriptionCategoryService = subscriptionCategoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_subscriptionCategoryService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                var subscriptionCategory = _subscriptionCategoryService.GetById(id);
                return Ok(subscriptionCategory);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("name/{name}")]
        public IActionResult GetByName(string name)
        {
            try
            {
                var subscriptionCategory = _subscriptionCategoryService.GetByName(name);
                return Ok(subscriptionCategory);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]                                                                  
        public IActionResult Create([FromBody] SubscriptionCategoryRequestDTO dto)
        {
            dto.Name = dto.Name.ToLower();

            if (_subscriptionCategoryService.ExistsByName(dto.Name))
                return BadRequest($"Category with the name '{dto.Name}' already exists.");

            try
            {
                long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var subscriptionCategory = _subscriptionCategoryService.Create(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = subscriptionCategory.Id }, subscriptionCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] SubscriptionCategoryRequestDTO dto)
        {
            try
            {
                var subscriptionCategory = _subscriptionCategoryService.Update(id, dto);
                return Ok(subscriptionCategory);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                _subscriptionCategoryService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }       
    }
}
