using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    [SerializeField] private int aggroRange;
    [SerializeField] private float chaseSpeed = 5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent.speed = chaseSpeed;
        animator.SetFloat("timeIdle", 0);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.transform.position);
        if (Vector3.Distance(animator.transform.position, player.transform.position) > aggroRange)
        {
            animator.SetBool("isChasing", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.ResetPath();
    }
}
