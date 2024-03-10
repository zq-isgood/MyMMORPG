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

    public HyperText textArea;//����������ʾ����
    public TabView cannelTab;
    public Dropdown channelSelect;

    //��������ʾƵ���������dropdown�Ƿ���Ƶ��
    private void Start()
    {
        this.cannelTab.OnTabSelect += OnDisplayChannelSelected;  //ѡ��Ƶ��
        ChatManager.Instance.OnChat += RefreshUI; //������Ϣ������ʱ������UI
    }
    private void OnDestroy()
    {
        ChatManager.Instance.OnChat -= RefreshUI; //������ر�ʱ��ȡ������
    }
    private void Update()
    {
        InputManager.Instance.isInputMode = chatText.isFocused; //����������û�н��㣬�о���������ģʽ
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
                chatTarget.text = "<��>";
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
    //���û����Ƶ���ɹ����Ͱ�ֵ�ָ���ȥ������û�ж��飬����Ҫ��Ƶ������Ϊ����Ƶ�����򷵻�false
    public void OnSendChannelChanged(int idx)
    {
        if (ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(idx + 1))
            return;  //����Ѿ����ˣ��Ͳ�������
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