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
    class TeamService : Singleton<TeamService>, IDisposable
    {
      
        public void Init() { }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }
        public TeamService()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        
        //����˷����������  ��UIFriends�Ľ�������
        public void SendTeamInviteRequest(int friendId, string friendName)
        {
            Debug.Log("SendTeamInviteRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteReq = new TeamInviteRequest();
            message.Request.teamInviteReq.FromId = User.Instance.CurrentCharacter.Id;
            message.Request.teamInviteReq.FromName = User.Instance.CurrentCharacter.Name;
            message.Request.teamInviteReq.ToId = friendId;
            message.Request.teamInviteReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);

        }
        //���ڻ�Ӧ���������Լ�����������Լ����ܻ��Ǿܾ�
        public void SendTeamInviteResponse(bool accept,TeamInviteRequest request)
        {
            Debug.Log("SendTeamInviteResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteRes = new TeamInviteResponse();
            message.Request.teamInviteRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.teamInviteRes.Errormsg = accept ? "��ӳɹ�" : "�Է��ܾ����������";
            message.Request.teamInviteRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }
        //��UITeam�Ľ�������
        public void SendTeamLeaveRequest(int id)
        {
            Debug.Log("SendTeamLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamLeave = new TeamLeaveRequest();
            message.Request.teamLeave.TeamId =User.Instance.TeamInfo.Id;
            message.Request.teamLeave.characterId = User.Instance.CurrentCharacter.Id;
            NetClient.Instance.SendMessage(message);

        }
        //�յ����˵�������󣬻�Ӧ����SendTeamInviteResponse
        private void OnTeamInviteRequest(object sender, TeamInviteRequest request)
        {
            var confirm = MessageBox.Show(string.Format("{0}������������", request.FromName), "�������", MessageBoxType.Confirm, "����", "�ܾ�");
            confirm.OnYes = () =>
            {
                this.SendTeamInviteResponse(true, request);
            };
            confirm.OnNo = () =>
            {
                this.SendTeamInviteResponse(false, request);
            };
        }
        //�յ����˵Ľ��ܻ�ܾ�����Ӧ
        private void OnTeamInviteResponse(object sender, TeamInviteResponse message)
        {
            if (message.Result == Result.Success)
                MessageBox.Show(message.Request.ToName + "�������Ķ���", "������ӳɹ�");
            else
                MessageBox.Show(message.Errormsg, "�������ʧ��");
        }
        private void OnTeamInfo(object sender, TeamInfoResponse message)
        {
            Debug.Log("OnTeamInfo");
            TeamManager.Instance.UpdateTeamInfo(message.Team);
        }
        private void OnTeamLeave(object sender, TeamLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                TeamManager.Instance.UpdateTeamInfo(null);

                MessageBox.Show("�˳��ɹ�", "�˳�����");
            }
            else
                MessageBox.Show("�˳�ʧ��", "�˳�����", MessageBoxType.Error);
        }
    }
}