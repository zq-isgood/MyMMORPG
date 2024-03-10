using Assets.Scripts.Managers;
using Common.Data;
using Entities;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    class ItemService : Singleton<ItemService>, IDisposable
    {
        public ItemService()
        {
            MessageDistributer.Instance.Subscribe < ItemBuyResponse > (this.OnItemBuy); 
            MessageDistributer.Instance.Subscribe<ItemEquipResponse>(this.OnItemEquip);
        }

        

        private void OnItemBuy(object sender, ItemBuyResponse message)
        {
            MessageBox.Show("��������" + message.Result + "\n" + message.Errormsg, "�������");
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Unsubscribe<ItemEquipResponse>(this.OnItemEquip);
        }
        public void SendBuyItem(int shopId,int shopItemId)
        {
            Debug.Log("SendBuyItem");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();
            message.Request.itemBuy.shopId = shopId;
            message.Request.itemBuy.shopItemId = shopItemId;
            NetClient.Instance.SendMessage(message);

        }
        Item pendingEquip = null;
        bool isEquip;
        //�ѵ�ǰ����װ����Ϣ��¼���������ظ�OnItemEquip,�����EquipManager���õģ�ͨ������������͸�������Ҫ������ʲôװ��
        public bool SendEquipItem(Item equip,bool isEquip)
        {
            if (pendingEquip != null)
                return false;
            Debug.Log("SendEquipItem");
            pendingEquip = equip;
            this.isEquip = isEquip;
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.Slot = (int)equip.EquipInfo.Slot;
            message.Request.itemEquip.itemId = equip.Id;
            message.Request.itemEquip.isEquip = isEquip;
            NetClient.Instance.SendMessage(message);
            return true;
        }
        //���Ƿ������߼�ִ����󣬷��ظ��ͻ��˺�Ҫִ�е��߼�
        private void OnItemEquip(object sender, ItemEquipResponse message)
        {
            if (message.Result == Result.Success)
            {
                if (pendingEquip != null)
                {
                    //isEquip���ж�Ҫ��������
                    if (this.isEquip)
                    {
                        EquipManager.Instance.OnEquipItem(pendingEquip);

                    }
                    else
                    {
                        EquipManager.Instance.OnUnEquipItem(pendingEquip.EquipInfo.Slot);

                    }
                    pendingEquip = null;
                }
            }
        }
    }
}