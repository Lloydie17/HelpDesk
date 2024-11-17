using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public string GetTeamNameById(int? id)
        {
            var team = _teamRepository.GetTeamById(id);

            return team != null ? team.TeamName : "Unknown";
        }

        public (bool, IEnumerable<Team>) GetTeams()
        { 
            var teams = _teamRepository.GetTeams();

            if (teams != null)
            {
                return (true, teams);
            }
            return (false, null);
        }

        public Team GetTeamById(int id)
        { 
            return _teamRepository.GetTeamById(id);
        }

        public void AddTeam(Team team)
        {
            try
            {
                var newTeam = new Team
                {
                    TeamName = team.TeamName,
                    TeamLeader = team.TeamLeader,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                };

                _teamRepository.AddTeam(newTeam);
            }
            catch (Exception)
            {
                throw new InvalidDataException("Error adding teams");
            }
        }

        public void UpdateTeam(Team team)
        {
            var existingTeam = _teamRepository.GetTeamById(team.TeamId);

            if (existingTeam == null)
            {
                throw new InvalidDataException("Team not found!");
            }

            existingTeam.TeamName = team.TeamName;
            existingTeam.TeamLeader = team.TeamLeader;
            existingTeam.UpdatedTime = DateTime.Now;

            _teamRepository.UpdateTeam(existingTeam);
        }

        public void DeleteTeam(int id)
        {
            var team = _teamRepository.GetTeamById(id);

            _teamRepository.DeleteTeam(team);
        }
    }
}
