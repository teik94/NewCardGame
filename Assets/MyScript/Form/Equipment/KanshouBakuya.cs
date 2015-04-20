using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class KanshouBakuya : Weapon
{
    public KanshouBakuya(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Kanshou & Bakuya", "KanshouBakuya", "", 2, suit, number, state, owner, g)
    {
        this.AbilityUseMax = 1;
        this.BeforeAttack += IncreaseAttackTime;
        this.BeginingOfTurn += ResetAttackTime;
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
        this.AbilityUsed = 0;
        if (this.AbilityUsed < this.AbilityUseMax && this.Form.Owner.MainAttack)
        {
            this.Form.Owner.MainAttack = false;
            AbilityUsed += 1;
        }
        base.Equipped();
    }

    public override void UnEquipped()
    {
        base.UnEquipped();
    }

    public IEnumerator ResetAttackTime(int number, Player source, Player vicitm)
    {
        this.AbilityUsed = 0;
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator IncreaseAttackTime(int number, Player source, Player vicitm)
    {
        int busy = game.GetBusyTask();
        if (this.AbilityUsed < this.AbilityUseMax && this.Form.Owner.MainAttack)
        {
            this.Form.Owner.MainAttack = false;
            this.AbilityUsed += 1;
        }
        if (busy >= 0) game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }
}