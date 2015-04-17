using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CardForm
{
    public CardController Form;
    public Guid CardID;
    public Game game;
    public float MoveSpeed = 1500f;

    public delegate IEnumerator CardEffectDelete(int number, Player source, Player victim);
    public CardEffectDelete AttackAction = null;
    public CardEffectDelete DodgeAction = null;
    public CardEffectDelete CauseMagicDamage = null;
    public CardEffectDelete CausePhysicDamage = null;
    public CardEffectDelete TakeMagicDamage = null;
    public CardEffectDelete TakePhysicDamage = null;
    public CardEffectDelete DamageIncrease = null;
    public CardEffectDelete DamageDecrease = null;
    public CardEffectDelete DrawCard = null;
    public CardEffectDelete Heal = null;

    
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

    #region Animation
    public virtual void DrawFromDeck(int number, Player player)
    {
        if (player == game.myPlayer)
        {
            GameObject handPanel = game.handPanel;
            //GameObject mainPanel = GameObject.Find("MainPlayer Panel");
            //RectTransform rt = handPanel.GetComponent<RectTransform>();
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
                    game.DrawXCard(number, player);
                };
                this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
            }
        }
        else
        {
            GameObject playerPanel = player.gameObject;
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
                game.DrawXCard(number, player);
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
        //float cardHeight = this.Form.Height;
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

    public virtual void Discard()
    {
        Vector3 position = game.tempPanel.transform.localPosition;
        Player owner = this.Form.Owner;
        this.Form.Active = true;
        if(this.Form.State == Card.CardState.Equipment)
        {
            owner.DiscardEquipment(this);
        }
        Action action = delegate()
        {
            owner = this.Form.Owner;
            this.Form.UpdateLastInteract();
            this.Form.Owner = null;
            this.Form.FaceUp = true;
            this.Form.CardData.State = Card.CardState.Using;
            this.Form.State = Card.CardState.Using;
            RefreshPiles();
            RefreshHand(game.myPlayer);
            if (owner.Discard != null) owner.StartCoroutine(owner.Discard());
            if (game.GetBusyTask() <0) game.PilesCollect();
            //mainAction.Invoke();
        };
        this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
    }

    public virtual void EquipAnimation()
    {
        Player user = this.Form.Owner;
        GameObject equipObject = null;
        if(this is Weapon)
        {
            equipObject = user.Weapon;
        }
        else if (this is Armor)
        {
            equipObject = user.Armor;
        }
        else if (this is PlusVehicle)
        {
            equipObject = user.PlusVehicle;
        }
        else if (this is MinusVehicle)
        {
            equipObject = user.MinusVehicle;
        }
        
        if (equipObject != null)
        {
            equipObject.SetActive(true);
            Equipment equip = equipObject.GetComponent<Equipment>();
            if (equip.Form != null)
            {
                equip.Form.GetType().GetMethod("Discard").Invoke(equip.Form, null);
                //equip.Form.GetType().GetMethod("UnEquipped").Invoke(equip.Form, null);
            }
            equipObject.SetActive(true);
            equip.Form = this;

            equip.Form.GetType().GetMethod("Equipped").Invoke(equip.Form, null);
            this.Form.UpdateLastInteract();
            this.Form.FaceUp = true;
            this.Form.CardData.State = Card.CardState.Equipment;
            this.Form.State = Card.CardState.Equipment;

            if (user == game.myPlayer) this.Form.Position = game.equipmentPanel.transform.localPosition;
            else
            {
                this.Form.Position = user.gameObject.transform.localPosition;
            }
            this.Form.Active = false;
            RefreshHand(game.myPlayer);
        }
    }

    public void UseEquipEffect()
    {
        this.GetType().GetMethod("Ability").Invoke(this, null);
    }

    public virtual void DoJudgment(Action action)
    {
        Vector3 position = game.tempPanel.transform.localPosition;
        this.Form.Active = true;
        Action action2 = delegate()
        {
            this.Form.UpdateLastInteract();
            this.Form.Owner = null;
            this.Form.FaceUp = true;
            this.Form.CardData.State = Card.CardState.Using;
            this.Form.State = Card.CardState.Using;
            RefreshPiles();
            action.Invoke();
        };
        this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action2);
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
            if (mainAction!=null) mainAction.Invoke();
            mainAction = null;
        };
        this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
    }

    public virtual void RefreshPiles()
    {
        float padding = 3;
        float cardWidth = this.Form.Width;
        //float cardHeight = this.Form.Height;
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

    public virtual void Equipped()
    {

    }

    public virtual void UnEquipped()
    {

    }
    #endregion

    #region Action
    public virtual void Attack()
    {
        List<GameObject> panelList = game.GetPlayerInAttackRange();
        if (this is MagicAttack)
        {
            Player owner = this.Form.Owner;
            if (owner != null)
            {
                Equipment equip = owner.Weapon.GetComponent<Equipment>();
                if (equip.Form is PrelatiSpellbook)
                {
                    panelList.Clear();
                    foreach (Player p in game.playerList)
                    {
                        if (p == game.myPlayer) continue;
                        else panelList.Add(p.gameObject);
                    }
                }
            }
        }

        foreach (GameObject panel in panelList)
        {
            GameObject zzz = panel;
            Outline border = panel.GetComponent<Outline>();
            if (border != null)
            {
                border.effectColor = Color.red;
                border.enabled = true;
            }
            EventTrigger et = panel.GetComponent<EventTrigger>();
            if (et != null)
            {
                //EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => { PerformAttack(zzz); });
                et.delegates = new System.Collections.Generic.List<EventTrigger.Entry>();
                et.delegates.Add(entry);
            }
        }

        //Cancel Attack Button
        game.btnCancel.SetActive(true);
        game.CancelClick += delegate()
        {
            foreach (GameObject panel in panelList)
            {
                Outline border = panel.GetComponent<Outline>();
                if (border != null) border.enabled = false;
                EventTrigger et = panel.GetComponent<EventTrigger>();
                if (et != null) et.delegates = null;
            }
        };
    }

    public void PerformAttack(GameObject panelClick)
    {
        //Set All Player within attacking become unselectable
        bool physical = true;
        if (this is MagicAttack)
        {
            physical = false;
            Player owner = this.Form.Owner;
            if (owner != null)
            {
                Equipment equip = owner.Weapon.GetComponent<Equipment>();
                if (equip.Form is PrelatiSpellbook)
                {
                    owner.DamageIncrease += 2;
                }
            }
        }

        foreach (Player player in game.playerList)
        {
            GameObject panel = player.gameObject;
            Outline border = panel.GetComponent<Outline>();
            if (border != null) border.enabled = false;
            EventTrigger et = panel.GetComponent<EventTrigger>();
            if (et != null) et.delegates = null;
        }
        game.btnCancel.SetActive(false);
        game.CancelClick = null;
        //Use Card Animation
        Vector3 position = game.tempPanel.transform.localPosition;
        Player source = this.Form.Owner;
        Player target = panelClick.GetComponent<Player>();

        this.Form.Owner = null;
        this.Form.FaceUp = true;
        this.Form.CardData.State = Card.CardState.Using;
        this.Form.State = Card.CardState.Using;
        Action action = delegate()
        {
            this.Form.UpdateLastInteract();
            RefreshHand(game.myPlayer);
            RefreshPiles();
            target.lastDamageCard = this;
            target.Attacked = true;
            game.Attack(1, source, target, physical);
        };
        this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
    }
    #endregion

    #region Handler
    public virtual void OnMouseClick()
    {
        if (game.Modal) return;
        if (this.Form.Owner == game.myPlayer) game.ProcessingCard = this;
    }
    public virtual void OnMouseEnter()
    {
        if (game.Modal) return;
        Form.Height = Form.Height + 10;
        Form.Width = Form.Width + 10;
    }
    public virtual void OnMouseMove()
    {

    }
    public virtual void OnMouseLeave()
    {
        if (game.Modal) return;
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

