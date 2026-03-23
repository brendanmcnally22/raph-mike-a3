using UnityEngine;

public class UIEventListener : MonoBehaviour, IEventListener
{
    void OnEnable() => Subscribe();
    void OnDisable() => Unsubscribe();

    public void Subscribe()
    {
        // just listening for debug / feedback events
        SkeletonAI.OnSkeletonRoar += OnRoar;
        Chest.OnChestLooted += OnChestOpened;
    }

    public void Unsubscribe()
    {
        SkeletonAI.OnSkeletonRoar -= OnRoar;
        Chest.OnChestLooted -= OnChestOpened;
    }

    void OnRoar(Vector3 pos)
    {
        // simple feedback so we know event is firing
        Debug.Log("Enemies alerted!");
    }

    void OnChestOpened(int goldAmount)
    {
        // log gold amount for debugging / feedback
        Debug.Log("Chest opened! Gold gained: " + goldAmount);
    }
}