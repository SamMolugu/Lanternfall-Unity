using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lanternfall.Core
{
    /// <summary>Title splash — Space / Click to enter the Character Lab.</summary>
    public class TitleScreenDirector : MonoBehaviour
    {
        [SerializeField] string nextScene = "CharacterLab";
        [SerializeField] CanvasGroup fade;
        [SerializeField] Text subtitle;
        [SerializeField] float fadeSpeed = 1.4f;

        bool leaving;
        float alpha = 1f;

        void Start()
        {
            if (subtitle != null)
                subtitle.text = "CARRY THE LIGHT · CLAIM WHAT YOU CAN";
            if (fade != null) fade.alpha = 0f;
        }

        void Update()
        {
            if (leaving)
            {
                alpha += Time.deltaTime * fadeSpeed;
                if (fade != null) fade.alpha = Mathf.Clamp01(alpha);
                if (alpha >= 1f)
                    SceneManager.LoadScene(nextScene);
                return;
            }

            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
                leaving = true;
        }
    }
}
