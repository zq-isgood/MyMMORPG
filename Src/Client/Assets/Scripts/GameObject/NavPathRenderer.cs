using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Entities;
using SkillBridge.Message;
using Services;
using UnityEngine.UIElements;

public class NavPathRenderer : MonoSingleton<NavPathRenderer>
{
    LineRenderer pathRenderer;
    NavMeshPath path;
    private void Start()
    {
        pathRenderer = this.GetComponent<LineRenderer>();
        pathRenderer.enabled = false; //如果path是空的话，把它隐藏掉
    }
    public void SetPath(NavMeshPath path,Vector3 target)
    {
        this.path = path;
        if (this.path == null)
        {
            pathRenderer.enabled = false;
            pathRenderer.positionCount = 0;
        }
        else
        {
            pathRenderer.enabled = true;
            pathRenderer.positionCount = path.corners.Length + 1;
            pathRenderer.SetPositions(path.corners);
            pathRenderer.SetPosition(pathRenderer.positionCount - 1, target);  //设置目标点
            for(int i = 0; i < pathRenderer.positionCount; i++)
            {
                pathRenderer.SetPosition(i, pathRenderer.GetPosition(i) + Vector3.up * 0.2f);
            }
        }
    }

}