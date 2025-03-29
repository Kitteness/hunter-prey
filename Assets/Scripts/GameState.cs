using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goal;
    [SerializeField] private GameObject uiMessage;
    [SerializeField] private TextMeshProUGUI uiMessageText;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;
<<<<<<< Updated upstream
=======
    [SerializeField] private GameObject beanieButton;
    [SerializeField] private GameObject beanie;
    [SerializeField] private AudioManager audioManager;
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
>>>>>>> Stashed changes

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2)
        {
            Capture();
        }
        else if (Vector3.Distance(goal.transform.position, player.transform.position) < 2)
        {
            GoalReached();
        }
    }

    //Pause the game, bringing up the pause menu.
    public void PauseGame()
    {
        pauseScreen.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    //Unpause the game, closing the pause menu
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    private void Capture()
    {
        uiMessageText.text = $"Captured!";
<<<<<<< Updated upstream
        StartCoroutine(GameOver());
=======
        lifeManager.LoseLife();
        audioManager.PlaySFX(audioManager.damage);
        if (lifeManager.totalLives > 0)
        {
            StartCoroutine(RestartScene());
        }
        else
        {
            StartCoroutine(GameOver());
        }
>>>>>>> Stashed changes
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
}
