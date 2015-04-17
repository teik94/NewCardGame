using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CharacterAbility
{
    public enum AbilityType
    {
        Passive, Active, Special
    }
    public AbilityType Type;

    public enum AbilityForm
    {
        JudgmentChange, JudgementDone, Attacking, Attacked, CausePhysicalDamage, TakePhysicalDamage,
        CauseMagicDamage, TakeMagicDamage, JudgmentPhase, DrawPhase, ActionPhase, DiscardPhase, EndTurn,
        BeginningOfTurn, Dodging, UsingCard, Discard, GainHealth, BrinkOfDeath, AttackDamageModifier,
        EndAttack, LoseHealth, OnJudgment
    }
    public AbilityForm Form;

    public Character CharacterOwner;
    public bool Status = true;
    public bool Used = false;
    public Game game;
    public int UsedTime = 0;
    public int UsedMax = 0;

    public enum LimitType
    {
        PerTurn, PerRound, PerGame, PerAction
    }
    public LimitType limitType = LimitType.PerTurn;
    public CharacterAbility(AbilityType type,Character character)
    {
        Type = type;
        CharacterOwner = character;
        this.game = this.CharacterOwner.game;
    }

    public virtual IEnumerator Ability(int number, Player source, Player victim)
    {
        yield break;
    }
}

