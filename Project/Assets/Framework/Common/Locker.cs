using System.Collections.Generic;

public class Locker<T>
{
	List<T> lockKeys = new List<T>();

	public void Lock(T key)
	{
		lockKeys.Add (key);
	}

	public void UnLock(T key)
	{
		lockKeys.Remove (key);
	}

	public bool IsLock(T key)
	{
		return lockKeys.Contains (key);
	}
}
