using UnityEngine;

public class GhostAI : MonoBehaviour, IEventListener
{
    public enum State { Drift, Investigate, Haunt }
    public State currentState;

    public Transform player;
    public float detectRadius = 6f;
    public float speed = 2f;

    Vector3 targetPosition;
    float memoryTimer;

    void OnEnable() => Subscribe();
    void OnDisable() => Unsubscribe();

    public void Subscribe()
    {
        SkeletonAI.OnSkeletonRoar += OnRoarHeard;
    }

    public void Unsubscribe()
    {
        SkeletonAI.OnSkeletonRoar -= OnRoarHeard;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Drift:
                Wander();
                if (dist < detectRadius)
                    ChangeState(State.Investigate);
                break;

            case State.Investigate:
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (dist < 2f)
                    ChangeState(State.Haunt);

                memoryTimer = 4f;
                break;

            case State.Haunt:
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * 1.5f * Time.deltaTime);

                if (dist > detectRadius)
                {
                    memoryTimer -= Time.deltaTime;
                    if (memoryTimer <= 0)
                        ChangeState(State.Drift);
                }
                break;
        }
    }

    void Wander()
    {
        transform.position += new Vector3(Mathf.Sin(Time.time), 0, Mathf.Cos(Time.time)) * 0.5f * Time.deltaTime;
    }

    void OnRoarHeard(Vector3 source)
    {
        targetPosition = source;
        ChangeState(State.Investigate);
    }

    void ChangeState(State newState)
    {
        currentState = newState;
    }
}