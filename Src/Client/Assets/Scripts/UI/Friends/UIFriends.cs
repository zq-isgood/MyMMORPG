using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using System;
using Models;
using Assets.Scripts.Managers;

public class UIFriends: UIWindow
{
    public GameObject itemPrefab; //Ҫ�󶨵��б�Ԫ��
    public ListView listMain; //�б��
    public Transform itemRoot;
    public UIFriendItem selectedItem;
    private void Start()
    {
        FriendService.Instance.OnFriendUpdate = RefreshUI;
        this.listMain.onItemSelected += this.OnFriendSelected; //�������onItemSelected֪����ǰѡ������һ��
        RefreshUI();
    }

    public void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UIFriendItem;
    }
    public void OnClickFriendAdd()
    {
        InputBox.Show("����Ҫ��ӵĺ������ƻ�ID", "��Ӻ���").OnSubmit += OnFriendAddSubmit; //���ѡ���ύ��ִ��OnFriendAddSubmit����߼�
    }

    private bool OnFriendAddSubmit(string input, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        //���ڼ����������ID�������֣������־ͻ�ΪID
        if (!int.TryParse(input, out friendId))
            friendName = input;
        if (friendId == User.Instance.CurrentCharacter.Id || friendName == User.Instance.CurrentCharacter.Name)
        {
            tips = "����Ц���������Լ�Ŷ";
            return false;
        }
        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }
    public void OnClickFriendChat()
    {
        MessageBox.Show("��δ����");
    }
    public void OnClickTeamInvite()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫ��ӵĺ���");
            return;
        }
        if (selectedItem.Info.Status == 0)
        {
            MessageBox.Show("��ѡ�����ߵĺ���");
            return;
        }
        MessageBox.Show(String.Format("��ȷ�����롾{0}�������", selectedItem.Info.friendInfo.Name), "����������", MessageBoxType.Confirm, "����", "ȡ��").OnYes = () =>
           { TeamService.Instance.SendTeamInviteRequest(this.selectedItem.Info.friendInfo.Id, this.selectedItem.Info.friendInfo.Name); };
    }
    public void OnClickFriendRemove()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("��ѡ��Ҫɾ���ĺ���");
            return;
        }
        MessageBox.Show(string.Format("ȷ��Ҫɾ�����ѡ�{0}����", selectedItem.Info.friendInfo.Name), "ɾ������", MessageBoxType.Confirm, "ɾ��", "ȡ��").OnYes = () =>
        {
            FriendService.Instance.SendFriendRemoveRequest(this.selectedItem.Info.Id, this.selectedItem.Info.friendInfo.Id);
        };

    }
    void RefreshUI()
    {
        ClearFriendList();
        InitFriendItems();
    }

    private void InitFriendItems()
    {
        foreach(var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIFriendItem ui = go.GetComponent<UIFriendItem>();
            ui.SetFriendInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearFriendList()
    {
        this.listMain.RemoveAll();
    }
}