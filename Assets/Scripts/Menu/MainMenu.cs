using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;

    //开始游戏
    public void PlayGame()
    {
        //异步操作
        StartCoroutine(LoadingSceneAsync("Level"));
    }
    //完成显示加载页面功能
    IEnumerator LoadingSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            loadingBar.value=operation.progress;
            yield return null;
        }
    }
    ////打开optionDialog直接在unity编辑器中完成了
    //public void Option()
    //{
    //    optionMenu.SetActive(true);
    //    mainMenu.SetActive(false);
    //}

    //退出
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
