﻿using AutoMapper;
using Demo.Core.Interfaces;
using Demo.Domain.DTOs.Menu;
using Demo.Domain.DTOs.Product;
using Demo.Domain.Models;
using Demo.Domain.Models.Collections;
using Demo.Domain.RepositoryContract;
using Demo.Domain.Utilities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMongoRepository<RoleMenu> _roleMenuRepository;
        private readonly IMongoRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(UserManager<User> userManager, IMongoRepository<RoleMenu> roleMenuRepository, IMongoRepository<Menu> menuRepository, IMapper mapper)
        {
            _userManager = userManager;
            _roleMenuRepository = roleMenuRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<ResponseStatus> AddAsync(MenuInput model)
        {
            var response = new ResponseStatus();
            try
            {
                await _menuRepository.InsertOneAsync(_mapper.Map<Menu>(model));
                response.IsSuccess = Constants.StatusData.True;
                response.Message = Constants.Msg.InsertComplete;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }
        public async Task<ResponseStatus> UpdateAsync(MenuDTO model)
        {
            var response = new ResponseStatus();
            try
            {
                var findId = await _menuRepository.FindByIdAsync(model.Id);

                await _menuRepository.ReplaceOneAsync(_mapper.Map(model, findId));
                response.IsSuccess = Constants.StatusData.True;
                response.Message = Constants.Msg.UpdateComplete;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public async Task<ResponseStatus> DeleteByIdAsync(string id)
        {
            var response = new ResponseStatus();
            try
            {
                await _menuRepository.DeleteByIdAsync(id);
                response.IsSuccess = Constants.StatusData.True;
                response.Message = Constants.Msg.DeleteComplete;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

        public Response<List<MenuDTO>> GetList(Guid userId)
        {
            IQueryable<User> tbUser = _userManager.Users.Where(x => x.Id == userId);
            IQueryable<RoleMenu> tbRoleMenu = _roleMenuRepository.AsQueryable();
            IQueryable<Menu> tbMenu = _menuRepository.AsQueryable();
            var response = new Response<List<MenuDTO>>();
            try
            {
                IQueryable<Menu> tbResult = (from u in tbUser
                                             join rm in tbRoleMenu on u.RoleCode equals rm.RoleCode
                                             join m in tbMenu on rm.MenuCode equals m.MenuCode
                                             select m).AsQueryable();

                var listMenus = tbResult.ToList();
                response.Value = _mapper.Map<List<MenuDTO>>(listMenus);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseStatus> AddRoleManuAsync(RoleMenu model)
        {
            var response = new ResponseStatus();
            try
            {
                await _roleMenuRepository.InsertOneAsync(model);
                response.IsSuccess = Constants.StatusData.True;
                response.Message = Constants.Msg.InsertComplete;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }
    }
}