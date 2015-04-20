using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Clarent : Weapon
{
    public Clarent(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Clarent", "Clarent", "", 2, suit, number, state, owner, g)
    {
        this.BeforeAttack += IncreaseDodge;
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

    public IEnumerator IncreaseDodge(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();

        int damage = (source.DamageIncrease + number) - victim.DamageDecrease;
        if (damage > 1) victim.AdditionDodge += damage - 1;

        if (busy >= 0) game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }
}