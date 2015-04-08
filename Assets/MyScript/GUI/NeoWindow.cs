using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NeoWindow : MonoBehaviour
{
    public GUISkin skin;
    private Rect rctWindow1;
    public string title = "Dialog";
    public float width = 250;
    public float height = 110;
    public bool center = true;
    public Vector2 position = new Vector2(20, 20);

    void Awake()
    {
        //rctWindow1 = new Rect(position.x, position.y, width, height);
        if (center)
        {
            rctWindow1 = new Rect(Screen.width - width * 2, Screen.height - height * 2, width, height);
        }
        else
        {
            rctWindow1 = new Rect(position.x, position.y, width, height);
        }
    }

    void OnGUI()
    {
        GUI.skin = skin;
        rctWindow1 = GUI.Window(1, rctWindow1, MyWindow, title, GUI.skin.GetStyle("window"));
        
    }

    void Start()
    {
        if(skin==null)skin = Resources.Load("MetalGUISkin") as GUISkin;
    }

    private void MyWindow(int id)
    {
        //GUI.
        GUI.DragWindow();
    }

    void Update()
    {

    }
}

