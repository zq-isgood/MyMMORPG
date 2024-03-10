using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Models;
using UnityEditor;
using Assets.Scripts.Managers;

public class NpcController : MonoBehaviour
{
    public int npcId;
    Animator anim;
    NpcDefine npc;
    NpcQuestStatus questStatus;
    SkinnedMeshRenderer renderer;
    Color originColor;
    private bool inInteractive = false;

    void Start()
    {
        renderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = renderer.sharedMaterial.color;
        anim = this.gameObject.GetComponent<Animator>();
        npc = NPCManager.Instance.GetNpcDefine(npcId);
        NPCManager.Instance.UpdateNpcPosition(this.npcId, this.transform.position);
        this.StartCoroutine(Actions());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }

    private void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.npcId);
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

    void OnQuestStatusChanged(Quest quest)
    {
        this.RefreshNpcStatus();
    }
    private void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if (UIWorldElementManager.Instance != null)
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
                yield return new WaitForSeconds(Random.Range(5f,10f));
            this.Relax();
        }
    }
    void Update()
    {
        
    }
    void Relax()
    {
        anim.SetTrigger("Relax");
    }
    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }
    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(npc))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }
    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }
    void OnMouseDown()
    {
        //if (Vector3.Distance(this.transform.position, User.Instance.CurrentCharacterObject.transform.position) > 2f)
        //{
        //    User.Instance.CurrentCharacterObject.StartNav(this.transform.position);
        //}
        Interactive();
    }
    private void OnMouseOver()
    {
        Highlight(true);
    }
    private void OnMouseEnter()
    {
        Highlight(true);
    }
    private void OnMouseExit()
    {
        Highlight(false);
    }
    void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (renderer.sharedMaterial.color != Color.white)
                renderer.sharedMaterial.color = Color.white;
        }
        else
        {
            if (renderer.sharedMaterial.color != originColor)
            {
                renderer.sharedMaterial.color = originColor;
            }
        }
    }
}
