using web_bmstu.DTO;
using web_bmstu.ModelsBL;
using web_bmstu.Services;

namespace web_bmstu.ModelsConverters
{
    public class ArtistConverters
    {
        private readonly IArtistService artistService;

        public ArtistConverters(IArtistService artistService)
        {
            this.artistService = artistService;
        }

        public ArtistBL convertPatch(int id, ArtistBaseDto artist)
        {
            var existedArtist = artistService.GetByID(id);

            return new ArtistBL
            {
                Id = id,
                Name = artist.Name ?? existedArtist.Name,
                Country = artist.Country ?? existedArtist.Country
            };
        }
    }
}