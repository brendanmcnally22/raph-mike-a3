using UnityEngine;

public class UIEventListener : MonoBehaviour, IEventListener
{
    void OnEnable() => Subscribe();
    void OnDisable() => Unsubscribe();

    public void Subscribe()
    {
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
        Debug.Log(" Enemies alerted!");
    }

    void OnChestOpened(int goldAmount)
    {
        Debug.Log("Chest opened! Gold gained: " + goldAmount);
    }
}