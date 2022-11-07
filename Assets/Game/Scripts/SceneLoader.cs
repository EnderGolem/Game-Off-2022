using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    public string[] unloadableScenes;
    public string loadableScene;
    public string loadingScene;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Debug.Log("Был создан еще один   класс синглотона, удален " + gameObject.name);
        Destroy(gameObject);
    }

    public void LoadScene()
    {
        LoadScene(unloadableScenes, loadableScene, loadingScene);
    }

    public void LoadScene(string[] unloadableScenes, string loadableScene, string loadingScene)
    {
        StartCoroutine(Load(unloadableScenes,  loadableScene,  loadingScene));
    }

    IEnumerator Load(string[] unloadableScenes, string loadableScene, string loadingScene)
    {
        yield return LoadableAscync(unloadableScenes, loadableScene);
        yield return LoadingAscync(loadableScene, loadingScene);
    }
    IEnumerator LoadableAscync(string[] unloadableScenes, string loadableScene)
    {
        var loadSceneAsync = SceneManager.LoadSceneAsync(loadableScene);
        while (!loadSceneAsync.isDone)
        {
            yield return null;
        }
        foreach (var unloadableScene in unloadableScenes)
        {
            SceneManager.UnloadSceneAsync(unloadableScene);
        }
    }

    IEnumerator LoadingAscync(string loadableScene, string loadingScene)
    {
        var f = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);
        f.allowSceneActivation = false;
        while (!f.isDone)
        {
            if (f.progress >= 0.9f && !f.allowSceneActivation)
                if (Input.anyKeyDown)
                {
                    f.allowSceneActivation = true;
                }

            yield return null;
        }
        SceneManager.UnloadSceneAsync(loadableScene);
    }
}