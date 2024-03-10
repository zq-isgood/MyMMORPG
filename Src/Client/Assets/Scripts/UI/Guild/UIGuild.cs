using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;
    private void Start()
    {
        
        GuildService.Instance.OnGuildUpdate+= UpdateUI;  //OnGuildUpdate这个事件需要多个界面接收，所以变成+=,还有一个UI要用到这个事件
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }

   

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -=UpdateUI;
    }
    void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();
        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    

    void ClearList()
    {
        this.listMain.RemoveAll();
    }
    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }
    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }
    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>(); //一旦出现这句，就要在UIManager把资源的加载补充上
    }
    public void OnClickLeave()
    {
        MessageBox.Show("暂未开放");
    }
    public void OnClickChat()
    {

    }
    public void OnClickKickOut()
    {
        //标准步骤：先看是否选中，选中就弹消息框
        if (selectedItem == null)
        {
            MessageBox.Show("请选中要踢的成员");
            return;
        }
        MessageBox.Show(string.Format("要踢【{0}】出公会吗？", selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选中要晋升的成员");
            return;
        }
        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方已经身份尊贵");
            return;
        }
        MessageBox.Show(string.Format("要晋升【{0}】为副会长吗？", selectedItem.Info.Info.Name), "晋升成员", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选中要罢免的成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方无职可以罢免");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("会长不是你能动的");
            return;
        }
        MessageBox.Show(string.Format("要罢免【{0}】吗？", selectedItem.Info.Info.Name), "罢免成员", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选中要转让会长给哪位成员");
            return;
        }
        MessageBox.Show(string.Format("要转让会长给【{0}】吗？", selectedItem.Info.Info.Name), "转让会长", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickSetNotice()
    {
        MessageBox.Show("暂未开放");
    }
}
