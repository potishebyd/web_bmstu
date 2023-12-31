﻿using System;
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
using Microsoft.IdentityModel.Tokens;

namespace web_bmstu.Services
{
    public interface IRecordingStudioService
    {
        RecordingStudioBL Add(RecordingStudioBL recordingStudio);
        RecordingStudioBL Delete(int id);
        RecordingStudioBL Update(RecordingStudioBL recordingStudio);

        IEnumerable<RecordingStudioBL> GetAll(RecordingStudioFilterDto filter, RecordingStudioSortState? sortState);
        RecordingStudioBL GetByID(int id);
        RecordingStudioBL GetByName(string name);
        IEnumerable<RecordingStudioBL> GetByCountry(string country);
        IEnumerable<RecordingStudioBL> GetByYearFounded(int yearFounded);


        Task<RecordingStudioBL> AddAsync(RecordingStudioBL recordingStudio);
        Task<RecordingStudioBL> DeleteAsync(int id);
        Task<RecordingStudioBL> UpdateAsync(RecordingStudioBL recordingStudio);

        Task<IEnumerable<RecordingStudioBL>> GetAllAsync(RecordingStudioFilterDto filter, RecordingStudioSortState? sortState);
        Task<RecordingStudioBL> GetByIDAsync(int id);
        Task<RecordingStudioBL> GetByNameAsync(string name);
        Task<IEnumerable<RecordingStudioBL>> GetByCountryAsync(string country);
        Task<IEnumerable<RecordingStudioBL>> GetByYearFoundedAsync(int yearFounded);
    }

    public class RecordingStudioService : IRecordingStudioService
    {
        private readonly IRecordingStudioRepository _recordingStudioRepository;
        private readonly IMapper _mapper;

        public RecordingStudioService(IRecordingStudioRepository recordingStudioRepository, IMapper mapper)
        {
            _recordingStudioRepository = recordingStudioRepository;
            _mapper = mapper;
        }

        private bool IsExist(RecordingStudioBL recordingStudio)
        {
            return _recordingStudioRepository.GetAll().FirstOrDefault(elem => elem.Name == recordingStudio.Name) != null;
        }

        private bool IsNotExist(int id)
        {
            return _recordingStudioRepository.GetByID(id) == null;
        }

        public RecordingStudioBL Add(RecordingStudioBL recordingStudio)
        {
            if (IsExist(recordingStudio))
                throw new Exception("Студия звукозаписи с таким именем уже существует");

            return _mapper.Map<RecordingStudioBL>(_recordingStudioRepository.Add(_mapper.Map<RecordingStudio>(recordingStudio)));
        }

        public async Task<RecordingStudioBL> AddAsync(RecordingStudioBL recordingStudio)
        {
            if (IsExist(recordingStudio))
                throw new Exception("Студия звукозаписи с таким именем уже существует");

            return _mapper.Map<RecordingStudioBL>(await _recordingStudioRepository.AddAsync(_mapper.Map<RecordingStudio>(recordingStudio)));

        }

        public RecordingStudioBL Delete(int id)
        {
            if (IsNotExist(id))
                throw new Exception("Такой студии звукозаписи не существует");

            return _mapper.Map<RecordingStudioBL>(_recordingStudioRepository.Delete(id));
        }

        public async Task<RecordingStudioBL> DeleteAsync(int id)
        {
            if (IsNotExist(id))
                throw new Exception("Такой студии звукозаписи не существует");

            return _mapper.Map<RecordingStudioBL>(await _recordingStudioRepository.DeleteAsync(id));
        }
        public IEnumerable<RecordingStudioBL> GetAll(RecordingStudioFilterDto filter, RecordingStudioSortState? sortState)
        {
            var recordingStudios = FilterRecordingStudios(filter);

            if (sortState != null)
                recordingStudios = SortRecordingStudiosByOption(recordingStudios, sortState.Value);
            else
                recordingStudios = SortRecordingStudiosByOption(recordingStudios, RecordingStudioSortState.IdAsc);

            return recordingStudios;
        }

        public async Task<IEnumerable<RecordingStudioBL>> GetAllAsync(RecordingStudioFilterDto filter, RecordingStudioSortState? sortState)
        {
            var recordingStudios = await FilterRecordingStudiosAsync(filter);

            if (sortState != null)
                recordingStudios = SortRecordingStudiosByOption(recordingStudios, sortState.Value);
            else
                recordingStudios = SortRecordingStudiosByOption(recordingStudios, RecordingStudioSortState.IdAsc);

            return recordingStudios;
        }

        public RecordingStudioBL GetByID(int id)
        {
            return _mapper.Map<RecordingStudioBL>(_recordingStudioRepository.GetByID(id));
        }

        public async Task<RecordingStudioBL> GetByIDAsync(int id)
        {
            return _mapper.Map<RecordingStudioBL>(await _recordingStudioRepository.GetByIDAsync(id));
        }

        public RecordingStudioBL GetByName(string name)
        {
            return _mapper.Map<RecordingStudioBL>(_recordingStudioRepository.GetByName(name));
        }

        public async Task<RecordingStudioBL> GetByNameAsync(string name)
        {
            return _mapper.Map<RecordingStudioBL>(await _recordingStudioRepository.GetByNameAsync(name));
        }


