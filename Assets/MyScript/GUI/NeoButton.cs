using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NeoButton: MonoBehaviour
{
    public string Text = "Test";
    public float Width = 100;
    public float Height = 20;
    public Vector2 Position = new Vector2(0,0);
    GameObject go;
    public event EventHandler OnClick = null;
    public Button button;
    public Image buttonImage;
    public GameObject textObject;
    public Font font;
    public int Size = 12;
    public TextAnchor Aligment;
    Text txt;
    void Start()
    {
        buttonImage = gameObject.AddComponent<Image>();
        Texture2D texture = Resources.Load("Texture/Button 1") as Texture2D;
        Sprite mainSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0f));
        buttonImage.sprite = mainSprite;
        button = gameObject.AddComponent<Button>();
        button.onClick.AddListener(() => onClick(null));
        textObject = new GameObject("Text");
        textObject.transform.SetParent(gameObject.transform);
        txt = textObject.AddComponent<Text>();
        font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txt.font = font;
        txt.text = Text;
        txt.fontSize = Size;
        Aligment = TextAnchor.MiddleCenter;
        txt.alignment = Aligment;
        RectTransform rt = textObject.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0, 0);

    }

    public void OnGUI()
    {
        go = gameObject;
        if (go == null) Debug.Log("Object Null");
        
        //btnClick.onClick.AddListener(() => onClick(null));
    }

    public virtual void onClick(EventArgs e)
    {
        if (OnClick != null)
        {
            OnClick(this, e);
        }
    }

    void Update()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.anchoredPosition = Position;
        RectTransform rt2 = textObject.GetComponent<RectTransform>();
        rt2.anchoredPosition = Position;
        rt.sizeDelta = new Vector2(Width, Height);
        rt2.sizeDelta = new Vector2(Width, Height);
        txt.font = font;
        txt.text = Text;
        txt.fontSize = Size;
        //txt.alignment = Aligment;
    }
}

