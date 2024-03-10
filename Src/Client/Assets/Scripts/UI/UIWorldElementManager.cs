using Assets.Scripts.Managers;
using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager:MonoSingleton<UIWorldElementManager>
{
    public GameObject nameBarPrefab;
    public GameObject npcStatusPrefab;
    private Dictionary<Transform, GameObject> elementNames= new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> elementStatus = new Dictionary<Transform, GameObject>();
    protected override void OnStart()
    {

    }

 
    public void AddCharacterNameBar(Transform owner,Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPrefab, this.transform);
        goNameBar.name = "NameBar" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner; //owner不在unity上赋值，在这里赋值
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        this.elementNames[owner] = goNameBar;
    }
    public void RemoveCharacterNameBar(Transform owner)
    {
        if (this.elementNames.ContainsKey(owner))
        {
            Destroy(this.elementNames[owner]); //elements是血条，owner是角色
            this.elementNames.Remove(owner);
        }
    }

    public void AddNpcQuestStatus(Transform owner,NpcQuestStatus status)
    {
        if (this.elementStatus.ContainsKey(owner))
        {
            elementStatus[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        GameObject go = Instantiate(npcStatusPrefab, this.transform);
        go.name = "NpcQuestStatus" + owner.name;
        go.GetComponent<UIWorldElement>().owner = owner; 
        go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
        go.SetActive(true);
        this.elementStatus[owner] = go;
    }
    public void RemoveNpcQuestStatus(Transform owner)
    {
        if (this.elementStatus.ContainsKey(owner))
        {
            Destroy(this.elementStatus[owner]); //elements是血条，owner是角色
            this.elementStatus.Remove(owner);
        }
    }
}
