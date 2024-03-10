using Assets.Scripts.Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindow
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI money;
    public GameObject itemPrefab;
    public GameObject itemEquipedPrefab;
    //是左边的装备列表的根节点
    public Transform itemListRoot;
    public List<Transform> slots;

    void Start()
    {
        //启动时先刷新UI
        RefreshUI();
        //只要穿或脱装备了UI实时刷新
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }
    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

    /// <summary>
    /// 初始化所有装备列表
    /// </summary>
    /// <returns></returns>
    void RefreshUI()
    {
        //清空左边的装备列表
        ClearAllEquipList();
        //初始化左边的装备列表
        InitAllEquipItems();
        //清空右边的装备列表
        ClearEquipedList();
        //初始化右边的装备列表
        InitEquipedItems();
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    void InitAllEquipItems()
    {
        foreach(var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacter.Class)
            {
                if (EquipManager.Instance.Contains(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                //布尔值区分是哪个装备列表
                ui.SetEquipItem(kv.Key, kv.Value, this, false);

            }
        }
    }
    void ClearAllEquipList()
    {
        foreach(var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }
    }
    void ClearEquipedList()
    {
        foreach(var item in slots)
        {
            if (item.childCount > 0)
                Destroy(item.GetChild(0).gameObject);
        }
    }

    void InitEquipedItems()
    {
        for(int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
            var item = EquipManager.Instance.Equips[i];
            {
                if(item!=null)
                {
                    GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }
            }
        }
    }
    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }
    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }
}