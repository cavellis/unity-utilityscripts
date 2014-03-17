using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
 * The ObjectPool class is used to pool objects. You can pool and retrieve different types of objects
 * in one object pool instance. 
 * 
 * Usage:
 * // create the object pool/add the script to my gameobject
 * ObjectPool objPool = (ObjectPool)gameObject.AddComponent("ObjectPool");
 * 
 * // get a GameObject instance
 * Gameobject monsterInstance = objPool.GetObject("blueMonster");
 * 
 * //add back to pool
 * objPool.ReleaseObject(monsterInstance, "blueMonster");
 * 
 * 
 */
public class ObjectPool : MonoBehaviour 
{
	// my hashtable lookup for pooled objects
	internal Hashtable objectHashtable;

	// Use this for initialization
	void Start () 
	{
		objectHashtable = new Hashtable();
	}
	/**
	 * Checks that the object pool for a certain object type exists, if not creates it
	 * gameObjectType The object type to create the pool for
	 * return void
	 */
	internal void checkHashtable(string gameObjectType)
	{
		if(objectHashtable == null)
		{
			objectHashtable = new Hashtable();
		}
		// if no object of this type has been cached, create the new list
		List<GameObject> objList = (List<GameObject>)objectHashtable[gameObjectType];
		if(objList == null)
		{
			objectHashtable[gameObjectType] = new List<GameObject>();
		}
	}
	/**
	 * Get an object
	 * gameObjectType the path to the resource
	 * returns GameObject instance
	 * 
	 * Example: objectPool.GetObject("redBullet");
	 */
	public GameObject GetObject(string gameObjectType)
	{
		// see if this object pool needs to be created
		checkHashtable(gameObjectType);

		// get the appropriate list
		List<GameObject> objList = (List<GameObject>) objectHashtable[gameObjectType];
		// if there's more than 0 in the list, retrieve it
		if(objList.Count > 0)
		{
			GameObject pooledObject = objList[0];
			objList.RemoveAt(0);
			return pooledObject;
		}
		else
		{
			// nothing in this list, create a new one
			return Instantiate(Resources.Load(gameObjectType)) as GameObject; 
		}
	
	}
	/**
	 * Release a game object back into a specific pool
	 * gameObj The game object instance to add back 
	 * gameObjectType The type of object
	 * return void
	 * 
	 * Example: objectPool.ReleaseObject(bulletGameObjectInstance, "redBullet");
	 */
	public void ReleaseObject(GameObject gameObj, string gameObjectType)
	{
		// see if this object pool needs to be created
		checkHashtable(gameObjectType);
		List<GameObject> objList = (List<GameObject>) objectHashtable[gameObjectType];
		objList.Add(gameObj);
	}
	/**
	 * Clear the pool
	 */
	public void ClearObjectPool(string gameObjectType)
	{
		if(objectHashtable != null)
		{
			List<GameObject> objList = (List<GameObject>)objectHashtable[gameObjectType];
			if(objList != null)
			{
				objectHashtable[gameObjectType] = new List<GameObject>();
			}
			
		}
	}
	/**
	 * Reset the pool
	 */
	public void Reset()
	{
		objectHashtable = new Hashtable();
	}
}
