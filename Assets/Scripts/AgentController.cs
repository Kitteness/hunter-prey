using UnityEngine;

public class AgentController : MonoBehaviour
{
    public Transform target;

    [Header("Agent Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 120f;
    public float safeRadius = 10f;
    [Header("Wander Settings")]
    public float wanderDistance = 10f;
    public float wanderRadius = 10f;
    public float updateDelayInSeconds = 0.75f;
    private float timeSinceLastUpdate = 0f;

    private Vector3 direction = Vector3.forward;

    // Update is called once per frame
    public void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (target != null && Vector3.Distance(transform.position, target.position) < safeRadius)
        {
            direction = SteeringUtility.Flee(transform.position, target.position);
        }
        else
        {
            if (timeSinceLastUpdate > updateDelayInSeconds)
            {
                timeSinceLastUpdate = 0f;
                direction = SteeringUtility.Wander(transform, wanderDistance, wanderRadius);
            }
        }

        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        transform.Translate(moveSpeed * Time.deltaTime * transform.forward, Space.World);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);
    }
}
