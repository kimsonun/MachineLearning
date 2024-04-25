using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterController : MonoBehaviour
{
    public WorldBehaviors worldBehaviors;
    public Transform target;
    [SerializeField] float updateSpeed = 0.1f;
    private NavMeshAgent monster;

    private void Awake()
    {
        monster = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);
        while (enabled)
        {
            monster.SetDestination(target.transform.position);
            yield return wait;
        }
    }

    public void MoveMonster(Vector3 position)
    {
        monster.Warp(position);
    }

    private void OnDestroy()
    {
        worldBehaviors.spawnedMonsterList.Remove(this);
    }
}
