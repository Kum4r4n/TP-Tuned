﻿namespace Chinook.ClientModels
{
    public class PlayListTrackViewModel
    {
        public long TrackId { get; set; }
        public string TrackName { get; set; }
        public string AlbumTitle { get; set; }
        public string ArtistName { get; set; }
        public bool IsFavorite { get; set; }
    }
}
