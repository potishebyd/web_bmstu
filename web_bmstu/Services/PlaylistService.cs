using System;
using web_bmstu.Models;
using web_bmstu.ModelsBL;
using web_bmstu.Enums;
using web_bmstu.Interfaces;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Build.Tasks;
using web_bmstu.DTO;
using web_bmstu.Repository;
using System.Xml.Linq;
using System.Media;
using System.Numerics;

namespace web_bmstu.Services
{
    public interface IPlaylistService
    {
        PlaylistBL Add(PlaylistBL playlist);
        PlaylistBL Update(PlaylistBL playlist);
        PlaylistBL Delete(int id);

        PlaylistBL GetByID(int id);
        PlaylistBL GetByName(string name);
        PlaylistBL GetByUserId(int userId);
        SongPlaylistBL GetSongPlaylist(int songId, int playlistId);

        IEnumerable<PlaylistBL> GetAll(PlaylistSortState? sortState);

        void DeleteSongPlaylistsBySongId(int songId);
        void DeleteSongPlaylistsByPlaylistId(int playlistId);

        IEnumerable<SongBL> GetMySongsByPlaylistId(int playlistId);
        IEnumerable<SongBL> GetMySongsByUserLogin(string userLogin);

        PlaylistBL AddSongToMyPlaylist(int songId, int playlistId);
        PlaylistBL DeleteSongFromMyPlaylist(int songId, int playlistId);

        Task<PlaylistBL> AddAsync(PlaylistBL playlist);
        Task<PlaylistBL> UpdateAsync(PlaylistBL playlist);
        Task<PlaylistBL> DeleteAsync(int id);

        Task<PlaylistBL> GetByIDAsync(int id);
        Task<PlaylistBL> GetByNameAsync(string name);
        Task<PlaylistBL> GetByUserIdAsync(int userId);
        Task<SongPlaylistBL> GetSongPlaylistAsync(int songId, int playlistId);

        Task<IEnumerable<PlaylistBL>> GetAllAsync(PlaylistSortState? sortState);

        void DeleteSongPlaylistsBySongIdAsync(int songId);
        void DeleteSongPlaylistsByPlaylistIdAsync(int playlistId);

        Task<IEnumerable<SongBL>> GetMySongsByPlaylistIdAsync(int playlistId);
        Task<IEnumerable<SongBL>> GetMySongsByUserLoginAsync(string userLogin);

        Task<PlaylistBL> AddSongToMyPlaylistAsync(int songId, int playlistId);
        Task<PlaylistBL> DeleteSongFromMyPlaylistAsync(int songId, int playlistId);
    }

    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _appDBContext;
        private readonly IMapper _mapper;

        public PlaylistService(IPlaylistRepository playlistRepository, IUserRepository userRepository,
                            ApplicationDbContext appDBContext, IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _userRepository = userRepository;
            _appDBContext = appDBContext;
            _mapper = mapper;
        }

        private bool IsExist(PlaylistBL playlist)
        {
            return _playlistRepository.GetAll().FirstOrDefault(p =>
                    p.Name == playlist.Name) != null;
        }
        private bool IsNotExist(int id)
        {
            return _playlistRepository.GetByID(id) == null;
        }

        private bool PlaylistSongIsExist(int songId, int playlistId)
        {
            return _playlistRepository.GetAllSongPlaylist().FirstOrDefault(sp =>
                    sp.PlaylistId == songId &&
                    sp.SongId == playlistId) != null;
        }

        private bool PlaylistSongIsNotExist(int playlistId, int songId)
        {
            return _playlistRepository.GetSongPlaylist(playlistId, songId) == null;
        }

        public PlaylistBL Add(PlaylistBL playlist)
        {
            return _mapper.Map<PlaylistBL>(_playlistRepository.Add(_mapper.Map<Playlist>(playlist)));
        }
        public async Task<PlaylistBL> AddAsync(PlaylistBL playlist)
        {
            return _mapper.Map<PlaylistBL>(await _playlistRepository.AddAsync(_mapper.Map<Playlist>(playlist)));
        }

        public PlaylistBL Update(PlaylistBL playlist)
        {
            if (IsNotExist(playlist.Id))
                return null;

            return _mapper.Map<PlaylistBL>(_playlistRepository.Update(_mapper.Map<Playlist>(playlist)));
        }

        public async Task<PlaylistBL> UpdateAsync(PlaylistBL playlist)
        {
            if (IsNotExist(playlist.Id))
                return null;

            return _mapper.Map<PlaylistBL>(await _playlistRepository.UpdateAsync(_mapper.Map<Playlist>(playlist)));
        }

        public PlaylistBL Delete(int id)
        {
            return _mapper.Map<PlaylistBL>(_playlistRepository.Delete(id));
        }

