using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject uiMessage;
    [SerializeField] private TextMeshProUGUI uiMessageText;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject beanieButton;
    [SerializeField] private GameObject beanie;
    private LifeManager lifeManager;
    private bool captured = false;

    private void Start()
    {
        lifeManager = player.GetComponent<LifeManager>();
        if (!PlayerPrefs.HasKey("BeaniePurchased"))
        {
            PlayerPrefs.SetInt("BeaniePurchased", 0);
        }
        if (PlayerPrefs.GetInt("BeaniePurchased") == 1)
        {
            beanie.SetActive(true);
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("BeaniePurchased") == 1)
        {
            beanieButton.SetActive(false);
        }
        if (Vector3.Distance(transform.position, player.transform.position) < 2 && captured == false)
        {
            captured = true;
            Capture();
        }
        else if (Vector3.Distance(goal.transform.position, player.transform.position) < 2 && captured == false)
        {
            GoalReached();
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
        uiMessageText.text = $"Success!";
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(2);

        uiMessage.SetActive(false);
    }

    IEnumerator GameOver()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Game Over");
    }

    IEnumerator RestartScene()
    {
        uiMessage.SetActive(true);

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
