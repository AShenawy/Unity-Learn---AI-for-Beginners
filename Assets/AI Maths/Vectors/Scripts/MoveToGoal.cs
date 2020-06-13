using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGoal : MonoBehaviour
{
    public Transform goal;
    public float speed = 2f;
    public float accuracy = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(goal);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionVector = goal.position - transform.position;
        Debug.DrawRay(transform.position, directionVector, Color.red);
        if (Input.GetKey(KeyCode.G))
        {
            if (directionVector.magnitude > accuracy)
                transform.Translate(directionVector.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}
