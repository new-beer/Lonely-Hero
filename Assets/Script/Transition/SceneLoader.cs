using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans;
    public Vector3 firstPosition;

    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;

    [Header("广播")]
    public VoidEVentSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;

    private GameSceneSO currentSceneLoad;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;

    public float fadeDuration;
    private bool fadeScreen;
    private bool isLoading;

    private void Awake()
    {
        
        //currentSceneLoad = firstLoadScene;
        //currentSceneLoad.senceReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        NewGame();
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        OnLoadRequestEvent(sceneToLoad,firstPosition,true);
    }

    /// <summary>
    /// 场景加载事件请求
    /// </summary>
    /// <param name="locationToLoad"></param>
    /// <param name="posToGo"></param>
    /// <param name="fadeScreen"></param>
    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentSceneLoad != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            //TODO:实现渐入渐出
            fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        //删除旧场景
        yield return currentSceneLoad.senceReference.UnLoadScene();

        //切换场景时，关闭人物
        playerTrans.gameObject.SetActive(false);
       
        LoadNewScene();
        
    }
    //加载新场景
    private void LoadNewScene()
    {
        var loadingOption =   sceneToLoad.senceReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadCompleted;
    }

    /// <summary>
    /// 场景加载完成之后
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {

        currentSceneLoad = sceneToLoad;
        //传送后移动人物坐标
        playerTrans.position = positionToGo;

        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            //TODO:逐渐透明
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        //场景加载完成后事件
        afterSceneLoadedEvent.RaiseEvent();
    }
}
