using GameServer.Entities;
using GameServer.Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    //对标角色管理器
    class MonsterManager
    {
        private Map Map;
        public Dictionary<int, Monster> Monsters = new Dictionary<int, Monster>();
        public void Init(Map map)
        {
            this.Map = map;
        }
        internal Monster Create(int spawnMonID,int spawnLevel,NVector3 position,NVector3 direction)
        {
            Monster monster = new Monster(spawnMonID, spawnLevel, position, direction);
            EntityManager.Instance.AddEntity(this.Map.ID, monster);
            monster.Id = monster.entityId;
            monster.Info.EntityId = monster.entityId;
            monster.Info.mapId = this.Map.ID;
            //设置完基本信息添加到资源里
            Monsters[monster.Id] = monster;
            //为了让怪物进入地图通知到其他的玩家
            this.Map.MonsterEnter(monster);
            return monster;
        }
    }
}
