using UnityEngine;

public class Move : MonoBehaviour
{

    public Vector3 goal = new Vector3(5, 0, 4);
    public float speed = 0.5f;

    void Start()
    {


    }

    private void Update()
    {
        transform.Translate(goal.normalized * speed * Time.deltaTime);
    }
}
