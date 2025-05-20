using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using SmartTimePlanner.Models;

namespace SmartTimePlanner.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleConflictController : UmbracoApiController
    {
        private readonly IUmbracoContextFactory _contextFactory;

        public ScheduleConflictController(IUmbracoContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpPost("check")]
        public IActionResult CheckConflict([FromBody] ScheduleInputModel input)
        {
            using var contextRef = _contextFactory.EnsureUmbracoContext();
            var umbracoContext = contextRef.UmbracoContext;

            var root = umbracoContext.Content.GetAtRoot().FirstOrDefault();
            if (root == null)
                return NotFound("Root node not found.");

            var consultantPages = root.DescendantsOrSelfOfType("consultantPage");
            var consultant = consultantPages.FirstOrDefault(x =>
                x.Value<string>("consultantName")?.Equals(input.ConsultantName, StringComparison.OrdinalIgnoreCase) == true);

            if (consultant == null)
                return NotFound($"Consultant '{input.ConsultantName}' not found.");

            var scheduleItems = consultant.Value<IEnumerable<BlockListItem>>("consultantSchedule");

            if (scheduleItems == null)
                return Ok(new { Message = "No existing schedules." });

            foreach (var item in scheduleItems)
            {
                var start = item.Content.Value<DateTime>("startTime");
                var end = item.Content.Value<DateTime>("endTime");

                bool overlaps =
                    input.StartTime < end &&
                    input.EndTime > start;

                if (overlaps)
                {
                    return Conflict(new
                    {
                        Message = "Time conflict detected with existing schedule.",
                        ConflictStart = start,
                        ConflictEnd = end,
                        InputActivity = input.Activity
                    });
                }
            }

            return Ok(new { Message = "No conflict – time is available." });
        }
    }
}
