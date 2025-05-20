using Microsoft.AspNetCore.Mvc;
using SmartTimePlanner.Models;

namespace SmartTimePlanner.Controllers
{
    [Route("api/schedulesubmit")]
    [ApiController]
    public class ScheduleSubmitController : ControllerBase
    {
        [HttpPost("save")]
        public IActionResult Save([FromBody] ScheduleInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed",
                    errors = ModelState
                });
            }

            return Ok(new
            {
                message = "Data received successfully",
                consultant = input.ConsultantName,
                start = input.StartTime,
                end = input.EndTime,
                activity = input.Activity,
                day = input.DayOfWeek
            });
        }
    }
}
