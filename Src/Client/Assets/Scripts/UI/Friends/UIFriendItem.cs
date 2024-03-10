using SkillBridge.Message;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem
{
    public TextMeshProUGUI nickname;
    public TextMeshProUGUI @class;
    public TextMeshProUGUI level;
    public TextMeshProUGUI status;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;
    public NFriendInfo Info;
    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    private void Start()
    {
       
    }
    bool isEquiped = false;
    public void SetFriendInfo(NFriendInfo item)
    {
        this.Info = item;
        if (this.nickname != null) this.nickname.text = this.Info.friendInfo.Name;
        if (this.@class != null) this.@class.text = this.Info.friendInfo.Class.ToString();
        if (this.level != null) this.level.text = this.Info.friendInfo.Level.ToString();
        if (this.status != null) this.status.text = this.Info.Status == 1 ? "‘⁄œﬂ" : "¿Îœﬂ";

    }

}