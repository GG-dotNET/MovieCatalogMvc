using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Model.Models
{
    public class Film
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Заполните название фильма")]
        [StringLength(100)]
        [Display(Name = "Название фильма")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Заполните описание")]
        [StringLength(200)]
        [Display(Name = "Описание фильма")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата выхода")]
        public DateTime RelaseDate { get; set; }

        [Required(ErrorMessage = "Заполните имя режисера")]
        [Display(Name = "Режиссер")]
        public string Director { get; set; }

        [Display(Name = "Постер")]
        public string PosterPath { get; set; }
    }
}
