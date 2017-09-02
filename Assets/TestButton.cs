using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TestButton : MonoBehaviour {

    public GameObject AlterBurst;
    public event EventHandler OnClick = null;

    // Use this for initialization
    void Start () {
        
        AlterBurst = GameObject.Find("AlterBurst");
        
    }
    public virtual void onClick(System.EventArgs e)
    {
        if (OnClick != null)
        {
            OnClick(this, e);
        }
    }

    // Update is called once per frame
    void Update () {
        AlterBurst.transform.position = Vector3.Lerp(AlterBurst.transform.position, new Vector3(5, 0, 0), 2 * Time.deltaTime);
    }


}
