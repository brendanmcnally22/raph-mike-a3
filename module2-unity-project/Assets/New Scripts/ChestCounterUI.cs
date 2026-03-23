using UnityEngine;
using TMPro;

public class ChestCounterUI : MonoBehaviour
{
    // text reference from TMP UI
    public TextMeshProUGUI text;

    // how many chests opened so far
    int count = 0;

    void OnEnable()
    {
        // subscribe to chest event
        // anytime a chest is looted this runs
        Chest.OnChestLooted += AddChest;
    }

    void OnDisable()
    {
        // unsub to avoid memory leaks / weird bugs
        Chest.OnChestLooted -= AddChest;
    }

    void AddChest(int amount)
    {
        // im just counting how many chests opened, not total gold
        count++;

        // update UI text
        // could format nicer but this works fine
        text.text = "Chest Amount " + count;
    }
}