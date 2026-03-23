using UnityEngine;

public class GhostAI : MonoBehaviour
{
    // basic state machine
    public enum State
    {
        Drift,
        Investigate,
        Haunt
    }

    public State currentState = State.Drift;

    public Transform player;

    // movement tuning
    public float driftRadius = 2f;
    public float driftSpeed = 1f;

    public float investigateSpeed = 2f;
    public float hauntSpeed = 4f;

    public float hauntDuration = 5f;
    public float investigateTime = 2f;

    public float playerDetectRadius = 6f;

    Vector3 startPos;
    Vector3 investigateTarget;

    float stateTimer;

    Renderer rend;

    // cooldown so ghost doesnt melt player instantly lol
    float damageCooldown = 1f;
    float damageTimer = 0f;

    void Start()
    {
        startPos = transform.position;
        rend = GetComponent<Renderer>();

        // auto find player if i forgot to drag it in inspector
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Character");
            if (p != null)
                player = p.transform;
        }
    }

    void OnEnable()
    {
        // listens for skeleton roar
        SkeletonAI.OnSkeletonRoar += ReactToRoar;
    }

    void OnDisable()
    {
        SkeletonAI.OnSkeletonRoar -= ReactToRoar;
    }

    void Update()
    {
        // simple state machine update
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
        // idle floating motion
        float x = Mathf.Sin(Time.time * driftSpeed) * driftRadius;
        float z = Mathf.Cos(Time.time * driftSpeed) * driftRadius;

        transform.position = startPos + new Vector3(x, 0, z);

        // also check player so ghost isnt useless
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
        // move to last known pos
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

        // chase player faster
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            hauntSpeed * Time.deltaTime
        );

        // damage logic (distance based cause triggers were buggy)
        damageTimer -= Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < 1.5f && damageTimer <= 0f)
        {
            IHittable hit = player.GetComponent<IHittable>();

            if (hit != null)
            {
                hit.Hit(gameObject);
                damageTimer = damageCooldown;
            }
        }

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            ExitHaunt();
        }
    }

    void ReactToRoar(Vector3 pos)
    {
        // ghost reacts to skeleton event
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

        // small visual change so player can tell
        if (rend != null)
        {
            Color c = rend.material.color;
            c.a = 0.9f;
            rend.material.color = c;
        }

        // trigger global event
        GraveyardEvents.OnHauntStart?.Invoke();
    }

    void ExitHaunt()
    {
        currentState = State.Drift;

        if (rend != null)
        {
            Color c = rend.material.color;
            c.a = 0.4f;
            rend.material.color = c;
        }

        GraveyardEvents.OnHauntEnd?.Invoke();
    }
}