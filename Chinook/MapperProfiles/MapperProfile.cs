using AutoMapper;
using Chinook.ClientModels;
using Chinook.Models;
using Playlist = Chinook.Models.Playlist;

namespace Chinook.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Artist, ArtistViewModel>();
            CreateMap<Album, AlbumViewModel>();
            CreateMap<Track, PlayListTrackViewModel>();
            CreateMap<Playlist, PlayListViewModel>();
        }
    }
}
