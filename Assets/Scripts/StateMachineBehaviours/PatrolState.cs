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

    private bool investigatingNoise = false;
    private int previousWaypointIndex = -1;

    private Vector3 noiseLocation;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

        waypoints = new Transform[4];
        waypoints[0] = GameObject.FindGameObjectWithTag("Waypoint 1").transform;
        waypoints[1] = GameObject.FindGameObjectWithTag("Waypoint 2").transform;
        waypoints[2] = GameObject.FindGameObjectWithTag("Waypoint 3").transform;
        waypoints[3] = GameObject.FindGameObjectWithTag("Waypoint 4").transform;

        Debug.Log($"[PatrolState] Found waypoints: {waypoints[0]}, {waypoints[1]}, {waypoints[2]}, {waypoints[3]}");


        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned for PatrolState!");
            return;
        }

        waypointIndex = 0;

        investigatingNoise = false;
        previousWaypointIndex = -1;

        animator.SetFloat("timeIdle", 0);
        UpdateDestination();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TaskStation taskStation = FindFirstObjectByType<TaskStation>();

        if (Vector3.Distance(animator.transform.position, player.transform.position) < aggroRange)
        {
            animator.SetBool("isChasing", true);
            return;
        }

        if (!investigatingNoise && taskStation != null && taskStation.hasNoise)
        {
            investigatingNoise = true;
            previousWaypointIndex = waypointIndex;
            noiseLocation = TaskStation.currentNoiseLocation;
            agent.SetDestination(noiseLocation);
        }

        if (investigatingNoise)
        {
            float distToNoise = Vector3.Distance(animator.transform.position, noiseLocation);
            if (distToNoise < 2f)
            {
                investigatingNoise = false;
                taskStation.hasNoise = false;

                target = waypoints[previousWaypointIndex].position;
                agent.SetDestination(target);

                waypointIndex = previousWaypointIndex;
                previousWaypointIndex = -1;
            }
            return;
        }


        if (Vector3.Distance(animator.transform.position, target) < 1f)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }

    private void UpdateDestination()
    {
        if (waypoints == null || waypoints.Length == 0) return;

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

        investigatingNoise = false;
        previousWaypointIndex = -1;
    }
}