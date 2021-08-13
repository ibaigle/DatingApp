using System;
using System.Linq;
using API.DTOS;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser,MemberDto>()
                .ForMember(destination => destination.PhotoUrl, options => options.MapFrom(source => 
                source.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(destination => destination.Age, options => options.MapFrom(source => source.DateofBirth.CalculateAge()));
            CreateMap<Photo,PhotoDto>();
            CreateMap<MemberUpdateDto,AppUser>();
            CreateMap<RegisterDto,AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(destiny => destiny.SenderPhotoUrl, optional=> optional.MapFrom(src =>
                    src.Sender.Photos.FirstOrDefault(x=> x.IsMain).Url
                ))
                .ForMember(destiny => destiny.RecipientPhotoUrl, optional=> optional.MapFrom(src =>
                    src.Recipient.Photos.FirstOrDefault(x=> x.IsMain).Url
                ));
                CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}