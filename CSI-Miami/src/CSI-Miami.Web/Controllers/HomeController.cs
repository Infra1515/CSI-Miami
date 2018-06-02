using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CSI_Miami.Web.Models;
using Microsoft.AspNetCore.Authorization;
using CSI_Miami.Infrastructure.Providers.Contracts;
using CSI_Miami.Services.Internal.Contracts;
using CSI_Miami.DTO.MovieService;
using CSI_Miami.Web.Models.HomeViewModels.Results;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CSI_Miami.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMappingProvider mapper;
        private readonly IUserManagerProvider userManager;
        private readonly IMovieService movieService;
        private readonly IExporterProvider exporterProvider;
        private readonly IMemoryCache memoryCache;

        public HomeController(IMappingProvider mapper,
            IUserManagerProvider userManager,
            IMovieService movieService,
            IExporterProvider exporterProvider,
            IMemoryCache memoryCache)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            this.exporterProvider = exporterProvider ?? throw new ArgumentNullException(nameof(exporterProvider));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

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
            var allMovies = this.movieService.GetAllMovies(0);
            var userName = this.userManager.GetUserName(User);
            var allMoviesViewModel = this.mapper
                            .EnumerableProjectTo<MovieDto, ResultsMoviesViewModel>(allMovies);

            var resultsViewModel = new ResultsViewModel
            {
                Movies = allMoviesViewModel,
                UserName = userName
            };

            TempData.Clear();
            TempData.Add("moviesToSkip", 0);
            return View(resultsViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMovie(ResultsMoviesViewModel movie)
        {
            var isEdited = false;
            if (ModelState.IsValid)
            {
                var editedMovieDto = this.mapper.MapTo<MovieDto>(movie);
                isEdited = this.movieService.EditMovie(editedMovieDto);
            }
            return this.Json(new JsonResult(new { isEdited }));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMovie(ResultsMoviesViewModel newMovie)
        {
            var isCreated = false;
            if (ModelState.IsValid)
            {
                var createdMovieDto = this.mapper.MapTo<MovieDto>(newMovie);
                isCreated = this.movieService.CreateMovie(createdMovieDto);
            }

            return this.Json(new JsonResult(new { isCreated }));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {

            if (id == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(), id);

            var dataAsJsonString = string.Empty;


            if (!this.memoryCache.TryGetValue("moviesInDb", out dataAsJsonString))
            {
                dataAsJsonString = exporterProvider
                .ExportDataAsJson("SELECT Id, DirectorName, ReleaseDate, Title FROM Movies");
                this.memoryCache.Set("moviesInDb", dataAsJsonString,
                   TimeSpan.FromHours(8));
                exporterProvider.WriteDataAsJson(dataAsJsonString);
            }



            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            var mimeType = "application/json";
            return File(memory, mimeType, Path.GetFileName(path));
        }

        [Authorize]
        public IActionResult LoadNext()
        {

            int moviesToSkip;
            var totalMoviesCount = this.movieService.GetTotalMoviesCount();
            int moviesToShow = this.movieService.GetMoviesPerPage();

            var obj = TempData["moviesToSkip"];
            moviesToSkip = (int)obj;
            moviesToSkip += moviesToShow;

            TempData["moviesToSkip"] = moviesToSkip;

            if (moviesToSkip >= totalMoviesCount)
            {
                moviesToSkip = totalMoviesCount;
                TempData["moviesToSkip"] = moviesToSkip;
                return this.PartialView("_NoMoreMoviesPartial");
            }

            var allMovies = this.movieService.LoadNext(moviesToSkip);
            var allMoviesViewModel = this.mapper
                .EnumerableProjectTo<MovieDto, ResultsMoviesViewModel>(allMovies);

            return this.PartialView("_MoviesListPartial", allMoviesViewModel);
        }

        [Authorize]
        public IActionResult LoadPrevious()
        {
            int moviesToSkip;
            var moviesToShow = this.movieService.GetMoviesPerPage();


            var obj = TempData["moviesToSkip"];
            moviesToSkip = (int)obj;

            if (moviesToSkip % moviesToShow != 0)
            {
                // find the closest number less than moviesToSkip that can be 
                // divided by moviesToShow without remainder
                var x = moviesToSkip / moviesToShow;
                moviesToSkip = x * moviesToShow;
            }
            else
            {
                moviesToSkip -= moviesToShow;
            }

            TempData["moviesToSkip"] = moviesToSkip;


            if (moviesToSkip < 0)
            {
                moviesToSkip = -moviesToShow;
                TempData["moviesToSkip"] = moviesToSkip;
                return this.PartialView("_NoMoreMoviesPartial");
            }

            var allMovies = this.movieService.LoadPrevious(moviesToSkip);
            var allMoviesViewModel = this.mapper
                .EnumerableProjectTo<MovieDto, ResultsMoviesViewModel>(allMovies);

            return this.PartialView("_MoviesListPartial", allMoviesViewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
