using UnityEngine;

// this is a "Hittable" interface. all objects that adhere to 
// this interface *must* implement a Hit() method
public interface IHittable
{
    public void Hit(GameObject gameObject);
}