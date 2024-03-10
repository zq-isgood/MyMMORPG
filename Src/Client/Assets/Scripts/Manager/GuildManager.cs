using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GuildManager : Singleton<GuildManager>
{
    public NGuildInfo guildInfo;
    //�������ڹ����е���Ϣ���ҵ���ݵ�
    public NGuildMemberInfo myMemberInfo;  //N��ͷ��������ͨѶ���ڴ�
    public bool HasGuild
    {
        get { return this.guildInfo != null; }
    }
    public void Init(NGuildInfo guild)
    {
        this.guildInfo = guild; //��¼ʱ�����һ�γ�ʼ��
        //�����û�й��ᣬ myMemberInfo��û��ֵ������У��ͱ��������Ա�����ҳ�������Ϣ���
        if (guild == null)
        {
            myMemberInfo = null;
            return;
        }
        foreach(var mem in guild.Members)
        {
            if (mem.characterId == User.Instance.CurrentCharacter.Id)
            {
                myMemberInfo = mem;
                break;
            }
        }

    }
    public void ShowGuild()
    {
        if (this.HasGuild)
        {
            UIManager.Instance.Show<UIGuild>();
        }
        else
        {
            var win = UIManager.Instance.Show<UIGuildPopNoGuild>();
            win.OnClose += PopNoGuild_OnClose;

        }
    }

    private void PopNoGuild_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        if (result == UIWindow.WindowResult.Yes)
        {
            UIManager.Instance.Show<UIGuildPopCreate>();
        }
        else if(result == UIWindow.WindowResult.No)
        {
            UIManager.Instance.Show<UIGuildList>();
        }
    }
}