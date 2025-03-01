using System.ComponentModel.DataAnnotations;

namespace XPETask.Host.DTO
{
    public record SkillRequest
    {
        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int DateGained { get; set; }
        public int CandidateId { get; set; }
    }
}
