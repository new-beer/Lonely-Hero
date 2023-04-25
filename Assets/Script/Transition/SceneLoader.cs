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

    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO firstLoadScene;

    private GameSceneSO currentSceneLoad;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;

    public float fadeDuration;
    private bool fadeScreen;
    private bool isLoading;
    private void Awake()
    {
        
        currentSceneLoad = firstLoadScene;
        currentSceneLoad.senceReference.LoadSceneAsync(LoadSceneMode.Additive);
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentSceneLoad != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
    }
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
            //TODO:ʵ�ֽ��뽥��
        }
        yield return new WaitForSeconds(fadeDuration);

        //ɾ���ɳ���
        yield return currentSceneLoad.senceReference.UnLoadScene();
        LoadNewScene();
        
    }
    //�����³���
    private void LoadNewScene()
    {
        var loadingOption =   sceneToLoad.senceReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadCompleted;
    }

    /// <summary>
    /// �����������֮��
    /// </summary>
    /// <param name="obj"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        currentSceneLoad = sceneToLoad;

        playerTrans.position = positionToGo;
        if (fadeScreen)
        {
            //TODO
        }


    }
}
