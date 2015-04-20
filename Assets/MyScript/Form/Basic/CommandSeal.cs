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
public class CommandSeal : CardForm
{
    public CommandSeal(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, string asset , Player owner, Game g)
        : base(new Card(Card.CardType.Basic, "Command Seal", asset, "Used to increase your attack damage by one or save you one unit health when you are on the brink of death.",
        suit, number, state, owner), g)
    {
        this.UseCondition += useCondition;
    }

    private bool useCondition()
    {
        Player owner = this.Form.Owner;
        if (owner != null && owner.actionState == Player.ActionState.Free && !owner.CommandSeal)
        {
            return true;
        }
        else if (owner != null && owner.actionState == Player.ActionState.WaitingBoD)
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
        if(this.Form.Owner.actionState == Player.ActionState.Free)
        {
            PerformCSIncreaseDam();
        }
        if (this.Form.Owner.actionState == Player.ActionState.WaitingBoD)
        {
            PerformSaving();
        }
        //base.UseCard();
    }
}


