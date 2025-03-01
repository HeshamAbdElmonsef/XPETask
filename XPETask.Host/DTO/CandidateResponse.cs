using System.ComponentModel.DataAnnotations;

namespace XPETask.Host.DTO
{
    public class CandidateResponse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Nickname { get; set; }


        [Required, EmailAddress]
        public string Email { get; set; }

        [Range(0, int.MaxValue)]
        public int YearsOfExperience { get; set; }

        [Range(1, int.MaxValue)]
        public int? MaxNumSkills { get; set; }

        public List<SkillRequest> Skills { get; set; } = new List<SkillRequest>();
    }
}
