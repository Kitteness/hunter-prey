using UnityEngine;
using UnityEngine.AI;

public class TapToMove : MonoBehaviour
{
    public NavMeshAgent agent;

    // Update is called once per frame
    public void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }
    }
}
