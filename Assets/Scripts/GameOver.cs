using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public static GameOver SharedInstance;
    public Animator gameOver;
    public float transitionTime = 1f;

    private void Awake()
    {
        SharedInstance = this;
    }

    public void EndGame()
    {
        FindObjectOfType<AudioManager>().PlaySound("GameOver");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        FindObjectOfType<AudioManager>().StopSound("LevelThemeSound");
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        gameOver.SetTrigger("GameOver");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(levelIndex);

    }

}
