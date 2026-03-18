using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rock : MonoBehaviour
{
    Collider triggerCollider;

    Rigidbody weaponRigidbody;
    private void Awake()
    {
        Collider[] colliderComponents = GetComponents<Collider>();
        foreach (Collider collider in colliderComponents)
        {
            if (collider.isTrigger)
            {
                triggerCollider = collider;
                break;
            }
        }

        weaponRigidbody = GetComponent<Rigidbody>();

        // the rock is initially set up as kinematic, so that
        // when it is in the player's hand, it stays parented and 
        // does not move with physics
        weaponRigidbody.isKinematic = true;
    }

    private void Start()
    {
        // the rock is initially disabled. swinging a rock at an
        // Hittable object does nothing. you might change this design choice
        EnableHitbox(0);
    }

    public void Throw(Vector3 direction, float force)
    {
        EnableHitbox(1);
        
        // clearing the Rock's parent is important, because otherwise it stays
        // attached to the Character arm
        transform.parent = null;

        Vector3 position = transform.position;
        transform.position = new Vector3(position.x, 1.5f, position.z);

        // no longer kinematic since we want it to move with its velocity parameters
        weaponRigidbody.isKinematic = false;

        weaponRigidbody.velocity = Vector3.zero;
        weaponRigidbody.angularVelocity = Vector3.zero;

        weaponRigidbody.velocity = direction.normalized * force;
    }

    public void EnableHitbox(int value)
    {
        triggerCollider.enabled = value == 1 ? true : false;
    }

    // technically this code is repeated for all "Weapon" objects
    // this can be improved with something called inheritance
    // both Shovel and Rock could inherit Weapon - but this introduces more complications
    // with Start(). namely, reflection and overriding virtual functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IHittable>() != null)
        {
            IHittable toggle = other.GetComponent<IHittable>();
            toggle.Hit(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
