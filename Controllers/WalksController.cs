using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalksController : ControllerBase
{   
    private readonly IWalkRepository _walkRepository;
    private readonly IMapper _mapper;
    
    public WalksController(IWalkRepository walkRepository, IMapper mapper)
    {
        this._walkRepository = walkRepository;
        this._mapper = mapper;
    }

    // GET API: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filterOn,[FromQuery] string? filterQuery,
        [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
        [FromQuery] int pageNumber=1, [FromQuery] int pageSize=100)
    {
       var walksDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery,
           sortBy, isAscending??true, pageNumber, pageSize);
       
       return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetById([FromRoute] Guid id)
    {
        var walkDomainModel = await _walkRepository.GetByIdAsync(id);

        if (walkDomainModel == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }
    
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
        
        await _walkRepository.CreateAsync(walkDomainModel);
        
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
            
    }

    [HttpPut]
    [Route("{id}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
    {
        var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
        walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

        if (walkDomainModel == null)
        { 
            return NotFound();
        }
        return Ok(_mapper.Map<WalkDto>(walkDomainModel));
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleteWalkDomainModel = await _walkRepository.DeleteAsync(id);
        if (deleteWalkDomainModel == null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<WalkDto>(deleteWalkDomainModel));
    }
    
}