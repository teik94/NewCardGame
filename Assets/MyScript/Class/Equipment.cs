using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Equipment: MonoBehaviour
{
    public CardForm form;

    public CardForm Form
    {
        get { return form; }
        set 
        { 
            form = value; 
            if(form!=null)
            {
                try
                {
                    if (image == null) image = gameObject.transform.FindChild("Image").GetComponent<Image>();
                    if (text == null) text = gameObject.transform.FindChild("Text").GetComponent<Text>();
                }
                catch {
                    image = null;
                    text = null;
                }
                if (image == null) image = gameObject.GetComponent<Image>();
                Texture2D texture = Resources.Load("Icon/" + form.Form.CardData.Asset + "Icon") as Texture2D;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0f));
                image.sprite = sprite;
                if (text != null) text.text = form.Form.CardData.Name;
            }
        }
    }


    public Image image;
    public Text text;
    void Start()
    {
        try
        {
            image = gameObject.transform.FindChild("Image").GetComponent<Image>();
            text = gameObject.transform.FindChild("Text").GetComponent<Text>();
        }catch
        {
            
        }
    }

    public void UseEquip()
    {
        if(this.form != null)
        {
            this.form.UseEquipEffect();
        }
    }

    void Update()
    {
        
    }
}

