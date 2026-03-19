using UnityEngine;

public class EventDebugger : MonoBehaviour
{
    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        // Print every 2 seconds (not spammy)
        if (timer > 2f)
        {
            DebugListeners();
            timer = 0f;
        }
    }

    void DebugListeners()
    {
        int roarListeners = SkeletonAI.OnSkeletonRoar?.GetInvocationList().Length ?? 0;
        int chestListeners = Chest.OnChestLooted?.GetInvocationList().Length ?? 0;

        Debug.Log($"[EVENT DEBUG] Roar Listeners: {roarListeners} | Chest Listeners: {chestListeners}");
    }
}