
namespace MyAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using MyAPI.Models;
    using MyAPI.Repository;
    using MyAPI.Services;

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorsController : ControllerBase
    {
        private readonly ILogger<VisitorsController> _logger;
        private readonly IVisitorRepository repo;

        public VisitorsController(IVisitorRepository repo, ILogger<VisitorsController> logger)
        {
            this.repo = repo;
            _logger = logger;
        }

        private async Task<Visitor> DoesVistorExist(string hash)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger<VisitorsController>();
            logger.LogInformation("DoesUserExist");

            return await repo.GetVisitor(hash).ConfigureAwait(false);
        }

        [AllowAnonymous]
        [HttpGet("WhoIs")]
        public async Task<ActionResult> WhoIs()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger<VisitorsController>();
            logger.LogInformation("USer COntroller Getimg");

            var ip = string.Empty;

            // IP is forwaded from nginx proxy
            // may throw an erro when running without

            ip = Request.Headers["X-Forwarded-For"].ToString();

            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var hash = SecurityHelper.CreateMD5Hash($"{ip}{userAgent}");

            Visitor exisitingVisitor = await DoesVistorExist(hash).ConfigureAwait(false);

            if (exisitingVisitor is null)
            {
                // new user hash not found
                var newVisitor = new Visitor() { Hash = hash, IP = ip, UserAgent = userAgent, LastVisit = DateTime.Now, Count = 1 };

                await repo.InsertVisitor(newVisitor).ConfigureAwait(false);
                return new StatusCodeResult(200);
            }

            exisitingVisitor.LastVisit = DateTime.Now;
            exisitingVisitor.Count++;
            await repo.UpdateVisitor(1, exisitingVisitor).ConfigureAwait(false);
            return new StatusCodeResult(200);
        }

        [HttpGet()]
        public async Task<ActionResult<List<Visitor>>> Get()
        {
            var vistors = await repo.GetAllVisitors().ConfigureAwait(false);
            return Ok(vistors);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, [FromBody] Visitor visitors)
        {   // update a user
            await repo.UpdateVisitor(id, visitors).ConfigureAwait(false);
            return Ok(visitors);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await repo.DeleteVisitorById(id).ConfigureAwait(false);
            return Ok(id);

        }
    }
}