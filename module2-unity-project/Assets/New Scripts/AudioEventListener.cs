using UnityEngine;

public class AudioEventListener : MonoBehaviour, IEventListener
{
    // audio source for playing sounds
    public AudioSource audioSource;

    public AudioClip roarClip;
    public AudioClip chestClip;

    void OnEnable() => Subscribe();
    void OnDisable() => Unsubscribe();

    public void Subscribe()
    {
        // listening to multiple events
        // shows how 1 system can react to diff things
        SkeletonAI.OnSkeletonRoar += PlayRoarSound;
        Chest.OnChestLooted += PlayChestSound;
    }

    public void Unsubscribe()
    {
        SkeletonAI.OnSkeletonRoar -= PlayRoarSound;
        Chest.OnChestLooted -= PlayChestSound;
    }

    void PlayRoarSound(Vector3 pos)
    {
        // move audio source to where roar happened
        // so it feels more 3D and not just global sound
        audioSource.transform.position = pos;

        audioSource.PlayOneShot(roarClip);
    }

    void PlayChestSound(int amount)
    {
        // doesnt really need the amount but event requires it
        audioSource.PlayOneShot(chestClip);
    }
}