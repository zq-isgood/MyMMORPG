using Common.Data;
using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//用于设置ui中间的面板信息
public class UIQuestInfo:MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI[] targets;
    public TextMeshProUGUI description;
    public UIIconItem rewardItems;
    public TextMeshProUGUI rewardMoney;
    public TextMeshProUGUI rewardExp;
    //public Button navButtton;
    //private int npc = 0;
    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}", quest.Define.Type, quest.Define.Name);
        if (quest.Info == null)
        {
            this.description.text = quest.Define.Overview;       
        }
        else
        {
            if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
            {
                this.description.text = quest.Define.Overview;
            }
        }
        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();
        //if (quest.Info == null)
        //{
        //    this.npc=quest.Define.AcceptNPC; //任务是空的则找接受的npc

        //}
        //else if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
        //{
        //    //任务完成了则找完成任务的npc
        //    this.npc = quest.Define.SubmitNPC;
        //}
        //this.navButtton.gameObject.SetActive(this.npc > 0);  //如果没有npc则不显示这个寻路按钮
        foreach(var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }
    public void OnClickAbandon() { }
    //public void OnClickNav()
    //{
    //    Vector3 pos = NPCManager.Instance.GetNpcPosition(this.npc);
    //    User.Instance.CurrentCharacterObject.StartNav(pos); //玩家控制器
    //    UIManager.Instance.Close<UIQuestSystem>();
    //}

}