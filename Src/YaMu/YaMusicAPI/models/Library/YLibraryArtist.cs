using System.Collections.Generic;

using Yandex.Music.Api.Models.Artist;
using Yandex.Music.Api.Models.Common;

namespace Yandex.Music.Api.Models.Library
{
    public class YLibraryArtist : YArtist
    {
        public new bool Available { get; set; }
        public new YArtistCounts Counts { get; set; }
        public new List<YLink> Links { get; set; }
        public new YArtistRatings Ratings { get; set; }
        public new bool TicketsAvailable { get; set; }
    }
}