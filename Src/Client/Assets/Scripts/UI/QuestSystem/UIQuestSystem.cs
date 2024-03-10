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
        //OnTabSelect����ˣ����Լ����������tabview.cs����һ�� public UnityAction<int> OnTabSelect;
        this.Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();
    }

    public void OnQuestSelected(ListView.ListViewItem item)
    {
        UIQuestItem questItem = item as UIQuestItem;
        //��ѡ�������ʱ�������м�����������Ϣ
        this.questInfo.SetQuestInfo(questItem.quest);
    }

    //ͨ�������ui��������ť���л����Ƿ���ʾ�ɽ�����
    public void OnSelectTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }
    void RefreshUI()
    {
        ClearAllQuestList();
        //��ʼ������
        InitAllQuestItems();
    }

    private void InitAllQuestItems()
    {
        //�����������������������
        foreach(var kv in QuestManager.Instance.allQuests)
        {
            //continue����ѭ����showAvailableListΪtrue˵���ǿɽ�����ť��info�ǿ�˵��û�н����������Ҫ��ʾ��showAvailableListΪfalse˵���ǽ����а�ť������info���ǿ������������ǽ����е�������Ҫ��ʾ
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