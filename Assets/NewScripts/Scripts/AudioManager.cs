using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;   // Singleton

    [Header("Audio Source")]
    public AudioSource source;

    [Header("Clips")]
    public AudioClip plipHit;
    public AudioClip positiveFeedback;
    public AudioClip negativeFeedback;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        source = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            source.PlayOneShot(clip);
        }
    }
}