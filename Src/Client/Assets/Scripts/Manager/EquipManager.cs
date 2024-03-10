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
    //װ��ϵͳ��Manager���ǹ������߸�����
    public class EquipManager : Singleton<EquipManager>
    {
        public delegate void OnEquipChangeHandler();
        public event OnEquipChangeHandler OnEquipChanged;
        //���ݿ������ֽڱ��棬�ͻ����ö������鱣��
        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];
        //����ת��
        byte[] Data;
        //һ����Ϸ����UserServiceҪ�ѱ��������ߣ�װ����Manager��ʼ��һ��
        unsafe public void Init(byte[] data)
        {
            this.Data = data;
            //�����ݿ���ֽڽ���
            this.ParseEquipData(data); 
        }
        //����Ƿ���ʲô����
        public bool Contains(int equipId)
        {
            for(int i = 0; i < this.Equips.Length; i++)
            {
                if (Equips[i] != null && Equips[i].Id == equipId)
                    return true;
            }
            return false;
        }
        //���ĳ�����ӷ���ʲô����
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
                    //������itemId,Ȼ����䵽Equip��
                    int itemId = *(int*)(pt + i * sizeof(int)); 
                    if (itemId > 0)
                        Equips[i] = ItemManager.Instance.Items[itemId];
                    else
                        Equips[i] = null;
                }
            }
        }
        //��װ����Ϣ��ԭ���ֽڷ��͸�������
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
                Debug.LogFormat("ֱ���˳�");
                return;
            }
            //�յ�װ��������Ϣ����װ���ӵ���ϵͳ�ó������������
            this.Equips[(int)equip.EquipInfo.Slot] = ItemManager.Instance.Items[equip.Id];
            if (OnEquipChanged != null)
                //֪ͨ�ı䣬��ע����Ϣ��֪ͨ����UICharEquip��RefreshUI
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