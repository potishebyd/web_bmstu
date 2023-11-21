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


namespace web_bmstu.Services
{
    public interface IArtistService
    {
        ArtistBL Add(ArtistBL artist);
        Task AddAsync(ArtistBL artist);
        ArtistBL Delete(int id);
        Task<ArtistBL> DeleteAsync(int id);
        ArtistBL Update(ArtistBL artist);
        Task UpdateAsync(ArtistBL artist);
        ArtistBL GetByID(int id);
        Task<ArtistBL> GetByIDAsync(int id);
        ArtistBL GetByName(string name);
        Task<ArtistBL> GetByNameAsync(string name);
        IEnumerable<ArtistBL> GetByCountry(string country);
        Task<IEnumerable<ArtistBL>> GetByCountryAsync(string country);
        IEnumerable<ArtistBL> GetAll(ArtistFilterDto filter, ArtistSortState? sortState);
        Task<IEnumerable<ArtistBL>> GetAllAsync(ArtistFilterDto filter, ArtistSortState? sortState);

    }
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        private bool IsExist(ArtistBL artist)
        {
            return _artistRepository.GetAll().FirstOrDefault(elem => elem.Name == artist.Name) != null;
        }

        private bool IsNotExist(int id)
        {
            return _artistRepository.GetByID(id) == null;
        }

        public ArtistBL Add(ArtistBL artist)
        {
            if (IsExist(artist))
                throw new Exception("Артист с таким именем уже существует");

            return _mapper.Map<ArtistBL>(_artistRepository.Add(_mapper.Map<Artist>(artist)));

        }
        public async Task AddAsync(ArtistBL artist)
        {
            if (IsExist(artist))
                throw new Exception("Артист с таким именем уже существует");

            var artistDB = _mapper.Map<Artist>(artist);
            await _artistRepository.AddAsync(artistDB);
        }

        public ArtistBL Delete(int id)
        {
            return _mapper.Map<ArtistBL>(_artistRepository.Delete(id));
        }

        public async Task<ArtistBL> DeleteAsync(int id)
        {
            var deletedArtist = await _artistRepository.DeleteAsync(id);
            return _mapper.Map<ArtistBL>(deletedArtist);
        }
        public ArtistBL Update(ArtistBL artist)
        {
            return _mapper.Map<ArtistBL>(_artistRepository.Update(_mapper.Map<Artist>(artist)));
        }
        public async Task UpdateAsync(ArtistBL artist)
        {
            var ArtistDb = _mapper.Map<Artist>(artist);
            await _artistRepository.UpdateAsync(ArtistDb);
        }
        public ArtistBL GetByID(int id)
        {
            return _mapper.Map<ArtistBL>(_artistRepository.GetByID(id));
        }

        public async Task<ArtistBL> GetByIDAsync(int id)
        {
            var artist = await _artistRepository.GetByIDAsync(id);
            return _mapper.Map<ArtistBL>(artist);
        }
        public ArtistBL GetByName(string name)
        {
            return _mapper.Map<ArtistBL>(_artistRepository.GetByName(name));
        }
        public async Task<ArtistBL> GetByNameAsync(string name)
        {
            var artist =  await _artistRepository.GetByNameAsync(name);
            return _mapper.Map<ArtistBL>(artist);
        }
        public IEnumerable<ArtistBL> GetByCountry(string country)
        {
            return _mapper.Map<IEnumerable<ArtistBL>>(_artistRepository.GetByCountry(country));
        }

        public async Task<IEnumerable<ArtistBL>> GetByCountryAsync(string country)
        {
            var artist = _artistRepository.GetByCountryAsync(country);
            return _mapper.Map<IEnumerable<ArtistBL>>(artist);
        }
        public IEnumerable<ArtistBL> GetAll(ArtistFilterDto filter, ArtistSortState? sortState)
        {
            var artists = FilterArtists(filter);

            if (sortState != null)
                artists = SortClubsByOption(artists, sortState.Value);
            else
                artists = SortClubsByOption(artists, ArtistSortState.IdAsc);

            return artists;
        }

        public async  Task<IEnumerable<ArtistBL>> GetAllAsync(ArtistFilterDto filter, ArtistSortState? sortState)
        {
            var artists = await FilterArtistsAsync(filter);

            if (sortState != null)
                artists = SortClubsByOption(artists, sortState.Value);
            else
                artists = SortClubsByOption(artists, ArtistSortState.IdAsc);

            return artists;
        }

        private IEnumerable<ArtistBL> FilterArtists(ArtistFilterDto filter)
        {
            var filteredArtists = _artistRepository.GetAll();

            if (!String.IsNullOrEmpty(filter.Country))
                filteredArtists = filteredArtists.Where(elem => elem.Country.Contains(filter.Country));

            if (!String.IsNullOrEmpty(filter.Name))
                filteredArtists = filteredArtists.Where(elem => elem.Name.Contains(filter.Name));

            return _mapper.Map<IEnumerable<ArtistBL>>(filteredArtists);
        }

        private async Task<IEnumerable<ArtistBL>> FilterArtistsAsync(ArtistFilterDto filter)
        {
            var filteredArtists = await _artistRepository.GetAllAsync();

            if (!String.IsNullOrEmpty(filter.Country))
                filteredArtists = filteredArtists.Where(elem => elem.Country.Contains(filter.Country));

            if (!String.IsNullOrEmpty(filter.Name))
                filteredArtists = filteredArtists.Where(elem => elem.Name.Contains(filter.Name));

            return _mapper.Map<IEnumerable<ArtistBL>>(filteredArtists);
        }

        private IEnumerable<ArtistBL> SortClubsByOption(IEnumerable<ArtistBL> artist, ArtistSortState sortOrder)
        {
            IEnumerable<ArtistBL> sortedArtists;

            if (sortOrder == ArtistSortState.IdDesc)
            {
                sortedArtists = artist.OrderByDescending(elem => elem.Id);
            }
            else if (sortOrder == ArtistSortState.CountryAsc)
            {
                sortedArtists = artist.OrderBy(elem => elem.Country);
            }
            else if (sortOrder == ArtistSortState.CountryDesc)
            {
                sortedArtists = artist.OrderByDescending(elem => elem.Country);
            }
            else if (sortOrder == ArtistSortState.NameAsc)
            {
                sortedArtists = artist.OrderBy(elem => elem.Name);
            }
            else if (sortOrder == ArtistSortState.NameDesc)
            {
                sortedArtists = artist.OrderByDescending(elem => elem.Name);
            }
            else
            {
                sortedArtists = artist.OrderBy(elem => elem.Id);
            }

            return sortedArtists;
        }

    }
}
