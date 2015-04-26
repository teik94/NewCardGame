using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GateOfBabylon : Tool
{
    public GateOfBabylon(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Gate Of Babylon", "GoB", "", suit, number, state, ToolType.NonTimeDelay, owner, g)
    {
        this.Form.CardData.Description = "";

    }

    public override void UseCard()
    {
        Player owner = this.Form.Owner;
        owner.actionState = Player.ActionState.None;
        List<Player> pList = game.GetPlayerByRound(owner);
        foreach (Player player in pList)
        {
            player.Respond += DistantTramplingRespond;
        }
        mainAction = delegate()
        {
            game.busy[game.GetFreeTask()] = true;
            game.StartCoroutine(game.DamageAOE(1, owner, pList, this, Game.DamageType.Physical));
        };
        UseCardAnimation();
    }

    private IEnumerator DistantTramplingRespond(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        int busy = game.GetBusyTask();
        victim.actionState = Player.ActionState.WaitingDodge;
        if (!victim.AutoAI)
        {
            if (victim == game.myPlayer)
            {
                //victim.actionState = Player.ActionState.WaitingAttack;
                game.btnCancel.SetActive(true);
                game.CancelClick += delegate()
                {
                    victim.IsRespond = false;
                    game.btnCancel.SetActive(false);
                    game.CancelClick = null;
                    game.busy[busy] = false;
                    victim.actionState = Player.ActionState.None;
                };
                while (victim.actionState == Player.ActionState.WaitingDodge) yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            List<CardForm> hand = game.CardList.GetHandList(victim);
            int atkCount = 0;
            foreach (CardForm cf in hand)
            {
                if (cf is Dodge)
                { atkCount++; }
            }
            if (atkCount >= 1)
            {
                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i] is Dodge)
                    {
                        Dodge dodgeCard = hand[i] as Dodge;
                        dodgeCard.UseCard();
                        break;
                    }
                }
            }
            else
            {
                victim.IsRespond = false;
            }
        }
        if (victim.actionState == Player.ActionState.WaitingDodge) victim.actionState = Player.ActionState.None;
        if (busy >= 0) game.busy[busy] = false;
        yield return new WaitForSeconds(0.1f);
    }

}

