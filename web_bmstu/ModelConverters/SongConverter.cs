using web_bmstu.DTO;
using web_bmstu.ModelsBL;
using web_bmstu.Services;

namespace web_bmstu.ModelsConverters
{
    public class SongConverters
    {
        private readonly ISongService songService;

        public SongConverters(ISongService songService)
        {
            this.songService = songService;
        }

        public SongBL convertPatch(int id, SongBaseDto song)
        {
            var existedSong = songService.GetByID(id);

            return new SongBL
            {
                Id = id,
                Title = song.Title ?? existedSong.Title,
                AlbumTitle = song.AlbumTitle ?? existedSong.AlbumTitle,
                Genre = song.Genre ?? existedSong.Genre,
                Duration = song.Duration == TimeSpan.Zero ? existedSong.Duration : song.Duration,
                RecordingStudioId = song.RecordingStudioId ?? existedSong.RecordingStudioId,
                ArtistId = song.ArtistId ?? existedSong.ArtistId
            };
        }
    }
}