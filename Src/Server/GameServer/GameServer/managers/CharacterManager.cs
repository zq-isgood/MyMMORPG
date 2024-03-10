using Common;
using GameServer.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class CharacterManager:Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>(); //存储角色的字典
        public CharacterManager()
        {

        }
        public void Dispose()
        {

        }
        public void Init()
        {

        }
        public void Clear()
        {
            this.Characters.Clear();
        }
        public Character AddCharacter(TCharacter cha)
        {
            Character character = new Character(CharacterType.Player, cha); //传入的是TCharacter,根据数据库的角色创建实体，在进入游戏时创建，因为如果只登陆不进入游戏，没必要创建实体
            EntityManager.Instance.AddEntity(cha.MapID, character);
            character.Info.EntityId = character.entityId;
            this.Characters[character.Id] = character; //添加到角色管理器
            return character;
        }
        public void RemoveCharacter(int characterId)
        {
            var cha = this.Characters[characterId];
            EntityManager.Instance.RemoveEntity(cha.Data.MapID, cha);
            this.Characters.Remove(characterId); 
        }
        public Character GetCharacter(int characterId)
        {
            Character character = null;
            this.Characters.TryGetValue(characterId, out character);
            return character;
        }
    }
}
