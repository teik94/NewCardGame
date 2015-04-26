using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Collaboration : Tool
{
    public Collaboration(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("Collaboration", "Collaboration", "", suit, number, state, ToolType.NonTimeDelay, owner, g)
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
            game.StartCoroutine(Collaborate(owner, target));
        };
        UseCardAnimation();
    }

    private IEnumerator Collaborate(Player owner, Player target)
    {
        int free = game.GetFreeTask();
        game.busy[free] = true;
        game.StartCoroutine(game.DrawCardAction(1, target));
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        game.busy[free] = true;
        game.StartCoroutine(game.DrawCardAction(3, owner));
        while (game.busy[free]) yield return new WaitForSeconds(0.1f);
        if (owner.Turn == Player.PlayerTurn.Action) owner.actionState = Player.ActionState.Free;
        if (game.GetBusyTask() < 0) game.PilesCollect();
    }
}