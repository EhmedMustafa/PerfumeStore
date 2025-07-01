using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PerfumeStore.Application.Interfaces;
using PerfumeStore.Domain.Entities;
using PerfumeStore.Infrastructure.Data;

namespace PerfumeStore.Infrastructure.Repositories
{
    public class FragranceNoteRepository : IFragranceNoteRepository
    {
        private readonly AppDbContext _appDbContext;

        public FragranceNoteRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<FragranceNote> GetByIdWithDetailsAsync(int id)
        {
            return await  _appDbContext.FragranceNotes
                .Include(x => x.NoteTypes)
                .Include(x => x.ProductNotes)
                    .ThenInclude(pn => pn.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
         }
    }
}
