using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private UI_LoadingBar loadingBar;
    
    public void OnClickNewGameButton(string sceneName) 
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_LoadingScreen");
        loadingBar = go.GetComponentInChildren<UI_LoadingBar>();
     
        GI.Inst.NewGame(EStartGameProgress.NewGame); 
        
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    public void OnClickContinueGameButton()
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_LoadingScreen");
        loadingBar = go.GetComponentInChildren<UI_LoadingBar>();
        
        string sceneName = GI.Inst.LoadGame(EStartGameProgress.LoadGame);
        
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void GoToTitle()
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_LoadingScreen");
        loadingBar = go.GetComponentInChildren<UI_LoadingBar>();
        
        GI.Inst.StartGameProgress = EStartGameProgress.GoToTitle;
        
        StartCoroutine(LoadSceneAsync("Title"));
    }

    public void RequestLoadSceneAsync(string sceneName)
    {
        GameObject go = GI.Inst.ResourceManager.Instantiate("UI_LoadingScreen");
        loadingBar = go.GetComponentInChildren<UI_LoadingBar>();
        
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        
        loadOperation.allowSceneActivation = false;
        float progress = 0;
        while (!loadOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, loadOperation.progress, Time.deltaTime);
            
            loadingBar.SetBar(progress);

            if (progress >= 0.9f)
            {
                loadingBar.SetBar(1f);
                GI.Inst.ResourceManager.Destroy( loadingBar.transform.root.gameObject);
                loadOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    
}
