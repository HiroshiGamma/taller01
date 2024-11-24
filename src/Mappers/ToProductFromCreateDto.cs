using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.models;

namespace api.src.Mappers
{
    public static class ToProductFromCreateDtoMapper
    {
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