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

public class MinusVehicle : CardForm
{

    public MinusVehicle(string name, string asset, string ability, Card.CardSuit suit,
        Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base(new Card(Card.CardType.MinusVehicle, name, asset, ability, suit, number, state, owner), g)
    {
        this.UseCondition += useCondition;
	}

    public virtual bool useCondition()
    {
        Player owner = this.Form.Owner;
        if (owner != null && owner.actionState == Player.ActionState.Free)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Equipped()
    {
        
    }

    public override void UnEquipped()
    {
        
    }

    public virtual void Ability()
    {

    }


	public override void UseCard ()
	{
        EquipAnimation();
		//base.UseCard ();
	}
}


