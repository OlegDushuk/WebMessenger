using AutoMapper;
using WebMessenger.Core.Entities;
using WebMessenger.Core.Enums;
using WebMessenger.Shared.DTOs.Responses;
using WebMessenger.Shared.Enums;

namespace WebMessenger.Application.Mappings;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<User, UserDto>();
    
    CreateMap<GroupType, GroupTypeDto>().ConvertUsing(src => (GroupTypeDto)(int)src);
    CreateMap<GroupTypeDto, GroupType>().ConvertUsing(src => (GroupType)(int)src);
    CreateMap<GroupDetails, GroupDetailsDto>();
    
    CreateMap<ChatMessage, ChatMessageDto>();
    
    CreateMap<ChatMemberRole, ChatMemberRoleDto>().ConvertUsing(src => (ChatMemberRoleDto)(int)src);
    CreateMap<ChatMemberRoleDto, ChatMemberRole>().ConvertUsing(src => (ChatMemberRole)(int)src);
    CreateMap<ChatMember, ChatMemberDto>()
      .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
      .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar));

    CreateMap<Chat, ChatDto>();
    
    CreateMap<User, SearchItemDto>()
      .ForMember(dest => dest.Type, opt => opt.MapFrom(src => SearchItemTypeDto.User));
    
    CreateMap<Chat, SearchItemDto>()
      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GroupDetails!.Name))
      .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.GroupDetails!.Avatar))
      .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.GroupDetails!.Bio))
      .ForMember(dest => dest.Type, opt => opt.MapFrom(src => SearchItemTypeDto.Group));
  }
}