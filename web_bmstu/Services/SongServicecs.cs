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
using System.Numerics;


namespace web_bmstu.Services
{
    public interface ISongService
    {
        SongBL Add(SongBL song);
        SongBL Delete(int id);
        SongBL Update(SongBL song);

        IEnumerable<SongBL> GetAll(SongFilterDto filter, SongSortState? sortState);
        SongBL GetByID(int id);
        IEnumerable<SongBL> GetByTitle(string title);
        IEnumerable<SongBL> GetByAlbumTitle(string albumTitle);
        IEnumerable<SongBL> GetByGenre(string genre);
        IEnumerable<SongBL> GetByRecordingStudioName(string recordingStudioName);
        IEnumerable<SongBL> GetByArtistName(string artistName);
        IEnumerable<SongBL> GetSongsByPlaylistId(int playlistId, SongFilterDto filter, SongSortState? sortState);


        Task<SongBL> AddAsync(SongBL song);
        Task<SongBL> DeleteAsync(int id);
        Task<SongBL> UpdateAsync(SongBL song);

        Task<IEnumerable<SongBL>> GetAllAsync(SongFilterDto filter, SongSortState? sortState);
        Task<SongBL> GetByIDAsync(int id);
        Task<IEnumerable<SongBL>> GetByTitleAsync(string title);
        Task<IEnumerable<SongBL>> GetByAlbumTitleAsync(string albumTitle);
        Task<IEnumerable<SongBL>> GetByGenreAsync(string genre);
        Task<IEnumerable<SongBL>> GetByRecordingStudioNameAsync(string recordingStudioName);
        Task<IEnumerable<SongBL>> GetByArtistNameAsync(string artistName);
        Task<IEnumerable<SongBL>> GetSongsByPlaylistIdAsync(int playlistId, SongFilterDto filter, SongSortState? sortState);
    }

    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IRecordingStudioRepository _recordingStudioRepository;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMapper _mapper;

        public SongService(ISongRepository songRepository, IArtistRepository artistRepository, IRecordingStudioRepository recordingStudioRepository, IPlaylistRepository playlistRepository, IMapper mapper)
        {
            _songRepository = songRepository;
            _artistRepository = artistRepository;
            _recordingStudioRepository = recordingStudioRepository;
            _playlistRepository = playlistRepository;
            _mapper = mapper;
        }

        private bool IsExist(SongBL song)
        {
            return _songRepository.GetAll().FirstOrDefault(elem =>
                   elem.Title == song.Title &&
                   elem.AlbumTitle == song.AlbumTitle &&
                   elem.Genre == song.Genre &&
                   elem.ArtistId == song.ArtistId &&
                   elem.RecordingStudioId == song.RecordingStudioId) != null;
        }

        private bool IsNotExist(int id)
        {
            return _songRepository.GetByID(id) == null;
        }

        public SongBL Add(SongBL song)
        {
            if (IsExist(song))
                throw new Exception("Такая песня уже существует");

            return _mapper.Map<SongBL>(_songRepository.Add(_mapper.Map<Song>(song)));
        }

        public async Task<SongBL> AddAsync(SongBL song)
        {
            if (IsExist(song))
                throw new Exception("Такая песня уже существует");

            return _mapper.Map<SongBL>(await _songRepository.AddAsync(_mapper.Map<Song>(song)));
        }

        public SongBL Delete(int id)
        {
            return _mapper.Map<SongBL>(_songRepository.Delete(id));
        }


        public async Task<SongBL> DeleteAsync(int id)
        {
            return _mapper.Map<SongBL>(await _songRepository.DeleteAsync(id));
        }
        public SongBL Update(SongBL song)
        {
            if (IsNotExist(song.Id))
                return null;

            if (IsExist(song))
                throw new Exception("Такой песня уже существует");

            return _mapper.Map<SongBL>(_songRepository.Update(_mapper.Map<Song>(song)));
        }

        public async Task<SongBL> UpdateAsync(SongBL song)
        {
            if (IsNotExist(song.Id))
                return null;

            if (IsExist(song))
                throw new Exception("Такой песня уже существует");

            return _mapper.Map<SongBL>(await _songRepository.UpdateAsync(_mapper.Map<Song>(song)));
        }

        public SongBL GetByID(int id)
        {
            return _mapper.Map<SongBL>(_songRepository.GetByID(id));
        }
        public async Task<SongBL> GetByIDAsync(int id)
        {
            return _mapper.Map<SongBL>(await _songRepository.GetByIDAsync(id));
        }
        public IEnumerable<SongBL> GetByAlbumTitle(string albumTitle)
        {
            return _mapper.Map<IEnumerable<SongBL>>(_songRepository.GetByAlbumTitle(albumTitle));
        }
        public async Task<IEnumerable<SongBL>> GetByAlbumTitleAsync(string albumTitle)
        {
            return _mapper.Map<IEnumerable<SongBL>>(await _songRepository.GetByAlbumTitleAsync(albumTitle));
        }
        public IEnumerable<SongBL> GetByGenre(string genre)
        {
            return _mapper.Map<IEnumerable<SongBL>>(_songRepository.GetByGenre(genre));
        }

