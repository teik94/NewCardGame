using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Excalibur : Weapon
{
    public Excalibur(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Excalibur", "Excalibur 1", "", 2, suit, number, state, owner, g)
    {
        this.BeforeAttack += ExcaliburEffect;
	}

    public System.Collections.IEnumerator ExcaliburEffect(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        if(victim.lastDamageCard != null)
        {
            if(victim.lastDamageCard.Form.CardData.Suit == Card.CardSuit.Heart 
                || victim.lastDamageCard.Form.CardData.Suit == Card.CardSuit.Diamond)
            {
                victim.AdditionDodge += 1;
            }
        }
        if (busy >= 0) game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
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
        base.Equipped();
    }

    public override void UnEquipped()
    {
        base.UnEquipped();
    }
}