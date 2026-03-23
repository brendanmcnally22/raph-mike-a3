using UnityEngine;

public class Gravestone : MonoBehaviour, IInteractable
{
    // starting pos so we can reset after haunt ends
    Vector3 startPos;

    // just a simple toggle for floating
    bool floating = false;

    void Start()
    {
        // store starting position at begining
        startPos = transform.position;
    }

    void OnEnable()
    {
        // subscribing to global events
        // this is what makes it event driven instead of hard coded
        GraveyardEvents.OnHauntStart += StartFloating;
        GraveyardEvents.OnHauntEnd += StopFloating;
    }

    void OnDisable()
    {
        // always unsub or stuff breaks later (learned that the hard way)
        GraveyardEvents.OnHauntStart -= StartFloating;
        GraveyardEvents.OnHauntEnd -= StopFloating;
    }

    void Update()
    {
        // if haunt is active we float
        if (floating)
        {
            // using sin wave for easy float motion
            float y = Mathf.Sin(Time.time * 2f) * 0.5f;

            // apply it relative to original pos so it doesnt drift away
            transform.position = startPos + new Vector3(0, y, 0);
        }
    }

    void StartFloating()
    {
        // reset pos just in case
        startPos = transform.position;

        floating = true;
    }

    void StopFloating()
    {
        floating = false;

        // snap back to where it started
        transform.position = startPos;
    }

    public void Interact()
    {
        // could add UI here later
        Debug.Log("reading grave...");
    }
}