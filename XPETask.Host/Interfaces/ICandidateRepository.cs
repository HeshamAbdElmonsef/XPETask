using XPETask.Host.Entities;

namespace XPETask.Host.Interfaces
{
    public interface ICandidateRepository
    {
        Task<List<Candidate>> GetCandidatesAsync();
        Task<Candidate> GetCandidateByIdAsync(int id);
        Task AddCandidatesAsync(IEnumerable<Candidate> candidates);
        Task UpdateCandidateAsync(Candidate candidate);
        Task AddSkillToCandidateAsync(int candidateId, Skill skill);
        Task<bool> CandidateExistsAsync(string email);
    }
}

