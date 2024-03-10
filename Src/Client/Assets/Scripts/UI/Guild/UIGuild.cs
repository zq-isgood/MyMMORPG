using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGuild : UIWindow
{
    public GameObject itemPrefab;
    public ListView listMain;
    public Transform itemRoot;
    public UIGuildInfo uiInfo;
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;
    public GameObject panelLeader;
    private void Start()
    {
        
        GuildService.Instance.OnGuildUpdate+= UpdateUI;  //OnGuildUpdate����¼���Ҫ���������գ����Ա��+=,����һ��UIҪ�õ�����¼�
        this.listMain.onItemSelected += this.OnGuildMemberSelected;
        this.UpdateUI();
    }

   

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -=UpdateUI;
    }
    void UpdateUI()
    {
        this.uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();
        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    

    void ClearList()
    {
        this.listMain.RemoveAll();
    }
    public void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIGuildMemberItem;
    }
    private void InitItems()
    {
        foreach (var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            this.listMain.AddItem(ui);
        }
    }
    public void OnClickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>(); //һ��������䣬��Ҫ��UIManager����Դ�ļ��ز�����
    }
    public void OnClickLeave()
    {
        MessageBox.Show("��δ����");
    }
    public void OnClickChat()
    {

    }
    public void OnClickKickOut()
    {
        //��׼���裺�ȿ��Ƿ�ѡ�У�ѡ�о͵���Ϣ��
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ�ߵĳ�Ա");
            return;
        }
        MessageBox.Show(string.Format("Ҫ�ߡ�{0}����������", selectedItem.Info.Info.Name), "�߳�����", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ�����ĳ�Ա");
            return;
        }
        if (selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("�Է��Ѿ�������");
            return;
        }
        MessageBox.Show(string.Format("Ҫ������{0}��Ϊ���᳤��", selectedItem.Info.Info.Name), "������Ա", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ����ĳ�Ա");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("�Է���ְ���԰���");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("�᳤�������ܶ���");
            return;
        }
        MessageBox.Show(string.Format("Ҫ���⡾{0}����", selectedItem.Info.Info.Name), "�����Ա", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickTransfer()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫת�û᳤����λ��Ա");
            return;
        }
        MessageBox.Show(string.Format("Ҫת�û᳤����{0}����", selectedItem.Info.Info.Name), "ת�û᳤", MessageBoxType.Confirm, "ȷ��", "ȡ��").OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Transfer, this.selectedItem.Info.Info.Id);
        };
    }
    public void OnClickSetNotice()
    {
        MessageBox.Show("��δ����");
    }
}
