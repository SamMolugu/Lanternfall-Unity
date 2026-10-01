#if UNITY_EDITOR
using Lanternfall.Character;
using Lanternfall.Core;
using Lanternfall.Equipment;
using Lanternfall.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Lanternfall.EditorTools
{
    public static class CharacterLabMenu
    {
        const string LabScenePath = "Assets/_Project/Scenes/CharacterLab.unity";
        const string TitleScenePath = "Assets/_Project/Scenes/Title.unity";

        [MenuItem("Lanternfall/Setup/Create Character Prefab Hierarchy")]
        public static void CreateCharacterHierarchy()
        {
            var root = new GameObject("Hero");
            Undo.RegisterCreatedObjectUndo(root, "Create Hero");
            BuildHero(root);
            Selection.activeGameObject = root;
            Debug.Log("Hero hierarchy created.");
        }

        [MenuItem("Lanternfall/Setup/Build Epic Character Lab Scene")]
        public static void BuildEpicCharacterLab()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var cam = Camera.main;
            if (cam != null)
            {
                cam.orthographic = true;
                cam.orthographicSize = 3.2f;
                cam.backgroundColor = new Color(0.05f, 0.04f, 0.03f);
                cam.transform.position = new Vector3(0f, 0.4f, -10f);
                if (cam.GetComponent<CinematicOrthoCamera>() == null)
                    cam.gameObject.AddComponent<CinematicOrthoCamera>();
            }

            var backdrop = CreateSpriteObject("Backdrop", 0);
            backdrop.transform.localScale = Vector3.one * 1.15f;
            var bg = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Art/Environments/bg-character-lab-cave.png");
            if (bg != null) backdrop.GetComponent<SpriteRenderer>().sprite = bg;
            backdrop.GetComponent<SpriteRenderer>().sortingOrder = -20;
            backdrop.GetComponent<SpriteRenderer>().color = new Color(0.85f, 0.85f, 0.9f, 1f);

            var glow = CreateSpriteObject("GroundGlow", -1);
            glow.transform.localPosition = new Vector3(0f, -1.55f, 0f);
            glow.transform.localScale = new Vector3(3.2f, 0.55f, 1f);
            var glowSr = glow.GetComponent<SpriteRenderer>();
            glowSr.color = new Color(1f, 0.65f, 0.25f, 0.3f);
            // Use a simple white sprite if available; otherwise leave empty for user to assign
            glowSr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");

            var lamp = new GameObject("LanternGlow");
            lamp.transform.position = new Vector3(-1.15f, 1.35f, 0f);
            lamp.transform.localScale = new Vector3(2.4f, 2.4f, 1f);
            var lampSr = lamp.AddComponent<SpriteRenderer>();
            lampSr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
            lampSr.color = new Color(1f, 0.7f, 0.3f, 0.22f);
            lampSr.sortingOrder = -10;
            lamp.AddComponent<LanternPulse>();

            var hero = new GameObject("Hero");
            hero.transform.localPosition = new Vector3(0f, -0.85f, 0f);
            BuildHero(hero);

            var canvasGo = new GameObject("LabCanvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGo.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            canvasGo.AddComponent<GraphicRaycaster>();

            var title = CreateUiText(canvasGo.transform, "Title", "LANTERNFALL", 64, TextAnchor.UpperCenter,
                new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -48f));
            title.fontStyle = FontStyle.Bold;
            title.color = new Color(0.95f, 0.82f, 0.45f);

            var body = CreateUiText(canvasGo.transform, "BodyLabel", "HUMAN MALE", 28, TextAnchor.LowerLeft,
                new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(48f, 96f));
            var loadout = CreateUiText(canvasGo.transform, "LoadoutLabel", "BLADE", 34, TextAnchor.LowerRight,
                new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-48f, 96f));
            loadout.alignment = TextAnchor.LowerRight;
            var hint = CreateUiText(canvasGo.transform, "Hint", "A / D  body   ·   Q / E  loadout   ·   Space  flourish", 20,
                TextAnchor.LowerCenter, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 28f));
            hint.color = new Color(0.75f, 0.7f, 0.6f, 0.85f);

            var barGo = new GameObject("AccentBar", typeof(RectTransform), typeof(Image));
            barGo.transform.SetParent(canvasGo.transform, false);
            var barRt = barGo.GetComponent<RectTransform>();
            barRt.anchorMin = new Vector2(0f, 0f);
            barRt.anchorMax = new Vector2(1f, 0f);
            barRt.pivot = new Vector2(0.5f, 0f);
            barRt.sizeDelta = new Vector2(0f, 6f);
            barRt.anchoredPosition = Vector2.zero;
            var barImg = barGo.GetComponent<Image>();
            barImg.color = new Color(0.79f, 0.64f, 0.15f);

            var director = hero.AddComponent<CharacterLabDirector>();
            var so = new SerializedObject(director);
            so.FindProperty("visual").objectReferenceValue = hero.GetComponent<CharacterVisual>();
            so.FindProperty("stageBackdrop").objectReferenceValue = backdrop.GetComponent<SpriteRenderer>();
            so.FindProperty("groundGlow").objectReferenceValue = glowSr;
            so.FindProperty("heroRoot").objectReferenceValue = hero.transform;
            so.FindProperty("titleLabel").objectReferenceValue = title;
            so.FindProperty("bodyLabel").objectReferenceValue = body;
            so.FindProperty("loadoutLabel").objectReferenceValue = loadout;
            so.FindProperty("hintLabel").objectReferenceValue = hint;
            so.FindProperty("accentBar").objectReferenceValue = barImg;

            // Bodies
            var bodies = so.FindProperty("bodies");
            var bodySprites = new[]
            {
                ("Human · Male", "Assets/_Project/Art/Characters/Human/Male/base.png"),
                ("Human · Female", "Assets/_Project/Art/Characters/Human/Female/base.png"),
                ("Glowkin · Male", "Assets/_Project/Art/Characters/Glowkin/Male/base.png"),
                ("Glowkin · Female", "Assets/_Project/Art/Characters/Glowkin/Female/base.png"),
            };
            bodies.arraySize = bodySprites.Length;
            for (var i = 0; i < bodySprites.Length; i++)
            {
                var el = bodies.GetArrayElementAtIndex(i);
                el.FindPropertyRelative("Label").stringValue = bodySprites[i].Item1;
                el.FindPropertyRelative("Body").objectReferenceValue =
                    AssetDatabase.LoadAssetAtPath<Sprite>(bodySprites[i].Item2);
            }

            // Loadouts use full-body overrides (Swords & Sandals style)
            var loadouts = so.FindProperty("loadouts");
            var opts = new[]
            {
                ("Blade", "Sword, shield, mail — the classic delver.", "Assets/_Project/Art/Characters/Human/Male/loadout-blade.png", new Color(0.85f, 0.75f, 0.35f)),
                ("Lamp", "Staff and robe — light against the dark.", "Assets/_Project/Art/Characters/Human/Male/loadout-lamp.png", new Color(0.45f, 0.7f, 1f)),
                ("Bulwark", "Helm, mace, iron — hold the line.", "Assets/_Project/Art/Characters/Human/Male/loadout-bulwark.png", new Color(0.7f, 0.75f, 0.8f)),
                ("Shadow", "Hood and dagger — soft steps.", "Assets/_Project/Art/Characters/Human/Male/loadout-shadow.png", new Color(0.55f, 0.35f, 0.75f)),
                ("Ember Plate", "Glowkin legend — flame and steel.", "Assets/_Project/Art/Characters/Glowkin/Male/loadout-plate-legend.png", new Color(1f, 0.45f, 0.15f)),
            };
            loadouts.arraySize = opts.Length;
            for (var i = 0; i < opts.Length; i++)
            {
                var el = loadouts.GetArrayElementAtIndex(i);
                el.FindPropertyRelative("Label").stringValue = opts[i].Item1;
                el.FindPropertyRelative("Tagline").stringValue = opts[i].Item2;
                el.FindPropertyRelative("FullBodyOverride").objectReferenceValue =
                    AssetDatabase.LoadAssetAtPath<Sprite>(opts[i].Item3);
                el.FindPropertyRelative("Accent").colorValue = opts[i].Item4;
            }

            so.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, LabScenePath);
            AssetDatabase.Refresh();
            Debug.Log("Epic Character Lab scene saved to " + LabScenePath);
        }

        static void BuildHero(GameObject root)
        {
            var body = new GameObject("Body");
            body.transform.SetParent(root.transform, false);
            var bodySr = body.AddComponent<SpriteRenderer>();
            bodySr.sortingOrder = 0;

            var visual = root.GetComponent<CharacterVisual>() ?? root.AddComponent<CharacterVisual>();
            if (root.GetComponent<CharacterAppearanceController>() == null)
                root.AddComponent<CharacterAppearanceController>();

            var slots = new[]
            {
                EquipmentSlot.Boots, EquipmentSlot.Chest, EquipmentSlot.Gloves,
                EquipmentSlot.Helm, EquipmentSlot.Offhand, EquipmentSlot.Weapon,
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
                    Slot = slot, Anchor = go.transform, Renderer = sr,
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
        }

        static GameObject CreateSpriteObject(string name, int order)
        {
            var go = new GameObject(name);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sortingOrder = order;
            return go;
        }

        static Text CreateUiText(Transform parent, string name, string content, int size, TextAnchor anchor,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.pivot = anchorMin;
            rt.anchoredPosition = anchoredPos;
            rt.sizeDelta = new Vector2(640f, 120f);
            var text = go.GetComponent<Text>();
            text.text = content;
            text.fontSize = size;
            text.alignment = anchor;
            text.color = new Color(0.93f, 0.88f, 0.78f);
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (text.font == null) text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            return text;
        }
    }
}
#endif
