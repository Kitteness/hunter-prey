using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private GameObject player;
    [SerializeField] private int aggroRange;
    private float timeIdle;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(animator.transform.position, player.transform.position) < aggroRange)
        {
            animator.SetBool("isChasing", true);
        }
        else
        {
            timeIdle += Time.deltaTime;
            animator.SetFloat("timeIdle", timeIdle);
        }
    }
}
