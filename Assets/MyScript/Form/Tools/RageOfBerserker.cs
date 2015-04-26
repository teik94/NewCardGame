using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RageOfBerserker : Tool
{
    public RageOfBerserker(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Rage of Berserker", "RageOfBerserker", "", suit, number, state, ToolType.NonTimeDelay, owner, g)
    {
        this.Form.CardData.Description = "";

    }
    public override bool useCondition()
    {
        Player owner = this.Form.Owner;
        int max = Convert.ToInt32(Math.Floor(owner.MaxHealth));
        if (owner.CurrentHealth == max)
        {
            return false;
        }
        return base.useCondition();
    }
    public override void UseCard()
    {
        Player owner = this.Form.Owner;
        owner.actionState = Player.ActionState.None;

        mainAction = delegate()
        {
            int heal = 0;
            int max = Convert.ToInt32(Math.Floor(owner.MaxHealth));
            heal = max - owner.CurrentHealth;

            if (owner.Healing != null) owner.StartCoroutine(owner.Healing(heal, owner, owner));
            owner.Rage = true;
            if (owner.Turn == Player.PlayerTurn.Action) owner.actionState = Player.ActionState.Free;
        };
        UseCardAnimation();
    }



}