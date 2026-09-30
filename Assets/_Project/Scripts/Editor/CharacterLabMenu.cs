#if UNITY_EDITOR
using Lanternfall.Character;
using Lanternfall.Equipment;
using UnityEditor;
using UnityEngine;

namespace Lanternfall.EditorTools
{
    /// <summary>Builds a socketed character hierarchy for the CharacterLab scene.</summary>
    public static class CharacterLabMenu
    {
        [MenuItem("Lanternfall/Setup/Create Character Prefab Hierarchy")]
        public static void CreateCharacterHierarchy()
        {
            var root = new GameObject("Hero");
            Undo.RegisterCreatedObjectUndo(root, "Create Hero");

            var body = new GameObject("Body");
            body.transform.SetParent(root.transform, false);
            var bodySr = body.AddComponent<SpriteRenderer>();
            bodySr.sortingOrder = 0;

            var visual = root.AddComponent<CharacterVisual>();
            root.AddComponent<CharacterAppearanceController>();

            var slots = new[]
            {
                EquipmentSlot.Boots,
                EquipmentSlot.Chest,
                EquipmentSlot.Gloves,
                EquipmentSlot.Helm,
                EquipmentSlot.Offhand,
                EquipmentSlot.Weapon,
            };

            var socketList = new CharacterVisual.SlotSocket[slots.Length];
            for (var i = 0; i < slots.Length; i++)
            {
                var slot = slots[i];
                var go = new GameObject(slot.ToString());
                go.transform.SetParent(root.transform, false);
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sortingOrder = 10 + i;
                socketList[i] = new CharacterVisual.SlotSocket
                {
                    Slot = slot,
                    Anchor = go.transform,
                    Renderer = sr,
                };
            }

            var so = new SerializedObject(visual);
            so.FindProperty("bodyRenderer").objectReferenceValue = bodySr;
            var socketsProp = so.FindProperty("sockets");
            socketsProp.arraySize = socketList.Length;
            for (var i = 0; i < socketList.Length; i++)
            {
                var el = socketsProp.GetArrayElementAtIndex(i);
                el.FindPropertyRelative("Slot").enumValueIndex = (int)socketList[i].Slot;
                el.FindPropertyRelative("Anchor").objectReferenceValue = socketList[i].Anchor;
                el.FindPropertyRelative("Renderer").objectReferenceValue = socketList[i].Renderer;
            }
            so.ApplyModifiedPropertiesWithoutUndo();

            Selection.activeGameObject = root;
            Debug.Log("Hero hierarchy created. Assign body sprite + loadout on CharacterAppearanceController.");
        }
    }
}
#endif
