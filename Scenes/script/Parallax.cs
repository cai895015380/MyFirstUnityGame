using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform Cam;
    public float moveRate;
    private float StartPointx, StartPointy;
    public bool locky;

    void Start()
    {
        StartPointx = transform.position.x;
        StartPointy = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (locky)
        {
            transform.position = new Vector2(StartPointx + Cam.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(StartPointx + Cam.position.x * moveRate, StartPointy+Cam.position.y*moveRate);
        }
    }
}
