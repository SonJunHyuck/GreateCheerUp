using UnityEngine;

public partial class Observer : MonoBehaviour
{
    private static Observer instance;

    public static Observer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("Observer");
                instance = obj.AddComponent<Observer>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            Destroy(gameObject);
            instance = null;
        }
    }
}