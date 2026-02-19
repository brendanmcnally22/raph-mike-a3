using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Barrier : MonoBehaviour
{
    public Vector3 upTarget = new Vector3(0, -1, 0);
    public Vector3 downTarget = new Vector3(0, -1, 0);
    public float speed = 1.0f;

    bool moveDownCommand = false;
    bool moveUpCommand = false;

    Vector3 initialPosition;
    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (moveUpCommand)
        {
            bool inPosition = MoveToPosition(upTarget);
            if (inPosition)
            {
                moveUpCommand = false;
            }
        }
        else if (moveDownCommand)
        {
            bool inPosition = MoveToPosition(downTarget);
            if (inPosition)
            {
                moveUpCommand = false;
            }
        }
    }

    bool MoveToPosition(Vector3 target)
    {
        // the target is effectively in local space
        // add the barrier's initial position to get it in global space
        Vector3 globalTarget = target + initialPosition;
        
        transform.position = Vector3.MoveTowards(
            transform.position,
            globalTarget,
            speed * Time.deltaTime
            );

        return Vector3.Distance(transform.position, target) < 0.01f;
    }

    // this is called from the toggle Unity Event
    public void Move(bool down)
    {
        if (down)
        {
            moveDownCommand = true;
            moveUpCommand = false;
        }
        else
        {
            moveDownCommand = false;
            moveUpCommand = true;
        }
    }
}
