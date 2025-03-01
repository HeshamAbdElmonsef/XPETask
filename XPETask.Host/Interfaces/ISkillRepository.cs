using XPETask.Host.Entities;

namespace XPETask.Host.Interfaces
{
    public interface ISkillRepository
    {
        Task AddSkillAsync(Skill skill);
    }
}
