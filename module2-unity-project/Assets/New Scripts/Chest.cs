using UnityEngine;
using System;

public class Chest : MonoBehaviour, IInteractable
{
    public enum State { Closed, Opening, Open }
    public State currentState;

    public static Action OnChestOpened;

    public GameObject closedVisual;
    public GameObject openVisual;

    public void Interact()
    {
        if (currentState == State.Closed)
            ChangeState(State.Opening);
    }

    void ChangeState(State newState)
    {
        currentState = newState;

        if (newState == State.Opening)
            Invoke(nameof(OpenChest), 1f);
    }

    void OpenChest()
    {
        closedVisual.SetActive(false);
        openVisual.SetActive(true);

        OnChestOpened?.Invoke(); // 🔥 EVENT
        currentState = State.Open;
    }
}