using Common;
using Common.Data;
using GameServer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class Spawner
    {
        public SpawnRuleDefine Define { get; set; }
        public Map Map;
        private float spawnTime = 0;
        private float unspawnTime = 0;
        private bool spawned = false;
        private SpawnPointDefine spawnPoint = null;
        public Spawner(SpawnRuleDefine define,Map map)
        {
            this.Define = define;
            this.Map = map;
            if (DataManager.Instance.SpawnPoints.ContainsKey(this.Map.ID))
            {
                if (DataManager.Instance.SpawnPoints[this.Map.ID].ContainsKey(this.Define.SpawnPoint))
                {
                    spawnPoint = DataManager.Instance.SpawnPoints[this.Map.ID][this.Define.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("SpawnRule[{0}] SpawnPoints[{1}] not existed", this.Define.ID, this.Define.SpawnPoint);
                }
            }
        }
        public void Update()
        {
            if (this.CanSpawn())
            {
                this.Spawn();
            }
        }
        //判断每一帧是否能刷
        bool CanSpawn()
        {
            if (this.spawned)
                return false;
            //unspawnTime是杀死刷怪的时间
            if (this.unspawnTime + this.Define.SpawnPeriod > Time.time)
                return false;
            return true;
        }
        public void Spawn()
        {
            this.spawned = true;
            Log.InfoFormat("Map[{0}]Spawn[{1}:Mon:{2},Lv:{3}] At Point:{4}", this.Define.MapID, this.Define.ID, this.Define.SpawnMonID, this.Define.SpawnLevel,this.Define.SpawnPoint);
            this.Map.MonsterManager.Create(this.Define.SpawnMonID, this.Define.SpawnLevel, this.spawnPoint.Position, this.spawnPoint.Direction);
        }
    }
}
