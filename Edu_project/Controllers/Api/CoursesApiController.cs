using Microsoft.AspNetCore.Mvc;
using EdukateMvc.Services;
using EdukateMvc.Models;


[ApiController]
[Route("api/[controller]")]
public class CoursesApiController : ControllerBase
{
    private readonly ICourseService _service;
    public CoursesApiController(ICourseService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id) =>
        Ok(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Course dto)
    {
        await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Course dto)
    {
        dto.Id = id;
        await _service.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
