﻿using AutoMapper;
using System;
using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary.Models;

namespace vodApi.MapProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<FilmwebResult, Result>();
            CreateMap<Result, FilmwebResult>();
            CreateMap<Result, ResultModel>();
            CreateMap<ResultModel, Result>();
            CreateMap<FilmwebResult, ResultModel>()
                .ForMember(x => x.Cast,
                    opt => opt.MapFrom(
                        src => string.Join(", ", src.Cast)));

            CreateMap<ResultModel, FilmwebResult>()
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
