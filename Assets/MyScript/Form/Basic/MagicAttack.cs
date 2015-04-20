//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MagicAttack: CardForm
{
    public MagicAttack(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base(new Card(Card.CardType.Basic, "Magical ATTACK", "MagicAttack", "Used to attack one player with magical damage.",
        suit, number, state, owner),g)
    {
        this.UseCondition += useCondition;
    }

    private bool useCondition()
    {
        Player owner = this.Form.Owner;
        if (owner != null && owner.actionState == Player.ActionState.Free && !owner.MainAttack)
        {
            return true;
        }
        else if (owner != null && owner.actionState == Player.ActionState.WaitingAttackDuel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void UseCard()
    {
        Player owner = this.Form.Owner;
        if (owner.actionState == Player.ActionState.WaitingAttackDuel)
        {
            RespondDuel();
        }
        else
        {
            Attack();
        }
    }


}


