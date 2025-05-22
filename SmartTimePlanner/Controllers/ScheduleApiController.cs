using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Core.Models.Blocks;
using SmartTimePlanner.Models;

namespace SmartTimePlanner.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleApiController : ControllerBase
    {
        private readonly IUmbracoContextFactory _contextFactory;

        public ScheduleApiController(IUmbracoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpGet]
        public IActionResult GetSchedule()
        {
            using var contextRef = _contextFactory.EnsureUmbracoContext();
            var umbracoContext = contextRef.UmbracoContext;

            var root = umbracoContext.Content.GetAtRoot().FirstOrDefault();
            if (root == null)
                return NotFound("Root node not found.");

            var consultantPages = root.DescendantsOfType("consultantPage");
            var scheduleList = new List<object>();

            foreach (var consultant in consultantPages)
            {
                var name = consultant.Value<string>("consultantName");
                var items = consultant.Value<IEnumerable<BlockListItem>>("consultantSchedule");

                if (items == null) continue;

                var schedules = items
                    .Select(item => item.Content.Properties.ToDictionary(p => p.Alias, p => p.GetValue()))
                    .ToList();

                // Dubbelbokningskontroll
                var conflicts = new List<object>();
                for (int i = 0; i < schedules.Count; i++)
                {
                    var a = schedules[i];
                    var aStart = Convert.ToDateTime(a["startTime"]);
                    var aEnd = Convert.ToDateTime(a["endTime"]);

                    for (int j = i + 1; j < schedules.Count; j++)
                    {
                        var b = schedules[j];
                        var bStart = Convert.ToDateTime(b["startTime"]);
                        var bEnd = Convert.ToDateTime(b["endTime"]);

                        bool overlap = aStart < bEnd && bStart < aEnd;
                        if (overlap)
                        {
                            conflicts.Add(new
                            {
                                Consultant = name,
                                ConflictBetween = new[] { a, b }
                            });
                        }
                    }
                }

                scheduleList.Add(new
                {
                    Consultant = name,
                    Schedule = schedules,
                    Conflicts = conflicts
                });
            }

            return Ok(scheduleList);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ScheduleInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input." });
            }

            try
            {
                // Här loggar vi för att se att värdena kommit in korrekt
                Console.WriteLine($"Saving schedule: {input.ConsultantName}, {input.StartTime}, {input.EndTime}, {input.Activity}");

                // Här skulle du lägga in schemat i Umbraco om du kopplar till BlockList etc

                return Ok(new { message = "Schedule saved successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error saving schedule.", error = ex.Message });
            }
        }

    }


}
