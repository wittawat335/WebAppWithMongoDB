﻿using Demo.Domain.DTOs.Menu;
using Demo.Domain.DTOs.Product;
using Demo.Domain.Models;
using Demo.Domain.Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Interfaces
{
    public interface IMenuService
    {
        Response<List<MenuDTO>> GetList(Guid userId);
        Task<ResponseStatus> AddAsync(MenuInput model);
        Task<ResponseStatus> AddRoleManuAsync(RoleMenu model);
        Task<ResponseStatus> UpdateAsync(MenuDTO model);
        Task<ResponseStatus> DeleteByIdAsync(string id);
    }
}