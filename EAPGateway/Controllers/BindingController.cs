using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EAPGateway.Models;
using EAPGateway.Services;

namespace EAPGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BindingController : ControllerBase
    {
        private readonly DatabaseService _dbService;
        private readonly LogService _logger;

        public BindingController(DatabaseService dbService, LogService logger)
        {
            _dbService = dbService;
            _logger = logger;
        }

        private string GetClientInfo() => Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        [HttpPost("bind")]
        public async Task<IActionResult> Bind([FromBody] BindRequest req)
        {
            _logger.Info($"[API] ({GetClientInfo()}) [{req.EqpId}] Received Bind Request for {req.CassetteId}", req.EqpId);
            bool ok = await _dbService.BindAsync(req);
            return ok ? Ok("PASS") : StatusCode(500, "FAIL|DB Error");
        }

        [HttpPost("unbind")]
        public async Task<IActionResult> Unbind([FromBody] UnbindRequest req)
        {
            _logger.Info($"[API] ({GetClientInfo()}) [{req.EqpId}] Received Unbind Request for {req.CassetteId}", req.EqpId);
            bool ok = await _dbService.UnbindAsync(req);
            return ok ? Ok("PASS") : StatusCode(500, "FAIL|DB Error");
        }

        [HttpGet("query/{eqpId}/{cassetteId}")]
        public async Task<IActionResult> Query(string eqpId, string cassetteId)
        {
            _logger.Info($"[API] ({GetClientInfo()}) [{eqpId}] Querying Binding for {cassetteId}", eqpId);
            var res = await _dbService.QueryBindingAsync(cassetteId, eqpId);
            if (res != null)
            {
                _logger.Debug($"[API] [{eqpId}] Query Result: PASS (WO: {res.WorkOrder}, Panels: {res.Panels.Count})", eqpId);
                return Ok(res);
            }
            _logger.Warn($"[API] [{eqpId}] Query Result: NOT FOUND for {cassetteId}", eqpId);
            return NotFound("FAIL|Not Found");
        }
    }
}
