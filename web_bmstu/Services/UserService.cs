using System;
using web_bmstu.Models;
using web_bmstu.ModelsBL;
using web_bmstu.Enums;
using web_bmstu.Interfaces;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using web_bmstu.DTO;
using web_bmstu.Repository;

namespace web_bmstu.Services
{
    public interface IUserService
    {
        UserBL Add(UserBL user);
        UserBL Delete(int id);
        UserBL Update(UserBL user);

        UserBL GetByID(int id);
        UserBL GetByLogin(string login);
        UserBL Login(LoginDto loginDto);

        IEnumerable<UserBL> GetByPermission(string permission);
        IEnumerable<UserBL> GetAll(UserSortState? sortState);

        Task<UserBL> AddAsync(UserBL user);
        Task<UserBL> DeleteAsync(int id);
        Task<UserBL> UpdateAsync(UserBL user);

        Task<UserBL> GetByIDAsync(int id);
        Task<UserBL> GetByLoginAsync(string login);
        Task<UserBL> LoginAsync(LoginDto loginDto);

        Task<IEnumerable<UserBL>> GetByPermissionAsync(string permission);
        Task<IEnumerable<UserBL>> GetAllAsync(UserSortState? sortState);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public UserBL Add(UserBL user)
        {
            if (IsExist(user))
                throw new Exception("Пользователь с таким логином уже существует");

            return _mapper.Map<UserBL>(_userRepository.Add(_mapper.Map<User>(user)));

        }

        public async Task<UserBL> AddAsync(UserBL user)
        {
            if (IsExist(user))
                throw new Exception("Пользователь с таким логином уже существует");

            return _mapper.Map<UserBL>(await _userRepository.AddAsync(_mapper.Map<User>(user)));

        }

        public UserBL Delete(int id)
        {
            return _mapper.Map<UserBL>(_userRepository.Delete(id));
        }

        public async Task<UserBL> DeleteAsync(int id)
        {
            return _mapper.Map<UserBL>(await _userRepository.DeleteAsync(id));
        }
        public UserBL Update(UserBL user)
        {
            if (IsNotExist(user.Id))
                return null;

            // if (IsExist(user))
            //     throw new Exception("Пользователь с таким логином уже существует");

            return _mapper.Map<UserBL>(_userRepository.Update(_mapper.Map<User>(user)));
        }

        public async Task<UserBL> UpdateAsync(UserBL user)
        {
            if (IsNotExist(user.Id))
                return null;

            // if (IsExist(user))
            //     throw new Exception("Пользователь с таким логином уже существует");

            return _mapper.Map<UserBL>(await _userRepository.UpdateAsync(_mapper.Map<User>(user)));
        }


        public UserBL GetByID(int id)
        {
            return _mapper.Map<UserBL>(_userRepository.GetByID(id));
        }

        public async Task<UserBL> GetByIDAsync(int id)
        {
            return _mapper.Map<UserBL>(await _userRepository.GetByIDAsync(id));
        }
        public UserBL GetByLogin(string login)
        {
            return _mapper.Map<UserBL>(_userRepository.GetByLogin(login));
        }

        public async Task<UserBL> GetByLoginAsync(string login)
        {
            return _mapper.Map<UserBL>(await _userRepository.GetByLoginAsync(login));
        }
        public UserBL Login(LoginDto loginDto)
        {
            UserBL user = GetByLogin(loginDto.Login);

            if (user == null)
                return null;

            if (user.Password == loginDto.Password)
                return user;
            else
                return null;
        }

        public async Task<UserBL> LoginAsync(LoginDto loginDto)
        {
            UserBL user = await GetByLoginAsync(loginDto.Login);

            if (user == null)
                return null;

            if (user.Password == loginDto.Password)
                return user;
            else
                return null;
        }

        public IEnumerable<UserBL> GetByPermission(string permission)
        {
            return _mapper.Map<IEnumerable<UserBL>>(_userRepository.GetByPermission(permission));
        }


        public async Task<IEnumerable<UserBL>> GetByPermissionAsync(string permission)
        {
            return _mapper.Map<IEnumerable<UserBL>>(await _userRepository.GetByPermissionAsync(permission));
        }

        public IEnumerable<UserBL> GetAll(UserSortState? sortState)
        {
            var users = _mapper.Map<IEnumerable<UserBL>>(_userRepository.GetAll());

            if (sortState != null)
                users = SortUsersByOption(users, sortState.Value);
            else
                users = SortUsersByOption(users, UserSortState.IdAsc);

            return users;
        }

        public async Task<IEnumerable<UserBL>> GetAllAsync(UserSortState? sortState)
        {
            var users = _mapper.Map<IEnumerable<UserBL>>(await _userRepository.GetAllAsync());

            if (sortState != null)
                users = SortUsersByOption(users, sortState.Value);
            else
                users = SortUsersByOption(users, UserSortState.IdAsc);

            return users;
        }

        private IEnumerable<UserBL> SortUsersByOption(IEnumerable<UserBL> users, UserSortState sortOrder)
        {
            IEnumerable<UserBL> sortedUsers;

            if (sortOrder == UserSortState.IdDesc)
            {
                sortedUsers = users.OrderByDescending(elem => elem.Id);
            }
            else if (sortOrder == UserSortState.LoginAsc)
            {
                sortedUsers = users.OrderBy(elem => elem.Login);
            }
            else if (sortOrder == UserSortState.LoginDesc)
            {
                sortedUsers = users.OrderByDescending(elem => elem.Login);
            }
            else if (sortOrder == UserSortState.PermissionAsc)
            {
                sortedUsers = users.OrderBy(elem => elem.Permission);
            }
            else if (sortOrder == UserSortState.PermissionDesc)
            {
                sortedUsers = users.OrderByDescending(elem => elem.Permission);
            }
            else
            {
                sortedUsers = users.OrderBy(elem => elem.Id);
            }

            return sortedUsers;
        }


        private bool IsExist(UserBL user)
        {
            return _userRepository.GetAll().FirstOrDefault(elem =>
                    elem.Login == user.Login) != null;
        }

        private bool IsNotExist(int id)
        {
            return _userRepository.GetByID(id) == null;
        }
    }
}