using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void Start()
    {
        var loadedScores = XMLManager.instance.LoadScores();
        int currentNumberOfScores = loadedScores.Count;
        print("currentNumberOfScores=" + currentNumberOfScores);
        if (currentNumberOfScores > 0 && GameControl.control.PlayerHighScoreList.Count==0)
        {
            for (int i = 0; i < currentNumberOfScores; i++)
            {
                GameControl.control.PlayerHighScoreList.Add(loadedScores[i]);
            }
        }
        if (currentNumberOfScores < 4 && GameControl.control.PlayerHighScoreList.Count == 0)
        {
            for (int j = currentNumberOfScores; j <= 4; j++)
            {
                GameControl.control.PlayerHighScoreList.Add(0);
            }
        }


        GameControl.control.PlayerHighestScore= GameControl.control.PlayerHighScoreList.Max(x => x);
    }

    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().PlaySound("ClickSound");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void ScoreList()
    {
        FindObjectOfType<AudioManager>().PlaySound("ClickSound");
        SceneManager.LoadScene("ShowResults");
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        FindObjectOfType<AudioManager>().PlaySound("ClickSound");
        XMLManager.instance.SaveScores(GameControl.control.PlayerHighScoreList);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
