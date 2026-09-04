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
    public class SubscriptionController : ControllerBase
    {
        public ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            long? filterByUserId = User.IsInRole("Admin") ? null : userId;
            var result = await _subscriptionService.GetAll(filterByUserId);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            try
            {
                var subscription = _subscriptionService.GetById(id);
                return Ok(subscription);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("category/{id}")]
        public IActionResult GetSubscriptionFromCategory(long id)
        {
            try
            {
                var subscription = _subscriptionService.GetSubscriptionFromCategory(id);
                return Ok(subscription);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] SubscriptionRequestDTO dto)
        {
            try
            {
                long userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var subscription = _subscriptionService.Create(dto, userId);
                return CreatedAtAction(nameof(GetById), new { id = subscription.Id }, subscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] SubscriptionRequestDTO dto)
        {

            try
            {
                var subscription = _subscriptionService.Update(id, dto);
                return Ok(subscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                _subscriptionService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}