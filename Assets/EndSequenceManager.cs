using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndSequenceManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image ghostImage;
    public Image blackScreen;

    [Header("Timing")]
    public float ghostFadeTime = 0.5f;
    public float delayBeforeBlack = 1.2f;
    public float blackFadeTime = 1.0f;

    public void TriggerEndSequence()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        Debug.Log("🎬 Ending Sequence Started");

        // Fade in ghost image
        if (ghostImage)
        {
            ghostImage.gameObject.SetActive(true);
            Color start = new Color(1, 1, 1, 0);
            Color end = new Color(1, 1, 1, 1);
            float t = 0f;

            while (t < ghostFadeTime)
            {
                t += Time.deltaTime;
                ghostImage.color = Color.Lerp(start, end, t / ghostFadeTime);
                yield return null;
            }
            ghostImage.color = end;
        }

        yield return new WaitForSeconds(delayBeforeBlack);

        // Fade in black screen
        if (blackScreen)
        {
            blackScreen.gameObject.SetActive(true);
            Color start = new Color(0, 0, 0, 0);
            Color end = new Color(0, 0, 0, 0.85f); // ≈ 217/255

            float t = 0f;

            while (t < blackFadeTime)
            {
                t += Time.deltaTime;
                blackScreen.color = Color.Lerp(start, end, t / blackFadeTime);
                yield return null;
            }
            blackScreen.color = end;
        }

        // Freeze gameplay
        Time.timeScale = 0f;
    }
}
