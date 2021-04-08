using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public void Play(Transform game)
    {
        AudioSource audio = game.GetComponent<AudioSource>();
        
        audio.mute = false;
        audio.Play();
    }
}
