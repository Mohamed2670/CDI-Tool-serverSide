using CDI_Tool.Dtos.LogDtos;
using CDI_Tool.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CDI_Tool.Controller
{
    [ApiController, Route("log"), Authorize(Policy = "Admin")]
    public class LogController(LogService _logService) : ControllerBase
    {
        [HttpGet("GetAllLogsPaginated"),AllowAnonymous]
        public async Task<IActionResult> GetAllLogsPaginated([FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var items = await _logService.GetAllLogsPaginated(pageSize, pageNumber);
            return Ok(items);
        }
        [HttpGet("GetFilterdLogsPaginated"),AllowAnonymous]
        public async Task<IActionResult> GetFilterdLogsPaginated([FromQuery] RequestFilteredLogReadDto requestFilteredLogReadDto, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var items = await _logService.GetFilterdLogsPaginated(requestFilteredLogReadDto, pageSize, pageNumber);
            return Ok(items);
        }
        [HttpPost("CreateLog"), AllowAnonymous]
        public async Task<IActionResult> CreateLog([FromBody] LogCreateDto logCreateDto)
        {
            var item = await _logService.CreateLog(logCreateDto);
            return Ok(item);
        }
    }
}