using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //Return to gameplay.
    public void Retry()
    {
        SceneManager.LoadScene("Gameplay");
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
