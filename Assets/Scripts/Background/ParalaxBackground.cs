using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] float paralaxEffect;
    [SerializeField] SpriteRenderer spriteRenderer;

    float length, startPos;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float textureOffsetX = cam.transform.position.x;
        textureOffsetX *= paralaxEffect;
        spriteRenderer.material.mainTextureOffset = new Vector2(textureOffsetX, 0);

        Vector2 camPos = cam.transform.position;
        transform.position = new(camPos.x, camPos.y, transform.position.z);
    }
}
