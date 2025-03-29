using TMPro;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalLivesText;

    public int totalLives;


    private void Start()
    {
        if (!PlayerPrefs.HasKey("totalLives") || PlayerPrefs.GetInt("totalLives") < 1)
        {
            PlayerPrefs.SetInt("totalLives", 3);
        }
        totalLives = PlayerPrefs.GetInt("totalLives");
        totalLivesText.text = $"Lives: {totalLives}";
    }

    public void GainLife(int livesToGain)
    {
        totalLives = totalLives + livesToGain;
        PlayerPrefs.SetInt("totalLives", totalLives);
        totalLivesText.text = $"Lives: {totalLives}";
    }

    public void LoseLife()
    {
        totalLives = totalLives - 1;
        PlayerPrefs.SetInt("totalLives", totalLives);
        totalLivesText.text = $"Lives: {totalLives}";
    }
}
