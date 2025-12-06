using UnityEngine;

/// <summary>
/// Generic singleton pattern implementation for MonoBehaviour classes.
/// </summary>
/// <typeparam name="T">The type of the singleton class.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Data

    #region Local
    /// <summary>
    /// Static instance of the singleton.
    /// </summary>
    static T instance;
    #endregion

    #region Serialized
    [Header("Singleton")]
    [Tooltip("Should the singleton's instance be carried over to new scenes.")]
    [SerializeField] bool persistent;
    #endregion

    #region Properties
    /// <summary>
    /// Public accessor for the singleton instance.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.transform.SetAsLastSibling();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                    //Debug.LogWarning($"{instance.name} singleton's instance has been auto-generated because it was missing from the current scene!");
                }
            }

            return instance;
        }
    }
    #endregion

    #endregion

    #region Mono
    /// <summary>
    /// Awake method that ensures singleton pattern integrity.
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;

            if (persistent)
                DontDestroyOnLoad(instance);
        }
        else if (instance != this as T)
            Destroy(gameObject);
    }
    #endregion
}