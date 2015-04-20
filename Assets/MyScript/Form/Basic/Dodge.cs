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

public class Dodge : CardForm
{
    public Dodge(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base(new Card(Card.CardType.Basic, "DODGE", "Dodge", "Used to dodge from an attack.",
        suit, number, state, owner),g)
    {
        this.UseCondition += useCondition;
    }

    private bool useCondition()
    {
        Player owner = this.Form.Owner;
        if (owner != null && owner.actionState == Player.ActionState.WaitingDodge)
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
        PerformDodge();
        //base.UseCard();
    }
}

