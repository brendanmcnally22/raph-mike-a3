using UnityEngine;
using System;

public class SkeletonAI : MonoBehaviour, IInteractable, IHittable
{
    public enum State { Idle, Chase }
    public State currentState = State.Idle;

    // event that ghosts listen to
    public static Action<Vector3> OnSkeletonRoar;

    public Transform player;

    public float detectRadius = 6f;
    public float moveSpeed = 2f;
    public float losePlayerTime = 4f;
    public float roarInterval = 6f;

    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    float loseTimer;
    float roarTimer;
    float attackTimer;

    void Start()
    {
        // auto find player (saves time in inspector)
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
                // move toward player
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                );

                // face player
                Vector3 dir = (player.position - transform.position).normalized;
                if (dir != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(dir);

                // distance based damage (way more reliable than triggers)
                attackTimer -= Time.deltaTime;

                if (dist < attackRange && attackTimer <= 0f)
                {
                    IHittable hit = player.GetComponent<IHittable>();

                    if (hit != null)
                    {
                        hit.Hit(gameObject);
                        attackTimer = attackCooldown;
                    }
                }

                // roar event
                roarTimer += Time.deltaTime;
                if (roarTimer >= roarInterval)
                {
                    OnSkeletonRoar?.Invoke(transform.position);
                    roarTimer = 0f;
                }

                // lose player logic
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