using System;
using System.Collections.Generic;
using System.Linq;
using CSI_Miami.Data.Models;
using CSI_Miami.Data.Repository;
using CSI_Miami.Data.UnitOfWork;
using CSI_Miami.DTO.MovieService;
using CSI_Miami.Services.Internal.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace CSI_Miami.Services.Internal
{
    public class MovieService : IMovieService
    {
        private readonly IMappingProvider mapper;
        private readonly IDataRepository<Movie> moviesRepo;
        private readonly IDataSaver dataSaver;
        private readonly IMemoryCache memoryCache;

        public MovieService(IMappingProvider mapper, IDataRepository<Movie> moviesRepo,
            IDataSaver dataSaver, IMemoryCache memoryCache)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.moviesRepo = moviesRepo ?? throw new ArgumentNullException(nameof(moviesRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }


        public IEnumerable<MovieDto> GetAllMovies()
        {

            IEnumerable<MovieDto> movieDtos;

            if (!this.memoryCache.TryGetValue("AllMovies", out movieDtos))
            {
                var allMovies = this.moviesRepo.All;

                movieDtos = this.mapper.ProjectTo<MovieDto>(allMovies).ToList();

                this.memoryCache.Set("AllMovies", movieDtos, TimeSpan.FromHours(8));
            }

            return movieDtos;

        }
    }
}
