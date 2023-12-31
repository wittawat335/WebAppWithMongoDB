﻿using AutoMapper;
using Demo.Core.Interfaces;
using Demo.Domain.DTOs.Product;
using Demo.Domain.Models;
using Demo.Domain.Models.Collections;
using Demo.Domain.Models.ViewModels;
using Demo.Domain.RepositoryContract;
using Demo.Domain.Utilities;

namespace Demo.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoRepository<Products> _repository;
        private readonly IMongoRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(IMongoRepository<Products> repository, IMongoRepository<Category> categoryRepository, IMapper mapper)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<ProductDTO>>> GetAllAsync(ProductSearchModel filter)
        {
            IQueryable<Products> tbProduct = _repository.AsQueryable();
            IQueryable<Category> tbCategory = _categoryRepository.AsQueryable();
            var response = new Response<List<ProductDTO>>();
            try
            {
                var model = (from p in tbProduct
                             join c in tbCategory on p.CategoryId equals c.Id
                             select new ProductDTO
                             {
                                 Id = p.Id.ToString(),
                                 CategoryId = c.Id.ToString(),
                                 CategoryName = c.Name,
                                 ProductName = p.ProductName,
                                 Price = p.Price,
                                 Stock = p.Stock,
                                 IsActive = p.IsActive == true ? "A" : "I",
                                 CreateBy = p.CreateBy,
                                 CreateDate = p.CreateDate
                             }).AsQueryable();

                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.ProductId))
                        model = model.Where(x => x.Id.Contains(filter.ProductId));
                    if (!string.IsNullOrEmpty(filter.CategoryId))
                        model = model.Where(x => x.CategoryId.Contains(filter.CategoryId));
                    if (filter.IsActive != null)
                        model = model.Where(x => x.IsActive == filter.IsActive);
                }

                response.Value = model.ToList();
                response.IsSuccess = Constants.StatusData.True;
                response.Message = Constants.Msg.GetList;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
        public async Task<Response<ProductDTO>> GetByIdAsync(string id)
        {
            var response = new Response<ProductDTO>();
            try
            {
                response.Value = _mapper.Map<ProductDTO>(await _repository.FindByIdAsync(id));
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
        public Task<Response<ProductDTO>> GetOneAsync(string code)
        {
            throw new NotImplementedException();
        }
        public async Task<Response<ProductDTO>> AddAsync(ProductDTO model)
        {
            var response = new Response<ProductDTO>();
            try
            {
                var test = _mapper.Map<ProductInput>(model);
                await _repository.InsertOneAsync(_mapper.Map<Products>(test));
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
        public async Task<Response<ProductDTO>> UpdateAsync(ProductDTO model)
        {
            var response = new Response<ProductDTO>();
            try
            {
                var findId = _repository.FindById(model.Id);

                await _repository.ReplaceOneAsync(_mapper.Map(model, findId));
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
        public async Task<Response<ProductDTO>> DeleteByIdAsync(string id)
        {
            var response = new Response<ProductDTO>();
            try
            {
                await _repository.DeleteByIdAsync(id);
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
        public Task<Response<ProductDTO>> DeleteListAsyncByCreateBy(string text)
        {
            throw new NotImplementedException();
        }
    }
}
