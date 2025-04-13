using UnityEngine;
using System.Collections;

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
        if (clip == goal)
        {
            StartCoroutine(PlayGoalSFX());
        }
        else
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    private IEnumerator PlayGoalSFX()
    {
        // Play Goal Sound
        musicSource.Pause();

        // Play Goal Sound
        SFXSource.PlayOneShot(goal);

        // Wait for the sound to finish playing
        yield return new WaitForSeconds(goal.length);

        // Resume background music
        musicSource.UnPause();
    }
}
