using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie : Monster, BillBoard
{
    private void Awake()
    {
        player = PlayManager.instance.Player;
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        
        monsterState = eMonsterState.Idle;

        moveSpeed = 0f;
        walkSpeed = 2f;
        runSpeed = 5f;

        hpBar = GetComponentInChildren<Slider>();
        monsterHP = 100f;

        patrolPos = new Vector3[3];
        patrolIndex = 0;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        hpBar.value = monsterHP;

        patrolPos[0] = new Vector3(21f, 1.628587f, -14f);
        patrolPos[1] = new Vector3(6f, 1.628587f, -15f);
        patrolPos[2] = new Vector3(15f, 1.628587f, -8f);
        agent.SetDestination(patrolPos[0]);
    }

    private void Update()
    {
        BillBoarding(hpBar.gameObject);

        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        moveSpeed = (monsterState == eMonsterState.Trace) ? runSpeed : walkSpeed;
        agent.speed = moveSpeed;

        switch (monsterState)
        {
            case eMonsterState.Idle:
                Idle();
                break;
            case eMonsterState.Walk:
                Walk();
                break;
            case eMonsterState.Trace:
                Trace();
                break;
            case eMonsterState.Attack:
                Attack();
                break;
            case eMonsterState.Dead:
                Dead();
                break;
        }
    }

    public void BillBoarding(GameObject ui)
    {
        ui.transform.forward = Camera.main.transform.forward;
    }
}
