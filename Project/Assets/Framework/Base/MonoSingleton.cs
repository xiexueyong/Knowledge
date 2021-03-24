using UnityEngine;

/// <summary>
/// 动态(Dynamic)
/// </summary>
public abstract class D_MonoSingleton<T> : MonoBehaviour where T : D_MonoSingleton<T> 
{
	private static T _instance = null;

	public static T Inst
	{
           get {
            if (_instance == null)
            {
                GameObject go = new GameObject();
                DontDestroyOnLoad(go);
                go.name = "MonoSingleton:" + typeof(T).ToString();
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                go.transform.localScale = Vector3.one;
                _instance = go.AddComponent<T>();
            }
            return _instance;
        }
	}
    private void Awake()
    {
        OnAwake();
    }
    protected virtual void OnAwake()
    {
    }
    private void Start()
    {
        OnStart();
    }
    protected virtual void OnStart()
    {
    }
    private void OnDestroy()
    {
        _instance = null;
    }
}
	
/// <summary>
/// 静态(static)
/// </summary>
public abstract class S_MonoSingleton<T> : MonoBehaviour where T : S_MonoSingleton<T> 
{
	private static T _instance = null;
	public static T Inst {
        get
        {
            return _instance;
        }
    }

	private void Awake()
	{
		if (_instance != null && _instance != (T)this) {
			Destroy (gameObject);
			return;
		}
		GameObject.DontDestroyOnLoad (gameObject);
		_instance = (T)this;
        OnAwake ();
	}

	protected virtual void OnAwake() 
	{
	}
    private void Start()
    {
        OnStart();
    }
    protected virtual void OnStart()
    {
    }
    private void OnDestroy()
    {
        _instance = null;
    }
}


/// <summary>
/// 静态可销毁(static)
/// </summary>
public abstract class S_Destroy_MonoSingleton<T> : MonoBehaviour where T : S_Destroy_MonoSingleton<T>
{
    private static T _instance = null;
    public static T Inst
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != (T)this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = (T)this;
        OnAwake();
    }
  

    protected virtual void OnAwake()
    {
    }
    private void Start()
    {
        OnStart();
    }
    protected virtual void OnStart()
    {
    }
    protected virtual void OnDestroy()
    {
        _instance = null;
    }
    public void DestroyInst()
    {
        Destroy(_instance);
        _instance = null;
    }
}

