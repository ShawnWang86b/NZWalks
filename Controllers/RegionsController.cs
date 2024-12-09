using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController: ControllerBase
{
    private readonly NZWalksDbContext _context;

    private readonly IRegionRepository _regionRepository;
    //DI ctor
    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
    {
        this._context = dbContext;
        this._regionRepository = regionRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllRegions()
    {   
        // Get Data from Database - Domain models
        // var regionsDomain = await _context.Regions.ToListAsync();

        var regionsDomain = await _regionRepository.GetAllAsync();
        
        // Map Domain Models to DTOs
        var regionsDto = new List<RegionDto>();
        foreach (var region in regionsDomain)
        {
            regionsDto.Add(new RegionDto()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            });
        }
        // Return DTOs
        return Ok(regionsDto);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetRegionById([FromRoute]Guid id)
    {   
        // Find method only take primary key;
        //var region = _context.Regions.Find(id);
        
        var regionsDomain = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (regionsDomain == null)
        {
            return NotFound();
        }

        var regionDto = new RegionDto()
        {
            Id = regionsDomain.Id,
            Code = regionsDomain.Code,
            Name = regionsDomain.Name,
            RegionImageUrl = regionsDomain.RegionImageUrl,
        };
        return Ok(regionDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto createRegionDto)
    {
        var regionDomainModel = new Region()
        {
            Code = createRegionDto.Code,
            Name = createRegionDto.Name,
            RegionImageUrl = createRegionDto.RegionImageUrl,
        };
        
        await _context.Regions.AddAsync(regionDomainModel);
        await _context.SaveChangesAsync();
        
        // Map Domain model back to Dto
        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
        };
        
        return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionDomainModel);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
    {
        var regionDomainModel = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }
        
        // Map Dto to domain model
        regionDomainModel.Code = updateRegionDto.Code;
        regionDomainModel.Name = updateRegionDto.Name;
        regionDomainModel.RegionImageUrl = updateRegionDto.RegionImageUrl;
  
        await _context.SaveChangesAsync();
        // Convert domain model to Dto
        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl,
        };
        return Ok(regionDto);

    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        var regionDomainModel = await _context.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }
        _context.Regions.Remove(regionDomainModel);
        await _context.SaveChangesAsync();
        
        // Delete Region back
        // Map domain Model to Dto
        var regionDto = new RegionDto()
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl,
        };
        return Ok(regionDto);
    }
}