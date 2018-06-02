using System;
using System.Collections.Generic;
using System.Linq;
using CSI_Miami.Data.Models;
using CSI_Miami.Data.Repository;
using CSI_Miami.Data.UnitOfWork;
using CSI_Miami.DTO.MovieService;
using CSI_Miami.Infrastructure.Providers.Contracts;
using CSI_Miami.Services.Internal.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CSI_Miami.Services.Internal
{
    public class MovieService : IMovieService
    {
        private readonly IMappingProvider mapper;
        private readonly IDataRepository<Movie> moviesRepo;
        private readonly IDataSaver dataSaver;
        private readonly IConfiguration configuration;
        private readonly IExporterProvider jsonExporterProvider;

        public MovieService(IMappingProvider mapper, IDataRepository<Movie> moviesRepo,
            IDataSaver dataSaver, IConfiguration configuration,
            IExporterProvider jsonExporterProvider)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.moviesRepo = moviesRepo ?? throw new ArgumentNullException(nameof(moviesRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.jsonExporterProvider = jsonExporterProvider ?? throw new ArgumentNullException(nameof(jsonExporterProvider));
        }


        public bool CreateMovie(MovieDto createdMovieDto)
        {
            if (createdMovieDto == null)
            {
                throw new ArgumentNullException(nameof(createdMovieDto));
            }

            var isCreated = false;

            try
            {
                var movieToAdd = new Movie
                {
                    IsDeleted = false,
                    Title = createdMovieDto.Title,
                    DirectorName = createdMovieDto.DirectorName,
                    ReleaseDate = createdMovieDto.ReleaseDate
                };

                this.moviesRepo.Add(movieToAdd);
                this.dataSaver.SaveChanges();
                isCreated = true;
            }
            catch
            {
                isCreated = false;
            }

            return isCreated;
        }

        public bool EditMovie(MovieDto editedMovie)
        {
            if (editedMovie == null)
            {
                throw new ArgumentNullException(nameof(editedMovie));
            }

            var isEdited = false;

            try
            {
                var movieToEdit = this.moviesRepo.All
                                             .Where(m => m.Id == editedMovie.Id)
                                             .FirstOrDefault();


                movieToEdit.Title = editedMovie.Title;
                movieToEdit.DirectorName = editedMovie.DirectorName;
                movieToEdit.ReleaseDate = editedMovie.ReleaseDate;
                this.dataSaver.SaveChanges();
                isEdited = true;
            }
            catch
            {
                isEdited = false;
            }

            return isEdited;

        }

        public IEnumerable<MovieDto> GetAllMovies(int moviesToSkip)
        {

            IEnumerable<MovieDto> movieDtos;
            int moviesPerPage;

            try
            {
                moviesPerPage = int.Parse(this.configuration["MoviesPerPage"]);
            }
            catch (KeyNotFoundException)
            {
                moviesPerPage = 10;
            }

            var allMovies = this.moviesRepo.All;

            if ((moviesToSkip + moviesPerPage) >= allMovies.Count())
            {
                moviesPerPage = allMovies.Count() - moviesToSkip;
                movieDtos = this.mapper.ProjectTo<MovieDto>(allMovies)
                .Skip(moviesToSkip)
                .Take(moviesPerPage);

            }
            else
            {
                movieDtos = this.mapper.ProjectTo<MovieDto>(allMovies)
                    .Skip(moviesToSkip)
                    .Take(moviesPerPage);
            }

            return movieDtos;
        }

        public int GetMoviesPerPage()
        {
            return int.Parse(this.configuration["MoviesPerPage"]);
        }

        public int GetTotalMoviesCount()
        {
            return this.moviesRepo.All.Count();
        }

        public IEnumerable<MovieDto> LoadNext(int moviesToSkip)
        {
            var moviesToReturn = this.GetAllMovies(moviesToSkip);
            return moviesToReturn;
        }

        public IEnumerable<MovieDto> LoadPrevious(int moviesToSkip)
        {
            var moviesToReturn = this.GetAllMovies(moviesToSkip);
            return moviesToReturn;
        }
    }
}
