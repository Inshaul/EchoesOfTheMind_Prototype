using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomLightController : MonoBehaviour
{
    [Header("Room Identity")]
    public string roomName;

    [Header("Lighting References")]
    public List<Light> lights;

    [Header("Effect References (Optional)")]
    public List<ParticleSystem> fireEffects;

    private bool isFlickering = false;

    // Turn ON lights and particles
    public void TurnOn()
    {
        foreach (var light in lights)
            light.enabled = true;

        foreach (var ps in fireEffects)
            if (!ps.isPlaying) ps.Play();
    }

    // Turn OFF lights and particles
    public void TurnOff()
    {
        foreach (var light in lights)
            light.enabled = false;

        foreach (var ps in fireEffects)
            if (ps.isPlaying) ps.Stop();
    }

    // Set light color
    public void SetLightColor(Color color)
    {
        foreach (var light in lights)
            light.color = color;
    }

    // Start flickering lights
    public void StartFlicker(float duration = 2f, float interval = 0.2f)
    {
        if (!isFlickering)
            StartCoroutine(FlickerRoutine(duration, interval));
    }

    private IEnumerator FlickerRoutine(float duration, float interval)
    {
        isFlickering = true;
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            foreach (var light in lights)
                light.enabled = !light.enabled;

            yield return new WaitForSeconds(interval);
        }

        TurnOn(); // Restore lights and particles
        isFlickering = false;
    }

    // Optional: Activate fire effects only (e.g., for Hell room)
    public void ActivateFire()
    {
        foreach (var ps in fireEffects)
            if (!ps.isPlaying) ps.Play();
    }

    public void DeactivateFire()
    {
        foreach (var ps in fireEffects)
            if (ps.isPlaying) ps.Stop();
    }
}
