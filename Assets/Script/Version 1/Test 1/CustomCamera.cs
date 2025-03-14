using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public float maxX = 13.50001f;
    public float minX = -13.50001f;
    public float moveSpeed = 3;

    public KeyCode moveLeft;
    public KeyCode moveRight;

    public Vector3 SYWS_Pos;
    public bool moveTo_SYWS;
    public Vector3 NLI_Pos;
    public bool moveTo_NLI;
    public Vector3 center;
    public bool moveToCenter;
    private void Start()
    {
        maxX = 13.50001f;
        minX = -13.50001f;
        moveSpeed = 3;

        moveLeft = KeyCode.LeftArrow;
        moveRight = KeyCode.RightArrow;

        SYWS_Pos = new Vector3(-13f, transform.position.y, transform.position.z);
        NLI_Pos = new Vector3(13f, transform.position.y, transform.position.z);
        center = new Vector3(0f, transform.position.y, transform.position.z);
        moveTo_SYWS = true;
        moveTo_NLI = false;
        moveToCenter = false;
    }
    private void Update()
    {
        if (moveTo_SYWS)
        {
            transform.position = Vector3.Lerp(transform.position, SYWS_Pos, 0.01f);
            if (transform.position.x < -12.5f) moveTo_SYWS = false;
        }
        else if (moveTo_NLI)
        {
            transform.position = Vector3.Lerp(transform.position, NLI_Pos, 0.01f);
            if (transform.position.x > 12.5f) moveTo_NLI = false;
        }
        else if (moveToCenter)
        {
            transform.position = Vector3.Lerp(transform.position, center, 0.01f);
            if (transform.position.x > -0.5f) moveToCenter = false;
        }
        else if (Input.GetKey(moveLeft) && transform.position.x > minX)
        {
            transform.Translate(Vector3.left * 3 * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(moveRight) && transform.position.x < maxX)
        {
            transform.Translate(Vector3.right * 3 * moveSpeed * Time.deltaTime);
        }
    }
}