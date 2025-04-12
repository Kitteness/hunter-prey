using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public static AudioManager instance;
    public AudioClip background;
    public AudioClip damage;
    public AudioClip goal;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();

    }


    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
