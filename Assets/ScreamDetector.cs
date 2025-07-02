using System.Collections;
using UnityEngine;

public class ScreamDetector : MonoBehaviour
{
    public string microphoneDevice;
    public float screamThreshold = 0.5f; // Adjust this sensitivity
    public float checkInterval = 0.1f;

    private AudioClip micClip;
    private float nextCheckTime;

    public AudioSource audioSource;
    public AudioClip jumpscareClip;

    public EndSequenceManager screenFader;

    public AudioSource bgMusicSource;
    public float musicFadeOutDuration = 1.5f;

    private float micLoudness = 0f; // ⬅️ New field to store loudness

    void Start()
    {
        // Use default mic if none specified
        microphoneDevice = Microphone.devices.Length > 0 ? Microphone.devices[0] : null;

        if (microphoneDevice == null)
        {
            Debug.LogError("No microphone found!");
            return;
        }

        micClip = Microphone.Start(microphoneDevice, true, 10, 44100);
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            micLoudness = GetMicVolume(); // ⬅️ Store the volume for other scripts
            // Debug.Log("Mic Volume: " + micLoudness.ToString("F3"));

            if (micLoudness > screamThreshold)
            {
                OnScreamDetected();
            }
        }
    }

    float GetMicVolume()
    {
        if (micClip == null || !Microphone.IsRecording(microphoneDevice)) return 0f;

        int micPosition = Microphone.GetPosition(microphoneDevice) - 128;
        if (micPosition < 0) return 0f;

        float[] samples = new float[256]; // More samples = better volume averaging

        micClip.GetData(samples, micPosition);

        float sum = 0f;
        foreach (float sample in samples)
            sum += sample * sample;

        return Mathf.Sqrt(sum / samples.Length); // RMS volume
    }

    void OnScreamDetected()
    {
        if (bgMusicSource != null)
            StartCoroutine(FadeOutMusic());

        if (audioSource && jumpscareClip)
        {
            audioSource.PlayOneShot(jumpscareClip);
        }

        if (screenFader != null)
            screenFader.TriggerEndSequence();

        // Delay game over by 1 second
        StartCoroutine(TriggerGameOverAfterDelay(1f));
    }

    IEnumerator FadeOutMusic()
    {
        float startVolume = bgMusicSource.volume;
        float time = 0f;

        while (time < musicFadeOutDuration)
        {
            time += Time.deltaTime;
            bgMusicSource.volume = Mathf.Lerp(startVolume, 0f, time / musicFadeOutDuration);
            yield return null;
        }

        bgMusicSource.Stop();
    }

    IEnumerator TriggerGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.LogWarning("💀 GAME OVER: Scream Triggered");

        // Example game over: freeze game
        //Time.timeScale = 0f;

        // Or load game over scene: SceneManager.LoadScene("GameOver");
    }

    // ✅ New public method for ghost AI to check mic activity
    public bool IsPlayerTalking()
    {
        return micLoudness > 0.1f; // Adjust based on testing for sensitivity
    }
}
