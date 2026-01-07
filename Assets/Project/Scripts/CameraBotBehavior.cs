using UnityEngine;
using UnityEngine.AI;

public class CameraBotBehavior : MonoBehaviour
{

    public enum BotState { Patrol, Talk, Malfunction }

    [Header("General Settings")]
    public BotState currentState = BotState.Patrol;
    public float range = 10f; //radius of sphere
    public Transform centerPoint; //center of the area the agent wants to move around in
    private NavMeshAgent agent;
    private Animator anim;
    private NPCDialogueBehavior dialogueScript;

    float yPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        dialogueScript = GetComponent<NPCDialogueBehavior>();
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
                anim.SetBool("IsBrokenDownBool", false);
                anim.SetBool("IsTalkingBool", false);
                Patrol();
                break;
            case BotState.Talk:
                anim.SetBool("IsPatrollingBool", false);
                anim.SetBool("IsBrokenDownBool", false);
                anim.SetBool("IsTalkingBool", true);
                Talk();
                break;
            case BotState.Malfunction:
                anim.SetBool("IsPatrollingBool", false);
                anim.SetBool("IsBrokenDownBool", true);
                anim.SetBool("IsTalkingBool", false);
                Malfunction();
                break;
        }
    }

    void Patrol()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centerPoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
    }

    void Talk()
    {
        if (!agent.isStopped)
        {
            agent.isStopped = true;
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in the sphere
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;

    }

    void Malfunction()
    {
        Transform malfunctionVFX = transform.Find("MalfunctionVFX");
        if (malfunctionVFX != null)
            malfunctionVFX.gameObject.SetActive(true);
    }
}
