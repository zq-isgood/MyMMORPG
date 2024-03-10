using Assets.Scripts.Managers;
using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRideItem : ListView.ListViewItem
{
    public Image icon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        
            this.background.overrideSprite = selected ? selectedBg : normalBg;
  

    }

    public Item item;

    //坐骑就是一个道具，所以命名没改
    public void SetEquipItem(Item item, UIRide owner, bool equiped)
    {

        this.item = item;
        if(this.title!=null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = item.Define.Level.ToString();
        if (this.icon!= null) this.icon.overrideSprite = Resloader.Load<Sprite>(item.Define.Icon);
    }

    

}
