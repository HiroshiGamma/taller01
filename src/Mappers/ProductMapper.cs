using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.models;
using taller01.src.Dtos;

namespace taller01.src.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this Product productModel) 
        {
            return new ProductDto 
            {
                Id = productModel.Id,
                Name = productModel.Name,
                Type = productModel.Type,
                Price = productModel.Price,
                ImageUrl = productModel.ImageUrl
            };
        }

          public static Product ToProductFromCreateDto(this CreateProductDto createProductDto, string url)
        {
            return new Product
            {
                Name = createProductDto.Name,
                Type = createProductDto.Type,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                ImageUrl = url
            };
        }
    }
}