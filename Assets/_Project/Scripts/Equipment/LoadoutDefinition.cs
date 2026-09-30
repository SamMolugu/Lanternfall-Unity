using UnityEngine;

namespace Lanternfall.Equipment
{
    /// <summary>Starting kit look (Blade, Lamp, Bulwark, Shadow) — Swords &amp; Sandals style.</summary>
    [CreateAssetMenu(menuName = "Lanternfall/Equipment/Loadout", fileName = "Loadout_")]
    public class LoadoutDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        [TextArea] public string Description;
        public EquipmentItemDefinition Weapon;
        public EquipmentItemDefinition Offhand;
        public EquipmentItemDefinition Helm;
        public EquipmentItemDefinition Chest;
        public EquipmentItemDefinition Gloves;
        public EquipmentItemDefinition Boots;
        public EquipmentItemDefinition Lantern;
    }
}
