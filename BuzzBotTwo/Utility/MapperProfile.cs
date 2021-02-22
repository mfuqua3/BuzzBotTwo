using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.External.SoftResIt.Models;

namespace BuzzBotTwo.Utility
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RaidModel, SoftResEvent>()
                .ForMember(ent => ent.Id, opt => opt.MapFrom(model => model.RaidId))
                .ForMember(ent => ent.Users, opt => opt.MapFrom(model => model.Reserved));
            CreateMap<SoftResUserModel, SoftResUser>()
                .ForMember(usr => usr.ReservedItems, opt => opt.MapFrom(model => CreateReservedItems(model)));
        }

        private List<ReservedItem> CreateReservedItems(SoftResUserModel model)
        {
            return model.Items.Select(itm => new ReservedItem
            {
                ItemId = itm
            }).ToList();
        }
    }
}