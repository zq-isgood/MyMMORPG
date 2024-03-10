using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.Managers
{ 
    //装备系统的Manager就是管理那七个格子
    public class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHandler();
        public event OnEquipChangeHandler OnEquipChanged;
        //数据库是用字节保存，客户端用定长数组保存
        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];
        //用于转换
        byte[] Data;
        //一进游戏，在UserService要把背包，道具，装备的Manager初始化一遍
        unsafe public void Init(byte[] data)
        {
            this.Data = data;
            //将数据库的字节解析
            this.ParseEquipData(data); 
        }
        //检查是否穿了什么道具
        public bool Contains(int equipId)
        {
            for(int i = 0; i < this.Equips.Length; i++)
            {
                if (Equips[i] != null && Equips[i].Id == equipId)
                    return true;
            }
            return false;
        }
        //检查某个格子放了什么道具
        public Item GetEquip(EquipSlot slot)
        {
            return Equips[(int)slot];
        }

        unsafe void ParseEquipData(byte[] data)
        {
            fixed(byte* pt = this.Data)
            {
                for(int i = 0; i < this.Equips.Length; i++)
                {
                    //解析出itemId,然后填充到Equip里
                    int itemId = *(int*)(pt + i * sizeof(int)); 
                    if (itemId > 0)
                        Equips[i] = ItemManager.Instance.Items[itemId];
                    else
                        Equips[i] = null;
                }
            }
        }
        //把装备信息还原成字节发送给服务器
        unsafe public byte[] GetEquipData()
        {
            fixed(byte* pt = Data)
            {
                for(int i = 0; i < (int)EquipSlot.SlotMax; i++)
                {
                    int* itemId = (int*)(pt + i * sizeof(int));
                    if (Equips[i] == null)
                        *itemId = 0;
                    else
                        *itemId = Equips[i].Id;
                }
            }
            return this.Data;

        }
        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, true);
        }
        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }
        public void OnEquipItem(Item equip)
        {
            if (this.Equips[(int)equip.EquipInfo.Slot] != null && this.Equips[(int)equip.EquipInfo.Slot].Id == equip.Id) 
            {
                Debug.LogFormat("直接退出");
                return;
            }
            //收到装备穿戴信息，将装备从道具系统拿出来，填到格子里
            this.Equips[(int)equip.EquipInfo.Slot] = ItemManager.Instance.Items[equip.Id];
            if (OnEquipChanged != null)
                //通知改变，给注册消息的通知，即UICharEquip的RefreshUI
                OnEquipChanged();
        }
        public void OnUnEquipItem(EquipSlot slot)
        {
            if (this.Equips[(int)slot]!=null)
            {
                this.Equips[(int)slot] = null;
                if(OnEquipChanged!=null)
                    OnEquipChanged();
            }

        }

    }


}