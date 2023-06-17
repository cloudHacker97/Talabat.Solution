﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specfications;

namespace Talabat.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IGenericRepository<Product> _genericRepositoryProductRepo;
		private readonly IGenericRepository<ProductType> _typeRepo;
		private readonly IGenericRepository<ProductBrand> _brandRepo;
		private readonly IMapper _mapper;

		public ProductController(IGenericRepository<Product> genericRepositoryProductRepo,
			IGenericRepository<ProductType> TypeRepo,
			IGenericRepository<ProductBrand>BrandRepo,IMapper mapper)
		{
			_genericRepositoryProductRepo = genericRepositoryProductRepo;
			_typeRepo = TypeRepo;
			_brandRepo = BrandRepo;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ProductToReturnDto), 200)]

		public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery]specParams specParams)
		{
			var spec = new ProductWithBrandAndTypeSpecfication(specParams);
			var Products = await _genericRepositoryProductRepo.GetAllWithSpecAsync(spec);
			
			return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products));
		}
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ProductToReturnDto), 200)]
		public async Task<ActionResult<Product>> GetProductById(int id)
		{
			var spec = new ProductWithBrandAndTypeSpecfication(id);
			var Products = await _genericRepositoryProductRepo.GetIdWithSpecAsync(spec);
			if(Products == null)
			{
				return NotFound(new ApiResponse(404));
			}

			return Ok(_mapper.Map<Product,ProductToReturnDto>(Products));
		}


		[HttpGet("brands")]
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var Brands = await _brandRepo.GetAllAsync();
			return Ok(Brands);
		}

		[HttpGet("types")]
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
		{
			var Types= await _brandRepo.GetAllAsync();

			return Ok(Types);

		}

	}
}
