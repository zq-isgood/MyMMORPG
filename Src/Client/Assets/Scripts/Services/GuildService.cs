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
        //�����������¼�
        public UnityAction OnGuildUpdate; //�������
        public UnityAction<bool> OnGuildCreateResult;  //���ᴴ���ɹ�
        public UnityAction<List<NGuildInfo>> OnGuildListResult;  //�����б��������
        public void Init() { }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<GuildCreateResponse>(this.OnGuildCreate); //����
            MessageDistributer.Instance.Unsubscribe<GuildListResponse>(this.OnGuildList);  //�����б�
            MessageDistributer.Instance.Unsubscribe<GuildJoinRequest>(this.OnGuildJoinRequest);  //���������
            MessageDistributer.Instance.Unsubscribe<GuildJoinResponse>(this.OnGuildJoinResponse);  //�������Ӧ
            MessageDistributer.Instance.Unsubscribe<GuildResponse>(this.OnGuild);  //������Ϣˢ��
            MessageDistributer.Instance.Unsubscribe<GuildLeaveResponse>(this.OnGuildLeave);  //�뿪����
            MessageDistributer.Instance.Unsubscribe<GuildAdminResponse>(this.OnGuildAdmin); 
        }
        public GuildService()
        {
            MessageDistributer.Instance.Subscribe<GuildCreateResponse>(this.OnGuildCreate); //����
            MessageDistributer.Instance.Subscribe<GuildListResponse>(this.OnGuildList);  //�����б�
            MessageDistributer.Instance.Subscribe<GuildJoinRequest>(this.OnGuildJoinRequest);  //���������
            MessageDistributer.Instance.Subscribe<GuildJoinResponse>(this.OnGuildJoinResponse);  //�������Ӧ
            MessageDistributer.Instance.Subscribe<GuildResponse>(this.OnGuild);  //������Ϣˢ��
            MessageDistributer.Instance.Subscribe<GuildLeaveResponse>(this.OnGuildLeave);  //�뿪����
            MessageDistributer.Instance.Subscribe<GuildAdminResponse>(this.OnGuildAdmin);
        }

        
        
        public void SendGuildCreate(string guildName,string notice)
        {
            Debug.Log("SendGuildCreate");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.guildCreate = new GuildCreateRequest();  //�ͻ��˹�������������,�ڷ������GuildCreateRequest���Э��
            message.Request.guildCreate.GuildName = guildName;
            message.Request.guildCreate.GuildNotice = notice;
            NetClient.Instance.SendMessage(message);

        }
        //�յ���������Ӧ
        private void OnGuildCreate(object sender, GuildCreateResponse response)
        {
            Debug.LogFormat("OnGuildCreateResponse:{0}",response.Result);
            if (OnGuildCreateResult != null)
            {
                this.OnGuildCreateResult(response.Result == Result.Success);  //ί��
            }
            if (response.Result == Result.Success)
            {
                GuildManager.Instance.Init(response.guildInfo);
                MessageBox.Show(String.Format("{0}���ᴴ���ɹ�", response.guildInfo.GuildName), "����");
            }
            else
                MessageBox.Show(String.Format("{0}���ᴴ��ʧ��", response.guildInfo.GuildName), "����");
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
        //�᳤�յ����˷�������,����˷��������Ľ��
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
        //�᳤�յ��ӹ����������Ӧ
        private void OnGuildJoinRequest(object sender, GuildJoinRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0}������빫��", request.Apply.Name), "��������", MessageBoxType.Confirm, "����", "�ܾ�");
            confirm.OnYes = () =>
            {
                this.SendGuildJoinResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendGuildJoinResponse(false, request);
            };
        }
        //�������յ�����������
        private void OnGuildJoinResponse(object sender, GuildJoinResponse message)
        {
            Debug.LogFormat("OnGuildJoinResponse:{0}", message.Result);
            if (message.Result == Result.Success)
            {
                MessageBox.Show("���빫��ɹ�", "����");
            }
            else
                MessageBox.Show("���빫��ʧ��", "����");
        }
        //�κ�ʱ�򹫻���Ϣ�仯����ˢ��
        private void OnGuild(object sender, GuildResponse message)
        {

            Debug.LogFormat("OnGuild:{0} {1} ��{2}", message.Result,message.guildInfo.Id,message.guildInfo.GuildName);
            GuildManager.Instance.Init(message.guildInfo);
            if (this.OnGuildUpdate != null)
                this.OnGuildUpdate();  //ί��
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

                MessageBox.Show("�뿪����ɹ�", "����");
            }
            else
                MessageBox.Show("�뿪����ʧ��", "����", MessageBoxType.Error);
        }
        //���󹫻��б�
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
                this.OnGuildListResult(response.Guilds); //ί��
            }

        }
        //���ͼ��빫��������UI��UIGuildApplyList�Ľ��ܺ;ܾ���ť����
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
            MessageBox.Show(string.Format("ִ�в�����{0} �����{1} {2}", message.Command, message.Result,message.Errormsg));
        }
    }
}