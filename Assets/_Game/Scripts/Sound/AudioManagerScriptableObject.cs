using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/AudioData", order  = -1)]
public class AudioManagerScriptableObject : ScriptableObject
{
    public AudioClip[] soundList;
}
