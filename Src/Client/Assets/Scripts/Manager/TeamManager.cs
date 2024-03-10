using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.Managers
{

    public class TeamManager : Singleton<TeamManager>
    {
        
        public void Init() { }
        
        public void UpdateTeamInfo(NTeamInfo team)
        {
            //��TeamService����
            User.Instance.TeamInfo = team;   //���û�֪���Լ��������Ϣ
            ShowTeamUI(team != null);    //��ʾ��UI��

        }
        public void ShowTeamUI(bool show)
        {
            if (UIMain.Instance != null)
                UIMain.Instance.ShowTeamUI(show);
        }
    }
}