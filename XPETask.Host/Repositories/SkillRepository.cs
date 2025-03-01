using XPETask.Host.Data;
using XPETask.Host.Entities;
using XPETask.Host.Interfaces;

namespace XPETask.Host.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _context;
        public SkillRepository(ApplicationDbContext context) { _context = context; }

        public async Task AddSkillAsync(Skill skill)
        {
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
        }
    }
}
