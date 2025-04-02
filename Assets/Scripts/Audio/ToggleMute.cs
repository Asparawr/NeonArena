using UnityEngine;
using UnityEngine.UI;

public class ToggleMute : MonoBehaviour
{
    public void ToggleMuteAudio()
    {
        FindObjectOfType<AudioManager>().ToggleMute();
    }
}
