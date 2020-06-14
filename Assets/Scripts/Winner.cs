using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    Rigidbody2D rb2;
    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        rb2.position = new Vector3(-11f, -2f, 0f);
        rb2.velocity = new Vector3(2, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (rb2.position.x >= 11.14f) {
            rb2.position = new Vector3(-11f, -2f, 0f);
        }
    }
}
