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
    public Guid CardID
    {
        get { return Form.CardData.CardID; }
        set { Form.CardData.CardID = value; }
    }
    public Game game;
    public float MoveSpeed = 1700f;

    public int AbilityUsed = 0;
    public int AbilityUseMax = 99;
    public bool LossOfHealth = false;

    public delegate IEnumerator CardEffectDelete(int number, Player source, Player victim);
    public CardEffectDelete AttackAction = null;
    public CardEffectDelete DodgeAction = null;
    public CardEffectDelete BeforeAttack = null;
    public CardEffectDelete BeforeAttacked = null;
    public CardEffectDelete DrawCard = null;
    public CardEffectDelete Heal = null;
    public CardEffectDelete AdditionAttack = null;
    public CardEffectDelete JudgmentPhase = null;
    public CardEffectDelete BeginingOfTurn = null;
    public CardEffectDelete DrawPhase = null;
    public CardEffectDelete EndPhase = null;

    public delegate IEnumerator CardDamageDelete(int number, Player source, Player victim, Game.DamageType dmgType);
    public CardDamageDelete DamageIncrease = null;
    public CardDamageDelete DamageDecrease = null;
    public CardDamageDelete CauseDamage = null;
    public CardDamageDelete TakeDamage = null;

    public delegate bool ConditionDelegate();
    public ConditionDelegate UseCondition = null;

    public CardForm(Card card, Game g)
    {
        //Card atkCard = new Card(Card.CardType.Basic, "ATTACK", "attack1", "Used to attack one player.",
        //    suit, number, state, owner);
        this.Form = new CardController(card, 0, 0, 50, 70);
        //this.CardID = Form.CardData.CardID;
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

    public virtual IEnumerator DrawCardAnimation(Player player)
    {
        int bs = game.GetBusyTask();
        if (player == game.myPlayer)
        {
            GameObject handPanel = game.handPanel;
            this.Form.Active = true;
            this.Form.Owner = player;
            this.Form.FaceUp = true;
            this.Form.CardData.State = Card.CardState.Hand;
            this.Form.State = Card.CardState.Hand;
            this.Form.UpdateLastInteract();
            if (handPanel != null && this.Form != null)
            {
                Vector3 position = handPanel.transform.localPosition;
                Vector2 newPos = new Vector2(position.x, position.y);
                this.Form.Move(newPos, MoveSpeed, null);
                while (this.Form.Position != newPos) yield return new WaitForSeconds(0.1f);
                RefreshHand(game.myPlayer);
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
            Vector2 newPos = new Vector2(position.x, position.y);
            this.Form.Move(newPos, MoveSpeed, null);
            while (this.Form.Position != newPos) yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(0.1f);
            this.Form.Active = false;
        }

        yield return new WaitForSeconds(0.1f);
        if (bs >= 0) game.busy[bs] = false;
    }

    public virtual IEnumerator ToHandAnimation(Player player)
    {
        int bs = game.GetBusyTask();
        Player owner = this.Form.Owner;
        if (this.Form.State == Card.CardState.Equipment)
        {
            owner.DiscardEquipment(this);
        }
        if (player == game.myPlayer)
        {
            GameObject handPanel = game.handPanel;
            this.Form.Active = true;
            this.Form.Owner = player;
            this.Form.FaceUp = true;
            this.Form.CardData.State = Card.CardState.Hand;
            this.Form.State = Card.CardState.Hand;
            this.Form.UpdateLastInteract();
            if (handPanel != null && this.Form != null)
            {
                Vector3 position = handPanel.transform.localPosition;
                Vector2 newPos = new Vector2(position.x, position.y);
                this.Form.Move(newPos, MoveSpeed, null);
                while (this.Form.Position != newPos) yield return new WaitForSeconds(0.1f);
                RefreshHand(game.myPlayer);
            }
        }
        else
        {
            GameObject playerPanel = player.gameObject;
            this.Form.Active = true;
            this.Form.Owner = player;
            this.Form.FaceUp = true;
            this.Form.UpdateLastInteract();
            this.Form.CardData.State = Card.CardState.Hand;
            this.Form.State = Card.CardState.Hand;
            Vector3 position = playerPanel.transform.localPosition;
            Vector2 newPos = new Vector2(position.x, position.y);
            this.Form.Move(newPos, MoveSpeed, null);
            while (this.Form.Position != newPos) yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(0.5f);
            this.Form.Active = false;
        }

        yield return new WaitForSeconds(0.1f);
        if (bs >= 0) game.busy[bs] = false;
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
        int free = game.GetFreeTask();
        game.busy[free] = true;
        Vector3 position = game.tempPanel.transform.localPosition;
        Player owner = this.Form.Owner;
        this.Form.Active = true;
        if (this.Form.State == Card.CardState.Equipment)
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
            game.busy[free] = false;
            if (game.GetBusyTask() < 0) game.PilesCollect();
            //mainAction.Invoke();
        };
        this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
    }

    public virtual void EquipAnimation()
    {
        Player user = this.Form.Owner;
        GameObject equipObject = null;
        if (this is Weapon)
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
        this.Form.Active = true;
        Vector3 position = game.tempPanel.transform.localPosition;
        if (this.Form.Owner == game.myPlayer)
        {
            game.btnCancel.SetActive(false);
            game.CancelClick = null;
        }
        Action action = delegate()
        {
            this.Form.UpdateLastInteract();
            this.Form.Owner = null;
            this.Form.FaceUp = true;
            this.Form.CardData.State = Card.CardState.Using;
            this.Form.State = Card.CardState.Using;
            RefreshPiles();
            RefreshHand(game.myPlayer);
            if (mainAction != null) mainAction.Invoke();
            mainAction = null;
        };
        this.Form.Move(new Vector2(position.x, position.y), MoveSpeed, action);
    }

    public void UseCardAnimation()
    {
        this.Form.Active = true;
        this.Form.FaceUp = true;
        Vector3 position = game.tempPanel.transform.localPosition;
        if (this.Form.Owner == game.myPlayer)
        {
            game.btnCancel.SetActive(false);
            game.CancelClick = null;
        }
        Action action = delegate()
        {
            this.Form.UpdateLastInteract();
            this.Form.Owner = null;
            this.Form.FaceUp = true;
            this.Form.CardData.State = Card.CardState.Using;
            this.Form.State = Card.CardState.Using;
            RefreshPiles();
            RefreshHand(game.myPlayer);
            if (mainAction != null) mainAction.Invoke();
            mainAction = null;
            if (game.GetBusyTask() < 0) game.PilesCollect();
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

    public virtual void PerformAttack(GameObject panelClick)
    {
        //Set All Player within attacking become unselectable
        bool physical = true;
        if (this is MagicAttack)
        {
            physical = false;
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

    public virtual void PerformDodge()
    {
        Player attacking = this.Form.Owner.targetPlayer;
        Player source = this.Form.Owner;
        this.mainAction = delegate()
        {
            //if (attacking.EndAttack != null) attacking.EndAttack.Invoke(0, attacking, source);
            //if (source.EndAttack != null) source.EndAttack.Invoke(0, attacking, source);
            //source.IsDodge = true;
            if (game.GetBusyTask() < 0) game.PilesCollect();
        };
        UseCardAnimation();
    }

    public virtual void PerformCSIncreaseDam()
    {
        Player source = this.Form.Owner;
        mainAction = delegate()
        {
            source.CommandSeal = true;
            if (game.GetBusyTask() < 0) game.PilesCollect();
        };
        UseCardAnimation();
    }

    public virtual void PerformHealing()
    {
        Player source = this.Form.Owner;
        mainAction = delegate()
        {
            if (source.Healing != null) source.StartCoroutine(source.Healing(1, source, source));
            if (game.GetBusyTask() < 0) game.PilesCollect();
        };
        UseCardAnimation();
    }

    public virtual void PerformSaving()
    {
        Player source = this.Form.Owner;
        mainAction = delegate()
        {
            if (source.Healing != null) source.StartCoroutine(source.Healing(1, source, source));
            if (game.GetBusyTask() < 0) game.PilesCollect();
        };
        UseCardAnimation();
    }

    public virtual void RespondDuel()
    {
        Player source = this.Form.Owner;
        source.actionState = Player.ActionState.OnDuel;
        UseCardAnimation();
    }

    public virtual void SelectDuelTarget()
    {
        //Highlight All Player
        List<GameObject> panelList = new List<GameObject>();
        foreach (Player p in game.playerList)
        {
            if (p == game.myPlayer) continue;
            else panelList.Add(p.gameObject);
        }

        //Set click event for each highlighted player
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
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => { PerformDuel(zzz); });
                et.delegates = new System.Collections.Generic.List<EventTrigger.Entry>();
                et.delegates.Add(entry);
            }
        }

        //Cancel Duel
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
            game.btnCancel.SetActive(false);
            game.CancelClick = null;
        };
    }

    public virtual void PerformDuel(GameObject playerPanel)
    {
        Player source = this.Form.Owner;
        Player target = playerPanel.GetComponent<Player>();

        mainAction = delegate()
        {
            target.lastDamageCard = this;
            game.Duel(1, source, target);
        };
        this.UseCardAnimation();

        foreach (Player player in game.playerList)
        {
            GameObject p = player.gameObject;
            Outline b = p.GetComponent<Outline>();
            if (b != null) b.enabled = false;
            EventTrigger et2 = p.GetComponent<EventTrigger>();
            if (et2 != null) et2.delegates = null;
        }
        game.btnCancel.SetActive(false);
        game.CancelClick = null;
    }

    public virtual void SelectToolTarget()
    {
        List<GameObject> panelList = new List<GameObject>();
        foreach (Player p in game.playerList)
        {
            if (p == game.myPlayer) continue;
            else panelList.Add(p.gameObject);
        }

        //Set click event for each highlighted player
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
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => { ToolTakeEffect(zzz); });
                et.delegates = new System.Collections.Generic.List<EventTrigger.Entry>();
                et.delegates.Add(entry);
            }
        }

        //Cancel
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
            game.btnCancel.SetActive(false);
            game.CancelClick = null;
        };
    }

    public virtual void ToolTakeEffect(GameObject playerPanel)
    {
        foreach (Player player in game.playerList)
        {
            GameObject p = player.gameObject;
            Outline b = p.GetComponent<Outline>();
            if (b != null) b.enabled = false;
            EventTrigger et2 = p.GetComponent<EventTrigger>();
            if (et2 != null) et2.delegates = null;
        }
        game.btnCancel.SetActive(false);
        game.CancelClick = null;
    }

    public virtual void Respond()
    {
        Player owner = this.Form.Owner;
        owner.IsRespond = true;
        this.UseCardAnimation();
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

