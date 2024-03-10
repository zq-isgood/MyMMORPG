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
        if (this.@class.text != null) this.@class.text = this.Info.Class.ToString(); //intת��Ϊstring
        if (this.level.text != null) this.level.text = this.Info.Level.ToString();
    }
    public void OnAccept()
    {
        MessageBox.Show(string.Format("Ҫͨ����{0}����������", this.Info.Name), "��������", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(true, this.Info);
        };
    }
    public void OnDecline()
    {
        MessageBox.Show(string.Format("Ҫ�ܾ���{0}����������", this.Info.Name), "��������", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(false, this.Info);
        };
    }
}