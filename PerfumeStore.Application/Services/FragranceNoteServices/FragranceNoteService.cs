using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Application.Dtos.FragranceNoteDtos;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Services.FragranceNoteServices
{
    public class FragranceNoteService : IFragranceNoteService
    {
        private readonly IGenericRepository<FragranceNote> _genericRepository;
        private readonly IMapper _mapper;
        private readonly IFragranceNoteRepository _fragranceNoteRepository;

        public FragranceNoteService(IGenericRepository<FragranceNote> genericRepository, IMapper mapper,IFragranceNoteRepository fragranceNoteRepository)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _fragranceNoteRepository = fragranceNoteRepository;
        }

        public async Task AddFragranceNoteAsync(CreateFragranceNoteDto createFragranceNoteDto)
        {
            var values =  _genericRepository.AddAsync(new FragranceNote
            {
                Name = createFragranceNoteDto.Name,
                ImageUrl= createFragranceNoteDto.ImageUrl,
                NoteTypes= new List<FragranceNoteType> 
                {
                    new FragranceNoteType { Type= NoteType.Top },
                    new FragranceNoteType { Type= NoteType.Middle },
                    new FragranceNoteType {Type= NoteType.Base}
                }
            });

            
            await _genericRepository.SaveChangesAsync();
            
        }
        public async Task<IEnumerable<ResultFragranceNoteDto>> GetAllFragranceNoteAsync()
        {
            var values = await _genericRepository.GetAllAsync();
            var map = _mapper.Map<IEnumerable<ResultFragranceNoteDto>>(values);
            return map;
        }

        public async Task<GetByIdFragranceNoteDto> GetByIdFragranceNoteAsync(int id)
        {
            //var values = await _genericRepository.GetByIdAsync(id);
            //var map = _mapper.Map<GetByIdFragranceNoteDto>(values);
            //return map;

            var note = await _fragranceNoteRepository.GetByIdWithDetailsAsync(id);

            if (note == null)
                throw new KeyNotFoundException("Not tapılmadı.");


            var map = _mapper.Map<GetByIdFragranceNoteDto>(note);
            return map;
        }

        public async Task UpdateFragranceNoteAsync(UpdateFragranceNoteDto updateFragranceNoteDto)
        {
            var valeus = await _genericRepository.GetByIdAsync(updateFragranceNoteDto.Id);
            valeus.Name=updateFragranceNoteDto.Name;
            valeus.ImageUrl = updateFragranceNoteDto.ImageUrl;

            if (updateFragranceNoteDto.Types != null && updateFragranceNoteDto.Types.Any())
            {
                valeus.NoteTypes.Clear();
                foreach (var type in updateFragranceNoteDto.Types.Distinct())
                {
                    valeus.NoteTypes.Add(new FragranceNoteType
                    {
                        FragranceNoteId=valeus.Id,
                        Type=type
                    });
                }
            }
            await _genericRepository.UpdateAsync(valeus);
            await _genericRepository.SaveChangesAsync();

        }
        public async Task DeleteFragranceNoteAsync(int id)
        {
            var values= await _genericRepository.GetByIdAsync(id);
            await _genericRepository.DeleteAsync(values);
            await _genericRepository.SaveChangesAsync();
        }

      

       
    }
}
