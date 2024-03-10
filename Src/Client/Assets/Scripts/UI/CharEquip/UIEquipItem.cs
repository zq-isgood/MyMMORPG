using Assets.Scripts.Managers;
using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    //public TextMeshProUGUI limitCategory;
    public TextMeshProUGUI limitClass;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;
    private bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }

    }
    public int index { get; set; }
    private UICharEquip owner;
    private Item item;
    bool isEquiped = false;
    public void SetEquipItem(int id, Item item, UICharEquip owner, bool equiped)
    {
        this.owner= owner;
        this.index = id;
        this.item = item;
        this.isEquiped = equiped;
        if(this.title!=null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = item.Define.Level.ToString();
        if (this.limitClass != null) this.limitClass.text = this.item.Define.LimitClass.ToString();
        //if (this.limitCategory != null) this.limitCategory.text = this.item.Define.Category.ToString();
        if (this.icon!= null) this.icon.overrideSprite = Resloader.Load<Sprite>(item.Define.Icon);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEquiped)
        {
            UnEquip();
        }
        else
        {
            if (this.selected)
            {
                DoEquip();
                this.Selected = false;
            }
            else
                this.Selected = true;
        }
    }
    void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("Ҫװ��[{0}]��", this.item.Define.Name), "ȷ��", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.Slot);
            if (oldEquip != null)
            {
                var newmsg=MessageBox.Show(string.Format("Ҫ�滻��[{0}]��", oldEquip.Define.Name), "ȷ��", MessageBoxType.Confirm);
                newmsg.OnYes = () =>
                {
                    this.owner.DoEquip(this.item);
                };
            }
            else
            {
                this.owner.DoEquip(this.item);
            }
            
        };
    }
    void UnEquip()
    {
        var msg = MessageBox.Show(string.Format("Ҫȡ��װ��[{0}]��", this.item.Define.Name), "ȷ��", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            this.owner.UnEquip(this.item);
        };
    }

}
