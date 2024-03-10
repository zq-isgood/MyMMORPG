using Common.Data;
using Entities;
using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    class StatusService : Singleton<StatusService>, IDisposable
    {
        public delegate bool StatusNotifyHandler(NStatus status);
        Dictionary<StatusType, StatusNotifyHandler> eventMap = new Dictionary<StatusType, StatusNotifyHandler>();
        HashSet<StatusNotifyHandler> handles = new HashSet<StatusNotifyHandler>();
        public void Init() { }
        public void RegisterStatusNotify(StatusType function,StatusNotifyHandler action)
        {
            if (handles.Contains(action))
                return;
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }
            handles.Add(action);
        }
        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StatusNotify>(this.OnStatusNotify);
        }

        private void OnStatusNotify(object sender, StatusNotify notify)
        {
            foreach(NStatus status in notify.Status)
            {
                Notify(status);
            }
        }
        private void Notify(NStatus status)
        {
            if (status.Type == StatusType.Money)
            {
                if (status.Action == StatusAction.Add)
                    User.Instance.AddGold(status.Value);
                else if(status.Action == StatusAction.Delete)
                    User.Instance.AddGold(-status.Value);
            }
            StatusNotifyHandler handler;
            if(eventMap.TryGetValue(status.Type,out handler))
            {
                handler(status);
            }
        }
    }
}