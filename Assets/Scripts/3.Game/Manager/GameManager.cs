using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static readonly object lockObject = new object();

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        GameObject singleton = new GameObject("GameManager");
                        instance = singleton.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }
    }

    private bool isEnd = false;
    public event Action onEndStage;

    private bool isPaused = false;
    public event Action<bool> onPauseGame;

    private float playTime = 0;
    public Action<int> onUpdatePlayTime;

    private int gainedGold = 0;
    public Action<int> onUpdateGainGold;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        // UI Scene 불러오기
        SceneManager.LoadScene("InGameUI", LoadSceneMode.Additive);
        
        // BGM 바꾸기
        AudioManager.Instance.ChangeBGM(AudioManager.Bgm.Forest);

        // Pool 관련 초기화
        PoolManager.Instance.InitPoolManager();
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            Destroy(gameObject);
            instance = null;
        }
    }

    private void FixedUpdate() 
    {
        if(isEnd)
        {
            return;
        }

        playTime += Time.fixedDeltaTime;
        onUpdatePlayTime?.Invoke(Mathf.FloorToInt(playTime));
    }

    public void HandlePauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            onPauseGame?.Invoke(false);
            AudioManager.Instance.PauseBGM(false);
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            onPauseGame?.Invoke(true);
            AudioManager.Instance.PauseBGM(true);
        }
    }

    public void GainGold(int gold)
    {
        gainedGold += gold;
        onUpdateGainGold?.Invoke(gainedGold);
    }

    public void ClearStage(bool isClear)
    {
        // 게임 중지
        isEnd = true;

        // BGM 스탑
        AudioManager.Instance.PauseBGM(true);

        // UnitController 정지
        // PlayerController 정지
        onEndStage?.Invoke();
        
        // 클리어 등급 결정
        var stageInfo = DataManager.Instance.GetCurrentStageInfo();
        int clearGrade = 0;

        if(isClear)
        {
            Debug.Log(stageInfo.timeLimit1 + " : " + stageInfo.timeLimit2 + " : " + playTime);

            clearGrade = 1;
            if(playTime <= stageInfo.timeLimit1)
            {
                clearGrade = 3;
            }
            else if(playTime <= stageInfo.timeLimit2)
            {
                clearGrade = 2;
            }

            // 스테이지 클리어 정보 저장
            PlayerDataManager.SaveStageClear(DataManager.Instance.currentStageId, clearGrade);
        }

        // 골드 획득 저장
        PlayerDataManager.AddCurrency(gainedGold);

        // 결과 팝업
        Observer.Instance.RaiseStageResultPopup(isClear, Mathf.RoundToInt(playTime), gainedGold, clearGrade);
    }
}