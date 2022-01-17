using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Dictionary<string, MainMenuWindow> MenuWindows = new Dictionary<string, MainMenuWindow>();

    public GameObject FadeOutInGame;
    public string startWindow;

    public static MainMenu singleton;

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        var allMenuWindows = GameObject.FindGameObjectsWithTag("MenuWindow");
        foreach (var window in allMenuWindows) {
            var e = window.GetComponent<MainMenuWindow>();
            MenuWindows[e.WindowName] = e;
        }

        if(startWindow != "")
            OpenWindow(startWindow);
    }

    void ChmodWindow(string windowName, bool isOpened, bool isAlt = false) {
        if (isOpened)
        {
            MenuWindows[windowName].Appear(isAlt);
        }
        else
            MenuWindows[windowName].Disappear(isAlt);
    }

    public void OpenWindow(string windowName, bool isAlt = false)
    {
        ChmodWindow(windowName, true, isAlt);
    }

    public void OpenWindow(string windowName) => OpenWindow(windowName, false);
    public void OpenWindowAlt(string windowName) => OpenWindow(windowName, true);

    public void CloseWindow(string windowName, bool isAlt = false)
    {
        ChmodWindow(windowName, false, isAlt);
    }

    public void CloseWindow(string windowName) => CloseWindow(windowName, false);
    public void CloseWindowAlt(string windowName) => CloseWindow(windowName, true);

    public void ExitGame() {
        Application.Quit();
    }

    public void StartGame() {
        StartCoroutine(LoadGame());
    }

    public void LoadScene(int sceneId) {
        SceneManager.LoadScene(sceneId);
    }

    IEnumerator LoadGame() {
        yield return new WaitForSeconds(1);
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
        while (async.progress < 0.8f)
        {
            Debug.Log(async.progress);
            yield return null;
        }

        FadeOutInGame.SetActive(true);
        yield return new WaitForSeconds(0.55f);
        async.allowSceneActivation = true;

        //SceneManager.UnloadSceneAsync(0);
    }

    public void OpenOnly(string windowName) {
        foreach (var e in MenuWindows) {
            if (e.Key != windowName)
            {
                CloseWindow(e.Key);
                e.Value.gameObject.GetComponent<Animator>().SetTrigger("Closed");
            }
            else
            {
                e.Value.IsOpened = false;
                OpenWindow(e.Key);
            }
        }
    }
}
