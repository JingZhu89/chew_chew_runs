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
        print("endgame triggered");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        gameOver.SetTrigger("GameOver");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);

    }

}
