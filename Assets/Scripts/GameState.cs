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
        StartCoroutine(GameOver());
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
