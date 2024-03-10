using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
namespace Assets.Scripts.Managers
{
     
    public class ChatManager : Singleton<ChatManager>
    {
        public enum LocalChannel
        {
            All=0,
            Local=1,
            World=2,
            Team=3,
            Guild=4,
            Private=5,
        }
        private ChatChannel[] ChannelFilter = new ChatChannel[6]
        {
            ChatChannel.Local|ChatChannel.World|ChatChannel.Guild|ChatChannel.Team|ChatChannel.Private|ChatChannel.System,
            ChatChannel.Local,
            ChatChannel.World,
            ChatChannel.Team,
            ChatChannel.Guild,
            ChatChannel.Private
        };
        public List<ChatMessage>[] Messages = new List<ChatMessage>[6]
        {
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>()
        }; //当前本地所有聊天信息存在这里
        public Action OnChat { get; internal set; }
        public LocalChannel sendChannel;
        public ChatChannel SendChannel 
        { 
            get {
                switch (sendChannel)
                {
                    case LocalChannel.Local: return ChatChannel.Local;
                    case LocalChannel.World: return ChatChannel.World;
                    case LocalChannel.Team: return ChatChannel.Team;
                    case LocalChannel.Guild: return ChatChannel.Guild;
                    case LocalChannel.Private: return ChatChannel.Private;
                } 
                return ChatChannel.Local;
            }
        }

        public LocalChannel displayChannel;
        
        public int PrivateID = 0;
        public string PrivateName = "";
        public void Init() 
        {
            foreach(var messages in this.Messages)
            {
                messages.Clear();
            }
        }
        public void SendChat(string content,int toId=0,string toName = "")
        {
            ChatService.Instance.SendChat(this.SendChannel, content, toId, toName);
        }
        public bool SetSendChannel(LocalChannel Channel)
        {
            if (Channel == LocalChannel.Team)
            {
                if (User.Instance.TeamInfo == null)
                {
                    this.AddSystemMessage("你没有加入任何队伍");
                    return false;
                }
            }
            if (Channel == LocalChannel.Guild)
            {
                if (User.Instance.CurrentCharacter.Guild == null)
                {
                    this.AddSystemMessage("你没有加入任何公会");
                    return false;
                }
            }
            
            this.sendChannel = Channel;
            Debug.LogFormat("Send Channel {0}", this.sendChannel);
            return true;
        }

        public void AddSystemMessage(string message ,string from="")
        {
            this.Messages[(int)LocalChannel.All].Add(new ChatMessage()
            {
                Channel = ChatChannel.System,
                Message = message,
                FromName=from
            });
            if (this.OnChat != null)
                this.OnChat();  //相当于刷新界面
        }

        public string GetCurrentMessages()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var message in this.Messages[(int)displayChannel])
            {
                sb.AppendLine(FormatMessage(message));
            }
            return sb.ToString();
        }
        private string FormatMessage(ChatMessage message)
        {
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    return string.Format("[本地]{0}{1}", FormatFromPlayer(message), message.Message);
                case ChatChannel.World:
                    return string.Format("<color=cyan>[世界]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.System:
                    return string.Format("<color=yellow>[系统]{0}</color>", message.Message);
                case ChatChannel.Private:
                    return string.Format("<color=magenta>[私聊]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Team:
                    return string.Format("<color=green>[队伍]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Guild:
                    return string.Format("<color=blue>[公会]{0}{1}</color>", FormatFromPlayer(message), message.Message);
            }
            return "";
        }

        private string FormatFromPlayer(ChatMessage message)
        {
            if (message.FromId == User.Instance.CurrentCharacter.Id)
            {
                return "<a name=\"\" class=\"player\">[我]</a>";
            }
            else
                return string.Format("<a name=\"c:{0}:{1}\" class=\"player\">[{1}]</a>", message.FromId, message.FromName);
        }
        internal void StartPrivateChat(int targetId,string targetName)
        {
            PrivateID = targetId;
            PrivateName = targetName;
            sendChannel = LocalChannel.Private;
            if (this.OnChat != null)
                OnChat();
        }
        public void AddMessages(ChatChannel channel,List<ChatMessage> messages)
        {
            for(int ch = 0; ch < 6; ch++)
            {
                if ((this.ChannelFilter[ch] & channel) == channel)
                {
                    this.Messages[ch].AddRange(messages);
                }
            }
            if (this.OnChat != null)
            {
                this.OnChat();
            }
        }
    }

}