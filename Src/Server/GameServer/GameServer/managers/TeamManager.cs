using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class TeamManager:Singleton<TeamManager>
    {
        public List<Team> Teams = new List<Team>(); //列表方便遍历用
        public Dictionary<int, Team> CharacterTeams = new Dictionary<int, Team>();  //字典方便精准查询用
        public Team GetTeamByCharacter(int characterId)
        {
            Team team = null;
            this.CharacterTeams.TryGetValue(characterId, out team);
            return team;
        }
        public void Init() { }
        public void AddTeamMember(Character leader,Character member)
        {
            if (leader.Team == null)
            {
                leader.Team = CreateTeam(leader);
            }
            leader.Team.AddMember(member);
        }
       
        //队伍一旦创建，终身不销毁，等该队伍被解散后，其他新创建的队伍用这个队伍，内存复用
        Team CreateTeam(Character leader)
        {
            Team team = null;
            for(int i = 0; i < Teams.Count; i++)
            {
                team = Teams[i];
                if (team.Members.Count == 0)
                {
                    team.AddMember(leader);
                    return team;
                }
            }
            team = new Team(leader);
            this.Teams.Add(team);
            team.Id = this.Teams.Count;
            return team;
        }
    }
}
