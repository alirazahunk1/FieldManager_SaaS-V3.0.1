using AutoMapper;
using ESSCommon.Core.SharedModels;
using ESSDataAccess.Models;

namespace ESSWebPortal
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SettingsModel, SettingsDto>();
            CreateMap<SettingsDto, SettingsModel>();
        }
    }
}
