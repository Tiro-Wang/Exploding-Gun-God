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

    //��ʼ��Ϸ
    public void PlayGame()
    {
        //�첽����
        StartCoroutine(LoadingSceneAsync("Level"));
    }
    //�����ʾ����ҳ�湦��
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
    ////��optionDialogֱ����unity�༭���������
    //public void Option()
    //{
    //    optionMenu.SetActive(true);
    //    mainMenu.SetActive(false);
    //}

    //�˳�
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
