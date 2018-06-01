using System;
using System.Collections.Generic;
using System.Text;
using CSI_Miami.DTO.MovieService;

namespace CSI_Miami.Services.Internal.Contracts
{
    public interface IMovieService
    {
        IEnumerable<MovieDto> GetAllMovies(int moviesToSkip);
        bool EditMovie(MovieDto editedMovie);
        bool CreateMovie(MovieDto createdMovieDto);
        IEnumerable<MovieDto> LoadNext(int moviesToSkip);
        IEnumerable<MovieDto> LoadPrevious(int moviesToSkip);
        int GetTotalMoviesCount();
    }
}
