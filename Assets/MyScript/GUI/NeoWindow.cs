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
    public float height = 140;
    public bool center = true;
    public Vector2 position = new Vector2(20, 20);
    public string caption = "";
    private bool yes = false;
    private bool no = false;
    private bool ok = false;
    private bool cancel = false;
    private bool retry = false;

    public List<CardForm> Cards = new List<CardForm>();

    public CardForm SelectedCard = null;

    public WindowType type = WindowType.YesNo;
    public WindowResult result = WindowResult.None;

    public enum WindowType
    {
        YesNo, OK, OKCancel, RetryCancel, YesNoCancel, FreeWindow
    }

    public enum WindowResult
    {
        OK, Cancel, Yes, No, Retry, None
    }

    void Awake()
    {
        //rctWindow1 = new Rect(position.x, position.y, width, height);

        if (this.type == WindowType.FreeWindow)
        {
            if (Cards.Count < 10)
            {
                height = 150;
                width = 600;
            }
            else if (Cards.Count < 20)
            {

            }
            else if (Cards.Count < 30)
            {

            }
            else
            {

            }
        }
        if (center)
        {
            rctWindow1 = new Rect(Screen.width / 2 - width +(width/3), Screen.height / 2 - height / 2, width, height);
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
        if (skin == null) skin = Resources.Load("MetalGUISkin") as GUISkin;
    }

    private void MyWindow(int id)
    {
        //GUI.
        if (this.type != WindowType.FreeWindow)
        {
            GUIStyle style = new GUIStyle();
            style.wordWrap = true;
            style.alignment = TextAnchor.UpperCenter;
            style.fontSize = 15;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(20, 30, this.width-40, this.height - 30), caption, style);
            if (this.type == WindowType.YesNo)
            {
                yes = GUI.Button(new Rect(40, 80, 80, 30), "Yes");
                no = GUI.Button(new Rect(130, 80, 80, 30), "No");
            }
            else if (this.type == WindowType.OKCancel)
            {
                ok = GUI.Button(new Rect(40, 80, 80, 30), "OK");
                cancel = GUI.Button(new Rect(130, 80, 80, 30), "Cancel");
            }
            else if (this.type == WindowType.RetryCancel)
            {
                retry = GUI.Button(new Rect(40, 80, 80, 30), "Retry");
                cancel = GUI.Button(new Rect(130, 80, 80, 30), "Cancel");
            }
            else if (this.type == WindowType.OK)
            {
                ok = GUI.Button(new Rect((this.width / 2) - 40, 80, 80, 30), "OK");
            }
        }
        else
        {
            int count = 0;
            int line = 0;
            foreach (CardForm cf in Cards)
            {
                CardForm c = cf;
                float cwidth = cf.Form.Width / 100 * 80;
                float cheight = cf.Form.Height / 100 * 80;
                Texture2D texture = cf.Form.texture;
                Rect r = new Rect(30 + ((cwidth + 10) * count), 30 + ((cheight+10) * line), cwidth, cheight);
                GUI.DrawTexture(r, texture);
                if(Input.GetMouseButton(0))
                {
                    if (r.Contains(Event.current.mousePosition))
                    {
                        SelectedCard = c;
                        //result = WindowResult.OK;
                    }
                }
                if(count > 9)
                {
                    count = 0;
                    line++;
                }
            }
        }
        GUI.DragWindow();
        GUI.FocusWindow(id);
    }

    public WindowResult Close()
    {
        //Debug.Log(this.result.ToString());
        //gameObject.SetActive(false);
        Destroy(gameObject);
        return result;
    }

    void Update()
    {
        if (ok)
        {
            result = WindowResult.OK;
            //this.Close();
        }
        else if (yes)
        {
            result = WindowResult.Yes;
            //this.Close();
        }
        else if (no)
        {
            result = WindowResult.No;
            //this.Close();
        }
        else if (retry)
        {
            result = WindowResult.Retry;
            //this.Close();
        }
        else if (cancel)
        {
            result = WindowResult.Cancel;
            //this.Close();
        }
    }
}

public class Window
{
    NeoWindow window;
    GameObject dialog;
    Game game;

    public List<CardForm> Cards
    {
        get { return window.Cards; }
        set { window.Cards = value; }
    }
    public CardForm SelectedCard
    {
        get { return window.SelectedCard; }
        set { window.SelectedCard = value; }
    }
    public NeoWindow.WindowType Type
    {
        get { return window.type; }
        set { window.type = value; }
    }
    public NeoWindow.WindowResult Result
    {
        get { return window.result; }
        set { window.result = value; }
    }

    public Vector2 Position
    {
        get { return window.position; }
        set { window.position = value; }
    }

    public string Title
    {
        get { return window.title; }
        set { window.title = value; }
    }
    public string Caption
    {
        get { return window.caption; }
        set { window.caption = value; }
    }
    public float Height
    {
        get { return window.height; }
        set { window.height = value; }
    }
    public float Width
    {
        get { return window.width; }
        set { window.width = value; }
    }
    public bool Center
    {
        get { return window.center; }
        set { window.center = value; }
    }

    public Window(Game _game)
    {
        dialog = new GameObject("Window");
        window = dialog.AddComponent<NeoWindow>();
        game = _game;
    }

    public void Show()
    {
        game.Modal = true;
        dialog.transform.SetParent(game.mainPanel.transform);
    }

    public NeoWindow.WindowResult Close()
    {
        game.Modal = false;
        return window.Close();
    }
}

