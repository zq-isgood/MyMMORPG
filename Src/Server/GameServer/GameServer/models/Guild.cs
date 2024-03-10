using Common;
using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utils;
using GameServer.Services;
using GameServer.Managers;
using System.Reflection;

namespace GameServer.Models
{
    
    class Guild
    {
        
        public double timestamp; 
        public string Name { get { return this.Data.Name; } }
        public int Id { get { return this.Data.Id; } }

        public TGuild Data; //公会数据库的映射方便取数据,内存
        public Guild(TGuild guild)
        {
            this.Data = guild;
        }
        internal bool JoinApply(NGuildApplyInfo apply)
        {
            //判断这个人是不是申请了一次，超过一次就不能再申请
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId);
            if (oldApply != null && oldApply.Result!=0)
            {
                
                return false;
            }
            //如果没申请过，则创建新的一个记录  只操作DB
            var dbApply = DBService.Instance.Entities.TGuildApplies.Create();
            dbApply.GuildId = apply.GuildId;
            dbApply.CharacterId = apply.characterId;
            dbApply.Name = apply.Name;
            dbApply.Class= apply.Class;
            dbApply.Level = apply.Level;
            dbApply.ApplyTime = DateTime.Now;
           
            this.Data.Applies.Add(dbApply);
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;
        }

        internal bool JoinAppove(NGuildApplyInfo apply)
        {
            
            var oldApply = this.Data.Applies.FirstOrDefault(v => v.CharacterId == apply.characterId && v.Result == 0);
            if (oldApply == null)
            {
                return false;
            }
            oldApply.Result = (int)apply.Result;
            if (apply.Result == ApplyResult.Accept)
            {
                this.AddMember(apply.characterId, apply.Name, apply.Class, apply.Level, GuildTitle.None);
            }
            DBService.Instance.Save();
            this.timestamp = TimeUtil.timestamp;
            return true;

        }

        public void AddMember(int characterId, string name, int @class, int level, GuildTitle title)
        {
            DateTime now = DateTime.Now;
            TGuildMember dbMember = new TGuildMember()
            {
                CharacterId = characterId,
                Name = name,
                Class = @class,
                Level = level,
                Title = (int)title,
                JoinTime = now,
                LastTime = now
            };
            this.Data.Members.Add(dbMember);
            //之前的问题的解决：公会有成员信息，但是该成员的公会信息并没有加到成员自己的信息中，通过在TCharacter中赋值GuildId就能找到公会
            //找到成员
            var character = CharacterManager.Instance.GetCharacter(characterId);
            //角色在线时
            if (character != null)
            {
                character.Data.GuildId = this.Id;
            }
            //角色不在线则存在数据库里
            else
            {
                TCharacter dbchar = DBService.Instance.Entities.Characters.SingleOrDefault(c => c.ID == characterId);
                dbchar.GuildId = this.Id;
            }
            this.timestamp = TimeUtil.timestamp;
        }
        public void Leave(Character member)
        {
            Log.InfoFormat("Leave Guild:{0}:{1}", member.Id, member.Info.Name);
            timestamp = TimeUtil.timestamp;
            //与AddMember几乎一一对应，把公会的该成员信息删除，把该成员的公会Id删除
        }


        public void PostProcess(Character from,NetMessageResponse message)
        {
         
             if (message.Guild == null)
             {
                message.Guild = new GuildResponse();
                message.Guild.Result = Result.Success;
                message.Guild.guildInfo = this.GuildInfo(from);  //填工会信息

             }
        }
        internal NGuildInfo GuildInfo(Character from)
        {
            //公会信息创建一份
            NGuildInfo info = new NGuildInfo()
            {
                Id = this.Id,
                GuildName = this.Name,
                Notice = this.Data.Notice,
                leaderId = this.Data.LeaderId,
                LeaderName = this.Data.LeaderName,
                createTime = (long)TimeUtil.GetTimestamp(this.Data.CreateTime),
                memberCount = this.Data.Members.Count
            };
            if (from != null)
            {
                //当from有值时，只有当前成员才能看公会成员信息,info是当前成员所掌握的工会信息
                info.Members.AddRange(GetMemberInfos());
                if (from.Id == this.Data.LeaderId)
                {
                    info.Applies.AddRange(GetApplyInfos());  //只有队长才有申请信息
                }
                /////////////////自己加的，已解决问题
                foreach(var mem in this.Data.Members)
                {
                    if (mem.Title == (int)GuildTitle.VicePresident)
                    {
                        if (mem.Id == from.Id)
                        {
                            info.Applies.AddRange(GetApplyInfos());
                        }
                    }
                }
                //////////////////////
            }
            return info;
        }

        

