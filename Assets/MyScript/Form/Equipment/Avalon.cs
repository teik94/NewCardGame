using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Avalon:Armor
{
    public Avalon(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Avalon", "Avalon", "", suit, number, state, owner, g)
    {

	}

    public override void Ability()
    {
        base.Ability();
    }

    public override void UseCard()
    {
        Equipped();
        base.UseCard();
    }

    public override void Discard()
    {
        UnEquipped();
        base.Discard();
    }

    public override void Equipped()
    {
        Player owner = Form.Owner;
        if (owner != null)
        {
            owner.BeginningOfTurn += HealStartTurn;
        }
        base.Equipped();
    }

    public override void UnEquipped()
    {
        Player owner = Form.Owner;
        if (owner != null)
        {
            owner.BeginningOfTurn -= HealStartTurn;
        }
        base.UnEquipped();
    }

    public void HealStartTurn()
    {
        Player owner = Form.Owner;
        if (owner != null)
        {
            if(owner.CurrentHealth<owner.MaxHealth)
            {
                owner.Healing.Invoke(1,owner,owner);
            }
        }
    }
}

