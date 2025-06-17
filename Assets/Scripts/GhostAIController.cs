using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAIController : MonoBehaviour
{
    public enum GhostState { HuntStart, Patrolling, ChasingPlayer, HearingPlayer }
    public GhostState currentState = GhostState.HuntStart;

    private NavMeshAgent agent;
    private Transform player;

    [Header("Roaming Settings")]
    public float roamRadius = 20f;
    public float roamDelay = 5f;
    private float nextRoamTime = 0f;

    [Header("Vision Settings")]
    public float visionRange = 12f;
    public float fieldOfView = 60f;

    [Header("Mic Detection")]
    public ScreamDetector screamDetector; // Drag reference in Inspector

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (!player)
        {
            Debug.LogError("❌ Player not found in scene!");
            enabled = false;
            return;
        }

        // Delay hunt start slightly
        Invoke(nameof(StartPatrolling), 2f);
    }

    void Update()
    {
        switch (currentState)
        {
            case GhostState.Patrolling:
                RandomRoam();
                DetectPlayerBySight();
                DetectMicInput();
                break;

            case GhostState.ChasingPlayer:
                ChasePlayer();
                break;

            case GhostState.HearingPlayer:
                MoveTowardPlayerSound();
                break;
        }
    }

    void StartPatrolling()
    {
        currentState = GhostState.Patrolling;
        Roam(); // Start with a random roam
    }

    void RandomRoam()
    {
        if (Time.time >= nextRoamTime && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Roam();
        }
    }

    void Roam()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            nextRoamTime = Time.time + roamDelay;
        }
    }

    void DetectPlayerBySight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (directionToPlayer.magnitude < visionRange && angle < fieldOfView / 2f)
        {
            Ray ray = new Ray(transform.position + Vector3.up * 1.5f, directionToPlayer.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, visionRange))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    currentState = GhostState.ChasingPlayer;
                    Debug.Log("👁️ Ghost sees the player!");
                }
            }
        }
    }

    void DetectMicInput()
    {
        if (screamDetector != null && screamDetector.IsPlayerTalking())
        {
            currentState = GhostState.HearingPlayer;
            Debug.Log("🎤 Ghost hears the player talking!");
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > visionRange * 1.5f)
        {
            currentState = GhostState.Patrolling;
            Roam();
        }
    }

    void MoveTowardPlayerSound()
    {
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < 3f || (screamDetector != null && !screamDetector.IsPlayerTalking()))
        {
            currentState = GhostState.Patrolling;
            Roam();
        }
    }
}