        List<NGuildMemberInfo> GetMemberInfos()
        {
            //遍历数据库的记录生成网络需要的记录,this.Data.Members数据库的列表
            List<NGuildMemberInfo> members = new List<NGuildMemberInfo>();
            foreach(var member in this.Data.Members)
            {
                var memberInfo = new NGuildMemberInfo()
                {
                    Id = member.Id,
                    characterId=member.CharacterId,
                    Title=(GuildTitle)member.Title,
                    joinTime=(long)TimeUtil.GetTimestamp(member.JoinTime),
                    lastTime = (long)TimeUtil.GetTimestamp(member.LastTime),

                };
                //如果成员在线，把成员的信息更新到公会的信息中
                var character = CharacterManager.Instance.GetCharacter(member.CharacterId);
                if (character != null)
                {
                    memberInfo.Info = character.GetBasicInfo();
                    memberInfo.Status = 1;
                    member.Name = character.Data.Name;
                    member.Level = character.Data.Level;
                    member.LastTime = DateTime.Now;

                }
                else
                {
                    //不在线的话就把在线状态设为0
                    memberInfo.Info = this.GetMemberInfo(member);
                    memberInfo.Status = 0;

                }
                members.Add(memberInfo);
            }
            return members;
        }

        NCharacterInfo GetMemberInfo(TGuildMember member)
        {
            return new NCharacterInfo()
            {
                Id = member.CharacterId,
                Name = member.Name,
                Class = (CharacterClass)member.Class,
                Level = member.Level,
            };
        }
        //申请的信息把DB转成网络的
        List<NGuildApplyInfo> GetApplyInfos()
        {
            List<NGuildApplyInfo> applies = new List<NGuildApplyInfo>();
            foreach(var apply in this.Data.Applies)
            {
                //当审批信息处理过，就不添加在列表里了
                if (apply.Result != (int)ApplyResult.None) continue;
                applies.Add(new NGuildApplyInfo()
                {
                    GuildId = apply.GuildId,
                    characterId = apply.CharacterId,
                    Name = apply.Name,
                    Class = apply.Class,
                    Level = apply.Level,
                    Result = (ApplyResult)apply.Result
                });
                
            }
            return applies;
            
        }

        //添加一个找成员的方法
        TGuildMember GetDBMember(int characterId)
        {
            foreach(var member in this.Data.Members)
            {
                if (member.CharacterId == characterId)
                    return member;
            }
            return null;
        }

        //无论成员在不在线，操作都要生效，所以要操作的是数据库
        internal void ExecuteAdmin(GuildAdminCommand command, int targetId, int sourceId)
        {
            var target = GetDBMember(targetId);
            var source = GetDBMember(sourceId);
            switch (command)
            {
                case GuildAdminCommand.Promote:
                    target.Title = (int)GuildTitle.VicePresident;
                    break;
                case GuildAdminCommand.Depost:
                    target.Title = (int)GuildTitle.None;
                    break;
                case GuildAdminCommand.Transfer:
                    source.Title=(int)GuildTitle.None;
                    target.Title = (int)GuildTitle.President;
                    this.Data.LeaderName = target.Name;
                    this.Data.LeaderId = targetId;
                    break;
                case GuildAdminCommand.Kickout:
                    //
                    break;
            }
            DBService.Instance.Save();
            timestamp = TimeUtil.timestamp; //这个相当于调用后处理了
        }
    }
}
