using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public Transform player;

    public float detectionRange = 6f;
    public float hauntSpeed = 2f;
    public float patrolSpeed = 1f;
    public float retreatDistance = 3f;

    public Transform[] patrolPoints;

    int patrolIndex = 0;

    enum GhostState
    {
        Idle,
        Patrol,
        Haunt,
        Retreat
    }

    GhostState currentState;

    void Start()
    {
        currentState = GhostState.Patrol;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case GhostState.Idle:
                Idle();
                if (distance < detectionRange)
                    currentState = GhostState.Haunt;
                break;

            case GhostState.Patrol:
                Patrol();

                if (distance < detectionRange)
                    currentState = GhostState.Haunt;

                break;

            case GhostState.Haunt:
                Haunt();

                if (distance > detectionRange)
                    currentState = GhostState.Patrol;

                if (distance < retreatDistance)
                    currentState = GhostState.Retreat;

                break;

            case GhostState.Retreat:
                Retreat();

                if (distance > detectionRange)
                    currentState = GhostState.Patrol;

                break;
        }
    }

    void Idle()
    {
        // float animation or idle behaviour
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[patrolIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            patrolSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
    }

    void Haunt()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            hauntSpeed * Time.deltaTime
        );
    }

    void Retreat()
    {
        Vector3 direction = (transform.position - player.position).normalized;

        transform.position += direction * hauntSpeed * Time.deltaTime;
    }
}