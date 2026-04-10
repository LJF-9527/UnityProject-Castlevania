using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerTrigger : Enemy_AnimationTrigger
{
    private Enemy_DeathBringer enemyDeathBringer => GetComponentInParent<Enemy_DeathBringer>();
    private void ReLocate() => enemyDeathBringer.FindPosition();
    private void MakeInvisible() => enemyDeathBringer.fX.MakeTransprent(true);
    private void MakeVisible() => enemyDeathBringer.fX.MakeTransprent(false);
}
