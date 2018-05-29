using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSI_Miami.Web.Models.HomeViewModels.Results
{
    public class ResultsViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<ResultsMoviesViewModel> Movies { get; set; }
    }
}
