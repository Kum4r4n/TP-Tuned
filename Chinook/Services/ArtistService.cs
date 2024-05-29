using Chinook.ClientModels;
using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ChinookContext _dbContext;

        public ArtistService(ChinookContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Artist>> GetArtists(string searchText)
        {
            return await _dbContext.Artists
                .Include(s => s.Albums)
                .AsNoTracking()
                .Where(s=> string.IsNullOrWhiteSpace(searchText) || s.Name.ToLower().Contains(searchText.ToLower()))
                .ToListAsync();
        }

        public async Task<Artist> GetArtist(long id)
        {
            return await _dbContext.Artists
                    .Include(s => s.Albums)
                        .ThenInclude(t => t.Tracks)
                            .ThenInclude(t=>t.Playlists)
                                .ThenInclude(tt=>tt.UserPlaylists)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(w => w.ArtistId == id);
        }
    }   
}
