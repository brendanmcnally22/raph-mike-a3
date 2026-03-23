using UnityEngine;

// simple interactable lantern toggle
public class LanternInteractable : MonoBehaviour, IInteractable
{
    // reference to the light component
    public Light lanternLight;

    // track state
    bool isOn = true;

    void Start()
    {
        // if not assigned, try get it from children
        if (lanternLight == null)
            lanternLight = GetComponentInChildren<Light>();
    }

    public void Interact()
    {
        // flip state
        isOn = !isOn;

        // apply it
        if (lanternLight != null)
            lanternLight.enabled = isOn;

        Debug.Log("lantern toggled: " + isOn);
    }
}