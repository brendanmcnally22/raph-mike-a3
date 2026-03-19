using UnityEngine;
using TMPro;

public class ChestCounterUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    int count = 0;

    void OnEnable()
    {
        Chest.OnChestLooted += AddChest;
    }

    void OnDisable()
    {
        Chest.OnChestLooted -= AddChest;
    }

    void AddChest(int amount)
    {
        count++;
        text.text = "Chest Amount" + count;
    }
}