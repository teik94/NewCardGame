using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Avalon:Armor
{
    public Avalon(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Avalon", "Avalon", "", suit, number, state, owner, g)
    {
        this.TakeMagicDamage += TakeDamage;
        this.TakePhysicDamage += TakeDamage;
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
    int free;
    public IEnumerator TakeDamage(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        free = game.GetFreeTask();
        game.busy[free] = true;
        Action<CardForm> action = delegate(CardForm cf)
        {
            source.StartCoroutine(this.AvalonTakeEffect(cf));
        };
        game.PerformJudgment(victim, action);

        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        game.busy[busy] = false;
    }

    public IEnumerator AvalonTakeEffect(CardForm cf)
    {
        if (cf.Form.CardData.Suit == Card.CardSuit.Heart)
        {
            bool freeTask2 = game.busy[game.GetFreeTask()];
            Player owner = this.Form.Owner;
            if (owner != null) owner.StartCoroutine(owner.Healing(1, owner, owner));
            while (freeTask2) yield return new WaitForSeconds(0.1f);
        }
        game.busy[free] = false;
        yield return new WaitForSeconds(0.1f);
        //game.JudgmentEffect -= AvalonTakeEffect;
    }
}

