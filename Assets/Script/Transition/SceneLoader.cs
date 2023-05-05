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
    public Vector3 menuPosition;

    [Header("�¼�����")]
    public SceneLoadEventSO loadEventSO;
    public VoidEVentSO newGameEvent;

    [Header("�㲥")]
    public VoidEVentSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unLoadedSceneEvent;
    [Header("����")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
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
        // NewGame();
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad,firstPosition,true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);

    }

    /// <summary>
    /// ���������¼�����
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
            //TODO:ʵ�ֽ��뽥��
            fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);

        //�㲥�¼�����Ѫ����ʾ
        unLoadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);  

        //ɾ���ɳ���
        yield return currentSceneLoad.senceReference.UnLoadScene();

        //�л�����ʱ���ر�����
        playerTrans.gameObject.SetActive(false);
       
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
        //���ͺ��ƶ���������
        playerTrans.position = positionToGo;

        playerTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            //TODO:��͸��
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;

        if(currentSceneLoad.SceneType != SceneType.Menu)
            //����������ɺ��¼�
            afterSceneLoadedEvent.RaiseEvent();
    }
}
