using SkillBridge.Message;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System;
using Services;

public class UIGuildApplyItem : ListView.ListViewItem
{
    public TextMeshProUGUI nickname;
    public TextMeshProUGUI @class;
    public TextMeshProUGUI level;
    public NGuildApplyInfo Info;

    public void SetItemInfo(NGuildApplyInfo item)
    {

        this.Info = item;
        if (this.nickname.text != null) this.nickname.text = item.Name;
        if (this.@class.text != null) this.@class.text = this.Info.Class.ToString(); //int转化为string
        if (this.level.text != null) this.level.text = this.Info.Level.ToString();
    }
    public void OnAccept()
    {
        MessageBox.Show(string.Format("要通过【{0}】的申请吗？", this.Info.Name), "审批申请", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(true, this.Info);
        };
    }
    public void OnDecline()
    {
        MessageBox.Show(string.Format("要拒绝【{0}】的申请吗？", this.Info.Name), "审批申请", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(false, this.Info);
        };
    }
}