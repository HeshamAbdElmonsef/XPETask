using Microsoft.EntityFrameworkCore;
using XPETask.Host.Data;
using XPETask.Host.Entities;
using XPETask.Host.Interfaces;

namespace XPETask.Host.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;
        public CandidateRepository(ApplicationDbContext context)
        { 
            _context = context;
        }

        public async Task<List<Candidate>> GetCandidatesAsync() 
            => await _context.Candidates.Include(c => c.Skills).ToListAsync();

        public async Task<Candidate> GetCandidateByIdAsync(int id)
            => await _context.Candidates.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddCandidatesAsync(IEnumerable<Candidate> candidates)
        {
            _context.Candidates.AddRange(candidates);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Update(candidate);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CandidateExistsAsync(string email)
            => await _context.Candidates.AnyAsync(c => c.Email == email);

        public async Task AddSkillToCandidateAsync(int candidateId, Skill skill)
        {
            var candidate = await _context.Candidates
                   .Include(c => c.Skills)
                   .FirstOrDefaultAsync(c => c.Id == candidateId);

            if (candidate == null)
                throw new Exception("Candidate not found.");

            
            if (candidate.MaxNumSkills.HasValue && candidate.Skills.Count >= candidate.MaxNumSkills)
                throw new Exception("Max skills limit reached.");

            skill.CandidateId = candidateId;
            candidate.Skills.Add(skill);

            await _context.SaveChangesAsync();
        }

    }
}
