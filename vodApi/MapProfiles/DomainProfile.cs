using AutoMapper;
using NodaTime;
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

            CreateMap<FilmResultWithMovieType, BlackListedMovieEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<AlreadyWatchedMovie, AlreadyWatchedModel>();
            CreateMap<FilmwebResult, MovieViewModel>();
            CreateMap<MovieViewModel, FilmwebResult>();
            CreateMap<MovieViewModel, MovieEntity>()
                .ForMember(x => x.DurationInMinutes, opt => opt.MapFrom(src => src.Duration.TotalMinutes));

            CreateMap<MovieEntity, MovieViewModel>()
                .ForMember(x => x.Duration, opt => opt.MapFrom(src => Duration.FromMinutes(src.DurationInMinutes)));

            CreateMap<FilmwebResult, MovieEntity>()
                .ForMember(x => x.Cast,
                    opt => opt.MapFrom(
                        src => string.Join(", ", src.Cast)))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.DurationInMinutes, opt => opt.MapFrom(src => src.Duration.TotalMinutes));

            CreateMap<MovieEntity, FilmwebResult>()
                .ForMember(x => x.FilmwebRating,
                    opt => opt.MapFrom(
                        src => src.FilmwebRating >= 10
                            ? src.FilmwebRating / 10
                            : src.FilmwebRating))
                .ForMember(x => x.Cast,
                    opt => opt.MapFrom(
                        src => src.Cast.Split(",", StringSplitOptions.None)))
                .ForMember(x => x.Duration, opt => opt.MapFrom(src => Duration.FromMinutes(src.DurationInMinutes)));
        }
    }
}
