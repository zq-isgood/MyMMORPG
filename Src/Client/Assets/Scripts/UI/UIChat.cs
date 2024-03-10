using Assets.Scripts.Managers;
using Candlelight.UI;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    public TextMeshProUGUI chatTarget;
    public TMP_InputField chatText;

    public HyperText textArea;//聊天内容显示区域
    public TabView cannelTab;
    public Dropdown channelSelect;

    //上面是显示频道，下面的dropdown是发送频道
    private void Start()
    {
        this.cannelTab.OnTabSelect += OnDisplayChannelSelected;  //选择频道
        ChatManager.Instance.OnChat += RefreshUI; //当有消息发过来时，更新UI
    }
    private void OnDestroy()
    {
        ChatManager.Instance.OnChat -= RefreshUI; //当界面关闭时，取消订阅
    }
    private void Update()
    {
        InputManager.Instance.isInputMode = chatText.isFocused; //检测聊天框有没有焦点，有就是在输入模式
    }
    void OnDisplayChannelSelected(int idx)
    {
        ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)idx;
        RefreshUI();
    }

    public void RefreshUI()
    {
        this.textArea.text = ChatManager.Instance.GetCurrentMessages();
        this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        if (ChatManager.Instance.SendChannel == SkillBridge.Message.ChatChannel.Private)
        {
            chatTarget.gameObject.SetActive(true);
            if (ChatManager.Instance.PrivateID != 0)
            {
                chatTarget.text = ChatManager.Instance.PrivateName + ":";

            }
            else
            {
                chatTarget.text = "<无>";
            }
        }
        else
        {
            chatTarget.gameObject.SetActive(false);
        }

    }
    public void OnClickChatLink(HyperText text,HyperText.LinkInfo link)
    {
        if (string.IsNullOrEmpty(link.Name))
            return;
        //<a name="c:1001:Name" class="player">Name</a>
        if (link.Name.StartsWith("c:"))
        {
            string[] strs = link.Name.Split(":".ToCharArray());
            UIPopCharMenu menu = UIManager.Instance.Show<UIPopCharMenu>();
            menu.targetId = int.Parse(strs[1]);
            menu.targetName = strs[2];
        }
    }
    public void OnClickSend()
    {
        OnEndInput(chatText.text);
    }

    public void OnEndInput(string text)
    {
        if (!string.IsNullOrEmpty(text.Trim()))
        {
            this.SendChat(text);
        }
        chatText.text = "";
    }
    void SendChat(string content)
    {
        ChatManager.Instance.SendChat(content, ChatManager.Instance.PrivateID, ChatManager.Instance.PrivateName);
    }
    //如果没设置频道成功，就把值恢复过去，比如没有队伍，但是要把频道设置为队伍频道，则返回false
    public void OnSendChannelChanged(int idx)
    {
        if (ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(idx + 1))
            return;  //如果已经是了，就不用设置
        if(!ChatManager.Instance.SetSendChannel((ChatManager.LocalChannel)(idx + 1)))
        {
            channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        }
        else
        {
            RefreshUI();
        }
    }
}