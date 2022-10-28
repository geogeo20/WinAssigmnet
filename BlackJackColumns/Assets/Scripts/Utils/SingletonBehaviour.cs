using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
	public static T Instance { get; protected set; }

	public static bool InstanceExists => Instance != null;

	protected virtual void Awake()
	{
		if (InstanceExists)
		{
			if (Application.isPlaying)
			{
				Destroy(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}
		else
		{
			Instance = (T)this;
		}
	}

	protected virtual void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}
}