using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class SpawnManager
    {
        //根据每个规则创建一个刷怪器，下面是刷怪器的列表
        private List<Spawner> Rules = new List<Spawner>();
        private Map Map;
        public void Init(Map map)
        {
            this.Map = map;
            if (DataManager.Instance.SpawnRules.ContainsKey(map.Define.ID))
            {
                //mapID=2时，value共1,2,3,4四组，对应spawnruledefine
                foreach(var define in DataManager.Instance.SpawnRules[map.Define.ID].Values)
                {
                    //根据规则和地图制定刷怪器
                    this.Rules.Add(new Spawner(define, this.Map));
                }
            }
        }
        public void Update()
        {
            if (Rules.Count == 0)
                return;
            for(int i = 0; i < this.Rules.Count; i++)
            {
                this.Rules[i].Update();
            }
        }
    }
}
