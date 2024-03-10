using SkillBridge.Message;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Common.Utils;

public class UIGuildMemberItem : ListView.ListViewItem
{
    public TextMeshProUGUI nickname;
    public TextMeshProUGUI @class;
    public TextMeshProUGUI level;
    public TextMeshProUGUI title;
    public TextMeshProUGUI joinTime;
    public TextMeshProUGUI status;

    public Image bg;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.bg.overrideSprite = selected ? selectedBg : normalBg;
    }
    public NGuildMemberInfo Info;
    public void SetGuildMemberInfo(NGuildMemberInfo item)
    {
        this.Info = item;
        if (this.nickname.text != null) this.nickname.text = this.Info.Info.Name;
        if (this.@class.text != null) this.@class.text = this.Info.Info.Class.ToString();
        if (this.level.text != null) this.level.text = this.Info.Info.Level.ToString();
        if (this.title.text != null) this.title.text = this.Info.Title.ToString();
        if (this.joinTime.text != null) this.joinTime.text = TimeUtil.GetTime(this.Info.joinTime).ToShortDateString();
        if (this.status.text != null) this.status.text = this.Info.Status==1? "ÔÚÏß": TimeUtil.GetTime(this.Info.lastTime).ToShortDateString();
    }
}