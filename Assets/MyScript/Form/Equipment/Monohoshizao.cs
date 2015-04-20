using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Monohoshizao: Weapon
{
    public Monohoshizao(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Monohoshizao", "Monohoshizao", "", 3, suit, number, state, owner, g)
    {

    }

    public override void Ability()
    {

        if(game.GetBusyTask()<0)
        {
            Player owner = this.Form.Owner;
            Player.ActionState oldState = owner.actionState;
            if (oldState == Player.ActionState.Free || oldState == Player.ActionState.WaitingAttack)
            {
                game.btnCancel.SetActive(true);
                game.CancelClick += delegate()
                {
                    game.CancelClick = null;
                    game.UseAsSomething = null;
                    game.CustomCondition = null;
                    game.UsedAsStr = "";
                    game.btnCancel.SetActive(false);
                    owner.actionState = oldState;
                };

                owner.actionState = Player.ActionState.UseAs;
                game.UsedAsStr = "P.ATK";
                game.CustomCondition += CheckSuit;

                game.UseAsSomething += delegate()
                {
                    if(game.ProcessingCard !=null)
                    {
                        game.CancelClick = null;
                        game.UseAsSomething = null;
                        game.CustomCondition = null;
                        game.btnCancel.SetActive(false);
                        game.UsedAsStr = "";
                        owner.actionState = oldState;
                        game.ProcessingCard.Attack();
                    }
                };
            }
        }
        base.Ability();
    }

    public bool CheckSuit()
    {
        if (this.Form.Owner.actionState == Player.ActionState.UseAs)
        {
            Card.CardNumber number = game.ProcessingCard.Form.CardData.Number;
            if (number > Card.CardNumber.Two && number < Card.CardNumber.Eight)
            {
                return true;
            }
        }
        return false;
    }

    public override void UseCard()
    {
        //Equipped();
        base.UseCard();
    }

    public override void Discard()
    {
        UnEquipped();
        base.Discard();
    }

    public override void Equipped()
    {
        base.Equipped();
    }

    public override void UnEquipped()
    {
        base.UnEquipped();
    }
}

