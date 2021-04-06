/*
 * Copyright 2021 Taeyoon Rim
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. 
 */

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
