using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rotation { NONE, RIGHT, LEFT };

public class SawScript : MonoBehaviour
{
    public Rotation rotation = Rotation.NONE;

    public float rotationSpeed;
    float movement;
    float rotattionz;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (rotation)
        {
            case Rotation.RIGHT:
                movement = -rotationSpeed;
                spriteRenderer.flipX = false;
                break;
            case Rotation.LEFT:
                movement = rotationSpeed;
                spriteRenderer.flipX = true;
                break;
        }
    }

    void FixedUpdate()
    {
        rotattionz += movement;
        this.transform.rotation = Quaternion.Euler(0, 0, rotattionz);
    }
}
