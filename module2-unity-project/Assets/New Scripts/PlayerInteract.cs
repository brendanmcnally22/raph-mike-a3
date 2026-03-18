using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInteract : MonoBehaviour
{
    public static Action OnPlayerInteract;

    public InputActionReference interactAction;
    public float interactRadius = 2f;

    void Update()
    {
        if (interactAction.action.triggered)
        {
            OnPlayerInteract?.Invoke();

            Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                    break;
                }
            }
        }
    }
}