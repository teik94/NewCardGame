using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Duel : Tool
{
    public Duel(Card.CardSuit suit, Card.CardNumber number, Card.CardState state, Player owner, Game g)
        : base("DUEL", "Duel", "", suit, number, state, ToolType.NonTimeDelay, owner, g)
    {
        this.Form.CardData.Description = "";
        
	}

    public override void UseCard()
    {
        SelectDuelTarget();
    }
    
}

