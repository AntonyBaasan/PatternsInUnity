using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomEditor(typeof(PoolObject))]
public class PoolObjectInspector : Editor {
	public int prefabCount;
	PoolObject targetObj;
	public override void OnInspectorGUI ()
    {
		PoolObject targetObj = (PoolObject)target;
		UpdateObject(targetObj);
		
		GUILayout.BeginVertical();
		
		targetObj.poolItemCount = EditorGUILayout.IntField("Prefab Count", targetObj.poolItemCount);
		
		ShowEntries(targetObj);
		GUILayout.EndVertical();
        if (GUI.changed)
		{
            EditorUtility.SetDirty (target);
		}
    }
	
	private void UpdateObject(PoolObject targetObj)
	{
		if(targetObj == null)
			return;
		
		if(targetObj.poolItems == null)
		{
			targetObj.poolItems = new PoolEntry[targetObj.poolItemCount];
		}
		if(targetObj.poolItems.Length != targetObj.poolItemCount)
		{
			PoolEntry[] temp = targetObj.poolItems;
			targetObj.poolItems = new PoolEntry[targetObj.poolItemCount];
			if(temp.Length < targetObj.poolItemCount) //increased
				Array.Copy(temp,targetObj.poolItems,temp.Length);
			else//decreased
				Array.Copy(temp,targetObj.poolItems,targetObj.poolItemCount);
		}
	}
	
	private void ShowEntries(PoolObject targetObj)
	{
		GUILayout.BeginVertical();	
		for(int i=0; i<targetObj.poolItems.Length; i++)
		{
			GUILayout.Label("");
//			GUILayout.BeginHorizontal();
			PoolEntry entry;
			if(targetObj.poolItems[i] == null)
				entry = new PoolEntry();
			else
				entry = targetObj.poolItems[i];
			
			Debug.Log("entry.prefab"+entry.prefab);
			
			GUILayout.Label ("Prefab Type "+i+" ("+(entry.prefab==null?"Empty":entry.prefab.name)+")");
				
			entry.prefab = EditorGUILayout.ObjectField("Prefab",entry.prefab, typeof(GameObject), true) as GameObject;
			entry.objectsInPool = EditorGUILayout.IntField("Count", entry.objectsInPool);
			entry.instantiateIfEmpty = EditorGUILayout.Toggle("Instantiate if Null", entry.instantiateIfEmpty);
			
//			GUILayout.EndHorizontal();
			
				
		}
		
		GUILayout.EndVertical();
	}
}
