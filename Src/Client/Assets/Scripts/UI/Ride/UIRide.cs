using Assets.Scripts.Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRide : UIWindow
{
    public TextMeshProUGUI descript;  //�ұߵ���������
    public GameObject itemPrefab;
    public ListView listMain;
    private UIRideItem selectedItem;

    void Start()
    {

        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
        
    }
    /// <summary>
    /// ��ʼ������װ���б�
    /// </summary>
    /// <returns></returns>
    void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    private void ClearItems()
    {
        this.listMain.RemoveAll();
    }

    void InitItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == ItemType.Ride && (kv.Value.Define.LimitClass==CharacterClass.None||kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class))
            {
                if (EquipManager.Instance.Contains(kv.Key))
                {
                    continue;
                }
                GameObject go = Instantiate(itemPrefab, listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetEquipItem( kv.Value, this, false);
                listMain.AddItem(ui);

            }
        }
    }
    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIRideItem;
        this.descript.text = selectedItem.item.Define.Description;
    }
    //��ӦUI���ٻ���ť
    public void DoRide()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ�ٻ�������", "��ʾ");
            return;
        }
        User.Instance.Ride(this.selectedItem.item.Id);
    }

}