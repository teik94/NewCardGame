using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Dialog
{
    GameObject dialog;
    Vector2 position = new Vector2(0, 0);
    float width = 120;

    public float Width
    {
        get { return width; }
        set { width = value; }
    }
    float height = 50;

    public float Height
    {
        get { return height; }
        set { height = value; }
    }
    string title = "Dialog";

    public string Title
    {
        get { return title; }
        set { title = value; }
    }
    private bool show = false;

    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }
    Guid dialogID;
    Game game;
    public Dialog(Game game)
    {
        this.game = game;
    }

    void OnGUI()
    {
        
    }

    void Start()
    {
        
    }

    public void Open(string text)
    {
        dialogID = Guid.NewGuid();
        GameObject go = GameObject.Find("Dialog");
        dialog = UnityEngine.Object.Instantiate(go);
        dialog.name = dialogID.ToString();
        dialog.transform.SetParent(this.game.mainPanel.transform);
        this.title = text;
        dialog.SetActive(true);
        RectTransform rt = dialog.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0 - rt.GetWidth(), 0 + rt.GetHeight());
        rt.sizeDelta = go.GetComponent<RectTransform>().sizeDelta;
    }

    void Update()
    {
        //RectTransform rt = dialog.GetComponent<RectTransform>();
        //rt.anchoredPosition = position;
        //rt.sizeDelta = new Vector2(width, height);
        //Text t = dialog.transform.FindChild("Panel").transform.FindChild("Text").GetComponent<Text>();
        //t.text = this.title;
    }
}

