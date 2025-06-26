using PerfumeStore.Application.Dtos.FragrancefamilyDtos;

namespace PerfumeStore.Application.Services.FragranceFamilyService
{
    public interface IFragranceFamilyService
    {
        Task<IEnumerable<ResultFragranceFamilyDto>> GetAllFragranceFamilyAsync();
        Task<GetByIdFragranceFamilyDto> GetByIdFragranceFamilyAsync(int id);
        Task AddFragranceFamilyAsync(CreateFragranceFamilyDto createFragranceFamilyDto);
        Task UpdateFragranceFamilyAsync(UpdateFragranceFamilyDto updateFragranceFamilyDto);
        Task DeleteFragranceFamilyAsync(int id);
    }
}
