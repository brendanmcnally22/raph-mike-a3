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

    Vector3 startPos;
    Vector3 investigateTarget;

    float stateTimer;

    Renderer rend;

    void Start()
    {
        startPos = transform.position;
        rend = GetComponent<Renderer>();
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

    // 🌫️ DRIFT (IDLE FLOATING)
    void Drift()
    {
        float x = Mathf.Sin(Time.time * driftSpeed) * driftRadius;
        float z = Mathf.Cos(Time.time * driftSpeed) * driftRadius;

        Vector3 offset = new Vector3(x, 0, z);
        transform.position = startPos + offset;
    }

    // 🔍 INVESTIGATE ROAR LOCATION
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
            Debug.Log("Ghost switching to HAUNT state");
            EnterHaunt();
        }
    }

    // 👻 HAUNT PLAYER
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
            Debug.Log("Ghost returning to DRIFT");
            ExitHaunt();
        }
    }

    // 🔥 EVENT RESPONSE
    void ReactToRoar(Vector3 pos)
    {
        float dist = Vector3.Distance(transform.position, pos);

        if (dist < 15f)
        {
            Debug.Log("👻 Ghost ALERTED by Skeleton Roar!");

            investigateTarget = pos;
            currentState = State.Investigate;
            stateTimer = investigateTime;
        }
    }

    // 👻 ENTER HAUNT
    void EnterHaunt()
    {
        currentState = State.Haunt;
        stateTimer = hauntDuration;

        if (rend != null)
            rend.material.EnableKeyword("_EMISSION");

        GraveyardEvents.OnHauntStart?.Invoke();
    }

    // 🌫️ EXIT HAUNT
    void ExitHaunt()
    {
        currentState = State.Drift;

        if (rend != null)
            rend.material.DisableKeyword("_EMISSION");

        GraveyardEvents.OnHauntEnd?.Invoke();
    }

    // 💥 DAMAGE PLAYER
    void OnTriggerEnter(Collider other)
    {
        IHittable hit = other.GetComponent<IHittable>();
        if (hit != null)
        {
            Debug.Log("Ghost hit something!");
            hit.Hit(gameObject);
        }
    }
}