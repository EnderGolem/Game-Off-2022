using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

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

    public void LoadScene(string loadableScene, string loadingScene, string[] unloadableScenes = null)
    {
        StartCoroutine(Load(loadableScene, loadingScene, unloadableScenes));
    }

    IEnumerator Load(string loadableScene, string loadingScene, string[] unloadableScenes)
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

        if (unloadableScenes != null)
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
                    f.allowSceneActivation = true;
            yield return null;
        }

        SceneManager.UnloadSceneAsync(loadableScene);
    }
}