using AutoMapper;
using XPETask.Host.DTO;
using XPETask.Host.Entities;
using XPETask.Host.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace XPETask.Host.MappingConf
{
    public class MappConf: Profile
    {
        public MappConf()
        {
            CreateMap<CandidateResponse, Candidate>().ReverseMap();
            CreateMap<SkillRequest, Skill>().ReverseMap();

        }
    }
}
