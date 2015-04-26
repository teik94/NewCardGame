using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class RuleBreaker : Tool
{
    public RuleBreaker(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Rule Breaker", "RuleBreaker", "", suit, number, state, ToolType.NonTimeDelay, owner, g)
    {
        this.Form.CardData.Description = "";
    }

    public override void UseCard()
    {
        SelectToolTarget();
    }

    public override void ToolTakeEffect(GameObject playerPanel)
    {
        base.ToolTakeEffect(playerPanel);
        Player owner = this.Form.Owner;
        Player target = playerPanel.GetComponent<Player>();
        owner.actionState = Player.ActionState.None;
        mainAction = delegate()
        {
            int free = game.GetFreeTask();
            game.busy[free] = true;
            game.StartCoroutine(Effect(owner, target));
        };
        UseCardAnimation();
    }

    private IEnumerator Effect(Player owner, Player target)
    {
        int free = game.GetFreeTask();
        int busy = game.GetBusyTask();

        List<CardForm> list = target.GetOwnCard();
        
        Window window = new Window(game);
        window.Title = "Choose one...";
        window.Type = NeoWindow.WindowType.FreeWindow;
        window.Cards = list;

        if (window.Cards.Count > 0)
        {
            window.Show();
            while (window.SelectedCard == null) yield return new WaitForSeconds(0.1f);
            CardForm cf = window.SelectedCard;
            window.Close();
            //game.busy[free] = true;
            //owner.StartCoroutine(cf.ToHandAnimation(owner));
            cf.Discard();
        }
        else
        {
            window.Close();
        }
        //game.busy[free] = false;

        //while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        if (owner.Turn == Player.PlayerTurn.Action) owner.actionState = Player.ActionState.Free;
        if (busy >= 0) game.busy[busy] = false;
        if (game.GetBusyTask() < 0) game.PilesCollect();
    }
}