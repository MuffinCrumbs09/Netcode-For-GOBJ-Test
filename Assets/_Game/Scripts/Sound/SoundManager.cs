using UnityEngine;

// List of SoundTypes
public enum SoundType
{
    SHOOT,
    RELOAD,
    FOOTSTEP
}

// Handles all sound in the game
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("AudioClips")]
    [SerializeField] private AudioClip[] soundList;

    private static SoundManager Instance;
    private AudioSource _audioSource;

    private void Awake()
    {
        //If there is an instance that isnt me, kill me
        if (Instance != null && Instance != this)
            Destroy(this);

        //Sets me as the instance
        Instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Plays a oneshot of a sound with a certain volume
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        Instance._audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }
}
