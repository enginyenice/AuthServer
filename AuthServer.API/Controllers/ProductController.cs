using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product,ProductDto> _genericService;

        public ProductController(IGenericService<Product, ProductDto> genericService)
        {
            _genericService = genericService;
        }
       [HttpGet]
       public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await _genericService.GetAllAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _genericService.AddAsync(productDto));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _genericService.Update(productDto,productDto.Id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultInstance(await _genericService.Remove(id));
        }

        }
}
