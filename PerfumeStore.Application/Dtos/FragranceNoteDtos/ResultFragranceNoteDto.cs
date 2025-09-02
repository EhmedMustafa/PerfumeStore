using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerfumeStore.Application.Dtos.ProductDtos;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Domain.Enums;

namespace PerfumeStore.Application.Dtos.FragranceNoteDtos
{
    public class ResultFragranceNoteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<NoteType> Types { get; set; }

        public ICollection<ResultProductDto> Products { get; set; }  // Bu not hansı məhsullardadır
    }
}
