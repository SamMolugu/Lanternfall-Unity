using UnityEngine;

namespace Lanternfall.Monsters
{
    [CreateAssetMenu(menuName = "Lanternfall/Monsters/Monster", fileName = "Monster_")]
    public class MonsterDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public Sprite Portrait;
        public RuntimeAnimatorController Animator;
        public int MaxHp = 10;
        public int Atk = 2;
        public bool IsBoss;
    }
}
