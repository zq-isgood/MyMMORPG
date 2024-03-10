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
        //��������´���ӽ���
        UpdateTeamUI();
    }
    public void ShowTeam(bool show)
    {
        //UIMain�ĺ�����������
        this.gameObject.SetActive(show);
        if (show)
        {
            UpdateTeamUI();
        }
    }
    public void UpdateTeamUI()
    {
        if (User.Instance.TeamInfo == null) return;
        this.teamTitle.text = string.Format("�ҵĶ���({0}/5)", User.Instance.TeamInfo.Members.Count);
        for(int i=0; i<5; i++)
        {
            if (i < User.Instance.TeamInfo.Members.Count)
            {
                //TeamInfo.Members��NCharacterInfo,��ʾ��Ա��˭,��this.Members��UITeamItem
                this.Members[i].SetMemberInfo(i, User.Instance.TeamInfo.Members[i], User.Instance.TeamInfo.Members[i].Id == User.Instance.TeamInfo.Leader);
                this.Members[i].gameObject.SetActive(true);
            }
            else
                this.Members[i].gameObject.SetActive(false);
        }
    }
    public void OnClickLeave()
    {
        MessageBox.Show("ȷ��Ҫ�뿪������?", "�뿪����", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () =>
        {
            TeamService.Instance.SendTeamLeaveRequest(User.Instance.TeamInfo.Id);
        };
    }

}