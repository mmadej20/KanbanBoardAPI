using AutoMapper;
using KanbanBoard.DataAccess.Entities;
using KanbanBoard.Domain.Entities;

namespace KanbanBoard.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BoardEntity, Board>()
                .ForMember(dest => dest.BoardItems, opt => opt.MapFrom(src => src.BoardItems))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.BoardMembers.Select(bm => bm.Member)))
                .ReverseMap();

            CreateMap<BoardItemEntity, BoardItem>()
                .ForMember(dest => dest.BoardId, opt => opt.MapFrom(src => src.BoardId ?? Guid.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Board, opt => opt.Ignore());

            CreateMap<MemberEntity, Member>()
                .ReverseMap()
                .ForMember(dest => dest.BoardMembers, opt => opt.Ignore())
                .ForMember(dest => dest.BoardItems, opt => opt.Ignore());
        }
    }
}
