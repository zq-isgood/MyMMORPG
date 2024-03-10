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
    //有IDeselectHandler这个接口，则只要点击其他的地方，这个方法就会被触发。但是selectable组件是绑定在根节点上的，如果点击他的子节点，这个方法也会被触发
    public void OnDeselect(BaseEventData eventData)
    {
        var ed = eventData as PointerEventData; 
        //ed.hovered代表当前事件所经历的所有节点，如果包含当前界面（可能是点在子节点上了）
        if (ed.hovered.Contains(this.gameObject))
        {
            return; //则界面不关闭
        }
        this.Close(WindowResult.None); //没有选中界面就关闭这个界面
    }
    //界面打开时，调用OnEnable这个函数
    public void OnEnable()
    {
        this.GetComponent<Selectable>().Select(); //保证组件是被选中的，因为只有选中才有OnDeselect这个事件的发生
        this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0); //加根节点为了保证那个小窗口弹出时在鼠标的右方
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