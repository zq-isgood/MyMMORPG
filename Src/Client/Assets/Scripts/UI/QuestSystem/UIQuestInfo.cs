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

//��������ui�м�������Ϣ
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
        //    this.npc=quest.Define.AcceptNPC; //�����ǿյ����ҽ��ܵ�npc

        //}
        //else if (quest.Info.Status == SkillBridge.Message.QuestStatus.Complated)
        //{
        //    //���������������������npc
        //    this.npc = quest.Define.SubmitNPC;
        //}
        //this.navButtton.gameObject.SetActive(this.npc > 0);  //���û��npc����ʾ���Ѱ·��ť
        foreach(var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
        {
            fitter.SetLayoutVertical();
        }
    }
    public void OnClickAbandon() { }
    //public void OnClickNav()
    //{
    //    Vector3 pos = NPCManager.Instance.GetNpcPosition(this.npc);
    //    User.Instance.CurrentCharacterObject.StartNav(pos); //��ҿ�����
    //    UIManager.Instance.Close<UIQuestSystem>();
    //}

}