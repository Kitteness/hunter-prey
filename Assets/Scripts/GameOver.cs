using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private AdsManager AdsManager;

    private void Start()
    {
        AdsManager.ShowAd("Interstitial");
        AnalyticsManager.Instance.adViewEvent("Interstitial");
    }

    //Return to gameplay.
    public void Retry()
    {
        SceneManager.LoadScene("Level 1");
    }

    //Exit the game, returning to the main menu
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
