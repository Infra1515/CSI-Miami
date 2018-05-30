using System;
using System.ComponentModel.DataAnnotations;

namespace CSI_Miami.DTO.MovieService
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string DirectorName { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }
    }
}
