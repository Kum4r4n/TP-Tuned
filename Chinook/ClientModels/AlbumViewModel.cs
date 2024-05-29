using Chinook.Models;

namespace Chinook.ClientModels
{
    public class AlbumViewModel
    {
        public long AlbumId { get; set; }
        public string Title { get; set; } = null!;
        public long ArtistId { get; set; }

        public virtual ArtistViewModel Artist { get; set; } = null!;

        public virtual ICollection<PlayListTrackViewModel> Tracks { get; set; }
    }
}
