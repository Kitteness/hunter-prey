using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // ÒýÈë TextMeshPro

public class StartMenu : MonoBehaviour
{
    public TMP_Dropdown levelDropdown; 

    // Strat game, select a level based on dropdown 
    public void Playgame()
    {
        string selectedLevel = levelDropdown.options[levelDropdown.value].text;
        SceneManager.LoadScene(selectedLevel);
    }

    // Quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
