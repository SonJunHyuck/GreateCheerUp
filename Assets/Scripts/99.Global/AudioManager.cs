using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("AudioManager");
                    instance = singleton.AddComponent<AudioManager>();
                    DontDestroyOnLoad(singleton);
                }
            }

            return instance;
        }
    }

    [Header("BGM Settings")]
    public AudioClip bgmClip;
    private AudioSource bgmPlayer;
    [SerializeField] private float bgmVolume = 1.0f;
    public float BGMVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = value;
            bgmPlayer.volume = value;

            ConfigDataManager.SetBgmVolume(value);
        }
    }


    [Header("SFX Settings")]
    private AudioSource[] sfxPlayers;
    private Queue<AudioSource> sfxQueue;
    public int channelCount = 10;
    [SerializeField] private float sfxVolume = 1.0f;
    public float SFXVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = value;
            for (int i = 0; i < sfxPlayers.Length; i++)
            {
                sfxPlayers[i].volume = value;
            }

            ConfigDataManager.SetSfxVolume(value);
        }
    }


    [Header("Addressable Labels")]
    public string commonSFXLabel = "CommonSFX"; // 자주 사용하는 SFX 라벨

    private Dictionary<Bgm, string> bgmKeys = new Dictionary<Bgm, string>(); // Enum-Key 매핑
    private Dictionary<Sfx, string> sfxKeys = new Dictionary<Sfx, string>(); // Enum-Key 매핑
    private Dictionary<Sfx, AudioClip> audioCache = new Dictionary<Sfx, AudioClip>(); // Enum-Clip 매핑


    public enum Bgm
    {
        Title,
        Ready,
        Forest
    }

    public enum Sfx
    {
        Button,
        Error,
        Success,
        Explosion,
        Arrow,
        Nuke
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        // BGM Player 초기화
        GameObject bgmPlayerObject = new GameObject("BgmPlayer");
        bgmPlayerObject.transform.SetParent(transform);
        bgmPlayer = bgmPlayerObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        // SFX Player 초기화
        GameObject sfxPlayerObject = new GameObject("SfxPlayer");
        sfxPlayerObject.transform.SetParent(transform);
        sfxPlayers = new AudioSource[channelCount];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxPlayerObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }

        sfxQueue = new Queue<AudioSource>(sfxPlayers);

        // BGM Enum-Key 매핑
        MapBgmKeys();

        // SFX Enum-Key 매핑
        MapSfxKeys();

        // 자주 사용하는 SFX 로드
        LoadCommonSFX();
    }

    // Enum과 Addressable Key 매핑
    private void MapBgmKeys()
    {
        bgmKeys[Bgm.Title] = "BgmTitle";
        bgmKeys[Bgm.Ready] = "BgmReady";
        bgmKeys[Bgm.Forest] = "BgmForest";
    }

    // Enum과 Addressable Key 매핑
    private void MapSfxKeys()
    {
        sfxKeys[Sfx.Button] = "SfxButton";
        sfxKeys[Sfx.Success] = "SfxSuccess";
        sfxKeys[Sfx.Error] = "SfxError";

        sfxKeys[Sfx.Explosion] = "SfxExplosion";
        sfxKeys[Sfx.Arrow] = "SfxArrowShoot";
        sfxKeys[Sfx.Nuke] = "SfxNuke";
    }

    // 자주 사용하는 SFX를 라벨을 통해 미리 로드하고 캐싱
    private void LoadCommonSFX()
    {
        Addressables.LoadResourceLocationsAsync(commonSFXLabel).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var location in handle.Result)
                {
                    string key = location.PrimaryKey;

                    // Enum과 Key 매핑 확인 후 로드
                    foreach (var kvp in sfxKeys)
                    {
                        if (kvp.Value == key && !audioCache.ContainsKey(kvp.Key))
                        {
                            Addressables.LoadAssetAsync<AudioClip>(key).Completed += clipHandle =>
                            {
                                if (clipHandle.Status == AsyncOperationStatus.Succeeded)
                                {
                                    audioCache[kvp.Key] = clipHandle.Result;
                                    Debug.Log($"Loaded and cached SFX: {kvp.Key}");
                                }
                                else
                                {
                                    Debug.LogError($"Failed to load SFX: {key}");
                                }
                            };
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"Failed to load SFX locations for label: {commonSFXLabel}");
            }
        };
    }

    // Enum을 사용해 SFX를 재생 (Enum : 오타 방지)
    public void PlaySFX(Sfx sfx)
    {
        if (audioCache.TryGetValue(sfx, out var clip))
        {
            PlaySFXInternal(clip);
        }
        else if (sfxKeys.TryGetValue(sfx, out var key))
        {
            Addressables.LoadAssetAsync<AudioClip>(key).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    audioCache[sfx] = handle.Result;
                    PlaySFXInternal(handle.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load SFX: {key}");
                }
            };
        }
        else
        {
            Debug.LogWarning($"SFX not mapped for: {sfx}");
        }
    }

    // 내부 함수 : AudioSource를 사용해 SFX를 재생
    private void PlaySFXInternal(AudioClip clip)
    {
        if (sfxQueue.Count > 0)
        {
            AudioSource player = sfxQueue.Dequeue();

            if (player.isPlaying)
            {
                player.Stop();
            }

            player.clip = clip;
            player.Play();

            sfxQueue.Enqueue(player);
        }
        else
        {
            Debug.LogWarning("No available AudioSource to play SFX.");
        }
    }

    public void ChangeBGM(Bgm bgm)
    {
        if (bgmKeys.TryGetValue(bgm, out var key))
        {
            Addressables.LoadAssetAsync<AudioClip>(key).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    bgmClip = handle.Result;
                    bgmPlayer.clip = bgmClip;
                    bgmPlayer.time = 0;
                    bgmPlayer.Play();
                }
                else
                {
                    Debug.LogError($"Failed to load BGM: {key}");
                }
            };
        }
        else
        {
            Debug.LogWarning($"BGM not mapped for: {bgm}");
        }
    }

    // BGM 재생/중지
    public void PlayBGM(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }
    
    public void PauseBGM(bool isPause)
    {
        if(isPause)
        {
            bgmPlayer.Pause();
        }
        else
        {
            bgmPlayer.UnPause();
        }
    }
}