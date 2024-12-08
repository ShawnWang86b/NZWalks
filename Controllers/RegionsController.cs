using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController: ControllerBase
{
    private readonly NZWalksDbContext _context;
    //DI ctor
    public RegionsController(NZWalksDbContext dbContext)
    {
        this._context = dbContext;
    }
    
    [HttpGet]
    public IActionResult GetAllRegions()
    {   
        // Get Data from Database - Domain models
        var regions = _context.Regions.ToList();
        
        // Map Domain Models to DTOs
        
        // Return DTOs
        return Ok(regions);
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetRegionById([FromRoute]Guid id)
    {   
        // Find method only take primary key;
        //var region = _context.Regions.Find(id);
        
        var region = _context.Regions.FirstOrDefault(x => x.Id == id);

        if (region == null)
        {
            return NotFound();
        }
        return Ok(region);
    }
}