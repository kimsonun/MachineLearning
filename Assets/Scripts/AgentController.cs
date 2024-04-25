using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class AgentController : Agent
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] WorldBehaviors worldBehaviors;
    [SerializeField] GunController gunController;

    private Rigidbody rb;
    private bool canShoot, hitTarget, hasShot;
    private int timeNextBullet = 0;
    private int maxTimeNextBullet = 30;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        worldBehaviors.SpawnAgent();
        worldBehaviors.CallSpawnMonster();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        canShoot = false;

        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];

        bool shoot = actions.DiscreteActions[0] > 0;

        rb.MovePosition(transform.position + moveForward * moveSpeed * Time.deltaTime * transform.forward);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);

        if (shoot && !hasShot)
        {
            canShoot = true;
        }
        if (canShoot)
        {
            hitTarget = gunController.ShootGun();
            timeNextBullet = maxTimeNextBullet;
            hasShot = true;
            if (hitTarget)
            {
                AddReward(10f);
                if (worldBehaviors.GetMonsterCount() <= 0)
                    EndEpisode();
            }
            else
            {
                AddReward(-1f);
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Monster")
        {
            AddReward(-30f);
            EndEpisode();
        }
    }

    private void FixedUpdate()
    {
        if (hasShot)
        {
            timeNextBullet--;
            if (timeNextBullet <= 0)
            {
                hasShot = false;
            }
        }
    }
}
