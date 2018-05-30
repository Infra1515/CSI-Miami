using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CSI_Miami.Data.CustomAttributes;

namespace CSI_Miami.Web.Models.HomeViewModels.Results
{
    public class ResultsMoviesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The movie title is required!")]
        [StringLength(200, MinimumLength = 1,
             ErrorMessage = "Title length must be between 1 and 200 symbols!")]
        public string Title { get; set; }

        [StringLength(100, MinimumLength = 1,
             ErrorMessage = "Director name length must be between 1 and 200 symbols!")]
        public string DirectorName { get; set; }

        [ValidateFutureDate]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }
    }
}
