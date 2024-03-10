using Services;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGuildPopCreate :UIWindow
{
    public TMP_InputField inputName;
    public TMP_InputField inputNotice;
    private void Start()
    {
        GuildService.Instance.OnGuildCreateResult = OnGuildCreated; //OnGuildCreateResultִ����OnGuildCreatedִ�У�

    }
    private void OnDestroy()
    {
        GuildService.Instance.OnGuildCreateResult = null;
    }
    public override void OnYesClick()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            MessageBox.Show("�����빫������", "����", MessageBoxType.Error);
            return;
        }
        if (inputName.text.Length < 4 || inputName.text.Length > 10)
        {
            MessageBox.Show("������4��10���ַ�", "����", MessageBoxType.Error);
            return;
        }
        if (string.IsNullOrEmpty(inputNotice.text))
        {
            MessageBox.Show("�����빫������", "����", MessageBoxType.Error);
            return;
        }
        if (inputNotice.text.Length < 3 || inputNotice.text.Length > 50)
        {
            MessageBox.Show("����������Ϊ3��50���ַ�", "����", MessageBoxType.Error);
            return;
        }
        GuildService.Instance.SendGuildCreate(inputName.text,inputNotice.text);
    }
    void OnGuildCreated(bool result)
    {
        if (result)
        {
            this.Close(WindowResult.Yes);
        }
    }
}
