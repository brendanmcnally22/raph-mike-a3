using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float hauntDuration = 5f;

    bool haunting = false;
    float hauntTimer;

    Renderer rend;

    void Start()
    {
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
        if (haunting && player != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            hauntTimer -= Time.deltaTime;
            if (hauntTimer <= 0)
            {
                StopHaunting();
            }
        }
    }

    void ReactToRoar(Vector3 pos)
    {
        float dist = Vector3.Distance(transform.position, pos);

        // 🔥 BIGGER THAN PLAYER DETECTION
        if (dist < 15f)
        {
            haunting = true;
            hauntTimer = hauntDuration;

            if (rend != null)
                rend.material.EnableKeyword("_EMISSION");

            GraveyardEvents.OnHauntStart?.Invoke();
        }
    }

    void StopHaunting()
    {
        haunting = false;

        if (rend != null)
            rend.material.DisableKeyword("_EMISSION");

        GraveyardEvents.OnHauntEnd?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        IHittable hit = other.GetComponent<IHittable>();
        if (hit != null)
        {
            hit.Hit(gameObject);
        }
    }
}