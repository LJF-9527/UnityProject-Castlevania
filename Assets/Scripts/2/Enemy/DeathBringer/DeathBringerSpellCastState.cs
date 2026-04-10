using UnityEngine;
using UnityEngine.U2D;

public class DeathBringerSpellCastState : EnemyState
{
    private Enemy_DeathBringer enemy;

    private int amountOfSpells;
    private float spellCooldown;
    private float spellTimer;
    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        amountOfSpells=enemy.amountOfSpells;
        spellTimer = .5f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast=Time.time;
    }

    public override void Update()
    {
        base.Update();
        spellTimer-=Time.deltaTime;
        if (CanCast())
        {
            enemy.CastSpell();
            
        }
        if (amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }
    private bool CanCast()
    {
        if(amountOfSpells>0&&spellTimer<0)
        {
            spellTimer = enemy.castCooldown;
            amountOfSpells--;
            return true;
        }
        return false;
    }
}
