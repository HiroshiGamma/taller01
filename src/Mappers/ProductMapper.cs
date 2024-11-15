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
                Nombre = productModel.Nombre,
                Tipo = productModel.Tipo,
                Precio = productModel.Precio,
                Stock = productModel.Stock
            };
        }
    }
}