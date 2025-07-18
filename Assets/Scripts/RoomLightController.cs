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
    public List<ParticleSystem> fireParticles;

    private bool isFlickering = false;

    public void TurnOn()
    {
        foreach (var light in lights)
            if (light != null) light.enabled = true;

        foreach (var ps in fireParticles)
            if (ps != null && !ps.isPlaying) ps.Play();
    }

    public void TurnOff()
    {
        foreach (var light in lights)
            if (light != null) light.enabled = false;

        foreach (var ps in fireParticles)
            if (ps != null && ps.isPlaying) ps.Stop();
    }

    public void SetLightColor(Color color)
    {
        foreach (var light in lights)
            if (light != null) light.color = color;
    }

    public void ActivateFire()
    {
        foreach (var ps in fireParticles)
            if (ps != null && !ps.isPlaying) ps.Play();
    }

    public void DeactivateFire()
    {
        foreach (var ps in fireParticles)
            if (ps != null && ps.isPlaying) ps.Stop();
    }

    public void StartFlicker(float duration, float interval)
    {
        StopAllCoroutines();
        StartCoroutine(Flicker(duration, interval));
    }

    IEnumerator Flicker(float duration, float interval)
    {
        float timer = 0f;
        bool on = false;
        while (timer < duration)
        {
            on = !on;
            foreach (var light in lights)
                if (light != null) light.enabled = on;
            timer += interval;
            yield return new WaitForSeconds(interval);
        }
        // Ensure lights are on after flicker
        foreach (var light in lights)
            if (light != null) light.enabled = true;
    }
}
