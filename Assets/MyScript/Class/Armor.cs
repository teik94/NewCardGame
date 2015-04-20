using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Armor : CardForm
{

    public Armor(string name, string asset, string ability, Card.CardSuit suit, 
        Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base(new Card(Card.CardType.Armor, name, asset, ability, suit, number, state, owner), g)
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

    public override void UseCard()
    {
        EquipAnimation();
        //base.UseCard();
    }
}

