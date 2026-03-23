using UnityEngine;

public class LootManager : MonoBehaviour, IEventListener
{

    //only in the debug
    public int totalGold;

    void OnEnable() => Subscribe();
    void OnDisable() => Unsubscribe();

    public void Subscribe()
    {
        Chest.OnChestLooted += AddGold;
    }

    public void Unsubscribe()
    {
        Chest.OnChestLooted -= AddGold;
    }

    void AddGold(int amount)
    {
        totalGold += amount;
        Debug.Log("Gold: " + totalGold);
    }
}