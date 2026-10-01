using UnityEngine;

namespace Lanternfall.World
{
    /// <summary>Soft lantern pulse — works with a Light and/or a SpriteRenderer glow.</summary>
    public class LanternPulse : MonoBehaviour
    {
        [SerializeField] float baseIntensity = 1.4f;
        [SerializeField] float amplitude = 0.25f;
        [SerializeField] float speed = 2.2f;
        [SerializeField] Color flame = new Color(1f, 0.72f, 0.35f);

        Light lamp;
        SpriteRenderer glow;
        Color glowBase;

        void Awake()
        {
            lamp = GetComponent<Light>();
            glow = GetComponent<SpriteRenderer>();
            if (lamp != null) lamp.color = flame;
            if (glow != null) glowBase = glow.color;
        }

        void Update()
        {
            var pulse = Mathf.Sin(Time.time * speed) * amplitude
                + Mathf.Sin(Time.time * speed * 2.7f) * (amplitude * 0.35f);
            if (lamp != null) lamp.intensity = baseIntensity + pulse;
            if (glow != null)
            {
                var c = glowBase;
                c.a = Mathf.Clamp01(glowBase.a + pulse * 0.35f);
                glow.color = c;
            }
        }
    }
}
