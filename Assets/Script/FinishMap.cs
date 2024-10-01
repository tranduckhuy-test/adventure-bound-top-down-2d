using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Finish map");
            // Save the unlocked maps
            UnlockNewMap();
            // Load the map menu
            SceneManager.LoadScene("MapsPanel");
        }
    }

    private void UnlockNewMap()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("unlockedMaps", PlayerPrefs.GetInt("unlockedMaps", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
