using UnityEngine;

/// <summary>
///     Handles singleton of type T
///     IMPORTANT: Make sure there is an instance of the type T in the same scene as from the scene the Instance is called
/// </summary>
/// <typeparam name="T">Type to create a singleton of</typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T _Instance;

    /// <summary>
    ///     An Instance of type <typeparamref name="T" />
    /// </summary>
    public static T Instance
    {
        get
        {
            T[] objects = FindObjectsOfType(typeof(T)) as T[];
            if (objects is { Length: > 0 })
                _Instance = objects[0];
            if (objects is { Length: > 1 })
            {
                int destroyedAmount = 0;
                for (int i = 1; i < objects.Length; i++)
                {
                    Destroy(objects[i].gameObject);
                    destroyedAmount++;
                }
                Debug.LogWarning($"There is more than one {typeof(T).Name} in the scene! Destroyed {destroyedAmount} Instances.");
            }
            if (_Instance == null)
            {
                GameObject obj = new GameObject
                {
                    hideFlags = HideFlags.DontSave,
                    name = $"{typeof(T).Name} (MonoSingleton created)"
                };
                _Instance = obj.AddComponent<T>();
                Debug.LogWarning($"There is no {typeof(T).Name} in the scene. Created a new default {typeof(T).Name}");
            }
            return _Instance;
        }
    }
}