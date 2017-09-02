using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class RotateScript : MonoBehaviour
{

    public event EventHandler OnClick = null;

    // Use this for initialization
    void Start()
    {
        newPos = transform.position;
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPos = transform.position;
            Vector3 position = transform.position;
            Vector3 move = new Vector3(0, 3f, 0f);
            transform.Translate(move * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPos = transform.position;
            Vector3 position = transform.position;
            Vector3 move = new Vector3(0, 3, 0f);
            transform.Translate(-move * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPos = transform.position;
            Vector3 position = transform.position;
            Vector3 move = new Vector3(3, 0, 0f);
            transform.Translate(-move * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPos = transform.position;
            Vector3 position = transform.position;
            Vector3 move = new Vector3(3, 0, 0f);
            transform.Translate(move * Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(0))
        {
            newPos = transform.position;
            Vector3 position = transform.position;
            newPos = new Vector3(0f, 0f, 0f);
        }
    }

    Vector3 newPos;
    public void MoveOn()
    {
        Vector3 position = transform.position;
        if (position != newPos)
        {
            transform.Translate((newPos- position) * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        MoveOn();
    }


}
