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

        [HttpPost("bind")]
        public async Task<IActionResult> Bind([FromBody] BindRequest req)
        {
            bool ok = await _dbService.BindAsync(req);
            return ok ? Ok("PASS") : StatusCode(500, "FAIL|DB Error");
        }

        [HttpPost("unbind")]
        public async Task<IActionResult> Unbind([FromBody] UnbindRequest req)
        {
            bool ok = await _dbService.UnbindAsync(req);
            return ok ? Ok("PASS") : StatusCode(500, "FAIL|DB Error");
        }

        [HttpGet("query/{cassetteId}")]
        public async Task<IActionResult> Query(string cassetteId)
        {
            var res = await _dbService.QueryBindingAsync(cassetteId);
            return res != null ? Ok(res) : NotFound("FAIL|Not Found");
        }
    }
}
