using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIGuildApplyList :UIWindow
{
    //有列表的item元素，一般都有下面几项
    public GameObject itemPrefab;
    public Transform itemRoot;
    public ListView listMain;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateList; //当打开公会申请面板时，有申请则需要刷新
        GuildService.Instance.SendGuildListRequest(); //只要打开一次这个界面，就向服务器发送申请列表的请求，强制刷新
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
            UIGuildApplyItem ui = go.GetComponent<UIGuildApplyItem>(); //把这个物体的组件拿出来
            ui.SetItemInfo(item); //组件里面设置了显示的字
            this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
        listMain.RemoveAll();
    }
}
