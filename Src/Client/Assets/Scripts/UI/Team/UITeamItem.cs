using Common.Data;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UITeamItem : ListView.ListViewItem
{
    public TextMeshProUGUI nickname;
    public Image background;
    public Image classIcon;
    public Image leaderIcon;
    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? true : false;
    }
    public int idx;
    public NCharacterInfo Info;
    private void Start()
    {
        this.background.enabled = false;
    }
    public void SetMemberInfo(int idx, NCharacterInfo item,bool isLeader)
    {
        this.idx = idx;
        this.Info = item;
        if (this.nickname != null) this.nickname.text = this.Info.Level.ToString().PadRight(4) + this.Info.Name;
        if (this.classIcon != null) this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.Info.Class - 1]; //图标根据职业选择在SpriteManager里的图标
        if (this.leaderIcon != null) this.leaderIcon.gameObject.SetActive(isLeader); //是队长就激活图标


    }
}