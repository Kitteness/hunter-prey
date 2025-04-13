using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject checkpoint;
    [SerializeField] private GameObject uiMessage;
    [SerializeField] private TextMeshProUGUI uiMessageText;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject beanieButton;
    [SerializeField] private GameObject beanie;
    private GameObject[] hunters;
    private LifeManager lifeManager;
    private bool captured = false;
    private bool goalReached = false;
    private static bool checkpointSet = false;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        hunters = GameObject.FindGameObjectsWithTag("Hunter");
        lifeManager = player.GetComponent<LifeManager>();
        if (!PlayerPrefs.HasKey("BeaniePurchased"))
        {
            PlayerPrefs.SetInt("BeaniePurchased", 0);
        }
        if (PlayerPrefs.GetInt("BeaniePurchased") == 1)
        {
            beanie.SetActive(true);
        }
        if (checkpointSet == true)
        {
            player.transform.position = checkpoint.transform.position;
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("BeaniePurchased") == 1)
        {
            beanieButton.SetActive(false);
        }
        for (int i = 0; i < hunters.Length; i++)
        {
            if (Vector3.Distance(hunters[i].transform.position, player.transform.position) < 2 && captured == false)
            {
                captured = true;
                Capture();
            }
        }
        if (Vector3.Distance(goal.transform.position, player.transform.position) < 2 && captured == false && !goalReached)
        {
            GoalReached();
        }
        else if (Vector3.Distance(checkpoint.transform.position, player.transform.position) < 2 && captured == false && checkpointSet == false)
        {
            CheckpointReached();
        }
    }

    //Pause the game, bringing up the pause menu.
    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        pauseButton.SetActive(false);
        if (PlayerPrefs.GetInt("BeaniePurchased") == 0)
        {
            beanieButton.SetActive(true);
        }
        Time.timeScale = 0;
    }

    //Unpause the game, closing the pause menu
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
        beanieButton.SetActive(false);
        Time.timeScale = 1;
    }

    private void Capture()
    {
        uiMessageText.text = $"Captured!";
        lifeManager.LoseLife();
        if (lifeManager.totalLives > 0)
        {
            StartCoroutine(RestartScene());
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    private void GoalReached()
    {
        goalReached = true;

        audioManager.PlaySFX(audioManager.goal);

        uiMessageText.text = $"Success!";
        StartCoroutine(DisplayText());
    }

    private void CheckpointReached()
    {
        checkpointSet = true;
        uiMessageText.text = $"Checkpoint!";
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(1);

        uiMessage.SetActive(false);
        goalReached = false;
    }

    IEnumerator GameOver()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(1);

        checkpointSet = false;
        SceneManager.LoadScene("Game Over");
    }

    IEnumerator RestartScene()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
