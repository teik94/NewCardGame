﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Vimana : MinusVehicle
{
    public Vimana(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Vimana", "Vimana", "-1 Physical Distance", suit, number, state, owner, g)
    {

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
        this.Form.Owner.MinusDistance += 1;
        base.Equipped();
    }

    public override void UnEquipped()
    {
        this.Form.Owner.MinusDistance -= 1;
        if (this.Form.Owner.MinusDistance < 0) this.Form.Owner.MinusDistance = 0;
        base.UnEquipped();
    }
}

