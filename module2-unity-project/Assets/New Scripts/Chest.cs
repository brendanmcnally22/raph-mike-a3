using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour, IInteractable
{
    public static System.Action<int> OnChestLooted;

    [Header("Setup")]
    public Transform lid;

    [Header("Settings")]
    public int goldAmount = 10;
    public float openAngle = 110f;
    public float speed = 2f;

    bool isOpen = false;
    bool isAnimating = false;

    Quaternion closedRot;
    Quaternion openRot;

    void Start()
    {
        closedRot = lid.localRotation;
        openRot = Quaternion.Euler(openAngle, 0, 0) * closedRot;
    }

    public void Interact()
    {
        if (isAnimating) return;

        if (!isOpen)
        {
            StartCoroutine(OpenChest());
        }
        else
        {
            StartCoroutine(CloseChest());
        }
    }

    IEnumerator OpenChest()
    {
        isAnimating = true;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            lid.localRotation = Quaternion.Slerp(closedRot, openRot, t);
            yield return null;
        }

        lid.localRotation = openRot;

        isOpen = true;
        isAnimating = false;

        //  FIRE EVENT
        OnChestLooted?.Invoke(goldAmount);

        Debug.Log("Chest opened, gold: " + goldAmount);
    }

    IEnumerator CloseChest()
    {
        isAnimating = true;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            lid.localRotation = Quaternion.Slerp(openRot, closedRot, t);
            yield return null;
        }

        lid.localRotation = closedRot;

        isOpen = false;
        isAnimating = false;

        Debug.Log("Chest closed");
    }
}