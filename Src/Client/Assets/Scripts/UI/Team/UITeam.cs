using Common.Data;
using JetBrains.Annotations;
using Managers;
using Models;
using Services;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UITeam : MonoBehaviour
{
    public TextMeshProUGUI teamTitle;
    public UITeamItem[] Members;
    public ListView list;
    private void Start()
    {
        if (User.Instance.TeamInfo == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        foreach(var item in Members)
        {
            this.list.AddItem(item);
        }
    }
    private void OnEnable()
    {
        //其他情况下打开组队界面
        UpdateTeamUI();
    }
    public void ShowTeam(bool show)
    {
        //UIMain的函数来调用它
        this.gameObject.SetActive(show);
        if (show)
        {
            UpdateTeamUI();
        }
    }
    public void UpdateTeamUI()
    {
        if (User.Instance.TeamInfo == null) return;
        this.teamTitle.text = string.Format("我的队伍({0}/5)", User.Instance.TeamInfo.Members.Count);
        for(int i=0; i<5; i++)
        {
            if (i < User.Instance.TeamInfo.Members.Count)
            {
                //TeamInfo.Members是NCharacterInfo,表示成员是谁,而this.Members是UITeamItem
                this.Members[i].SetMemberInfo(i, User.Instance.TeamInfo.Members[i], User.Instance.TeamInfo.Members[i].Id == User.Instance.TeamInfo.Leader);
                this.Members[i].gameObject.SetActive(true);
            }
            else
                this.Members[i].gameObject.SetActive(false);
        }
    }
    public void OnClickLeave()
    {
        MessageBox.Show("确定要离开队伍吗?", "离开队伍", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            TeamService.Instance.SendTeamLeaveRequest(User.Instance.TeamInfo.Id);
        };
    }

}