using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Hrunting : Weapon
{
    public Hrunting(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Hrunting", "Hrunting", "", 5, suit, number, state, owner, g)
    {
        this.DamageIncrease += HruntingIncreaseDamage;
        this.CauseDamage += HruntingEffect;
        //this.CausePhysicDamage += HruntingEffect;
    }

    public override void Ability()
    {
        Window window = new Window(game);
        window.Title = "Hrunting Effect";
        window.Caption = "Do you want to active Hrunting's Effect?";
        window.Type = NeoWindow.WindowType.YesNo;
        window.Show();
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

    public IEnumerator HruntingEffect(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        //while (game.Busy || game.Busy2) yield return new WaitForSeconds(0.1f);
        int busy = game.GetBusyTask();

        List<CardForm> list = new List<CardForm>();

        Equipment weapon = null, armor = null, plus = null, minus = null;
        if (victim.Weapon != null) weapon = victim.Weapon.GetComponent<Equipment>();
        if (victim.Armor != null) armor = victim.Armor.GetComponent<Equipment>();
        if (victim.PlusVehicle != null) plus = victim.PlusVehicle.GetComponent<Equipment>();
        if (victim.MinusVehicle != null) minus = victim.MinusVehicle.GetComponent<Equipment>();
        if (weapon != null && weapon.Form != null) list.Add(weapon.Form);
        if (armor != null && armor.Form != null) list.Add(armor.Form);
        if (plus != null && plus.Form != null) list.Add(plus.Form);
        if (minus != null && minus.Form != null) list.Add(minus.Form);

        if (list.Count == 0) { game.busy[busy] = false; yield break; }

        Window window = new Window(game);
        window.Title = "Hrunting Effect";
        window.Caption = "Do you want to active Hrunting's Effect?";
        window.Type = NeoWindow.WindowType.YesNo;
        window.Show();

        while (window.Result == NeoWindow.WindowResult.None) yield return new WaitForSeconds(0.1f);
        NeoWindow.WindowResult result = window.Close();
        if (result == NeoWindow.WindowResult.Yes)
        {
            window = new Window(game);
            window.Title = "Choose one...";
            window.Type = NeoWindow.WindowType.FreeWindow;
            window.Cards = list;

            if (window.Cards.Count > 0)
            {
                window.Show();
                while (window.SelectedCard == null) yield return new WaitForSeconds(0.1f);
                CardForm cf = window.SelectedCard;
                window.Close();
                cf.Discard();
            }
            else
            {
                window.Close();
            }
        }

        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator HruntingIncreaseDamage(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        //while (game.Busy || game.Busy2) yield return new WaitForSeconds(0.1f);
        int busy = game.GetBusyTask();

        CardForm weapon = victim.GetWeapon();
        CardForm armor = victim.GetArmor();
        CardForm plus = victim.GetPlusVehicle();
        CardForm minus = victim.GetMinusVehicle();

        if (weapon == null && armor == null && plus == null && minus == null)
        {
            source.DamageIncrease += 1;
        }

        game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }
}

