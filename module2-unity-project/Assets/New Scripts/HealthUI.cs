using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateUI;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateUI;
    }

    void UpdateUI(int hp)
    {
        text.text = "Health: " + hp;
    }
}