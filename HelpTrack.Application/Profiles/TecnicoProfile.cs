﻿using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HelpTrack.Application.Profiles
{
    public class TecnicoProfile : Profile
    {
        public TecnicoProfile()
        {
            CreateMap<TecnicoDTO, Tecnicos>().ReverseMap();
        }
    }
}