        public async  Task<IEnumerable<SongBL>> GetByGenreAsync(string genre)
        {
            return _mapper.Map<IEnumerable<SongBL>>(await _songRepository.GetByGenreAsync(genre));
        }
        public IEnumerable<SongBL> GetByTitle(string title)
        {
            return _mapper.Map<IEnumerable<SongBL>>(_songRepository.GetByTitle(title));
        }

        public async Task<IEnumerable<SongBL>> GetByTitleAsync(string title)
        {
            return _mapper.Map<IEnumerable<SongBL>>(await _songRepository.GetByTitleAsync(title));
        }
        public IEnumerable<SongBL> GetByArtistName(string artistName)
        {
            Artist artist = _artistRepository.GetByName(artistName);

            if (artist == null)
                return Enumerable.Empty<SongBL>();
            else
                return _mapper.Map<IEnumerable<SongBL>>(_songRepository.GetAll().Where(elem => elem.ArtistId == artist.Id));

        }

        public async Task<IEnumerable<SongBL>> GetByArtistNameAsync(string artistName)
        {
            Artist artist = await _artistRepository.GetByNameAsync(artistName);

            if (artist == null)
                return Enumerable.Empty<SongBL>();
            else
                return _mapper.Map<IEnumerable<SongBL>>((await _songRepository.GetAllAsync()).Where(elem => elem.ArtistId == artist.Id));

        }
        public IEnumerable<SongBL> GetByRecordingStudioName(string recordingStudioName)
        {
            RecordingStudio recordingStudio = _recordingStudioRepository.GetByName(recordingStudioName);

            if (recordingStudio == null)
                return Enumerable.Empty<SongBL>();
            else
                return _mapper.Map<IEnumerable<SongBL>>(_songRepository.GetAll().Where(elem => elem.RecordingStudioId == recordingStudio.Id));

        }

        public async Task<IEnumerable<SongBL>> GetByRecordingStudioNameAsync(string recordingStudioName)
        {
            RecordingStudio recordingStudio = await _recordingStudioRepository.GetByNameAsync(recordingStudioName);

            if (recordingStudio == null)
                return Enumerable.Empty<SongBL>();
            else
                return _mapper.Map<IEnumerable<SongBL>>((await _songRepository.GetAllAsync()).Where(elem => elem.RecordingStudioId == recordingStudio.Id));

        }
        public IEnumerable<SongBL> GetAll(SongFilterDto filter, SongSortState? sortState)
        {
            var songs = _mapper.Map<IEnumerable<SongBL>>(_songRepository.GetAll());

            songs = FilterSongs(songs, filter);

            if (sortState != null)
                songs = SortSongsByOption(songs, sortState.Value);
            else
                songs = SortSongsByOption(songs, SongSortState.IdAsc);

            return songs;
        }

        public async Task<IEnumerable<SongBL>> GetAllAsync(SongFilterDto filter, SongSortState? sortState)
        {
            var songs = _mapper.Map<IEnumerable<SongBL>>(await _songRepository.GetAllAsync());

            songs = FilterSongs(songs, filter);

            if (sortState != null)
                songs = SortSongsByOption(songs, sortState.Value);
            else
                songs = SortSongsByOption(songs, SongSortState.IdAsc);

            return songs;
        }

        public IEnumerable<SongBL> GetSongsByPlaylistId(int playlistId, SongFilterDto filter, SongSortState? sortState)
        {
            var songs = _mapper.Map<IEnumerable<SongBL>>(_playlistRepository.GetSongsByPlaylistId(playlistId));

            songs = FilterSongs(songs, filter);

            if (sortState != null)
                songs = SortSongsByOption(songs, sortState.Value);

            return songs;
        }
        public async Task<IEnumerable<SongBL>> GetSongsByPlaylistIdAsync(int playlistId, SongFilterDto filter, SongSortState? sortState)
        {
            var songs = _mapper.Map<IEnumerable<SongBL>>(await _playlistRepository.GetSongsByPlaylistIdAsync(playlistId));

            songs = await FilterSongsAsync(songs, filter);

            if (sortState != null)
                songs = SortSongsByOption(songs, sortState.Value);

            return songs;
        }

