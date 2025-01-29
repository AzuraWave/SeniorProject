using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected StateMachine stateMachine;
    protected PlayerController player;
    protected Enemy enemy;
    public virtual bool IsActionState => false;
    public virtual bool CanAttackFromState => false;

    public virtual void Enter(){}
    public virtual void Execute(){}
    public virtual void FixedExecute(){}
    public virtual void Exit(){}
    public virtual State CheckTransitions() => null;

    public void Setup(StateMachine stateMachine, PlayerController _controller){
        this.stateMachine = stateMachine;
        this.player = _controller;
        this.enemy = null;
    }
    public void Setup(StateMachine stateMachine, Enemy enemy){
        this.stateMachine = stateMachine;
        this.enemy = enemy;
        this.player = null;
    }
    
}
