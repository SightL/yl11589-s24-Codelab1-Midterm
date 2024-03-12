using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform top;
    public Transform bottom;
    public float speed = 5f;

    private float leftEdge;

    private void Start()
    {
        //convert from screen to world, left is zero! -1 for safe
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;

        if (transform.position.x < leftEdge) {
            //destroy when hit the edge
            Destroy(gameObject);
        }
    }

}
