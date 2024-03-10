using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{
    public TextMeshProUGUI avatarName;
    public TextMeshProUGUI avatarLevel;
    public UITeam TeamWindow;

    protected override void OnStart()
    {
        this.UpdateAvatar();
    }
    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }
 

    public void OnClickTest()
    {
        UITest test=UIManager.Instance.Show<UITest>();
        test.SetTitle("����һ������UI");
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("����˶Ի���ģ� " + result, "�Ի�����Ӧ���", MessageBoxType.Information);
    }
    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }
    public void OnClickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }
    public void OnClickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }
    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }
    public void ShowTeamUI(bool show)
    {
       //��TeamManager����
        TeamWindow.ShowTeam(show);
    }
    public void OnClickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }
    public void OnClickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }
    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();  //�����ť���ؽ��������
    }
    public void OnClickSkill()
    {

    }


}
