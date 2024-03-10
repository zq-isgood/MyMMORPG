using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIGuildApplyList :UIWindow
{
    //���б��itemԪ�أ�һ�㶼�����漸��
    public GameObject itemPrefab;
    public Transform itemRoot;
    public ListView listMain;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateList; //���򿪹����������ʱ������������Ҫˢ��
        GuildService.Instance.SendGuildListRequest(); //ֻҪ��һ��������棬������������������б������ǿ��ˢ��
        this.UpdateList();
    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateList;
    }
    void UpdateList()
    {
        ClearList();
        InitItems();
    }

    private void InitItems()
    {
        foreach(var item in GuildManager.Instance.guildInfo.Applies)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildApplyItem ui = go.GetComponent<UIGuildApplyItem>(); //��������������ó���
            ui.SetItemInfo(item); //���������������ʾ����
            this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
        listMain.RemoveAll();
    }
}
