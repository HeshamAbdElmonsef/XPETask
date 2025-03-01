using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using XPETask.Host.DTO;
using XPETask.Host.Entities;
using XPETask.Host.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace XPETask.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateRepository _candidateRepo;
        private readonly ISkillRepository _skillRepo;
        private readonly IMapper _mapper;
        private static readonly Regex NicknameRegex = new Regex(@"\d[§®™©ʬ@]");

        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private static readonly Regex SkillNameRegex = new Regex(@"^[^0-9]+$");

        public CandidatesController(ICandidateRepository candidateRepo, ISkillRepository skillRepo,IMapper mapper)
        {
            _candidateRepo = candidateRepo;
            _skillRepo = skillRepo;
            _mapper = mapper;
        }

        [HttpPost("upload")]
      
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            using var reader = new StreamReader(file.OpenReadStream());
            var candidates = new List<Candidate>();
            string line;
            bool isFirstLine = true;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (isFirstLine) { isFirstLine = false; continue; } 

                var parts = line.Split("\t\t");
                if (parts.Length < 5) continue; 

                string name = parts[0].Trim();
                string nickname = parts[1].Trim();
                string email = parts[2].Trim();

               if (!NicknameRegex.IsMatch(nickname)) continue; 
                if (!EmailRegex.IsMatch(email)) continue; 

                if (!int.TryParse(parts[3].Trim(), out int yearsOfExperience)) continue; 
                int? maxNumSkills = int.TryParse(parts[4].Trim(), out int maxSkills) ? maxSkills : (int?)null;

                if (!await _candidateRepo.CandidateExistsAsync(email)) 
                {
                    candidates.Add(new Candidate
                    {
                        Name = name,
                        Nickname = nickname,
                        Email = email,
                        YearsOfExperience = yearsOfExperience,
                        MaxNumSkills = maxNumSkills,
                        Skills = new List<Skill>()
                    });
                }
            }

            if (candidates.Any())
            {
                await _candidateRepo.AddCandidatesAsync(candidates);
                return Ok(new { Message = "Candidates uploaded successfully", Count = candidates.Count });
            }
            return BadRequest("No valid candidates found in the file");
        }


        [HttpGet("Get-All")]
        public async Task<IActionResult> GetCandidates()
        {
            var candidates = await _candidateRepo.GetCandidatesAsync();
            var candidatesRes=_mapper.Map<IEnumerable<CandidateResponse>>(candidates);
           return Ok(candidatesRes);
        }

        [HttpPost("skills")]
        public async Task<IActionResult> AddSkill (SkillRequest skill)
        {
           
            if (!SkillNameRegex.IsMatch(skill.Name))
                return BadRequest("Skill name should not contain numbers.");

            var candidate = await _candidateRepo.GetCandidateByIdAsync(skill.CandidateId); 
            if (candidate == null)
                return NotFound("Candidate not found.");

            if (skill.DateGained < candidate.YearsOfExperience)
                return BadRequest("Skill date cannot be before years of experience.");
            var cskill = new Skill
            {
                Name = skill.Name,
                DateGained = skill.DateGained,
                CandidateId = skill.CandidateId
            };
            try
            {
                await _candidateRepo.AddSkillToCandidateAsync(candidate.Id,cskill);
                return Ok("Skill added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
