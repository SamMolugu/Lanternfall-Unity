using UnityEngine;

namespace Lanternfall.Equipment
{
    /// <summary>
    /// Data for one equippable item. Visual is a worn sprite (or prefab), not an inventory icon.
    /// </summary>
    [CreateAssetMenu(menuName = "Lanternfall/Equipment/Item", fileName = "Item_")]
    public class EquipmentItemDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayName;
        public EquipmentSlot Slot;
        [Tooltip("Sprite drawn on the character socket when worn/held.")]
        public Sprite WornSprite;
        [Tooltip("Optional world/held prefab (3D or animated). Overrides sprite if set.")]
        public GameObject WornPrefab;
        public Sprite InventoryIcon;
        public Color RarityColor = Color.white;
    }
}
