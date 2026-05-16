using AutoMapper;
using PerfumeStore.Application.Dtos.OrderItemDtos;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
    internal class OrdemItemProfile : Profile
    {
        public OrdemItemProfile()
        {
            CreateMap<OrderItem, ResultOrderItemDto>();
            CreateMap<OrderItem, GetByIdOrderItemDto>();
        }
    }
}
