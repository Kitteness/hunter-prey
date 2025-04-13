using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Collider))]
public class TaskStation : MonoBehaviour
{
    public static Vector3 currentNoiseLocation = Vector3.positiveInfinity;
    public bool hasNoise = false;

    [Header("Task Configuration")]
    public string taskName = "SwitchTask";
    public bool requireKey = false;
    public string requiredKeyID = "KeyCard_Default";
    public float taskDuration = 3f;
    public float interruptRange = 5f;
    public DoorController doorToUnlock;

    [Header("UI & Audio")]
    public GameObject progressBarUI;
    public Slider taskProgressSlider;
    public AudioSource taskAudio;
    public AudioClip taskCompleteSfx;

    private float currentProgress = 0f;
    private bool isCompleted = false;
    private bool isPlayerInRange = false;

    private PlayerInventory playerInventory;
    private Transform playerTransform;
    private PlayerInput playerInput;
    private InputAction interactAction;

    void Start()
    {
        if (progressBarUI) progressBarUI.SetActive(false);
    }

    void Update()
    {
        if (isCompleted) return;
        if (!isPlayerInRange || interactAction == null) return;

        if (requireKey && playerInventory != null)
        {
            if (!playerInventory.HasItem(requiredKeyID))
            {
                return;
            }
        }

        if (interactAction.IsPressed())
        {
            currentNoiseLocation = transform.position;
            hasNoise = true;

            if (progressBarUI) progressBarUI.SetActive(true);

            currentProgress += Time.deltaTime;

            if (taskProgressSlider != null)
            {
                taskProgressSlider.value = currentProgress / taskDuration;
            }

            if (taskAudio != null && !taskAudio.isPlaying)
            {
                taskAudio.Play();
            }

            if (CheckAIInRange())
            {
                currentProgress = 0f;
                if (taskAudio != null) taskAudio.Stop();
                return;
            }

            if (currentProgress >= taskDuration)
            {
                OnTaskCompleted();
            }
        }
        else
        {
            if (currentProgress > 0f && !isCompleted)
            {
                currentProgress = 0f;
                if (taskAudio != null) taskAudio.Stop();
            }

            currentNoiseLocation = Vector3.positiveInfinity;
            hasNoise = false;
        }
    }

    private void OnTaskCompleted()
    {
        isCompleted = true;
        currentProgress = taskDuration;

        if (taskProgressSlider != null)
            taskProgressSlider.value = 1f;

        if (taskAudio != null) taskAudio.Stop();
        if (taskCompleteSfx != null)
            AudioSource.PlayClipAtPoint(taskCompleteSfx, transform.position);

        if (progressBarUI) progressBarUI.SetActive(false);

        Debug.Log($"{taskName} Complete!");

        currentNoiseLocation = Vector3.positiveInfinity;
        hasNoise = false;

        if (doorToUnlock != null)
        {
            doorToUnlock.OpenDoor();
        }

    }

    private bool CheckAIInRange()
    {
        GameObject[] hunters = GameObject.FindGameObjectsWithTag("Hunter");
        foreach (GameObject hunter in hunters)
        {
            float dist = Vector3.Distance(hunter.transform.position, transform.position);
            if (dist < interruptRange)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerTransform = other.transform;
            playerInventory = other.GetComponent<PlayerInventory>();

            var player = other.gameObject;
            playerInput = player.GetComponent<PlayerInput>();

            if (playerInput != null)
            {
                interactAction = playerInput.actions["Interact"];
            }

            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerTransform = null;
            playerInventory = null;
            currentProgress = 0f;
            if (progressBarUI) progressBarUI.SetActive(false);
            if (taskAudio != null) taskAudio.Stop();
        }

        currentNoiseLocation = Vector3.positiveInfinity;
        hasNoise = false;
    }
}
