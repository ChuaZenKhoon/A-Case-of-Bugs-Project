using UnityEngine;

/**
 * A manager superclass that handles playing sound effects.
 */
public class SFXPlayer : MonoBehaviour {

    [SerializeField] protected AudioSource audioSource;

    //SoundSO stores all audioclips, just reference it to get audioclip needed
    [SerializeField] protected SoundSO soundSO;

    protected float sfxVolume;

    protected void Start() {
        sfxVolume = SoundManager.Instance.GetSFXVolume();
    }

    public void SetSFXVolume(float volume) {
        sfxVolume = volume;
    }

    //Following functions handle audioclip playing
    protected void PlaySound(AudioClip[] audioClipArray, Vector3 pos, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], pos, volume);
    }

    protected void PlaySound(AudioClip audioClip, Vector3 pos, float volumeMultiplier = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, pos, volumeMultiplier * sfxVolume);
    }

    protected void PlaySoundOneShot(AudioClip audioClip, float volumeMultiplier = 1f) {
        audioSource.PlayOneShot(audioClip, volumeMultiplier * sfxVolume);
    }

    protected void PlaySoundOneShot(AudioClip[] audioClips, float volumeMultiplier = 1f) {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], volumeMultiplier * sfxVolume);
    }
}
