using System.ComponentModel.DataAnnotations;

namespace XPETask.Host.Entities
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int DateGained { get; set; }

        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }


    }
}
