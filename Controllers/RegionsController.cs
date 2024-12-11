using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
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
    private readonly IMapper _mapper;
    
    //DI ctor
    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
    {
        this._context = dbContext;
        this._regionRepository = regionRepository;
        this._mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllRegions()
    {    
        // Get Data from Database - Domain models
        // var regionsDomain = await _context.Regions.ToListAsync();

        var regionsDomain = await _regionRepository.GetAllAsync();
        
        // Map Domain Models to DTOs
        // var regionsDto = new List<RegionDto>();
        // foreach (var region in regionsDomain)
        // {
        //     regionsDto.Add(new RegionDto()
        //     {
        //         Id = region.Id,
        //         Code = region.Code,
        //         Name = region.Name,
        //         RegionImageUrl = region.RegionImageUrl,
        //     });
        // }

        // Return DTOs
        return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetRegionById([FromRoute]Guid id)
    {   
        // Find method only take primary key;
        //var region = _context.Regions.Find(id);
        
        var regionsDomain = await _regionRepository.GetByIdAsync(id);

        if (regionsDomain == null)
        {
            return NotFound();
        }

        // var regionDto = new RegionDto()
        // {
        //     Id = regionsDomain.Id,
        //     Code = regionsDomain.Code,
        //     Name = regionsDomain.Name,
        //     RegionImageUrl = regionsDomain.RegionImageUrl,
        // };

        return Ok(_mapper.Map<RegionDto>(regionsDomain));
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto createRegionDto)
    {
        
            var regionDomainModel = _mapper.Map<Region>(createRegionDto);
            
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
            
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionDomainModel);
        

        
        // var regionDomainModel = new Region()
        // {
        //     Code = createRegionDto.Code,
        //     Name = createRegionDto.Name,
        //     RegionImageUrl = createRegionDto.RegionImageUrl,
        // };
        // var regionDomainModel = _mapper.Map<Region>(createRegionDto);
        // regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
        //await _context.Regions.AddAsync(regionDomainModel);
        //await _context.SaveChangesAsync();
        
        // Map Domain model back to Dto
        // var regionDto = new RegionDto()
        // {
        //     Id = regionDomainModel.Id,
        //     Code = regionDomainModel.Code,
        //     Name = regionDomainModel.Name,
        // };
        // var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
        // return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionDomainModel);
    }

    [HttpPut]
    [Route("{id}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
    { 
        var regionDomainModel = _mapper.Map<Region>(updateRegionDto);
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
      
        // Map Dto to domain model
        // var regionDomainModel = new Region()
        // {
        //     Code = updateRegionDto.Code,
        //     Name = updateRegionDto.Name,
        //     RegionImageUrl = updateRegionDto.RegionImageUrl,
        // };
        // var regionDomainModel = _mapper.Map<Region>(updateRegionDto);
        // regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);
        //
        // if (regionDomainModel == null)
        // {
        //     return NotFound();
        // }
        //
        // await _context.SaveChangesAsync();
        // Convert domain model to Dto
        // var regionDto = new RegionDto()
        // {
        //     Id = regionDomainModel.Id,
        //     Code = regionDomainModel.Code,
        //     Name = regionDomainModel.Name,
        //     RegionImageUrl = regionDomainModel.RegionImageUrl,
        // };
        
        // return Ok(_mapper.Map<RegionDto>(regionDomainModel));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        var regionDomainModel =await _regionRepository.DeleteAsync(id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // Map domain Model to Dto
        // var regionDto = new RegionDto()
        // {
        //     Id = regionDomainModel.Id,
        //     Code = regionDomainModel.Code,
        //     Name = regionDomainModel.Name,
        //     RegionImageUrl = regionDomainModel.RegionImageUrl,
        // };
        
        return Ok(_mapper.Map<RegionDto>(regionDomainModel));
    }
}