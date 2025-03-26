using UnityEngine;
using UnityEngine.AI;


public class PatrolState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;
    private GameObject player;
    [SerializeField] private int aggroRange;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        waypoints = new Transform[4];
        waypoints[0] = GameObject.FindGameObjectWithTag("Waypoint 1").transform;
        waypoints[1] = GameObject.FindGameObjectWithTag("Waypoint 2").transform;
        waypoints[2] = GameObject.FindGameObjectWithTag("Waypoint 3").transform;
        waypoints[3] = GameObject.FindGameObjectWithTag("Waypoint 4").transform;
        waypointIndex = 0;
        animator.SetFloat("timeIdle", 0);
        UpdateDestination();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(animator.transform.position, player.transform.position) < aggroRange)
        {
            animator.SetBool("isChasing", true);
        }
        else if (Vector3.Distance(animator.transform.position, target) < 1)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }

    private void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.ResetPath();
    }
}
