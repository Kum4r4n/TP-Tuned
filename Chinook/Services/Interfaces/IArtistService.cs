using Chinook.Models;

namespace Chinook.Services.Interfaces
{
    public interface IArtistService
    {
        Task<List<Artist>> GetArtists(string searchText);
        Task<Artist> GetArtist(long id);
    }
}
