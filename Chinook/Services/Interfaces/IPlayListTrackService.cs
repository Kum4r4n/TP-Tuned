using Chinook.Models;
using System.Threading.Tasks;

namespace Chinook.Services.Interfaces
{
    public interface IPlayListTrackService
    {
        Task<Track> AddTrackAsFavorite(long trackId, string userId);
        Task<Track> RemoveTrackFromFavorite(long trackId, string userId);
        Task<List<Playlist>> GetPlayListByUser(string userId);
        Task<Playlist> GetPlayListData(long id);
        Task<Track> AddTrackToPlayList(long trackId, string userId, string? playListName = null, long? playListId = null);
        Task RemoveTrackFromPlayList(long trackId, long playListId);
    }
}
