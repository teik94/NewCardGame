using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NeoDialog: MonoBehaviour
{
    public GUISkin skin;
    private Rect rctWindow1;
    public string title = "Dialog";
    public float width = 250;
    public float height = 110;
    public bool center = true;
    public Vector2 position = new Vector2(20, 20);
    public enum DialogType
    {
        YesNo = 0,
        Comfirm = 1,
        Warning = 2,
    }
    public DialogType dialogType = DialogType.YesNo;
    public event EventHandler OkClick;
    public event EventHandler CancelClick;
    
    void Awake()
    {
        //rctWindow1 = new Rect(position.x, position.y, width, height);
        if (center)
        {
            rctWindow1 = new Rect(Screen.width - width*2, Screen.height - height*2, width, height);
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
        skin = Resources.Load("MetalGUISkin") as GUISkin;
    }

    private void MyWindow(int id)
    {
        GUI.Label(new Rect(0,30,rctWindow1.width, 20),"Are you sure?");
        if(GUI.Button(new Rect(20, 60, 100, 20), "OK"))
        {
            if (OkClick != null) OkClick.Invoke(this, null);
        }
        if(GUI.Button(new Rect(130, 60, 100, 20), "Cancel"))
        {
            if (CancelClick != null) CancelClick.Invoke(this, null);
        }
        GUI.DragWindow();
    }
}