        public async Task<PlaylistBL> DeleteAsync(int id)
        {
            return _mapper.Map<PlaylistBL>(await _playlistRepository.DeleteAsync(id));
        }

        public PlaylistBL GetByID(int id)
        {
            return _mapper.Map<PlaylistBL>(_playlistRepository.GetByID(id));
        }

        public async Task<PlaylistBL> GetByIDAsync(int id)
        {
            return _mapper.Map<PlaylistBL>(await _playlistRepository.GetByIDAsync(id));
        }

        public PlaylistBL GetByName(string name)
        {
            return _mapper.Map<PlaylistBL>(_playlistRepository.GetByName(name));
        }

        public async Task<PlaylistBL> GetByNameAsync(string name)
        {
            return _mapper.Map<PlaylistBL>(await _playlistRepository.GetByNameAsync(name));
        }
        public PlaylistBL GetByUserId(int userId)
        {
            return _mapper.Map<PlaylistBL>(_playlistRepository.GetByUserId(userId));
        }

        public async Task<PlaylistBL> GetByUserIdAsync(int userId)
        {
            return _mapper.Map<PlaylistBL>(await _playlistRepository.GetByUserIdAsync(userId));
        }

        public SongPlaylistBL GetSongPlaylist(int songId, int playlistId)
        {
            return _mapper.Map<SongPlaylistBL>(_playlistRepository.GetSongPlaylist(songId, playlistId));
        }

        public async Task<SongPlaylistBL> GetSongPlaylistAsync(int songId, int playlistId)
        {
            return _mapper.Map<SongPlaylistBL>(await _playlistRepository.GetSongPlaylistAsync(songId, playlistId));
        }

        public IEnumerable<PlaylistBL> GetAll(PlaylistSortState? sortState)
        {
            var playlists = _mapper.Map<IEnumerable<PlaylistBL>>(_playlistRepository.GetAll());

            if (sortState != null)
                playlists = SortPlaylistsByOption(playlists, sortState.Value);
            else
                playlists = SortPlaylistsByOption(playlists, PlaylistSortState.IdAsc);

            return playlists;
        }

        public async Task<IEnumerable<PlaylistBL>> GetAllAsync(PlaylistSortState? sortState)
        {
            var playlists = _mapper.Map<IEnumerable<PlaylistBL>>(await _playlistRepository.GetAllAsync());

            if (sortState != null)
                playlists = SortPlaylistsByOption(playlists, sortState.Value);
            else
                playlists = SortPlaylistsByOption(playlists, PlaylistSortState.IdAsc);

            return playlists;
        }

        private IEnumerable<PlaylistBL> SortPlaylistsByOption(IEnumerable<PlaylistBL> playlists, PlaylistSortState sortOrder)
        {
            IEnumerable<PlaylistBL> sortedPlaylists;

            if (sortOrder == PlaylistSortState.IdDesc)
            {
                sortedPlaylists = playlists.OrderByDescending(elem => elem.Id);
            }
            else if (sortOrder == PlaylistSortState.NameAsc)
            {
                sortedPlaylists = playlists.OrderBy(elem => elem.Name);
            }
            else if (sortOrder == PlaylistSortState.NameDesc)
            {
                sortedPlaylists = playlists.OrderByDescending(elem => elem.Name);
            }
            else if (sortOrder == PlaylistSortState.DurationAsc)
            {
                sortedPlaylists = playlists.OrderBy(elem => elem.Duration);
            }
            else if (sortOrder == PlaylistSortState.DurationDesc)
            {
                sortedPlaylists = playlists.OrderByDescending(elem => elem.Duration);
            }
            else if (sortOrder == PlaylistSortState.CreationDateAsc)
            {
                sortedPlaylists = playlists.OrderBy(elem => elem.CreationDate);
            }
            else if (sortOrder == PlaylistSortState.CreationDateDesc)
            {
                sortedPlaylists = playlists.OrderByDescending(elem => elem.CreationDate);
            }
            else
            {
                sortedPlaylists = playlists.OrderBy(elem => elem.Id);
            }
            return sortedPlaylists;
        }

        public void DeleteSongPlaylistsBySongId(int songId)
        {
            var songPlaylistList = _playlistRepository.GetAllSongPlaylist()
                .Where(elem => elem.SongId == songId);

            foreach (SongPlaylist elem in songPlaylistList)
            {
                DeleteSongFromMyPlaylist(songId, elem.PlaylistId);
            }
        }

