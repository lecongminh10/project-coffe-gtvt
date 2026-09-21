using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BaiTapLon.Data;

namespace BaiTapLon.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class TablesApiController : ControllerBase
{
    private readonly CoffeeShopDbContext _context;

    public TablesApiController(CoffeeShopDbContext context)
    {
        _context = context;
    }

    // GET: api/TablesApi
    [HttpGet]
    public async Task<IActionResult> GetTables()
    {
        var tables = await _context.CoffeeTables
            .OrderBy(t => t.Area)
            .ThenBy(t => t.TableName)
            .Select(t => new
            {
                t.TableId,
                t.TableName,
                t.Capacity,
                t.Area,
                t.Status
            })
            .ToListAsync();

        return Ok(tables);
    }

    public class UpdateStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

    // PUT: api/TablesApi/5/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var table = await _context.CoffeeTables.FindAsync(id);
        if (table == null)
        {
            return NotFound(new { message = $"Không tìm thấy bàn #{id}" });
        }

        table.Status = dto.Status;
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = $"Bàn {table.TableName} đã chuyển sang {dto.Status}", table });
    }
}
