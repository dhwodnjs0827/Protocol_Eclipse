using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDefinition;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public Player player;
    public float playerDistance;
    
    public eMonsterState monsterState;

    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;

    public Slider hpBar;
    public float monsterHP;

    public Vector3[] patrolPos;
    public int patrolIndex;

    public Animator animator;
    public NavMeshAgent agent;

    virtual public void Idle()
    {
        animator.SetInteger("monsterState", (int)eMonsterState.Idle);
        AnimatorStateInfo idleState = animator.GetCurrentAnimatorStateInfo(0);
        if (idleState.normalizedTime >= 0.95f)
        {
            agent.isStopped = false;
            monsterState = eMonsterState.Walk;
        }
    }
    virtual public void Walk()
    {
        animator.SetInteger("monsterState", (int)eMonsterState.Walk);
        if (agent.remainingDistance == 0f)
        {
            agent.isStopped = true;
            patrolIndex = (patrolIndex + 1) % patrolPos.Length;
            agent.SetDestination(patrolPos[patrolIndex]);
            monsterState = eMonsterState.Idle;
        }
    }
    virtual public void Trace()
    {
        animator.SetInteger("monsterState", (int)eMonsterState.Trace);
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);

        if (playerDistance <= 2f)
        {
            agent.isStopped = true;
            monsterState = eMonsterState.Attack;
        }
        else if (playerDistance > 10f)
        {
            agent.isStopped = true;
            monsterState = eMonsterState.Idle;
        }
    }
    virtual public void Attack()
    {
        animator.SetInteger("monsterState", (int)eMonsterState.Attack);
        AnimatorStateInfo attackState = animator.GetCurrentAnimatorStateInfo(0);
        if (attackState.normalizedTime >= 0.95f)
        {
            if (playerDistance > 2f)
            {
                agent.isStopped = false;
                monsterState = eMonsterState.Trace;
            }
        }
    }
    virtual public void Dead()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
    }
    public void Damaged(float damage)
    {
        animator.SetTrigger("tDamaged");
        animator.SetInteger("monsterState", (int)eMonsterState.Damaged);
        monsterHP -= damage;
        hpBar.value = monsterHP;

        monsterState = eMonsterState.Damaged;
        
        if (monsterHP > 0f)
        {
            monsterState = eMonsterState.Trace;
        }
        else if (monsterHP <= 0f)
        {
            monsterState = eMonsterState.Dead;
            animator.SetTrigger("tDead");
        }
    }
}
