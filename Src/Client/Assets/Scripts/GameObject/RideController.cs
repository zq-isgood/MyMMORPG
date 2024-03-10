using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Managers;

public class RideController : MonoBehaviour
{

    public EntityController rider;
    public Vector3 offset;
    public Animator anim;
    public Transform mountPoint;  //Æï³Ëµã

    // Use this for initialization
    void Start()
    {
        this.anim = this.GetComponent<Animator>();
        offset = new Vector3((float)0.2,(float) -0.4, 0);

    }

    private void Update()
    {
        if (this.mountPoint == null || this.rider == null) return;
        this.rider.SetRidePosition(this.mountPoint.position + this.mountPoint.TransformDirection(this.offset));
    }
    public void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
        }
    }
    public void SetRider(EntityController rider)
    {
        this.rider = rider;
    }
}
