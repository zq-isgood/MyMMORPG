using Common;
using Common.Utils;
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
    class GuildManager:Singleton<GuildManager>
    {
        
        public Dictionary<int, Guild> Guilds = new Dictionary<int, Guild>();  //字典方便精准查询用
        private HashSet<string> GuildNames = new HashSet<string>();
        
        public void Init()
        {
            this.Guilds.Clear();
            //把公会信息存储到内存中
            foreach(var guild in DBService.Instance.Entities.TGuilds)
            {
                this.AddGuild(new Guild(guild));
            }
        }

        private void AddGuild(Guild guild)
        {
            this.Guilds.Add(guild.Id, guild);
            this.GuildNames.Add(guild.Name);
            guild.timestamp = TimeUtil.timestamp;
        }

        public bool CheckNameExisted(string name)
        {
            return GuildNames.Contains(name); //Hashset进行重名检测
        }
        public bool CreateGuild(string name,string notice,Character leader)
        {
            DateTime now = DateTime.Now;
            TGuild dbGuild = DBService.Instance.Entities.TGuilds.Create();
            dbGuild.Name = name;
            dbGuild.Notice = notice;
            dbGuild.LeaderId = leader.Id;
            dbGuild.LeaderName = leader.Name;
            dbGuild.CreateTime = now;
            DBService.Instance.Entities.TGuilds.Add(dbGuild);

            Guild guild = new Guild(dbGuild);
            guild.AddMember(leader.Id, leader.Name, leader.Data.Class, leader.Data.Level, GuildTitle.President);
            leader.Guild = guild;
            DBService.Instance.Save();
            leader.Data.GuildId = dbGuild.Id;
            DBService.Instance.Save();
            this.AddGuild(guild);
            return true;

        }
        internal Guild GetGuild(int guildId)
        {
            if (guildId == 0)
            {
                return null;
            }
            Guild guild = null;
            this.Guilds.TryGetValue(guildId, out guild);
            return guild;
        }
        internal List<NGuildInfo> GetGuildsInfo()
        {
            List<NGuildInfo> result = new List<NGuildInfo>();
            foreach(var kv in this.Guilds)
            {
                result.Add(kv.Value.GuildInfo(null));  //null是from，最开始传的是无，因为没工会
            }
            return result;
        }
    }
}
