using AutoMapper;
using System;
using vod.Core.Boundary.Model;
using vod.Domain.Services;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary.Models;

namespace vodApi.MapProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<AlreadyWatchedMovie, UserMovieEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<NcPlusResult, BlackListedMovieEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<NetflixResult, BlackListedMovieEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Result, BlackListedMovieEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<AlreadyWatchedMovie, AlreadyWatchedModel>();
            CreateMap<FilmwebResult, MovieViewModel>();
            CreateMap<MovieViewModel, FilmwebResult>();
            CreateMap<MovieViewModel, MovieEntity>();
            CreateMap<MovieEntity, MovieViewModel>();
            CreateMap<FilmwebResult, MovieEntity>()
                .ForMember(x => x.Cast,
                    opt => opt.MapFrom(
                        src => string.Join(", ", src.Cast)))
                        .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<MovieEntity, FilmwebResult>()
                .ForMember(x => x.FilmwebRating,
                    opt => opt.MapFrom(
                        src => src.FilmwebRating >= 10
                            ? src.FilmwebRating / 10
                            : src.FilmwebRating))
                .ForMember(x => x.Cast,
                    opt => opt.MapFrom(
                        src => src.Cast.Split(",", StringSplitOptions.None)));
        }
    }
}
