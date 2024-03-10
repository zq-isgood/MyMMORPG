using Assets.Scripts.Managers;
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

public class UIQuestSystem : UIWindow
{
    public TextMeshProUGUI title;
    public GameObject itemPrefab;
    public TabView Tabs;
    public ListView listMain;
    public ListView listBranch;
    public UIQuestInfo questInfo;
    private bool showAvailableList = false;

    private void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
        this.listBranch.onItemSelected += this.OnQuestSelected;
        //OnTabSelect标红了，我自己解决的是在tabview.cs加了一句 public UnityAction<int> OnTabSelect;
        this.Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        //当选择好任务时，设置中间面板的任务信息
        this.questInfo.SetQuestInfo(questItem.quest);
    }

    //通过他完成ui的两个按钮的切换，是否显示可接任务
    public void OnSelectTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }
    void RefreshUI()
    {
        ClearAllQuestList();
        //初始化任务
        InitAllQuestItems();
    }

    private void InitAllQuestItems()
    {
        //从任务管理器拉出所有任务
        foreach(var kv in QuestManager.Instance.allQuests)
        {
            //continue跳出循环，showAvailableList为true说明是可接任务按钮，info是空说明没有接这个任务需要显示，showAvailableList为false说明是进行中按钮，加上info不是空则这个任务就是进行中的任务需要显示
            if (showAvailableList)
            {
                if (kv.Value.Info != null)
                    continue;
            }
            else
            {
                if (kv.Value.Info == null)
                    continue;
            }
            GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            ui.SetQuestInfo(kv.Value);
            if (kv.Value.Define.Type == QuestType.Main)
                this.listMain.AddItem(ui);
            else
                this.listBranch.AddItem(ui);
        }
    }

    private void ClearAllQuestList()
    {
        this.listMain.RemoveAll();
        this.listBranch.RemoveAll();
    }


}