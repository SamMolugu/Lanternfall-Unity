using Lanternfall.Equipment;
using UnityEngine;

namespace Lanternfall.Character
{
    /// <summary>Lab / runtime helper to swap race body + loadout for preview.</summary>
    public class CharacterAppearanceController : MonoBehaviour
    {
        [SerializeField] CharacterVisual visual;
        [SerializeField] Sprite bodySprite;
        [SerializeField] LoadoutDefinition startingLoadout;

        void Reset()
        {
            visual = GetComponent<CharacterVisual>();
        }

        void Start()
        {
            Apply();
        }

        [ContextMenu("Apply Appearance")]
        public void Apply()
        {
            if (visual == null) return;
            visual.SetBody(bodySprite);
            if (startingLoadout != null) visual.ApplyLoadout(startingLoadout);
        }

        public void PreviewLoadout(LoadoutDefinition loadout)
        {
            if (visual == null) return;
            visual.SetBody(bodySprite);
            visual.ApplyLoadout(loadout);
        }
    }
}
