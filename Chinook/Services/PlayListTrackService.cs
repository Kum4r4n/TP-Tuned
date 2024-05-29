using Chinook.Models;
using Chinook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlayListTrackService : IPlayListTrackService
    {
        private readonly ChinookContext _dbContext;

        public PlayListTrackService(ChinookContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task RemoveTrackFromPlayList(long trackId, long playListId )
        {
            var playList = await _dbContext.Playlists.SingleOrDefaultAsync(s=>s.PlaylistId == playListId);
            playList.Tracks.Remove(playList.Tracks.SingleOrDefault(s => s.TrackId == trackId));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Track> AddTrackToPlayList(long trackId, string userId,  string? playListName = null, long? playListId = null)
        {

            var track = await _dbContext.Tracks.SingleOrDefaultAsync(s=>s.TrackId == trackId);

            if(playListId != null && playListId > 0)
            {
                var playList = await _dbContext.Playlists.SingleOrDefaultAsync(x=>x.PlaylistId == playListId);
                if(playList?.Tracks == null)
                    playList.Tracks = new List<Track>();
                playList.Tracks.Add(track);
                await _dbContext.SaveChangesAsync();

            }else if(playListName != null)
            {
                var playList = new Playlist();
                playList.Name = playListName;
                playList.PlaylistId = new Random().Next(100000, 200000);

                var user = await _dbContext.Users
                .Include(i => i.UserPlaylists)
                    .ThenInclude(ii => ii.Playlist)
                        .ThenInclude(iii => iii.Tracks)
                            .ThenInclude(iiii => iiii.Album)
                                .ThenInclude(iiiii => iiiii.Artist)
                .SingleOrDefaultAsync(s => s.Id == userId);

                user.UserPlaylists.Add(new UserPlaylist()
                {
                    Playlist = playList,
                    UserId = userId
                });

                if (playList?.Tracks == null)
                    playList.Tracks = new List<Track>();
                playList.Tracks.Add(track);
                await _dbContext.SaveChangesAsync();
            }

            return track;

        }


        public async Task<Playlist> GetPlayListData(long id)
        {
            var playList = await _dbContext.Playlists
                    .Include(t=>t.UserPlaylists)
                    .Include(t => t.Tracks)
                        .ThenInclude(tt => tt.Album)
                            .ThenInclude(ttt => ttt.Artist)
                    .SingleOrDefaultAsync(s => s.PlaylistId == id);

            return playList;
        }

        public async Task<List<Playlist>> GetPlayListByUser(string userId)
        {
            var data = await _dbContext.UserPlaylists
                .Include(i => i.Playlist).Where(w=>w.UserId == userId).ToListAsync();

            return data?.Select(s => s.Playlist).ToList();
        }

        public async Task<Playlist> GetFavoritePlayListByUserId(string userId)
        {
            var user = await _dbContext.Users
                .Include(i => i.UserPlaylists)
                    .ThenInclude(ii => ii.Playlist)
                        .ThenInclude(iii => iii.Tracks)
                            .ThenInclude(iiii => iiii.Album)
                                .ThenInclude(iiiii => iiiii.Artist)
                .SingleOrDefaultAsync(s => s.Id == userId);

           

            var userFavoriteList = user.UserPlaylists.FirstOrDefault(f => f.Playlist.Name == "Favorite_1")?.Playlist;
            if (userFavoriteList == null){

                var favoritePlaylist = new Playlist { Name = "Favorite_1", PlaylistId = new Random().Next(100000, 200000) };
                _dbContext.Playlists.Add(favoritePlaylist);

                var userPlaylist = new UserPlaylist
                {
                    User = user,
                    Playlist = favoritePlaylist
                };

                _dbContext.UserPlaylists.Add(userPlaylist);
                await _dbContext.SaveChangesAsync();
                userFavoriteList = favoritePlaylist;
            }

            return userFavoriteList;
        }

        public async Task<Track> AddTrackAsFavorite(long trackId, string userId)
        {
            var track = await _dbContext.Tracks
                .Include(i=>i.Album)
                    .ThenInclude(ii=>ii.Artist)
                .SingleOrDefaultAsync(s => s.TrackId == trackId);

            var favoritePlayList = await GetFavoritePlayListByUserId(userId);
            if(favoritePlayList.Tracks == null)
                favoritePlayList.Tracks = new List<Track>();
            favoritePlayList.Tracks.Add(track);
            await _dbContext.SaveChangesAsync();
            return track;

        }

        public async Task<Track> RemoveTrackFromFavorite(long trackId, string userId)
        {

            var favoritePlaylist = await GetFavoritePlayListByUserId(userId);
            var trackToRemove = favoritePlaylist.Tracks
                .FirstOrDefault(t => t.TrackId == trackId);

            favoritePlaylist.Tracks.Remove(trackToRemove);
            await _dbContext.SaveChangesAsync();

            return trackToRemove;
        }
    }
}
