using Assets.Scripts.Managers;
using Common.Data;
using Entities;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    class GuildService : Singleton<GuildService>, IDisposable
    {
        //监听了三个事件
        public UnityAction OnGuildUpdate; //工会更新
        public UnityAction<bool> OnGuildCreateResult;  //公会创建成功
        public UnityAction<List<NGuildInfo>> OnGuildListResult;  //工会列表请求更新
        public void Init() { }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate); //创建
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);  //请求列表
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);  //加入的请求
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);  //加入的响应
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);  //公会信息刷新
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);  //离开公会
            MessageDistributer.Instance.Unsubscribe<GuildAdminResponse>(this.OnGuildAdmin); 
        }
        public GuildService()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate); //创建
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);  //请求列表
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);  //加入的请求
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);  //加入的响应
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);  //公会信息刷新
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);  //离开公会
            MessageDistributer.Instance.Subscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        
        
        public void SendGuildCreate(string guildName,string notice)
        {
            Debug.Log("SendGuildCreate");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildCreate = new GuildCreateRequest();  //客户端构建创建的请求,在服务端找GuildCreateRequest这个协议
            message.Request.guildCreate.GuildName = guildName;
            message.Request.guildCreate.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);

        }
        //收到创建的响应
        private void OnGuildCreate(object sender, GuildCreateResponse response)
        {
            Debug.LogFormat("OnGuildCreateResponse:{0}",response.Result);
            if (OnGuildCreateResult != null)
            {
                this.OnGuildCreateResult(response.Result == Result.Success);  //委托
            }
            if (response.Result == Result.Success)
            {
                GuildManager.Instance.Init(response.guildInfo);
                MessageBox.Show(String.Format("{0}公会创建成功", response.guildInfo.GuildName), "公会");
            }
            else
                MessageBox.Show(String.Format("{0}公会创建失败", response.guildInfo.GuildName), "公会");
        }
        
        public void SendGuildJoinRequest(int guildId)
        {
            Debug.Log("SendGuildJoinRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinReq = new GuildJoinRequest();
            message.Request.guildJoinReq.Apply = new NGuildApplyInfo();
            message.Request.guildJoinReq.Apply.GuildId = guildId;
            NetClient.Instance.SendMessage(message);
        }
        //会长收到别人发的申请,向别人发送审批的结果
        public void SendGuildJoinResponse(bool accept,GuildJoinRequest request)
        {
            Debug.Log("SendGuildJoinResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = request.Apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);

        }
        //会长收到加工会请求的响应
        private void OnGuildJoinRequest(object sender, GuildJoinRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0}申请加入公会", request.Apply.Name), "公会申请", MessageBoxType.Confirm, "接受", "拒绝");
            confirm.OnYes = () =>
            {
                this.SendGuildJoinResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendGuildJoinResponse(false, request);
            };
        }
        //申请者收到工会申请结果
        private void OnGuildJoinResponse(object sender, GuildJoinResponse message)
        {
            Debug.LogFormat("OnGuildJoinResponse:{0}", message.Result);
            if (message.Result == Result.Success)
            {
                MessageBox.Show("加入公会成功", "公会");
            }
            else
                MessageBox.Show("加入公会失败", "公会");
        }
        //任何时候公会信息变化，都刷新
        private void OnGuild(object sender, GuildResponse message)
        {

            Debug.LogFormat("OnGuild:{0} {1} ：{2}", message.Result,message.guildInfo.Id,message.guildInfo.GuildName);
            GuildManager.Instance.Init(message.guildInfo);
            if (this.OnGuildUpdate != null)
                this.OnGuildUpdate();  //委托
        }

        public void SendGuildLeaveRequest()
        {
            Debug.Log("SendGuildLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildLeave = new GuildLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }
        private void OnGuildLeave(object sender, GuildLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                GuildManager.Instance.Init(null);

                MessageBox.Show("离开公会成功", "公会");
            }
            else
                MessageBox.Show("离开公会失败", "公会", MessageBoxType.Error);
        }
        //请求公会列表
        public void SendGuildListRequest()
        {
            Debug.Log("SendGuildListRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildList = new GuildListRequest();
            NetClient.Instance.SendMessage(message);
        }
        private void OnGuildList(object sender, GuildListResponse response)
        {
            if (OnGuildListResult!=null)
            {
                this.OnGuildListResult(response.Guilds); //委托
            }

        }
        //发送加入公会审批，UI的UIGuildApplyList的接受和拒绝按钮触发
        public void SendGuildJoinApply(bool accept, NGuildApplyInfo apply)
        {
            Debug.Log("SendGuildJoinApply");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildJoinRes = new GuildJoinResponse();
            message.Request.guildJoinRes.Result = Result.Success;
            message.Request.guildJoinRes.Apply = apply;
            message.Request.guildJoinRes.Apply.Result = accept ? ApplyResult.Accept : ApplyResult.Reject;
            NetClient.Instance.SendMessage(message);

        }

        public void SendAdminCommand(GuildAdminCommand command,int characterId)
        {
            Debug.Log("SendAdminCommand");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildAdmin= new GuildAdminRequest();
            message.Request.guildAdmin.Command = command;
            message.Request.guildAdmin.Target = characterId;
            NetClient.Instance.SendMessage(message);

        }
        private void OnGuildAdmin(object sender,GuildAdminResponse message)
        {
            Debug.LogFormat("GuildAdmin:{0} {1}", message.Command, message.Result);
            MessageBox.Show(string.Format("执行操作：{0} 结果：{1} {2}", message.Command, message.Result,message.Errormsg));
        }
    }
}