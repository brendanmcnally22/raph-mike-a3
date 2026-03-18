using UnityEngine;

public class UIEventListener : MonoBehaviour, IEventListener
{
    void OnEnable() => Subscribe();
    void OnDisable() => Unsubscribe();

    public void Subscribe()
    {
        SkeletonAI.OnSkeletonRoar += OnRoar;
        Chest.OnChestOpened += OnChestOpened;
    }

    public void Unsubscribe()
    {
        SkeletonAI.OnSkeletonRoar -= OnRoar;
        Chest.OnChestOpened -= OnChestOpened;
    }

    void OnRoar(Vector3 pos)
    {
        Debug.Log(" Enemies alerted!");
    }

    void OnChestOpened()
    {
        Debug.Log("Chest opened!");
    }
}