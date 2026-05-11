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

        public BindingController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        private string GetClientInfo() => Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        [HttpPost("bind")]
        public async Task<IActionResult> Bind([FromBody] BindRequest req)
        {
            _dbService.TriggerLog($"[API] ({GetClientInfo()}) [{req.EqpId}] Received Bind Request for {req.CassetteId}");
            bool ok = await _dbService.BindAsync(req);
            return ok ? Ok("PASS") : StatusCode(500, "FAIL|DB Error");
        }

        [HttpPost("unbind")]
        public async Task<IActionResult> Unbind([FromBody] UnbindRequest req)
        {
            _dbService.TriggerLog($"[API] ({GetClientInfo()}) [{req.EqpId}] Received Unbind Request for {req.CassetteId}");
            bool ok = await _dbService.UnbindAsync(req);
            return ok ? Ok("PASS") : StatusCode(500, "FAIL|DB Error");
        }

        [HttpGet("query/{eqpId}/{cassetteId}")]
        public async Task<IActionResult> Query(string eqpId, string cassetteId)
        {
            _dbService.TriggerLog($"[API] ({GetClientInfo()}) [{eqpId}] Querying Binding for {cassetteId}");
            var res = await _dbService.QueryBindingAsync(cassetteId);
            if (res != null)
            {
                _dbService.TriggerLog($"[API] [{eqpId}] Query Result: PASS (WO: {res.WorkOrder}, Panels: {res.Panels.Count})");
                return Ok(res);
            }
            _dbService.TriggerLog($"[API] [{eqpId}] Query Result: NOT FOUND for {cassetteId}");
            return NotFound("FAIL|Not Found");
        }
    }
}