        private IEnumerable<SongBL> FilterSongs(IEnumerable<SongBL> songs, SongFilterDto filter)
        {
            var filteredSongs = songs;

            if (!String.IsNullOrEmpty(filter.Title))
                filteredSongs = filteredSongs.Where(elem => elem.Title.Contains(filter.Title));

            if (!String.IsNullOrEmpty(filter.AlbumTitle))
                filteredSongs = filteredSongs.Where(elem => elem.AlbumTitle.Contains(filter.AlbumTitle));

            if (!String.IsNullOrEmpty(filter.Genre))
                filteredSongs = filteredSongs.Where(elem => elem.Genre.Contains(filter.Genre));

            if (!String.IsNullOrEmpty(filter.ArtistName))
            {
                Artist artist = _artistRepository.GetByName(filter.ArtistName);

                if (artist == null)
                    filteredSongs = Enumerable.Empty<SongBL>();
                else
                    filteredSongs = filteredSongs.Where(elem => elem.ArtistId == artist.Id);
            }

            if (!String.IsNullOrEmpty(filter.RecordingStudioName))
            {
                RecordingStudio recordingStudio = _recordingStudioRepository.GetByName(filter.RecordingStudioName);

                if (recordingStudio == null)
                    filteredSongs = Enumerable.Empty<SongBL>();
                else
                    filteredSongs = filteredSongs.Where(elem => elem.RecordingStudioId == recordingStudio.Id);
            }
            return filteredSongs;
        }

        private async Task<IEnumerable<SongBL>> FilterSongsAsync(IEnumerable<SongBL> songs, SongFilterDto filter)
        {
            var filteredSongs = songs;

            if (!String.IsNullOrEmpty(filter.Title))
                filteredSongs = filteredSongs.Where(elem => elem.Title.Contains(filter.Title));

            if (!String.IsNullOrEmpty(filter.AlbumTitle))
                filteredSongs = filteredSongs.Where(elem => elem.AlbumTitle.Contains(filter.AlbumTitle));

            if (!String.IsNullOrEmpty(filter.Genre))
                filteredSongs = filteredSongs.Where(elem => elem.Genre.Contains(filter.Genre));

            if (!String.IsNullOrEmpty(filter.ArtistName))
            {
                Artist artist = await _artistRepository.GetByNameAsync(filter.ArtistName);

                if (artist == null)
                    filteredSongs = Enumerable.Empty<SongBL>();
                else
                    filteredSongs = filteredSongs.Where(elem => elem.ArtistId == artist.Id);
            }

            if (!String.IsNullOrEmpty(filter.RecordingStudioName))
            {
                RecordingStudio recordingStudio = await _recordingStudioRepository.GetByNameAsync(filter.RecordingStudioName);

                if (recordingStudio == null)
                    filteredSongs = Enumerable.Empty<SongBL>();
                else
                    filteredSongs = filteredSongs.Where(elem => elem.RecordingStudioId == recordingStudio.Id);
            }
            return filteredSongs;
        }


        private IEnumerable<SongBL> SortSongsByOption(IEnumerable<SongBL> players, SongSortState sortOrder)
        {
            IEnumerable<SongBL> sortedSongs;

            if (sortOrder == SongSortState.IdDesc)
            {
                sortedSongs = players.OrderByDescending(elem => elem.Id);
            }
            else if (sortOrder == SongSortState.TitleAsc)
            {
                sortedSongs = players.OrderBy(elem => elem.Title);
            }
            else if (sortOrder == SongSortState.TitleDesc)
            {
                sortedSongs = players.OrderByDescending(elem => elem.Title);
            }
            else if (sortOrder == SongSortState.GenreAsc)
            {
                sortedSongs = players.OrderBy(elem => elem.Genre);
            }
            else if (sortOrder == SongSortState.GenreDesc)
            {
                sortedSongs = players.OrderByDescending(elem => elem.Genre);
            }
            else if (sortOrder == SongSortState.AlbumAsc)
            {
                sortedSongs = players.OrderBy(elem => elem.AlbumTitle);
            }
            else if (sortOrder == SongSortState.AlbumDesc)
            {
                sortedSongs = players.OrderByDescending(elem => elem.AlbumTitle);
            }
            else if (sortOrder == SongSortState.DurationAsc)
            {
                sortedSongs = players.OrderBy(elem => elem.Duration);
            }
            else if (sortOrder == SongSortState.DurationDesc)
            {
                sortedSongs = players.OrderByDescending(elem => elem.Duration);
            }
            else if (sortOrder == SongSortState.ArtistNameAsc)
            {
                sortedSongs = players.OrderBy(elem => _artistRepository.GetByID(elem.ArtistId).Name);
            }
            else if (sortOrder == SongSortState.ArtistNameDesc)
            {
                sortedSongs = players.OrderByDescending(elem => _artistRepository.GetByID(elem.ArtistId).Name);
            }
            else if (sortOrder == SongSortState.RecordingStudioNameAsc)
            {
                sortedSongs = players.OrderBy(elem => _recordingStudioRepository.GetByID(elem.RecordingStudioId).Name);
            }
            else if (sortOrder == SongSortState.RecordingStudioNameDesc)
            {
                sortedSongs = players.OrderByDescending(elem => _recordingStudioRepository.GetByID(elem.RecordingStudioId).Name);
            }
            else
            {
                sortedSongs = players.OrderBy(elem => elem.Id);
            }

            return sortedSongs;
        }



    }
}
