using System;
using Lanternfall.Equipment;
using UnityEngine;

namespace Lanternfall.Character
{
    /// <summary>
    /// Swords &amp; Sandals style body: base sprite + one socket per visual gear slot.
    /// </summary>
    public class CharacterVisual : MonoBehaviour
    {
        [Serializable]
        public struct SlotSocket
        {
            public EquipmentSlot Slot;
            public Transform Anchor;
            public SpriteRenderer Renderer;
        }

        [Header("Body")]
        [SerializeField] SpriteRenderer bodyRenderer;
        [SerializeField] Sprite defaultBody;

        [Header("Sockets (Weapon, Offhand, Helm, Chest, Gloves, Boots)")]
        [SerializeField] SlotSocket[] sockets = Array.Empty<SlotSocket>();

        public void SetBody(Sprite body)
        {
            if (bodyRenderer == null) return;
            bodyRenderer.sprite = body != null ? body : defaultBody;
        }

        public void ClearSlot(EquipmentSlot slot)
        {
            var socket = FindSocket(slot);
            if (socket.Renderer != null) socket.Renderer.sprite = null;
            ClearChildren(socket.Anchor);
        }

        public void Equip(EquipmentItemDefinition item)
        {
            if (item == null) return;
            var socket = FindSocket(item.Slot);
            if (socket.Anchor == null) return;

            ClearChildren(socket.Anchor);

            if (item.WornPrefab != null)
            {
                Instantiate(item.WornPrefab, socket.Anchor);
                if (socket.Renderer != null) socket.Renderer.sprite = null;
                return;
            }

            if (socket.Renderer != null)
                socket.Renderer.sprite = item.WornSprite;
        }

        public void ApplyLoadout(LoadoutDefinition loadout)
        {
            if (loadout == null) return;
            ClearAllVisualSlots();
            Equip(loadout.Weapon);
            Equip(loadout.Offhand);
            Equip(loadout.Helm);
            Equip(loadout.Chest);
            Equip(loadout.Gloves);
            Equip(loadout.Boots);
            Equip(loadout.Lantern);
        }

        public void ClearAllVisualSlots()
        {
            ClearSlot(EquipmentSlot.Weapon);
            ClearSlot(EquipmentSlot.Offhand);
            ClearSlot(EquipmentSlot.Helm);
            ClearSlot(EquipmentSlot.Chest);
            ClearSlot(EquipmentSlot.Gloves);
            ClearSlot(EquipmentSlot.Boots);
            ClearSlot(EquipmentSlot.Lantern);
        }

        SlotSocket FindSocket(EquipmentSlot slot)
        {
            for (var i = 0; i < sockets.Length; i++)
                if (sockets[i].Slot == slot) return sockets[i];
            return default;
        }

        static void ClearChildren(Transform anchor)
        {
            if (anchor == null) return;
            for (var i = anchor.childCount - 1; i >= 0; i--)
                Destroy(anchor.GetChild(i).gameObject);
        }
    }
}
