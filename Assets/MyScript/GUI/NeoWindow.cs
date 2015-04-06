using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NeoWindow: MonoBehaviour
{
    public GUISkin skin;
    private Rect rctWindow1;
    public string title = "Window";
    public float width = 250;
    public float height = 110;
    public bool center = true;
    public Vector2 position = new Vector2(20, 20);
    
    void Awake()
    {
        rctWindow1 = new Rect(position.x, position.y, width, height);
        if (center)
        {
            rctWindow1 = new Rect(Screen.width - width*2, Screen.height - height*2, width, height);
        }
    }

    void OnGUI()
    {
        GUI.skin = skin;
        rctWindow1 = GUI.Window(1, rctWindow1, MyWindow, title, GUI.skin.GetStyle("window"));
    }
    
    void Start()
    {
        skin = Resources.Load("MetalGUISkin") as GUISkin;
    }

    private void MyWindow(int id)
    {
        GUI.Label(new Rect(0,30,rctWindow1.width, 20),"Are you sure?");
        GUI.Button(new Rect(20, 60, 100, 20), "OK");
        GUI.Button(new Rect(130, 60, 100, 20), "Cancel");
        GUI.DragWindow();
    }
}

