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
        ArtistBL Delete(int id);
        ArtistBL Update(ArtistBL artist);
        ArtistBL GetByID(int id);
        ArtistBL GetByName(string name);
        IEnumerable<ArtistBL> GetByCountry(string country);
        IEnumerable<ArtistBL> GetAll(ArtistFilterDto filter, ArtistSortState? sortState);

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

        public ArtistBL Delete(int id)
        {
            return _mapper.Map<ArtistBL>(_artistRepository.Delete(id));
        }

        public ArtistBL Update(ArtistBL club)
        {
            return _mapper.Map<ArtistBL>(_artistRepository.Update(_mapper.Map<Artist>(club)));
        }

        public ArtistBL GetByID(int id)
        {
            return _mapper.Map<ArtistBL>(_artistRepository.GetByID(id));
        }
        public ArtistBL GetByName(string name)
        {
            return _mapper.Map<ArtistBL>(_artistRepository.GetByName(name));
        }
        public IEnumerable<ArtistBL> GetByCountry(string country)
        {
            return _mapper.Map<IEnumerable<ArtistBL>>(_artistRepository.GetByCountry(country));
        }
        public IEnumerable<ArtistBL> GetAll(ArtistFilterDto filter, ArtistSortState? sortState)
        {
            var clubs = FilterArtists(filter);

            if (sortState != null)
                clubs = SortClubsByOption(clubs, sortState.Value);
            else
                clubs = SortClubsByOption(clubs, ArtistSortState.IdAsc);

            return clubs;
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
