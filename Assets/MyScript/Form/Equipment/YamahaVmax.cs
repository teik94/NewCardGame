using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YamahaVmax : MinusVehicle
{
    public YamahaVmax(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Yamaha V-Max", "YamahaVMax", "-2 Physical Distance", suit, number, state, owner, g)
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
        this.Form.Owner.MinusDistance += 2;
        base.Equipped();
    }

    public override void UnEquipped()
    {
        this.Form.Owner.MinusDistance -= 2;
        if (this.Form.Owner.MinusDistance < 0) this.Form.Owner.MinusDistance = 0;
        base.UnEquipped();
    }
}

