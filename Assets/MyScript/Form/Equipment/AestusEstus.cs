using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class AestusEstus : Weapon
{
    public AestusEstus(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Aestus Estus", "AestusEstus", "", 2, suit, number, state, owner, g)
    {
        this.CausePhysicDamage += AestusDamage;
        this.CauseMagicDamage += AestusDamage;
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
        base.Equipped();
    }

    public override void UnEquipped()
    {
        base.UnEquipped();
    }

    public IEnumerator AestusDamage(int number, Player source, Player victim)
    {
        int busy = game.GetBusyTask();
        int free = game.GetFreeTask();
        Window window = new Window(game);
        window.Title = "Aestus Estus Effect";
        window.Caption = "Do you want to active Aestus Estus's Effect?";
        window.Type = NeoWindow.WindowType.YesNo;
        window.Show();

        while (window.Result == NeoWindow.WindowResult.None) yield return new WaitForSeconds(0.1f);
        NeoWindow.WindowResult result = window.Close();
        if (result == NeoWindow.WindowResult.Yes)
        {
            source.CurrentHealth -= 1;

            if(source.CurrentHealth<=0)
            {
                game.busy[free] = true;
                source.StartCoroutine(source.BrinkOfDeath(1, source, victim));
                while (game.busy[free]) yield return new WaitForSeconds(0.1f);
            }

            game.busy[free] = true;
            source.StartCoroutine(game.InflictDamage(1, source, victim, true));
            while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        }
        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }
}