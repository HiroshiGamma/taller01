using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Stock = productModel.Stock
            };
        }
    }
}