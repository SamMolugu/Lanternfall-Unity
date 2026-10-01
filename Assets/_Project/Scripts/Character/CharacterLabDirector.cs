using System;
using Lanternfall.Equipment;
using UnityEngine;
using UnityEngine.UI;

namespace Lanternfall.Character
{
    /// <summary>
    /// Epic Character Lab — cycle race/gender/loadout with juice and branding.
    /// </summary>
    public class CharacterLabDirector : MonoBehaviour
    {
        [Serializable]
        public class BodyOption
        {
            public string Label;
            public Sprite Body;
        }

        [Serializable]
        public class LoadoutOption
        {
            public string Label;
            [TextArea] public string Tagline;
            public LoadoutDefinition Loadout;
            public Sprite FullBodyOverride;
            public Color Accent = new Color(0.79f, 0.64f, 0.15f);
        }

        [Header("Stage")]
        [SerializeField] CharacterVisual visual;
        [SerializeField] SpriteRenderer stageBackdrop;
        [SerializeField] SpriteRenderer groundGlow;
        [SerializeField] Transform heroRoot;
        [SerializeField] float breatheAmplitude = 0.04f;
        [SerializeField] float breatheSpeed = 1.2f;

        [Header("Catalog")]
        [SerializeField] BodyOption[] bodies = Array.Empty<BodyOption>();
        [SerializeField] LoadoutOption[] loadouts = Array.Empty<LoadoutOption>();

        [Header("UI (optional)")]
        [SerializeField] Text titleLabel;
        [SerializeField] Text bodyLabel;
        [SerializeField] Text loadoutLabel;
        [SerializeField] Text hintLabel;
        [SerializeField] Image accentBar;

        int bodyIndex;
        int loadoutIndex;
        Vector3 heroBasePos;
        float flashUntil;

        void Awake()
        {
            if (heroRoot != null) heroBasePos = heroRoot.localPosition;
            if (titleLabel != null) titleLabel.text = "LANTERNFALL";
            if (hintLabel != null)
                hintLabel.text = "A / D  body   ·   Q / E  loadout   ·   Space  flourish";
            Apply();
        }

        void Update()
        {
            HandleInput();
            AnimateStage();
        }

        void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                CycleBody(-1);
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                CycleBody(1);
            if (Input.GetKeyDown(KeyCode.Q))
                CycleLoadout(-1);
            if (Input.GetKeyDown(KeyCode.E))
                CycleLoadout(1);
            if (Input.GetKeyDown(KeyCode.Space))
                Flourish();
        }

        public void CycleBody(int delta)
        {
            if (bodies.Length == 0) return;
            bodyIndex = (bodyIndex + delta + bodies.Length) % bodies.Length;
            Flourish();
            Apply();
        }

        public void CycleLoadout(int delta)
        {
            if (loadouts.Length == 0) return;
            loadoutIndex = (loadoutIndex + delta + loadouts.Length) % loadouts.Length;
            Flourish();
            Apply();
        }

        public void Apply()
        {
            if (visual == null) return;

            var body = bodies.Length > 0 ? bodies[bodyIndex] : null;
            var loadout = loadouts.Length > 0 ? loadouts[loadoutIndex] : null;

            if (loadout != null && loadout.FullBodyOverride != null)
            {
                visual.ClearAllVisualSlots();
                visual.SetBody(loadout.FullBodyOverride);
            }
            else
            {
                visual.SetBody(body != null ? body.Body : null);
                if (loadout != null) visual.ApplyLoadout(loadout.Loadout);
                else visual.ClearAllVisualSlots();
            }

            if (bodyLabel != null)
                bodyLabel.text = body != null ? body.Label.ToUpperInvariant() : "—";
            if (loadoutLabel != null)
            {
                var name = loadout != null ? loadout.Label : "—";
                var tag = loadout != null ? loadout.Tagline : "";
                loadoutLabel.text = string.IsNullOrEmpty(tag) ? name.ToUpperInvariant() : $"{name.ToUpperInvariant()}\n<size=70%>{tag}</size>";
            }
            if (accentBar != null && loadout != null)
                accentBar.color = loadout.Accent;
            if (groundGlow != null && loadout != null)
                groundGlow.color = new Color(loadout.Accent.r, loadout.Accent.g, loadout.Accent.b, 0.35f);
        }

        void Flourish()
        {
            flashUntil = Time.time + 0.25f;
        }

        void AnimateStage()
        {
            if (heroRoot != null)
            {
                var y = Mathf.Sin(Time.time * breatheSpeed) * breatheAmplitude;
                var punch = Time.time < flashUntil ? 0.08f : 0f;
                heroRoot.localPosition = heroBasePos + new Vector3(0f, y + punch, 0f);
                var scale = 1f + (Time.time < flashUntil ? 0.04f : 0f);
                heroRoot.localScale = Vector3.one * scale;
            }

            if (groundGlow != null)
            {
                var c = groundGlow.color;
                c.a = 0.28f + Mathf.Sin(Time.time * 2.4f) * 0.08f;
                groundGlow.color = c;
            }
        }
    }
}
