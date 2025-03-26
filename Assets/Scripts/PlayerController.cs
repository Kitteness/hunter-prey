using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody body;
    [SerializeField] private float acceleration = 0.75f;
    [SerializeField] private float topSpeed = 2.5f;

    private Vector2 moveVal;

    [SerializeField] private Animator animator;

    private readonly int xSpeedParam = Animator.StringToHash("xSpeed");
    private readonly int zSpeedParam = Animator.StringToHash("zSpeed");

    public void Awake()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        Assert.IsNotNull(body);
        Assert.IsNotNull(animator);
    }

    public void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(moveVal.y, 0, -moveVal.x);
        body.AddForce(acceleration * moveDirection, ForceMode.VelocityChange);

        if (body.linearVelocity.magnitude > topSpeed)
        {
            body.linearVelocity = Vector3.ClampMagnitude(body.linearVelocity, topSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.action.inProgress)
        {
            moveVal = context.ReadValue<Vector2>();
            Vector3 relativeDirection = transform.InverseTransformDirection(new Vector3(moveVal.y, 1, -moveVal.x));

            // use relative direction to control animation.
            animator.SetFloat(xSpeedParam, relativeDirection.x);
            animator.SetFloat(zSpeedParam, relativeDirection.z);
        }
        else
        {
            moveVal = Vector2.zero;

            animator.SetFloat(xSpeedParam, 0f);
            animator.SetFloat(zSpeedParam, 0f);
        }
    }
}
