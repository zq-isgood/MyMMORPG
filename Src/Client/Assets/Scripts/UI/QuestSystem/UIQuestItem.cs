using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem:ListView.ListViewItem
{
    public TextMeshProUGUI title;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;
    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    public Quest quest;
    void Start()
    {
    
    }
    bool isEquiped = false;
    public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        if (this.title != null) this.title.text = this.quest.Define.Name;
    }
}