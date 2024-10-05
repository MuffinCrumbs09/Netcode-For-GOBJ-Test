using Unity.Netcode;
using UnityEngine;


public enum SoundType
{
    SHOOT,
    RELOAD,
    FOOTSTEP
}
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;

    private static SoundManager Instance;
    private AudioSource _audioSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        Instance._audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }
}
