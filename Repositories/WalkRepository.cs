using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class WalkRepository : IWalkRepository
{   
    private readonly NZWalksDbContext _context;
    public WalkRepository(NZWalksDbContext dbContext)
    {
        this._context = dbContext;
    }

    public async Task<Walk> CreateAsync(Walk walk)
    {
        await _context.Walks.AddAsync(walk);
        await _context.SaveChangesAsync();
        
        return walk;
    }

    public async Task<List<Walk>> GetAllAsync(string? filterOn=null, string? filterQuery=null,
    string? sortBy =null, bool isAscending = true,int pageNumber=1, int pageSize=1000)
    {       
        var walks = _context.Walks.Include("Difficulty").Include("Region").AsQueryable();
        
        // Filtering
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(w => w.Name.Contains(filterQuery));
            }
        }
        // Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
            }else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
            }
        }
        
        // Pagination
        var skip = (pageNumber - 1) * pageSize;
        return await walks.Skip(skip).Take(pageSize).ToListAsync();
        
        // no filter
        // return await _context.Walks.Include("Difficulty").Include("Region").ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await _context.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var existingWalk = await _context.Walks.FirstOrDefaultAsync(w => w.Id == id);
        if (existingWalk == null)
        {
            return null;
        }
        existingWalk.Name = walk.Name;
        existingWalk.Description = walk.Description;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        existingWalk.DifficultyId = walk.DifficultyId;
        existingWalk.RegionId = walk.RegionId;
        await _context.SaveChangesAsync();

        return (existingWalk);
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {   
        var existingWalk = await _context.Walks.FirstOrDefaultAsync(w => w.Id == id);
        if (existingWalk == null)
        {
            return null;
        } 
        _context.Walks.Remove(existingWalk);
        await _context.SaveChangesAsync();

        return (existingWalk);
    }
}