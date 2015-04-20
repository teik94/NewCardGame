using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaberArmor: Armor
{
    public SaberArmor(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Saber's Armor", "SaberArmor", "", suit, number, state, owner, g)
    {
        this.DamageDecrease += SaberArmorEffect;
    }

    private System.Collections.IEnumerator SaberArmorEffect(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        int busy = game.GetBusyTask();

        if (dmgType == Game.DamageType.Magical) victim.DamageDecrease += 1;

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

