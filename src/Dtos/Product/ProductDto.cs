using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taller01.src.Dtos
{
    public class ProductDto
    {
        public int Id {get; set;}

        [Required]
        [MaxLength(64, ErrorMessage = "Name must be less than 64 characters")]
        [MinLength(10, ErrorMessage = "Name must be at least 10 characters long")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must consist of alphabetic characters only.")]
        public String Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(Poleras|Gorros|Juguetería|Alimentación|Libros)$", ErrorMessage = "Type must be one of the following: Poleras, Gorros, Juguetería, Alimentación, Libros.")]
        public String Type { get; set; } = string.Empty;

        [Required]
        [Range(1, 99999999, ErrorMessage = "Price must be a positive integer less than 100 million.")]
        public int Price { get; set; }


        
    }
}