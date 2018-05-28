using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CSI_Miami.Data.Models
{
    public class Movie
    {
        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [MaxLength(100)]
        public string DirectorName { get; set; }

        [DataType(DataType.Date)]
        public string ReleaseDate { get; set; }
    }
}
