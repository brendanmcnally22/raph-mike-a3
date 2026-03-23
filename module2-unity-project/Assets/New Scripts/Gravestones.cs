using UnityEngine;

public class Gravestone : MonoBehaviour, IInteractable
{
    Vector3 startPos;
    bool floating = false;

    void Start()
    {
        startPos = transform.position;
    }

    void OnEnable()
    {
        GraveyardEvents.OnHauntStart += StartFloating;
        GraveyardEvents.OnHauntEnd += StopFloating;
    }

    void OnDisable()
    {
        GraveyardEvents.OnHauntStart -= StartFloating;
        GraveyardEvents.OnHauntEnd -= StopFloating;
    }

    void Update()
    {
        if (floating)
        {
            float y = Mathf.Sin(Time.time * 2f) * 0.5f;
            transform.position = startPos + new Vector3(0, y, 0);
        }
    }

    void StartFloating()
    {
        startPos = transform.position;
        floating = true;
    }

    void StopFloating()
    {
        floating = false;
        transform.position = startPos;
    }

    public void Interact()
    {
        Debug.Log("Reading gravestone...");
    }
}