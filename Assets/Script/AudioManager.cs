using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-----Audio Source-----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-----Audio Clip-----")]
    public AudioClip background;
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip shootSound;
    public AudioClip meeleSound;

    private void Start()
    {
        musicSource.clip = background;
        // musicSource.loop = true;
        musicSource.volume = 0.05f;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
