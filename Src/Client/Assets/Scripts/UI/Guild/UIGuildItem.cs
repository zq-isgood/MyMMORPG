using SkillBridge.Message;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Candlelight.ColorTween;

public class UIGuildItem : ListView.ListViewItem
{
    public TextMeshProUGUI GuildId;
    public TextMeshProUGUI GuildName;
    public TextMeshProUGUI MemberNum;
    public TextMeshProUGUI Leader;

    public Image bg;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.bg.overrideSprite = selected ? selectedBg : normalBg;
    }
    public NGuildInfo Info;
    public void SetGuildInfo(NGuildInfo item)
    {
        this.Info = item;
        if (this.GuildId.text != null) this.GuildId.text = item.Id.ToString();
        if (this.GuildName.text != null) this.GuildName.text = this.Info.GuildName;
        if (this.MemberNum.text != null) this.MemberNum.text = this.Info.memberCount.ToString();
        if (this.Leader.text != null) this.Leader.text = this.Info.LeaderName;
      
    }
}