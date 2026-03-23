using UnityEngine;

public class LanternInteractable : MonoBehaviour, IInteractable
{
    // light component
    public Light lanternLight;

    bool isOn = true;

    void Start()
    {
        // auto find light if not set
        if (lanternLight == null)
            lanternLight = GetComponentInChildren<Light>();
    }

    public void Interact()
    {
        // flip state
        isOn = !isOn;

        if (lanternLight != null)
            lanternLight.enabled = isOn;

        Debug.Log("lantern toggled");
    }
}