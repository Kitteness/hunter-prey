using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float delayTime;
    public float distance;
    public float height;
    public float smoothSpeed;
    public float pitch;

    private bool startFollowing = false;

    private void Start()
    {
        StartCoroutine(StartFollowingAfterDelay());
    }

    private IEnumerator StartFollowingAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        startFollowing = true;
    }

    void LateUpdate()
    {
        if (!startFollowing || target == null) return;

        Vector3 desiredPosition = target.position
                                - target.forward * distance
                                + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float targetY = target.eulerAngles.y;
        Quaternion desiredRot = Quaternion.Euler(pitch, targetY, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, smoothSpeed * Time.deltaTime);
    }
}
