using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ThomsonContender : Weapon
{
    public ThomsonContender(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Thompson Contender", "ThompsonContender", "", 4, suit, number, state, owner, g)
    {
        this.CauseDamage += ThomsonContenderEffect;
        //this.CausePhysicDamage += ThomsonContenderEffect;
        this.AbilityUseMax = 1;
        this.BeginingOfTurn += ResetTime;
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

    private IEnumerator ResetTime(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();

        this.AbilityUsed = 0;

        if (busy >= 0) game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator ThomsonContenderEffect(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        int busy = game.GetBusyTask();

        if (this.AbilityUsed < this.AbilityUseMax)
        {
            game.DrawXCard(1, source);
            this.AbilityUsed += 1;
            yield return new WaitForSeconds(0.5f);
        }

        if(busy >= 0)game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }
}