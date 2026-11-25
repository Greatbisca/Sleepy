using UnityEngine;

// Put this on the AlarmClock cube (needs the Audio Source already on it).
[RequireComponent(typeof(AudioSource))]
public class AlarmClock : MonoBehaviour, IInteractable
{
    public bool IsRinging { get; private set; } = true;
    public float TimeRinging { get; private set; }

    // The "E" prompt only shows while the alarm is still ringing.
    public bool CanInteract => IsRinging;

    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (IsRinging) TimeRinging += Time.deltaTime;
    }

    public void Interact()
    {
        if (!IsRinging) return;
        IsRinging = false;
        source.Stop();
    }
}