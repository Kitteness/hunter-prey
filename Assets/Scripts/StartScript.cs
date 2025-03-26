using UnityEngine;
using UnityEngine.SceneManagement;

//This script is used on the start menu
public class StartScript : MonoBehaviour
{
    //Starts a new game
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
