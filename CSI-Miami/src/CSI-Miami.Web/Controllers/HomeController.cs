using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSI_Miami.Web.Models;
using Microsoft.AspNetCore.Authorization;
using CSI_Miami.Infrastructure.Providers.Contracts;
using CSI_Miami.Services.Internal.Contracts;

namespace CSI_Miami.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMappingProvider mapper;
        private readonly IUserManagerProvider userManager;
        private readonly IMovieService movieService;

        public IActionResult Index()
        {
            if (this.User != null &&
               this.User.Identity != null &&
               this.User.Identity.IsAuthenticated)
            {

                return this.RedirectToAction("Results", "Home");
            }

            return this.RedirectToAction(nameof(AccountController.Authorize), "Account");
        }

        [Authorize]
        public IActionResult Results()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
