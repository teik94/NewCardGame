using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GrailOfHeaven : Tool
{
    public GrailOfHeaven(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Grail of Heaven", "GrailOfHeaven", "", suit, number, state, ToolType.NonTimeDelay, owner, g)
    {
        this.Form.CardData.Description = "";

    }

    public override bool useCondition()
    {
        bool result = false;
        foreach (Player player in game.playerList)
        {
            int max = Convert.ToInt32(Math.Floor(player.MaxHealth));
            if(player.CurrentHealth < max)
            {
                result = true;
                break;
            }
        }
        if (!result) return false;
        return base.useCondition();
    }

    public override void UseCard()
    {
        Player owner = this.Form.Owner;
        owner.actionState = Player.ActionState.None;
        List<Player> pList = game.GetPlayerByRound(owner);
        pList.Insert(0, owner);
        mainAction = delegate()
        {
            game.busy[game.GetFreeTask()] = true;
            game.StartCoroutine(game.HealAOE(1, owner, pList, this));
        };
        UseCardAnimation();
    }

}


