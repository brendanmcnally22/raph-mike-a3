using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public enum State
    {
        Drift,
        Investigate,
        Haunt
    }

    public State currentState = State.Drift;

    public Transform player;

    public float driftRadius = 2f;
    public float driftSpeed = 1f;

    public float investigateSpeed = 2f;
    public float hauntSpeed = 3.5f;

    public float hauntDuration = 5f;
    public float investigateTime = 3f;

    public float playerDetectRadius = 6f;

    Vector3 startPos;
    Vector3 investigateTarget;

    float stateTimer;

    Renderer rend;

    float damageCooldown = 1f;
    float damageTimer = 0f;

    void Start()
    {
        startPos = transform.position;
        rend = GetComponent<Renderer>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Character");
            if (p != null)
                player = p.transform;
        }
    }

    void OnEnable()
    {
        SkeletonAI.OnSkeletonRoar += ReactToRoar;
    }

    void OnDisable()
    {
        SkeletonAI.OnSkeletonRoar -= ReactToRoar;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Drift:
                Drift();
                break;

            case State.Investigate:
                Investigate();
                break;

            case State.Haunt:
                Haunt();
                break;
        }
    }

    void Drift()
    {
        float x = Mathf.Sin(Time.time * driftSpeed) * driftRadius;
        float z = Mathf.Cos(Time.time * driftSpeed) * driftRadius;

        transform.position = startPos + new Vector3(x, 0, z);

        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist < playerDetectRadius)
            {
                investigateTarget = player.position;
                currentState = State.Investigate;
                stateTimer = investigateTime;
            }
        }
    }

    void Investigate()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            investigateTarget,
            investigateSpeed * Time.deltaTime
        );

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            EnterHaunt();
        }
    }

    void Haunt()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            hauntSpeed * Time.deltaTime
        );

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            ExitHaunt();
        }
    }

    void ReactToRoar(Vector3 pos)
    {
        float dist = Vector3.Distance(transform.position, pos);

        if (dist < 15f)
        {
            investigateTarget = pos;
            currentState = State.Investigate;
            stateTimer = investigateTime;
        }
    }

    void EnterHaunt()
    {
        currentState = State.Haunt;
        stateTimer = hauntDuration;

        if (rend != null)
        {
            rend.material.EnableKeyword("_EMISSION");
            Color c = rend.material.color;
            c.a = 0.9f;
            rend.material.color = c;
        }

        GraveyardEvents.OnHauntStart?.Invoke();
    }

    void ExitHaunt()
    {
        currentState = State.Drift;

        if (rend != null)
        {
            rend.material.DisableKeyword("_EMISSION");
            Color c = rend.material.color;
            c.a = 0.4f;
            rend.material.color = c;
        }

        GraveyardEvents.OnHauntEnd?.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        damageTimer -= Time.deltaTime;

        if (damageTimer <= 0f)
        {
            IHittable hit = other.GetComponent<IHittable>();

            if (hit != null)
            {
                hit.Hit(gameObject);
                damageTimer = damageCooldown;
            }
        }
    }
}