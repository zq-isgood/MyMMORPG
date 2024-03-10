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
            //被TeamService调用
            User.Instance.TeamInfo = team;   //让用户知道自己的组队信息
            ShowTeamUI(team != null);    //显示在UI上

        }
        public void ShowTeamUI(bool show)
        {
            if (UIMain.Instance != null)
                UIMain.Instance.ShowTeamUI(show);
        }
    }
}