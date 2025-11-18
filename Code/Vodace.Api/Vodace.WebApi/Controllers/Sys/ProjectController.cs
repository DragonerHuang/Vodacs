using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Vodace.Api.Controllers.Sys
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetProjects()
        {
            var projects = new List<object>
            {
                new { Id = 1, Name = "Project A", Version = "v1" },
                new { Id = 2, Name = "Project B", Version = "v1" }
            };
            return Ok(projects);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetProjectsV2()
        {
            var projects = new List<object>
            {
                new { Id = 1, Name = "Project A", Description = "Description A", Version = "v2" },
                new { Id = 2, Name = "Project B", Description = "Description B", Version = "v2" }
            };
            return Ok(projects);
        }
    }
}