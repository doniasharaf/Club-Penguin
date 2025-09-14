using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioClip flipClip;
    [SerializeField] private AudioClip matchClip;
    [SerializeField] private AudioClip mismatchClip;
    [SerializeField] private AudioClip gameOverClip;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayFlipSFX()
    {
        PlayClip(flipClip);
    }
    public void PlayMatchSFX()
    {
        PlayClip(matchClip);
    }
    public void PlayMismatchSFX()
    {
        PlayClip(mismatchClip);
    }
    public void PlayGameOverSFX()
    {
        PlayClip(gameOverClip);
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        _audioSource.PlayOneShot(clip);
    }
}
