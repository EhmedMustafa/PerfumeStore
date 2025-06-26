using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.FragranceNoteDtos;

namespace PerfumeStore.Application.Services.FragranceNoteServices
{
    public interface IFragranceNoteService
    {
        Task<IEnumerable<ResultFragranceNoteDto>> GetAllFragranceNoteAsync();
        Task<GetByIdFragranceNoteDto> GetByIdFragranceNoteAsync(int id);
        Task AddFragranceNoteAsync(CreateFragranceNoteDto createFragranceNoteDto);
        Task UpdateFragranceNoteAsync(UpdateFragranceNoteDto updateFragranceNoteDto);
        Task DeleteFragranceNoteAsync(int id);
    }
}