        public IEnumerable<RecordingStudioBL> GetByCountry(string country)
        {
            return _mapper.Map<IEnumerable<RecordingStudioBL>>(_recordingStudioRepository.GetByCountry(country));
        }
        public async Task<IEnumerable<RecordingStudioBL>> GetByCountryAsync(string country)
        {
            return _mapper.Map<IEnumerable<RecordingStudioBL>>(await _recordingStudioRepository.GetByCountryAsync(country));
        }

        public IEnumerable<RecordingStudioBL> GetByYearFounded(int yearFounded)
        {
            return _mapper.Map<IEnumerable<RecordingStudioBL>>(_recordingStudioRepository.GetByYearFounded(yearFounded));
        }

        public async Task<IEnumerable<RecordingStudioBL>> GetByYearFoundedAsync(int yearFounded)
        {
            return _mapper.Map<IEnumerable<RecordingStudioBL>>(await _recordingStudioRepository.GetByYearFoundedAsync(yearFounded));
        }

        public RecordingStudioBL Update(RecordingStudioBL recordingStudio)
        {
            if (IsNotExist(recordingStudio.Id))
                return null;

            if (IsExist(recordingStudio))
                throw new Exception("Студия с таким названием уже существует");

            return _mapper.Map<RecordingStudioBL>(_recordingStudioRepository.Update(_mapper.Map<RecordingStudio>(recordingStudio)));
        }
        public async Task<RecordingStudioBL> UpdateAsync(RecordingStudioBL recordingStudio)
        {
            if (IsNotExist(recordingStudio.Id))
                return null;

            if (IsExist(recordingStudio))
                throw new Exception("Студия с таким названием уже существует");

            return _mapper.Map<RecordingStudioBL>(await _recordingStudioRepository.UpdateAsync(_mapper.Map<RecordingStudio>(recordingStudio)));
        }

        private IEnumerable<RecordingStudioBL> FilterRecordingStudios(RecordingStudioFilterDto filter)
        {
            var filteredRecordingStudios = _recordingStudioRepository.GetAll();

            if (filter.YearFounded != null)
                filteredRecordingStudios = filteredRecordingStudios.Where(elem => elem.YearFounded == filter.YearFounded);

            if (!String.IsNullOrEmpty(filter.Country))
                filteredRecordingStudios = filteredRecordingStudios.Where(elem => elem.Country.Contains(filter.Country));

            if (!String.IsNullOrEmpty(filter.Name))
                filteredRecordingStudios = filteredRecordingStudios.Where(elem => elem.Name.Contains(filter.Name));

            return _mapper.Map<IEnumerable<RecordingStudioBL>>(filteredRecordingStudios);
        }


        private async Task<IEnumerable<RecordingStudioBL>> FilterRecordingStudiosAsync(RecordingStudioFilterDto filter)
        {
            var filteredRecordingStudios = await _recordingStudioRepository.GetAllAsync();

            if (filter.YearFounded != null)
                filteredRecordingStudios = filteredRecordingStudios.Where(elem => elem.YearFounded == filter.YearFounded);

            if (!String.IsNullOrEmpty(filter.Country))
                filteredRecordingStudios = filteredRecordingStudios.Where(elem => elem.Country.Contains(filter.Country));

            if (!String.IsNullOrEmpty(filter.Name))
                filteredRecordingStudios = filteredRecordingStudios.Where(elem => elem.Name.Contains(filter.Name));

            return _mapper.Map<IEnumerable<RecordingStudioBL>>(filteredRecordingStudios);
        }
        private IEnumerable<RecordingStudioBL> SortRecordingStudiosByOption(IEnumerable<RecordingStudioBL> recordingStudios, RecordingStudioSortState sortOrder)
        {
            IEnumerable<RecordingStudioBL> sortedRecordingStudios;

            if (sortOrder == RecordingStudioSortState.IdDesc)
            {
                sortedRecordingStudios = recordingStudios.OrderByDescending(elem => elem.Id);
            }
            else if (sortOrder == RecordingStudioSortState.YearFoundedAsc)
            {
                sortedRecordingStudios = recordingStudios.OrderBy(elem => elem.YearFounded);
            }
            else if (sortOrder == RecordingStudioSortState.YearFoundedDesc)
            {
                sortedRecordingStudios = recordingStudios.OrderByDescending(elem => elem.YearFounded);
            }
            else if (sortOrder == RecordingStudioSortState.CountryAsc)
            {
                sortedRecordingStudios = recordingStudios.OrderBy(elem => elem.Country);
            }
            else if (sortOrder == RecordingStudioSortState.CountryDesc)
            {
                sortedRecordingStudios = recordingStudios.OrderByDescending(elem => elem.Country);
            }
            else if (sortOrder == RecordingStudioSortState.NameAsc)
            {
                sortedRecordingStudios = recordingStudios.OrderBy(elem => elem.Name);
            }
            else if (sortOrder == RecordingStudioSortState.NameDesc)
            {
                sortedRecordingStudios = recordingStudios.OrderByDescending(elem => elem.Name);
            }
            else
            {
                sortedRecordingStudios = recordingStudios.OrderBy(elem => elem.Id);
            }

            return sortedRecordingStudios;
        }



    }
}
