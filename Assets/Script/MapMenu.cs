using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapMenu : MonoBehaviour
{
    public Button[] mapButtons;

    private void Awake()
    {
        int unlockedMaps = PlayerPrefs.GetInt("unlockedMaps", 1);
        for (int i = 0; i < mapButtons.Length; i++)
        {
            mapButtons[i].interactable = i < unlockedMaps;
        }
    }

    public void OpenMap(string mapName)
    {
        Debug.Log("Open map: " + mapName);
        SceneManager.LoadScene(mapName);
    }
}
