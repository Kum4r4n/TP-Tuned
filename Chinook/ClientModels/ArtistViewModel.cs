namespace Chinook.ClientModels
{
    public class ArtistViewModel
    {
        public long ArtistId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<AlbumViewModel> Albums { get; set; }
    }
}
