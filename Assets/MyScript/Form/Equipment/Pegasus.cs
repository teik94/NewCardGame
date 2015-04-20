using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pegasus : PlusVehicle
{
    public Pegasus(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Pegasus", "Pegasus", "+2 Physical Distance", suit, number, state, owner, g)
    {

    }
    public override bool useCondition()
    {
        Player owner = this.Form.Owner;
        if (owner.CurrentHealth < 2)
        {
            return false;
        }
        else
        {
            return base.useCondition();
        }
    }
    public override void Ability()
    {
        base.Ability();
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
        this.Form.Owner.PlusDistance += 2;
        if (this.Form.Owner.CurrentHealth > 0) this.Form.Owner.CurrentHealth -= 1;
        base.Equipped();
    }

    public override void UnEquipped()
    {
        this.Form.Owner.PlusDistance -= 2;
        if (this.Form.Owner.PlusDistance < 0) this.Form.Owner.PlusDistance = 0;
        base.UnEquipped();
    }
}