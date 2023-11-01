using web_bmstu.DTO;
using web_bmstu.ModelsBL;
using web_bmstu.Services;

namespace web_bmstu.ModelsConverters
{
    public class PlaylistConverters
    {
        private readonly IPlaylistService playlistService;

        public PlaylistConverters(IPlaylistService playlistService)
        {
            this.playlistService = playlistService;
        }

        public PlaylistBL convertPatch(int id, PlaylistBaseDto playlist)
        {
            var existedPlaylist = playlistService.GetByID(id);

            return new PlaylistBL
            {
                Id = id,
                Name = playlist.Name ?? existedPlaylist.Name,
                CreationDate = playlist.CreationDate == DateTime.MinValue ? existedPlaylist.CreationDate : playlist.CreationDate,
                Duration = playlist.Duration == TimeSpan.Zero ? existedPlaylist.Duration : playlist.Duration,
                UserId = playlist.UserId ?? existedPlaylist.UserId
            };
        }
    }
}
