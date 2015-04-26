using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KingTreasure : Tool
{
    public KingTreasure(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("King's Treasure", "KingTreasure", "", suit, number, state, ToolType.NonTimeDelay, owner, g)
    {
        this.Form.CardData.Description = "";
    }

    public override void UseCard()
    {
        Player owner = this.Form.Owner;
        owner.actionState = Player.ActionState.None;
        mainAction = delegate()
        {
            game.DrawXCard(2, owner);
            if (owner.Turn == Player.PlayerTurn.Action) owner.actionState = Player.ActionState.Free;
        };
        UseCardAnimation();
    }

}