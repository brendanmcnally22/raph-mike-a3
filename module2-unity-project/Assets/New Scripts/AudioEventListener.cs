using UnityEngine;

public class AudioEventListener : MonoBehaviour, IEventListener
{
    public AudioSource audioSource;
    public AudioClip roarClip;
    public AudioClip chestClip;

    void OnEnable() => Subscribe();
    void OnDisable() => Unsubscribe();

    public void Subscribe()
    {
        SkeletonAI.OnSkeletonRoar += PlayRoarSound;
        Chest.OnChestOpened += PlayChestSound;
    }

    public void Unsubscribe()
    {
        SkeletonAI.OnSkeletonRoar -= PlayRoarSound;
        Chest.OnChestOpened -= PlayChestSound;
    }

    void PlayRoarSound(Vector3 pos)
    {
        audioSource.transform.position = pos;
        audioSource.PlayOneShot(roarClip);
    }

    void PlayChestSound()
    {
        audioSource.PlayOneShot(chestClip);
    }
}