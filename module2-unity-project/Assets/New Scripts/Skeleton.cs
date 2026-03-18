using UnityEngine;
using System;

public class SkeletonAI : MonoBehaviour, IInteractable
{
    public enum State { Bones, Rise, Attack, Roar }
    public State currentState = State.Bones;

    public static Action<Vector3> OnSkeletonRoar;

    public Transform player;
    public float detectRadius = 5f;
    public float attackRange = 1.5f;
    public float roarCooldown = 5f;

    float roarTimer;

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Bones:
                if (dist < detectRadius)
                    ChangeState(State.Rise);
                break;

            case State.Rise:
                Invoke(nameof(StartAttack), 1.5f);
                break;

            case State.Attack:
                transform.position = Vector3.MoveTowards(transform.position, player.position, 2f * Time.deltaTime);

                if (dist < attackRange && roarTimer <= 0)
                {
                    ChangeState(State.Roar);
                }

                roarTimer -= Time.deltaTime;
                break;

            case State.Roar:
                OnSkeletonRoar?.Invoke(transform.position); // 🔥 EVENT
                roarTimer = roarCooldown;
                ChangeState(State.Attack);
                break;
        }
    }

    void StartAttack()
    {
        ChangeState(State.Attack);
    }

    void ChangeState(State newState)
    {
        currentState = newState;
    }

    public void Interact()
    {
        if (currentState == State.Bones)
            ChangeState(State.Rise);
    }
}