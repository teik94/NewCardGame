using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrelatiSpellbook: Weapon
{
    public PrelatiSpellbook(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Prelati's Spellbook", "PrelatiSpellbook", "", 1, suit, number, state, owner, g)
    {
        this.DamageIncrease += PrelatiEffect;
    }

    private System.Collections.IEnumerator PrelatiEffect(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        int busy = game.GetBusyTask();

        if (dmgType == Game.DamageType.Magical) source.DamageIncrease += 2;

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

