using AutoMapper;
using PerfumeStore.Application.Dtos.OrderDto;
using PerfumeStore.Application.Dtos.OrderDtos;
using Order = PerfumeStore.Domain.Entities.Order;

namespace PerfumeStore.Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, ResultOrderDto>();
            CreateMap<Order, GetByIdOrderDto>();
        }
    }
}
