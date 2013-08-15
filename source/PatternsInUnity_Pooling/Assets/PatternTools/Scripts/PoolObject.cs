using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Pool entry. Every prefab type has a list of pooled objects
/// </summary>
/// [Serializable]
[Serializable]
public class PoolEntry
{
	[SerializeField]
	public string prefabName;
	[SerializeField]
	public GameObject prefab; 
	[SerializeField]
	public GameObject[] pooledObjects; 
	[SerializeField]
	public int objectsInPool = 0;
	[SerializeField]
	public bool instantiateIfEmpty = false;
	public int currentObjectsInPool = 0;
}

public class PoolObject : MonoBehaviour {
	public int poolItemCount;
	public PoolEntry[] poolItems;
	
	// Use this for initialization
	void Awake () {
		LoadPool();
	}
	
	private void LoadPool()
	{
		for(int i= 0; i< poolItems.Length; i++)
		{
			poolItems[i].pooledObjects = new GameObject[poolItems[i].objectsInPool];
			
			if(poolItems[i].prefab == null)
				Debug.LogError("Empty prefab pool found. Please set all prefab references");
				
			for(int j= 0; j< poolItems[i].objectsInPool; j++)
			{
				GameObject obj = Instantiate(poolItems[i].prefab,this.transform.position,Quaternion.identity) as GameObject;
				obj.name = poolItems[i].prefab.name;
//				PoolObject(poolItems[i].prefabName, obj);
				
				obj.transform.parent = this.transform;
				obj.SetActive(false);
				poolItems[i].pooledObjects[poolItems[i].currentObjectsInPool++] = obj;
				
			}
		}

//		for(int i= 0; i< prefabTypes.Length; i++)
//		{
//			Debug.LogWarning(":::: poolItems["+i+"].prefabName: "+poolItems[i].prefabName+", poolItems["+i+"].objectsInPool: "+poolItems[i].objectsInPool);
//			
//			for(int j= 0; j< poolItemCount; j++)
//			{
//				Debug.LogWarning("++++++ poolItems["+j+"].pooledObjects: "+poolItems[i].pooledObjects[j].transform.name);
//			}
//		}
	
	
	}
	public GameObject GetObjectByName(string prefabName)
	{
		Debug.LogWarning("*********GetObjectByName("+prefabName+")");
		foreach(PoolEntry item in poolItems)
		{
			if(!item.prefabName.Equals(prefabName))
				continue;
				
			if(item.pooledObjects.Length > 0 && item.currentObjectsInPool > 0)
			{
				GameObject obj = item.pooledObjects[--item.currentObjectsInPool];
				obj.SetActive(true);
				obj.transform.parent = null;
				
				Debug.LogWarning("++++++ item.prefabName: "+item.prefabName+", prefabName:"+prefabName+", obj:"+obj.name+", item.currentObjectsInPool:"+item.currentObjectsInPool);
				
				return obj;
			}
		}
		
		return null;
	}
	
	public void PutObjectIntoPool(string prefabName, GameObject obj)
	{
		Debug.LogWarning ("*******Start PoolObject("+prefabName+", "+obj+")");
		foreach(PoolEntry item in poolItems)
		{
			Debug.LogWarning("---- item.prefabName: "+item.prefabName+", prefabName:"+prefabName+", item.currentObjectsInPool:"+item.currentObjectsInPool+", item.pooledObjects.Length:"+item.pooledObjects.Length);
			if(item.prefabName.Equals(prefabName) && item.currentObjectsInPool<item.pooledObjects.Length)
			{
				Debug.LogWarning("---- Pooling Done ");
				obj.transform.parent = this.transform;
				obj.SetActive(false);
				item.pooledObjects[item.currentObjectsInPool++] = obj;
				return;
			}
		}
		
		Debug.LogError("Something wrong in: PoolObject, prefabName:"+prefabName);
		//if we are here, it means we didn't put obj into the pool. Have to destroy
		Destroy(obj);

	}
}