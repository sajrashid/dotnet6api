// <copyright file="VisitorsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using MyAPI.Models;
    using MyAPI.Repository;
    using MyAPI.Services;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorsController : ControllerBase
    {
        private readonly ILogger<VisitorsController> logger;
        private readonly IVisitorRepository repo;

        public VisitorsController(IVisitorRepository repo, ILogger<VisitorsController> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("WhoIs")]
        public async Task<ActionResult> WhoIs()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            ILogger logger = loggerFactory.CreateLogger<VisitorsController>();
            this.logger.LogInformation("USer COntroller Getimg");

            var ip = string.Empty;

            // IP is forwaded from nginx proxy
            // may throw an erro when running without
            ip = this.Request.Headers["X-Forwarded-For"].ToString();

            var userAgent = this.HttpContext.Request.Headers["User-Agent"].ToString();
            var hash = SecurityHelper.CreateMD5Hash($"{ip}{userAgent}");

            Visitor exisitingVisitor = await this.DoesVistorExist(hash).ConfigureAwait(false);

            if (exisitingVisitor is null)
            {
                // new user hash not found
                var newVisitor = new Visitor() { Hash = hash, IP = ip, UserAgent = userAgent, LastVisit = DateTime.Now, Count = 1 };

                await this.repo.InsertVisitor(newVisitor).ConfigureAwait(false);
                return new StatusCodeResult(200);
            }

            exisitingVisitor.LastVisit = DateTime.Now;
            exisitingVisitor.Count++;
            await this.repo.UpdateVisitor(1, exisitingVisitor).ConfigureAwait(false);
            return new StatusCodeResult(200);
        }

        [HttpGet]
        public async Task<ActionResult<List<Visitor>>> Get()
        {
            var vistors = await this.repo.GetAllVisitors().ConfigureAwait(false);
            return this.Ok(vistors);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, [FromBody] Visitor visitors)
        {
            await this.repo.UpdateVisitor(id, visitors).ConfigureAwait(false);
            return this.Ok(visitors);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await this.repo.DeleteVisitorById(id).ConfigureAwait(false);
            return this.Ok(id);
        }

        private Task<Visitor> DoesVistorExist(string hash)
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = loggerFactory.CreateLogger<VisitorsController>();

            this.logger.LogInformation("DoesUserExist");

            return this.repo.GetVisitor(hash);
        }
    }
}