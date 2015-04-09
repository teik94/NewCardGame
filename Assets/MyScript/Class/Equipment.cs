using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Equipment: MonoBehaviour
{
    private CardForm form;

    public CardForm Form
    {
        get { return form; }
        set 
        { 
            form = value; 
            if(form!=null)
            {
                if (image== null) image = gameObject.transform.FindChild("Image").GetComponent<Image>();
                if (text == null) text = gameObject.transform.FindChild("Text").GetComponent<Text>();
                Texture2D texture = Resources.Load("Icon/" + form.Form.CardData.Asset + "Icon") as Texture2D;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0f));
                if (sprite == null) Debug.Log("sprite null");
                if (image == null) Debug.Log("image null");
                image.sprite = sprite;
                text.text = form.Form.CardData.Name;
            }
        }
    }


    public Image image;
    public Text text;
    void Start()
    {
        image = gameObject.transform.FindChild("Image").GetComponent<Image>();
        text = gameObject.transform.FindChild("Text").GetComponent<Text>();
    }

    void Update()
    {
        
    }
}

