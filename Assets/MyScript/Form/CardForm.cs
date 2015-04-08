using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardForm
{
    public CardController Form;
    public Guid CardID;
    public Game game;
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

    public virtual void DrawFromDeck(int number, Player player)
    {
        if (player == game.myPlayer)
        {
            GameObject handPanel = game.handPanel;
            //GameObject mainPanel = GameObject.Find("MainPlayer Panel");
            RectTransform rt = handPanel.GetComponent<RectTransform>();
            if (handPanel == null) Debug.Log("hand panel null");
            if (this.Form == null) Debug.Log("card form null");
            else
            {
                this.Form.UpdateLastInteract();
                this.Form.Active = true;
                this.Form.Owner = player;
                this.Form.FaceUp = true;
                this.Form.CardData.State = Card.CardState.Hand;
                this.Form.State = Card.CardState.Hand;
                Vector3 position = handPanel.transform.localPosition;
                Action action = delegate()
                {
                    RefreshHand(game.myPlayer);
                    number -= 1;
                    game.DrawCard(number, player);
                };
                this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
            }
        }
        else
        {
            GameObject playerPanel = game.playerPanel6;
            this.Form.Active = true;
            this.Form.Owner = player;
            this.Form.FaceUp = false;
            this.Form.UpdateLastInteract();
            this.Form.CardData.State = Card.CardState.Hand;
            this.Form.State = Card.CardState.Hand;
            Vector3 position = playerPanel.transform.localPosition;
            Action action = delegate()
            {
                number -= 1;
                game.DrawCard(number, player);
                System.Threading.Timer time = new System.Threading.Timer(delegate(object sender)
                {
                    this.Form.Active = false;
                });
                time.Change(1000, 0);
                //RefreshHand();
            };
            this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
        }
    }

    public virtual void RefreshHand(Player player)
    {
        GameObject hand = GameObject.Find("Hand Card Panel");
        List<CardForm> handList = game.CardList.GetHandList(player);
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
            int sorting = 17;
            for (int i = 0; i < handList.Count; i++)
            {
                order = order + 0.1f;
                sorting++;
                float newX = position.x - (handWidth / 2) + (cardWidth / 2) + 3;
                float newY = handList[0].Form.Position.y;
                newX = newX + ((cardWidth + padding) * i);
                //handList[i].Form.Sorting = sorting;
                //handList[i].Form.UpdateLastInteract();
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
                //handList[i].Form.Sorting = sorting;
                //handList[i].Form.UpdateLastInteract();
                handList[i].Form.Move(new Vector2(newX, newY), MoveSpeed, null);
                //handList[i].Form.Position = new Vector2(newX, newY);
            }
        }
    }

    public Action mainAction = null;
    public virtual void UseCard()
    {
        Vector3 position = game.tempPanel.transform.localPosition;
        if (this.Form.Owner == game.myPlayer)
        {
            game.btnCancel.SetActive(false);
            game.CancelClick = null;
        }
        this.Form.Active = true;
        Action action = delegate()
        {
            this.Form.UpdateLastInteract();
            this.Form.Owner = null;
            this.Form.FaceUp = true;
            this.Form.CardData.State = Card.CardState.Using;
            this.Form.State = Card.CardState.Using;
            RefreshPiles();
            RefreshHand(game.myPlayer);
            mainAction.Invoke();
        };
        this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
    }

    public virtual void RefreshPiles()
    {
        float padding = 3;
        float cardWidth = this.Form.Width;
        float cardHeight = this.Form.Height;
        List<CardForm> usingCard = game.CardList.GetUsingList();
        float pileWidth = usingCard.Count * (cardWidth + padding);
        RectTransform rt = game.tempPanel.GetComponent<RectTransform>();
        float maxPileWidth = rt.GetWidth();
        Vector3 position = game.tempPanel.transform.localPosition;
        if (pileWidth < maxPileWidth)
        {
            for (int i = 0; i < usingCard.Count; i++)
            {
                float newX = position.x - (pileWidth / 2) + (cardWidth / 2) + 3;
                float newY = position.y;
                newX = newX + ((cardWidth + padding) * i);
                usingCard[i].Form.Move(new Vector2(newX, newY), MoveSpeed, null);
            }
        }
        else
        {
            float newpadding = (pileWidth - maxPileWidth) / usingCard.Count;
            float handWidth2 = usingCard.Count * (cardWidth - newpadding);
            for (int i = 0; i < usingCard.Count; i++)
            {
                float newX = position.x - (handWidth2 / 2) + ((cardWidth - newpadding) / 2);
                float newY = position.y;
                newX = newX + ((cardWidth - newpadding) * i);
                usingCard[i].Form.Move(new Vector2(newX, newY), MoveSpeed, null);
            }
        }
    }


    #region Handler
    public virtual void OnMouseClick()
    {
        if (this.Form.Owner == game.myPlayer) game.ProcessingCard = this;
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
        if (this.Form.Visible) this.Form.Update();
    }
}

