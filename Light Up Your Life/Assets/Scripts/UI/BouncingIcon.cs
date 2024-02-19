using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingIcon : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 startPosition;
    private float timer;
    void Start()
    {
        startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Vector3 movement = new Vector3(0, Mathf.Max(Mathf.Sin(timer * 3) / 4, 0), 0);
        transform.position = startPosition + movement;
    }
}
