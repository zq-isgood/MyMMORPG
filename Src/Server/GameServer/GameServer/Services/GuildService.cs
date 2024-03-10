using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class GuildService : Singleton<GuildService>
    {

        public GuildService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildCreateRequest>(this.OnGuildCreate); 
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildListRequest>(this.OnGuildList);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildLeaveRequest>(this.OnGuildLeave);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<GuildAdminRequest>(this.OnGuildAdmin);

        }

        

        public void Init()
        {
            GuildManager.Instance.Init();
        }

        //实实在在去判断能不能创建公会，并调用GuildManager去创建
        private void OnGuildCreate(NetConnection<NetSession> sender, GuildCreateRequest request)
        {
            Character character = sender.Session.Character; //代表我自己
            Log.InfoFormat("OnGuildCreate::GuildName:{0}  Character:[{1}] {2}", request.GuildName, character.Id, character.Name);
            sender.Session.Response.guildCreate = new GuildCreateResponse();
            if (character.Guild != null)
            {

                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "已经有公会";
                sender.SendResponse();
                return;
            }
            if (GuildManager.Instance.CheckNameExisted(request.GuildName))
            {
                sender.Session.Response.guildCreate.Result = Result.Failed;
                sender.Session.Response.guildCreate.Errormsg = "公会名字已存在";
                sender.SendResponse();
                return;
            }
            GuildManager.Instance.CreateGuild(request.GuildName, request.GuildNotice, character);
            sender.Session.Response.guildCreate.guildInfo = character.Guild.GuildInfo(character);
            sender.Session.Response.guildCreate.Result = Result.Success;
            sender.SendResponse();
        }
        private void OnGuildList(NetConnection<NetSession> sender, GuildListRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildList::character:[{0}]{1}", character.Id, character.Name);
            sender.Session.Response.guildList = new GuildListResponse();
            sender.Session.Response.guildList.Guilds.AddRange(GuildManager.Instance.GetGuildsInfo());
            sender.Session.Response.guildList.Result = Result.Success;
            sender.SendResponse();
        }


        private void OnGuildJoinRequest(NetConnection<NetSession> sender, GuildJoinRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinRequest::GuildId:{0} characterId:[{1}]{2}", request.Apply.GuildId, request.Apply.characterId, request.Apply.Name);
            var guild = GuildManager.Instance.GetGuild(request.Apply.GuildId);
            if (guild == null)
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "公会不存在";
                sender.SendResponse();
                return;
            }
            request.Apply.characterId = character.Data.ID;
            request.Apply.Name = character.Data.Name;
            request.Apply.Class = character.Data.Class;
            request.Apply.Level = character.Data.Level;
            if (guild.JoinApply(request.Apply))
            {
                //先由guild的JoinApply把申请加入列表
                //然后判断会长在不在线，在线就把请求发给会长
                var leader = SessionManager.Instance.GetSession(guild.Data.LeaderId);
                if (leader != null)
                {
                    leader.Session.Response.guildJoinReq = request;
                    leader.SendResponse();

                }
                //////自己加的 已解决问题
                foreach(var mem in guild.Data.Members)
                {
                    if (mem.Title == (int)GuildTitle.VicePresident)
                    {
                        var admin = SessionManager.Instance.GetSession(mem.Id);
                        admin.Session.Response.guildJoinReq = request;
                        admin.SendResponse();
                    }
                }
                ///////////////////
            }
            else
            {
                sender.Session.Response.guildJoinRes = new GuildJoinResponse();
                sender.Session.Response.guildJoinRes.Result = Result.Failed;
                sender.Session.Response.guildJoinRes.Errormsg = "请勿重复申请";
                sender.SendResponse();
            }
        }
        //加入公会申请通过的响应
        private void OnGuildJoinResponse(NetConnection<NetSession> sender, GuildJoinResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildJoinResponse::GuildId:{0} characterId:{[1}]{2}", response.Apply.GuildId, response.Apply.characterId, response.Apply.Name);
            var guild = GuildManager.Instance.GetGuild(response.Apply.GuildId);
            if (response.Result == Result.Success)
            {
                guild.JoinAppove(response.Apply);
            }
            //查看原来申请工会的人在不在线，在线就发消息
            var requester = SessionManager.Instance.GetSession(response.Apply.characterId);
            if (requester != null)
            {
                requester.Session.Character.Guild = guild;
                requester.Session.Response.guildJoinRes = response;
                requester.Session.Response.guildJoinRes.Result = Result.Success;
                requester.Session.Response.guildJoinRes.Errormsg = "加入公会成功";
                requester.SendResponse();
            }
        }
        private void OnGuildLeave(NetConnection<NetSession> sender, GuildLeaveRequest request)
        {
            Character character = sender.Session.Character; 
            Log.InfoFormat("OnGuildLeave::character:{0}", character.Id);
            sender.Session.Response.guildLeave = new GuildLeaveResponse();
            character.Guild.Leave(character);
            sender.Session.Response.guildLeave.Result = Result.Success;
            DBService.Instance.Save();
            sender.SendResponse();
        }

        private void OnGuildAdmin(NetConnection<NetSession> sender, GuildAdminRequest message)
        {
            //找到发送协议的角色
            Character character = sender.Session.Character;
            Log.InfoFormat("OnGuildAdmin:character:{0}", character.Id);
            //构建要发送的响应
            sender.Session.Response.guildAdmin = new GuildAdminResponse();
            //先判断当前角色有没有公会
            if (character.Guild == null)
            {
                sender.Session.Response.guildAdmin.Result = Result.Failed;
                sender.Session.Response.guildAdmin.Errormsg = "你没公会不要乱来";
                sender.SendResponse();
                return;
            }
            //在公会类里进行操作命令
            character.Guild.ExecuteAdmin(message.Command, message.Target, character.Id);
            //看看调用的目标在不在线，在线就把结果发送过去
            var target = SessionManager.Instance.GetSession(message.Target);
            if (target != null)
            {
                sender.Session.Response.guildAdmin = new GuildAdminResponse();
                sender.Session.Response.guildAdmin.Result = Result.Success;
                sender.Session.Response.guildAdmin.Command = message;
                sender.SendResponse();
            }
            sender.Session.Response.guildAdmin.Result = Result.Success;
            sender.Session.Response.guildAdmin.Command = message;
            sender.SendResponse();
        }
    }
}
