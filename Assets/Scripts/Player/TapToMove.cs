using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class TapToMove : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;
    [Header("Move and Rotation Value")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 120f;
    [SerializeField] private bool isUseNavMeshAgent = false;
    [Header("NavMeshAgent Settings")]
    [SerializeField] private float navDestinationDistance = 1.8f;
    [Header("Respawn")]
    public Transform respawnTransform;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction pointAction;
    private InputAction clickAction;
    private VirtualJoystick leftJoystick;
    private VirtualJoystick rightJoystick;
    [SerializeField] private bool isClickNavigation = false;
    [SerializeField] private TMP_Text modeText;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
            lookAction = playerInput.actions["Look"];
            jumpAction = playerInput.actions["Jump"];
            pointAction = playerInput.actions["Point"];
            clickAction = playerInput.actions["Click"];
        }
        else
        {
            Debug.LogError("PlayerInput is missing on this GameObject.");
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb.isKinematic = true;
        GameObject leftJoyObj = GameObject.Find("LeftJoystickBackground");
        if (leftJoyObj != null)
            leftJoystick = leftJoyObj.GetComponent<VirtualJoystick>();
        GameObject rightJoyObj = GameObject.Find("RightJoystickBackground");
        if (rightJoyObj != null)
            rightJoystick = rightJoyObj.GetComponent<VirtualJoystick>();
        agent.updateRotation = isClickNavigation;
        if (modeText != null)
            modeText.text = isClickNavigation ? "Control Mode: Click" : "Control Mode: Joystick";
    }

    void OnEnable()
    {
        moveAction?.Enable();
        lookAction?.Enable();
        jumpAction?.Enable();
        pointAction?.Enable();
        clickAction?.Enable();
    }

    void OnDisable()
    {
        moveAction?.Disable();
        lookAction?.Disable();
        jumpAction?.Disable();
        pointAction?.Disable();
        clickAction?.Disable();
    }

    void FixedUpdate()
    {
        if (!isUseNavMeshAgent)
        {
            rb.isKinematic = false;
            agent.enabled = false;
            Vector2 moveInput = GetMoveInput();
            if (moveInput.magnitude > 0.1f)
            {
                animator.SetFloat("xSpeed", moveInput.x);
                animator.SetFloat("zSpeed", moveInput.y);
                Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
                moveDir = transform.TransformDirection(moveDir);
                moveDir.y = 0f;
                rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                animator.SetFloat("xSpeed", 0f);
                animator.SetFloat("zSpeed", 0f);
            }
            Vector2 lookInput = GetLookInput();
            float horizontalLook = lookInput.x;
            if (Mathf.Abs(horizontalLook) > 0.1f)
            {
                float turnAngle = horizontalLook * turnSpeed * Time.fixedDeltaTime;
                rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAngle, 0f));
            }
            if (jumpAction != null && jumpAction.WasPerformedThisFrame())
            {
                Debug.Log("Jump!");
            }
        }
        else
        {
            agent.enabled = true;
            rb.isKinematic = true;
            if (!isClickNavigation)
            {
                Vector2 moveInput = GetMoveInput();
                animator.SetFloat("xSpeed", moveInput.x);
                animator.SetFloat("zSpeed", moveInput.y);
                if (moveInput.magnitude > 0.1f)
                {
                    Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);
                    moveDir = transform.TransformDirection(moveDir);
                    moveDir.y = 0f;
                    Vector3 targetPos = transform.position + moveDir.normalized * navDestinationDistance;
                    agent.SetDestination(targetPos);
                }
                else
                {
                    agent.SetDestination(transform.position);
                }
                Vector2 lookInput = GetLookInput();
                float horizontalLook = lookInput.x;
                if (Mathf.Abs(horizontalLook) > 0.1f)
                {
                    float turnAngle = horizontalLook * turnSpeed * Time.fixedDeltaTime;
                    transform.rotation = transform.rotation * Quaternion.Euler(0f, turnAngle, 0f);
                }
                if (jumpAction != null && jumpAction.WasPerformedThisFrame())
                {
                    Debug.Log("Jump with NavMeshAgent!");
                }
            }
            else
            {
                float velocity = agent.velocity.magnitude;
                animator.SetFloat("xSpeed", 0f);
                animator.SetFloat("zSpeed", velocity / moveSpeed);
            }
        }
    }

    void Update()
    {
        if (isUseNavMeshAgent && isClickNavigation && clickAction != null && pointAction != null)
        {
            if (clickAction.WasPerformedThisFrame())
            {
                Vector2 screenPos = pointAction.ReadValue<Vector2>();
                Ray ray = Camera.main.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    agent.SetDestination(hit.point);
                    //Debug.Log("Set Destination to " + hit.point);
                    //Debug.Log($"Player pos: {transform.position}, Destination: {hit.point}");
                }
            }
        }
    }

    void LateUpdate()
    {
        if (agent.enabled)
        {
            //Debug.Log($"PathPending={agent.pathPending}, PathStatus={agent.pathStatus}, RemainingDistance={agent.remainingDistance}, isOnNavMesh={agent.isOnNavMesh},UpdateRotation={agent.updateRotation}");
        }
    }

    private Vector2 GetMoveInput()
    {
        Vector2 input = Vector2.zero;
        if (leftJoystick != null && leftJoystick.InputVector.magnitude > 0.1f)
        {
            input = leftJoystick.InputVector;
        }
        else if (moveAction != null)
        {
            input = moveAction.ReadValue<Vector2>();
        }
        return input;
    }

    private Vector2 GetLookInput()
    {
        Vector2 input = Vector2.zero;
        if (rightJoystick != null && rightJoystick.InputVector.magnitude > 0.1f)
        {
            input = rightJoystick.InputVector;
        }
        else if (lookAction != null)
        {
            input = lookAction.ReadValue<Vector2>();
        }
        return input;
    }

    public void Respawn()
    {
        if (respawnTransform != null)
        {
            transform.position = respawnTransform.position;
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void SwitchControlMode(bool clickMode)
    {
        isClickNavigation = clickMode;
        agent.updateRotation = isClickNavigation;
        if (modeText != null)
            modeText.text = isClickNavigation ? "Control Mode: Click" : "Control Mode: Joystick";
        if (leftJoystick != null)
            leftJoystick.gameObject.SetActive(!isClickNavigation);
        if (rightJoystick != null)
            rightJoystick.gameObject.SetActive(!isClickNavigation);
    }

    public void OnClickToggle()
    {
        SwitchControlMode(!isClickNavigation);
    }
}
