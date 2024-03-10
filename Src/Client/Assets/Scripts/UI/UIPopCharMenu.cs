using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPopCharMenu : UIWindow, IDeselectHandler
{
    public int targetId;
    public string targetName;
    //��IDeselectHandler����ӿڣ���ֻҪ��������ĵط�����������ͻᱻ����������selectable����ǰ��ڸ��ڵ��ϵģ������������ӽڵ㣬�������Ҳ�ᱻ����
    public void OnDeselect(BaseEventData eventData)
    {
        var ed = eventData as PointerEventData; 
        //ed.hovered����ǰ�¼������������нڵ㣬���������ǰ���棨�����ǵ����ӽڵ����ˣ�
        if (ed.hovered.Contains(this.gameObject))
        {
            return; //����治�ر�
        }
        this.Close(WindowResult.None); //û��ѡ�н���͹ر��������
    }
    //�����ʱ������OnEnable�������
    public void OnEnable()
    {
        this.GetComponent<Selectable>().Select(); //��֤����Ǳ�ѡ�еģ���Ϊֻ��ѡ�в���OnDeselect����¼��ķ���
        this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0); //�Ӹ��ڵ�Ϊ�˱�֤�Ǹ�С���ڵ���ʱ�������ҷ�
    }
    public void OnChat()
    {

        ChatManager.Instance.StartPrivateChat(targetId, targetName);
        this.Close(WindowResult.No);
    }
    public void OnAddFriend()
    {
        this.Close(WindowResult.No);
    }
    public void OnInviteTeam()
    {
        this.Close(WindowResult.No);
    }
}