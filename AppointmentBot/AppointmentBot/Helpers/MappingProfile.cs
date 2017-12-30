using AppointmentBot.Model;
using AutoMapper;

namespace AppointmentBot.Helpers
{
    internal class MappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "MappingProfile"; }
        }
        public MappingProfile()
        {
            CreateMap<ApiResponse, ApiAiSDK.Model.AIResponse>()
                      .ForMember(dest => dest.Result.Metadata.IntentName, map => map.MapFrom(src => src.Action))
            .ForMember(dest => dest.Result.ResolvedQuery, map => map.MapFrom(src => src.Query))
            .ForMember(dest => dest.Result.Parameters, map => map.MapFrom(src => src.Parameters))
                        .ForMember(dest => dest.Result.Score, map => map.MapFrom(src => src.Score))
            .ForMember(dest => dest.Result.Fulfillment.Speech, map => map.MapFrom(src => src.ResponseMessage));

        }
    }
}