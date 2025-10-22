using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.Profiles
{
    public class SlaProfile : Profile
    {
        public SlaProfile()
        {
            CreateMap<SlaDTO, Sla>().ReverseMap();
        }
    }
}
