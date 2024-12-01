using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float    transisitonTime = 1;
    public enum SceneIndex {
        MainMenu = 0,
        LoadSaves = 1,
        Intro = 2,
        Stage1 = 3,
        Stage2 = 4,
        Stage3 = 5,
        Stage4 = 6,
    }

    public void LoadNextLevel(SceneIndex index) {
        StartCoroutine(LoadLevelRoutine(index));
    }

    private IEnumerator LoadLevelRoutine(SceneIndex index) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transisitonTime);
        SceneManager.LoadScene((int) index);
    } 
}
