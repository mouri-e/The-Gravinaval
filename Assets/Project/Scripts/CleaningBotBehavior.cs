using UnityEngine;
using UnityEngine.AI;

public class CleaningBotBehavior : MonoBehaviour
{
    public enum BotState { Patrol, Talk }

    [Header("General Settings")]
    public BotState currentState = BotState.Patrol;
    public float Speed;
    private NavMeshAgent agent;
    private Animator anim;
    public Transform[] patrolPoints;
    private int currentIndex = 0;
    private NPCDialogueBehavior dialogueScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        dialogueScript = GetComponent<NPCDialogueBehavior>();
        agent.speed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueScript != null)
        {
            currentState = dialogueScript.getInDialogue() ? BotState.Talk : BotState.Patrol;
        }

        switch (currentState)
        {
            case BotState.Patrol:
                anim.SetBool("IsPatrollingBool", true);
                anim.SetBool("IsTalkingBool", false);
                Patrol();
                break;
            case BotState.Talk:
                anim.SetBool("IsPatrollingBool", false);
                anim.SetBool("IsTalkingBool", true);
                Talk();
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
    
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentIndex].position);
        }
    }

    void Talk()
    {
        if (!agent.isStopped)
        {
            agent.isStopped = true;
        }
    }
}
