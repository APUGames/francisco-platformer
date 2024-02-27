using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exits : MonoBehaviour
{
    [SerializeField]float LevelLoadDelay = 2.0f;
    [SerializeField] float slowMoFactor = 0.2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        Time.timeScale = slowMoFactor;
        yield return new WaitForSecondsRealtime(LevelLoadDelay);

        yield return new WaitForSeconds(LevelLoadDelay);
        
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
