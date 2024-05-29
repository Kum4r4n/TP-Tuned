namespace Chinook.ClientModels
{
    public class PlayListViewModel
    {
        public long PlaylistId { get; set; }
        public string? Name { get; set; }

        public List<PlayListTrackViewModel> Tracks { get; set; }
    }
}
