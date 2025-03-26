using UnityEngine;
using UnityEngine.AI;

public static class SteeringUtility
{
    public static Vector3 Seek(Vector3 origin, Vector3 target)
    {
        Vector3 direction = target - origin;
        return direction;
    }

    public static Vector3 Seek(Transform origin, Transform target)
        => Seek(origin.position, target.position);

    public static void Seek(NavMeshAgent agent, Vector3 target)
    {
        agent.SetDestination(target);
    }

    public static void Seek(NavMeshAgent agent, Transform target)
        => Seek(agent, target.position);

    public static Vector3 Flee(Vector3 origin, Vector3 target)
        => Seek(target, origin);

    public static Vector3 Flee(Transform origin, Transform target)
        => Seek(target.position, origin.position);

    public static void Flee(NavMeshAgent agent, Vector3 target, float safeRadius)
    {
        Vector3 direction = agent.transform.position - target;
        Vector3 safePoint = agent.transform.position + direction.normalized * safeRadius;

        NavMesh.SamplePosition(safePoint, out NavMeshHit hitPoint, 15f, NavMesh.AllAreas);
        agent.SetDestination(hitPoint.position);
    }

    public static void Flee(NavMeshAgent agent, Transform target, float safeRadius)
        => Flee(agent, target.position, safeRadius);

    public static Vector3 Wander(Transform origin, float wanderDistance, float wanderRadius)
    {
        Vector3 wanderCentre = origin.position + origin.forward * wanderDistance;
        Vector2 pointOnCircle = Random.insideUnitCircle.normalized * wanderRadius;
        Vector3 randomPoint = wanderCentre + new Vector3(pointOnCircle.x, 0, pointOnCircle.y);

        return Seek(origin.position, randomPoint);
    }

    public static void Wander(NavMeshAgent agent, float wanderDistance, float wanderRadius)
    {
        Vector3 wanderCentre = agent.transform.position + agent.transform.forward * wanderDistance;
        Vector2 pointOnCircle = Random.insideUnitCircle.normalized * wanderRadius;
        Vector3 randomPoint = wanderCentre + new Vector3(pointOnCircle.x, 0, pointOnCircle.y);

        NavMesh.SamplePosition(randomPoint, out NavMeshHit hitPoint, 15f, NavMesh.AllAreas);
        agent.SetDestination(hitPoint.position);
    }
}
