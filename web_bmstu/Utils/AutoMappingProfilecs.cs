using AutoMapper;
using web_bmstu.DTO;
using web_bmstu.Models;
using web_bmstu.ModelsBL;

namespace web_bmstu.Utils
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Song, SongBL>().ReverseMap();
            CreateMap<Artist, ArtistBL>().ReverseMap();
            CreateMap<RecordingStudio, RecordingStudioBL>().ReverseMap();
            CreateMap<Playlist, PlaylistBL>().ReverseMap();
            CreateMap<SongPlaylist, SongPlaylistBL>().ReverseMap();
            CreateMap<User, UserBL>().ReverseMap();

            CreateMap<ArtistBaseDto, ArtistBL>().ReverseMap();
            CreateMap<ArtistDto, ArtistBL>().ReverseMap();
            CreateMap<RecordingStudioBaseDto, RecordingStudioBL>().ReverseMap();
            CreateMap<RecordingStudioDto, RecordingStudioBL>().ReverseMap();
            CreateMap<SongBaseDto, SongBL>().ReverseMap();
            CreateMap<SongDto, SongBL>().ReverseMap();
            CreateMap<PlaylistBaseDto, PlaylistBL>().ReverseMap();
            CreateMap<PlaylistDto, PlaylistBL>().ReverseMap();
            CreateMap<SongPlaylistBaseDto, SongPlaylistBL>().ReverseMap();
            CreateMap<SongPlaylistDto, SongPlaylistBL>().ReverseMap();
            CreateMap<UserBaseDto, UserBL>().ReverseMap();
            CreateMap<UserDto, UserBL>().ReverseMap();
            CreateMap<UserPasswordDto, UserBL>().ReverseMap();
            CreateMap<UserIdPasswordDto, UserBL>().ReverseMap();
        }
    }
}