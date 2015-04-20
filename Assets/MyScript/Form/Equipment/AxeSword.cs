using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class AxeSword : Weapon
{
    public AxeSword(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Axe-Sword", "AxeSword", "", 3, suit, number, state, owner, g)
    {
        this.BeforeAttack += AxeSwordJudgemnt;
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

    int free;
    CardForm armorTarget = null;
    public IEnumerator AxeSwordJudgemnt(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        free = game.GetFreeTask();

        Equipment armor = null;
        if (victim.Armor != null) armor = victim.Armor.GetComponent<Equipment>();
        if (armor != null && armor.Form != null) armorTarget = armor.Form;
        
        if(armorTarget == null)
        { game.busy[busy] = false; yield break; }

        Window window = new Window(game);
        window.Title = "Axe-Sword Effect";
        window.Caption = "Do you want to active Axe-Sword's Effect?";
        window.Type = NeoWindow.WindowType.YesNo;
        window.Show();

        while (window.Result == NeoWindow.WindowResult.None) yield return new WaitForSeconds(0.1f);
        NeoWindow.WindowResult result = window.Close();
        if (result == NeoWindow.WindowResult.Yes)
        {
            game.busy[free] = true;
            Action<CardForm> action = delegate(CardForm cf)
            {
                source.StartCoroutine(this.AxeSwordTakeEffect(cf));
            };
            game.PerformJudgment(source, action);

            while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        }

        if(busy>=0)game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator AxeSwordTakeEffect(CardForm cf)
    {
        if (cf.Form.CardData.Suit != Card.CardSuit.Heart && armorTarget != null)
        {
            armorTarget.Discard();
            armorTarget = null;
            yield return new WaitForSeconds(0.3f);
        }
        if (free >= 0) game.busy[free] = false;
        yield return new WaitForSeconds(0.1f);
    }
}