using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSI_Miami.Web.Models.HomeViewModels.Results
{
    public class ResultsMoviesViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string DirectorName { get; set; }

        public string ReleaseDate { get; set; }
    }
}
