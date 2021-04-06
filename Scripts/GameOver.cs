using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Transform player;

    private GameObject[] platforms;
    private float lowest;

    // disappear animations
    public RuntimeAnimatorController disappearScore;
    public RuntimeAnimatorController disappearHighscore;
    
    public void OnGameOver()
    {
        // save high score
        if (GameObject.Find("HighScore").GetComponent<Text>().color.a != 1)
        {
            DataManager.dataManager.highscore = FindObjectOfType<ScoreManager>().currScore;
            DataManager.dataManager.Save();
        }

        // save final score
        DataManager.dataManager.finalScore = FindObjectOfType<ScoreManager>().currScore;

        // disable camera focus to player
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().enabled = false;

        // anim disappear for score and high score
        GameObject.Find("Score").GetComponent<Animator>().runtimeAnimatorController = disappearScore;
        GameObject.Find("HighScore").GetComponent<Animator>().runtimeAnimatorController = disappearHighscore;

        // load Death
        SceneManager.LoadScene("Death", LoadSceneMode.Additive);
    }

    public void OnPlatformUpdate()
    {
        // find all platform
        platforms = GameObject.FindGameObjectsWithTag("Platforms");

        // find the lowest platform.y
        lowest = platforms[0].transform.position.y;

        for (int i = 1; i < platforms.Length; i++)
        {
            if (lowest > platforms[i].transform.position.y)
            {
                lowest = platforms[i].transform.position.y;
            }
        }
    }

    private void Update()
    {
        // player.y is 1 unit lower than platform.y
        if (lowest > (player.position.y + 3) && !SceneManager.GetSceneByName("Death").isLoaded)
        {
            OnGameOver();
        }
    }
}
