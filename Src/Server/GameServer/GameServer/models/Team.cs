using Common;
using Common.Utils;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    //这个类由五个人共享，队伍信息改变的话要通知到五个人
    class Team
    {
        public double timestamp; //表示队伍信息变更的时间
        public int Id;
        public Character Leader;
        public List<Character> Members = new List<Character>();
        public Team(Character leader)
        {
            this.AddMember(leader);
        }
        public void AddMember(Character member)
        {
            if (this.Members.Count == 0)
            {
                this.Leader = member;
            }
            this.Members.Add(member);
            member.Team = this;
            timestamp = TimeUtil.timestamp;
        }
        public void Leave(Character member)
        {
            Log.InfoFormat("Leave Team :{0}:{1}", member.Id, member.Team);
            this.Members.Remove(member);
            if (member == this.Leader)
            {
                if (this.Members.Count > 0)
                {
                    this.Leader = this.Members[0];
                }
                else
                {
                    this.Leader = null;
                }
            }
            member.Team = null;
            timestamp = TimeUtil.timestamp;
        }
        public void PostProcess(NetMessageResponse message)
        {
            //NetMessageResponse和sender.Session.Response不一样，前面是设置信息，后面是发送信息
            if (message.teamInfo == null)
            {
                message.teamInfo = new TeamInfoResponse();
                message.teamInfo.Result = Result.Success;
                message.teamInfo.Team = new NTeamInfo();
                message.teamInfo.Team.Id = this.Id;
                message.teamInfo.Team.Leader = this.Leader.Id;
                foreach(var member in this.Members)
                {
                    message.teamInfo.Team.Members.Add(member.GetBasicInfo());
                }
            }
        }
    }
}
