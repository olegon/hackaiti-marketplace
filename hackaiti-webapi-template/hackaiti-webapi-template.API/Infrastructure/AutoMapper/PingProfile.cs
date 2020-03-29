using AutoMapper;
using hackaiti_webapi_template.API.Models;

namespace hackaiti_webapi_template.API.Infrastructure.AutoMapper
{
    public class PingProfile : Profile
    {
        public PingProfile()
        {
            CreateMap<PingRequest, PingResponse>();
        }
    }
}