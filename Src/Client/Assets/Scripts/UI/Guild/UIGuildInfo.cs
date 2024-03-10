using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Common;

public class UIGuildInfo : MonoBehaviour
{
    public TextMeshProUGUI guildName;
    public TextMeshProUGUI guildID;
    public TextMeshProUGUI guildNotice;
    public TextMeshProUGUI memberNumber;
    public TextMeshProUGUI leader;
    private NGuildInfo info;
    public NGuildInfo Info
    {
        get { return this.info; }
        set { this.info = value;this.UpdateUI(); }
    }
    void UpdateUI()
    {
        if (this.info == null)
        {
            guildName.text = "��";
            guildID.text = "ID:0";
            leader.text = "�᳤����";
            guildNotice.text = "";
            memberNumber.text ="��Ա������0/500"; //GameDefine.GuildMaxMemberCount
        }
        else
        {
            guildName.text = Info.GuildName;
            guildID.text = "ID:"+this.Info.Id;
            leader.text = "�᳤"+Info.LeaderName;
            guildNotice.text = Info.Notice;
            memberNumber.text = string.Format("��Ա������{0}/500", info.memberCount);
        }
    }
}
