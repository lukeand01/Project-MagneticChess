using DG.Tweening;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderHandler : MonoBehaviour
{
    [Separator("SCREEN FOR LOADING")]
    [SerializeField] Image background;

    GameHandler handler;
    private void Awake()
    {
        handler = GetComponent<GameHandler>();
    }


    SceneType _sceneType;
    [SerializeField]int currentIndex;

    const int SCENEINDEX_MAINMENU = 0;
    const int SCENEINDEX_LOAD = 1;
    const int SCENEINDEX_GAME = 2;
    public void LoadGame(SceneType _sceneType)
    {

        if(_sceneType == SceneType.None || _sceneType == SceneType.Close)
        {
            return;
        }

        this._sceneType = _sceneType;
        StopAllCoroutines();
        StartCoroutine(LoadSceneProcess(SCENEINDEX_GAME));
    }
    public void LoadMainMenu()
    {
        StopAllCoroutines();
        StartCoroutine(LoadSceneProcess(0));
    }

    IEnumerator LoadSceneProcess(int index)
    {
        handler._pool_Main.ClearMagnets();


        yield return StartCoroutine(LowerCurtainProcess());

        yield return StartCoroutine(LoadProcess(index));

        SceneOrder(_sceneType);

        yield return StartCoroutine(RaiseCurtainProcess());

    }

    void SceneOrder(SceneType _scene)
    {
        switch(_sceneType)
        {
            case SceneType.None:
                break;
            case SceneType.Singleplayer:
                //we inform it to start the single player
                handler._singlePlayer.StartSinglePlayer();
                break;
            case SceneType.Against_AI:

                break;
        }
    }


    IEnumerator LoadProcess(int index)
    {
        AsyncOperation emptyAsync = SceneManager.LoadSceneAsync(SCENEINDEX_LOAD, LoadSceneMode.Additive); //this is just empty.

        yield return new WaitUntil(() => emptyAsync.isDone);


        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(currentIndex, UnloadSceneOptions.None);

        yield return new WaitUntil(() => unloadAsync.isDone);

        //yield break;

        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        yield return new WaitUntil(() => loadAsync.isDone);

        AsyncOperation unloadEmptyAsync = SceneManager.UnloadSceneAsync(SCENEINDEX_LOAD); //this is just empty.

        yield return new WaitUntil(() => unloadEmptyAsync.isDone);


        yield return new WaitUntil(() => GameHandler.instance != null);


        currentIndex = index;
    }



    //make the screen dark
    IEnumerator LowerCurtainProcess()
    {
        float duration = 0.15f;

        background.DOKill();

        background.gameObject.SetActive(true);
        background.DOFade(0, 0).SetUpdate(true).SetEase(Ease.Linear);
        
        background.DOFade(1, duration).SetUpdate(true).SetEase(Ease.Linear);
        yield return new WaitForSeconds(duration);

    }


    //make the screen 
    IEnumerator RaiseCurtainProcess()
    {
        background.DOKill();

        float duration = 0.15f;
        background.DOFade(0, duration).SetUpdate(true).SetEase(Ease.Linear);
        yield return new WaitForSeconds(duration);
        background.gameObject.SetActive(false);
    }


}

public enum SceneType
{
    None,
    Close,
    Singleplayer,
    Against_AI,
    Multiplayer_Local,
    Multiplayer_Server
}
