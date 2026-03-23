using UnityEngine;
using System;

public class SkeletonAI : MonoBehaviour, IInteractable, IHittable
{
    public enum State { Idle, Chase }
    public State currentState = State.Idle;

    public static Action<Vector3> OnSkeletonRoar;

    public Transform player;

    public float detectRadius = 6f;
    public float moveSpeed = 2f;
    public float losePlayerTime = 4f;
    public float roarInterval = 6f;

    float loseTimer;
    float roarTimer;

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Character");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (dist < detectRadius)
                {
                    currentState = State.Chase;
                    loseTimer = losePlayerTime;
                    roarTimer = 0f;
                }
                break;

            case State.Chase:
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                );

                Vector3 dir = (player.position - transform.position).normalized;
                if (dir != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(dir);

                roarTimer += Time.deltaTime;
                if (roarTimer >= roarInterval)
                {
                    OnSkeletonRoar?.Invoke(transform.position);
                    roarTimer = 0f;
                }

                if (dist > detectRadius)
                {
                    loseTimer -= Time.deltaTime;
                    if (loseTimer <= 0)
                        currentState = State.Idle;
                }
                else
                {
                    loseTimer = losePlayerTime;
                }
                break;
        }
    }

    public void Interact()
    {
        currentState = State.Chase;
    }

    public void Hit(GameObject source)
    {
        Destroy(gameObject);
    }
}