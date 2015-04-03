using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CardForm
{
    public CardController Form;
    public Guid CardID;
    Game game;
    public float MoveSpeed = 1500f;
    public CardForm(Card card, Game g)
    {
        //Card atkCard = new Card(Card.CardType.Basic, "ATTACK", "attack1", "Used to attack one player.",
        //    suit, number, state, owner);
        this.Form = new CardController(card, 0, 0, 50, 70);
        this.CardID = Form.CardData.CardID;
        this.game = g;
        this.Form.OnClick += OnMouseClick;
        this.Form.OnMouseMove += OnMouseMove;
        this.Form.OnMouseEnter += OnMouseEnter;
        this.Form.OnMouseLeave += OnMouseLeave;
    }

    public virtual void DrawFromDeck()
    {
        GameObject hand = GameObject.Find("Hand Card Panel");
        GameObject mainPanel = GameObject.Find("MainPlayer Panel");
        RectTransform rt = hand.GetComponent<RectTransform>();
        if (hand == null) Debug.Log("hand panel null");
        if (this.Form == null) Debug.Log("card form null");
        else
        {
            this.Form.gameObject.SetActive(true);
            Vector3 position = hand.transform.localPosition;
            Action action = delegate()
            {
                this.Form.UpdateLastInteract();
                this.Form.CardData.State = Card.CardState.Hand;
                this.Form.State = Card.CardState.Hand;
                RefreshHand();
            };
            this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
        }
    }

    public virtual void RefreshHand()
    {
        GameObject hand = GameObject.Find("Hand Card Panel");
        List<CardForm> handList = game.CardList.GetHandList();
        RectTransform rt = hand.GetComponent<RectTransform>();
        float padding = 3;
        float cardWidth = this.Form.Width;
        float cardHeight = this.Form.Height;
        float maxHandWidth = rt.GetWidth();
        float handWidth = handList.Count * (cardWidth + padding);
        Vector3 position = hand.transform.localPosition;

        if (handWidth < maxHandWidth)
        {
            float order = 0.5f;
            int sorting = 17 ;
            for (int i = 0; i < handList.Count; i++)
            {
                order = order + 0.1f;
                sorting++;
                float newX = position.x - (handWidth / 2) + (cardWidth/2)+3;
                float newY = handList[0].Form.Position.y;
                newX = newX + ((cardWidth + padding) * i);
                handList[i].Form.Sorting = sorting;
                handList[i].Form.Move(new Vector2(newX, newY), MoveSpeed, null);
                
                //handList[i].Form.Position = new Vector2(newX, newY);
            }
        }
        else
        {
            float order = 0.5f;
            int sorting = 17;
            float newpadding = (handWidth - maxHandWidth) / handList.Count;
            float handWidth2 = handList.Count * (cardWidth - newpadding);
            for (int i = 0; i < handList.Count; i++)
            {
                order = order + 0.1f;
                sorting++;
                float newX = position.x - (handWidth2 / 2) + ((cardWidth - newpadding) / 2);
                float newY = handList[0].Form.Position.y;
                newX = newX + ((cardWidth - newpadding) * i);
                handList[i].Form.Sorting = sorting;
                handList[i].Form.Move(new Vector2(newX, newY), MoveSpeed, null);
                //handList[i].Form.Position = new Vector2(newX, newY);
            }
        }
    }

    

    #region Handler
    public virtual void OnMouseClick()
    {

    }
    public virtual void OnMouseEnter()
    {
        Form.Height = Form.Height + 10;
        Form.Width = Form.Width + 10;
    }
    public virtual void OnMouseMove()
    {

    }
    public virtual void OnMouseLeave()
    {
        Form.Height = Form.Height - 10;
        Form.Width = Form.Width - 10;
        //Form.Position = new Vector2(0, 0);
    }
    #endregion

    public virtual void Update()
    {
        if(this.Form.Visible)this.Form.Update();
    }
}