        public async void DeleteSongPlaylistsBySongIdAsync(int songId)
        {
            var songPlaylistList = (await _playlistRepository.GetAllSongPlaylistAsync())
                .Where(elem => elem.SongId == songId);

            foreach (SongPlaylist elem in songPlaylistList)
            {
                DeleteSongFromMyPlaylist(songId, elem.PlaylistId);
            }
        }
        public void DeleteSongPlaylistsByPlaylistId(int playlistId)
        {
            var songPlaylistList = _playlistRepository.GetAllSongPlaylist()
                .Where(elem => elem.PlaylistId == playlistId);

            foreach (SongPlaylist elem in songPlaylistList)
            {
                DeleteSongFromMyPlaylist(elem.SongId, playlistId);
            }
        }

        public async void DeleteSongPlaylistsByPlaylistIdAsync(int playlistId)
        {
            var songPlaylistList =(await _playlistRepository.GetAllSongPlaylistAsync())
                .Where(elem => elem.PlaylistId == playlistId);

            foreach (SongPlaylist elem in songPlaylistList)
            {
                DeleteSongFromMyPlaylist(elem.SongId, playlistId);
            }
        }

        public IEnumerable<SongBL> GetMySongsByPlaylistId(int playlistId)
        {
            return _mapper.Map<IEnumerable<SongBL>>(_playlistRepository.GetSongsByPlaylistId(playlistId));
        }
        public async Task<IEnumerable<SongBL>> GetMySongsByPlaylistIdAsync(int playlistId)
        {
            return _mapper.Map<IEnumerable<SongBL>>(await _playlistRepository.GetSongsByPlaylistIdAsync(playlistId));
        }


        public IEnumerable<SongBL> GetMySongsByUserLogin(string userLogin)
        {
            User user = _userRepository.GetByLogin(userLogin);
            IEnumerable<Song> mySongs;

            if (user == null)
                mySongs = Enumerable.Empty<Song>();
            else
                mySongs = _playlistRepository.GetSongsByPlaylistId(user.Id);

            return _mapper.Map<IEnumerable<SongBL>>(mySongs);
        }

        // LASDASDASDASDASDASDASD
        public async Task<IEnumerable<SongBL>> GetMySongsByUserLoginAsync(string userLogin)
        {
            User user = _userRepository.GetByLogin(userLogin);
            IEnumerable<Song> mySongs;

            if (user == null)
                mySongs = Enumerable.Empty<Song>();
            else
                mySongs = await _playlistRepository.GetSongsByPlaylistIdAsync(user.Id);

            return _mapper.Map<IEnumerable<SongBL>>(mySongs);
        }

        public PlaylistBL AddSongToMyPlaylist(int songId, int playlistId)
        {
            if (PlaylistSongIsExist(songId, playlistId))
                throw new Exception("Данная песня уже добавлена в плейлист");

            _playlistRepository.AddSongPlaylist(songId, playlistId);

            return UpdateMyPlaylistDuration(playlistId);
        }
        public async Task<PlaylistBL> AddSongToMyPlaylistAsync(int songId, int playlistId)
        {
            if (PlaylistSongIsExist(songId, playlistId))
                throw new Exception("Данная песня уже добавлена в плейлист");

            _playlistRepository.AddSongPlaylistAsync(songId, playlistId);

            return UpdateMyPlaylistDuration(playlistId);
        }

        public PlaylistBL DeleteSongFromMyPlaylist(int songId, int playlistId)
        {
            if (PlaylistSongIsNotExist(songId, playlistId))
                throw new Exception("Такого песни в плейлисте нет");

            _playlistRepository.DeleteSongPlaylist(songId, playlistId);

            return UpdateMyPlaylistDuration(playlistId);
        }
        public async Task<PlaylistBL> DeleteSongFromMyPlaylistAsync(int songId, int playlistId)
        {
            if (PlaylistSongIsNotExist(songId, playlistId))
                throw new Exception("Такого песни в плейлисте нет");

            _playlistRepository.DeleteSongPlaylistAsync(songId, playlistId);

            return UpdateMyPlaylistDuration(playlistId);
        }

        private TimeSpan SumDuration(IEnumerable<Song> songs)
        {
            TimeSpan sumDuration = TimeSpan.FromSeconds(0);

            foreach (Song song in songs)
                sumDuration += song.Duration;

            return sumDuration;
        }

        private PlaylistBL UpdateMyPlaylistDuration(int playlistId)
        {
            Playlist playlist = _playlistRepository.GetByID(playlistId);
            IEnumerable<Song> songs = _playlistRepository.GetSongsByPlaylistId(playlistId);

            TimeSpan newDuration = TimeSpan.FromSeconds(0);

            newDuration = SumDuration(songs);

            playlist.Duration = newDuration;

            return _mapper.Map<PlaylistBL>(_playlistRepository.Update(playlist));
        }
    }
}
