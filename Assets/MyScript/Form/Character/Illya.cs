using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Illya : Character
{
    void Start()
    {
        this.Type = CharacterType.Master;
        this.CharacterName = "Illya";
        this.MaxHealth = 1.5f;
        this.Asset = "Illya";
    }

    void OnGUI()
    {

    }

    void Update()
    {
        if(this.PlayerOwner !=null)
        {
            if (Ability[0] == null) Ability[0] = new IllyaAbility1(CharacterAbility.AbilityType.Passive, this);
            if (Ability[1] == null) Ability[1] = new IllyaAbility2(CharacterAbility.AbilityType.Active, this);
            if (Ability[2] == null) Ability[2] = new IllyaAbility3(CharacterAbility.AbilityType.Active, this);
        }
    }
}

public class IllyaAbility1 : CharacterAbility
{
    public IllyaAbility1(AbilityType type, Character character)
        : base(type, character)
    {

    }
}

public class IllyaAbility2 : CharacterAbility
{
    //Draw one card everytime Illya do the judgment
    public IllyaAbility2(AbilityType type, Character character)
        : base(type, character)
    {
        this.Form = AbilityForm.OnJudgment;
    }

    public override IEnumerator Ability(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        int busy = game.GetBusyTask();
        //int free = game.GetFreeTask();
        game.DrawXCard(1, this.CharacterOwner.PlayerOwner);
        yield return new WaitForSeconds(0.5f);
        if (busy >= 0) game.busy[busy] = false;
    }
}

public class IllyaAbility3 : CharacterAbility
{
    //Redo Judgment Ability
    public IllyaAbility3(AbilityType type, Character character)
        : base(type, character)
    {
        this.Form = AbilityForm.JudgmentChange;
    }


    public override IEnumerator Ability(int number, Player source, Player victim, Game.DamageType dmgType)
    {
        int busy = game.GetBusyTask();
        Debug.Log("Start Illya Ability");
        //int free = game.GetFreeTask();
        //Waiting for using ability
        source.actionState = Player.ActionState.WaitingDiscard;
        if (source == game.myPlayer)
        {
            game.CustomCondition += CustomCondition;
            game.btnCancel.SetActive(true);
            game.CancelClick += delegate()
            {
                game.btnCancel.SetActive(false);
                game.btnUse.SetActive(false);
                game.CancelClick = null;
                if (busy >= 0) game.busy[busy] = false;
                game.CustomCondition = null;
                source.Discard = null;
                source.actionState = Player.ActionState.None;
            };
        }
        source.Discard += ReJudgment;

        //Auto AI
        if (source.AutoAI)
        {
            if (busy >= 0) game.busy[busy] = false;
            game.CustomCondition = null;
            source.Discard = null;
            source.actionState = Player.ActionState.None;
        }
        while (source.actionState == Player.ActionState.WaitingDiscard)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Open task " + busy);
        if (busy >= 0) game.busy[busy] = false;
    }

    public IEnumerator ReJudgment()
    {
        game.btnCancel.SetActive(false);
        game.btnUse.SetActive(false);
        game.CancelClick = null;
        //game.Busy2 = false;
        game.CustomCondition = null;
        this.CharacterOwner.PlayerOwner.Discard = null;
        this.CharacterOwner.PlayerOwner.actionState = Player.ActionState.None;
        Used = true;

        yield break;
    }

    public bool CustomCondition()
    {
        if (this.CharacterOwner.PlayerOwner.actionState == Player.ActionState.WaitingDiscard)
        {
            if (game.ProcessingCard.Form.CardData.Suit == Card.CardSuit.Club ||
                     game.ProcessingCard.Form.CardData.Suit == Card.CardSuit.Spade)
            {
                return true;
            }
        }
        return false;
    }
}

