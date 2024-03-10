using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GuildManager : Singleton<GuildManager>
{
    public NGuildInfo guildInfo;
    //包含我在公会中的信息，我的身份等
    public NGuildMemberInfo myMemberInfo;  //N开头的类型是通讯的内存
    public bool HasGuild
    {
        get { return this.guildInfo != null; }
    }
    public void Init(NGuildInfo guild)
    {
        this.guildInfo = guild; //登录时候调用一次初始化
        //如果我没有公会， myMemberInfo就没有值，如果有，就遍历公会成员把我找出来，信息填充
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