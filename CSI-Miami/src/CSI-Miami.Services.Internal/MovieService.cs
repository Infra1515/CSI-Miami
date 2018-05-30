﻿using System;
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
using Microsoft.Extensions.Options;

namespace CSI_Miami.Services.Internal
{
    public class MovieService : IMovieService
    {
        private readonly IMappingProvider mapper;
        private readonly IDataRepository<Movie> moviesRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMemoryCache memoryCache;
        private readonly IConfiguration configuration;

        public MovieService(IMappingProvider mapper, IDataRepository<Movie> moviesRepo,
            IDataSaver dataSaver, IMemoryCache memoryCache, IConfiguration configuration)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.moviesRepo = moviesRepo ?? throw new ArgumentNullException(nameof(moviesRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
                moviesPerPage = int.Parse(configuration["MoviesPerPage"]);
            }
            catch (KeyNotFoundException)
            {
                moviesPerPage = 10;
            }

            var allMovies = this.moviesRepo.All;

            if ((moviesToSkip + moviesPerPage) >= allMovies.Count())
            {
                moviesPerPage = allMovies.Count() - (moviesToSkip + moviesPerPage);
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
    }
}
