using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    // UI text reference
    public TextMeshProUGUI text;

    void OnEnable()
    {
        // listen to health updates
        PlayerHealth.OnHealthChanged += UpdateUI;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateUI;
    }

    void UpdateUI(int hp)
    {
        // update health display
        // simple but works
        text.text = "Health: " + hp;
    }
}