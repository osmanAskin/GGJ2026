using UnityEngine;
                
                public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
                {
                    public static T Instance
                    {
                        get
                        {
                            if (!instance)
                            {
                                instance = FindAnyObjectByType<T>();
                
                                if (!instance)
                                {
                                    GameObject go = new GameObject(typeof(T).ToString());
                                    instance = go.AddComponent<T>();
                                }
                            }
                
                            return instance;
                        }
                    }
                
                    [SerializeField]
                    private bool dontDestroyOnLoad;
                
                    private static T instance;
                
                    protected void Awake()
                    {
                        if (Instance != this)
                        {
                            Destroy(this);
                        }
                        else if (Instance != null)
                        {
                            if (dontDestroyOnLoad)
                            {
                                transform.SetParent(null);
                                DontDestroyOnLoad(gameObject);
                            }
                
                            ChildAwake();
                        }
                    }
                
                    protected virtual void ChildAwake() { }
                }