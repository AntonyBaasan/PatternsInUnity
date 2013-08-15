using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class PoolCreatePanel : EditorWindow {
	
	private string objectName = "PoolObject1";
	private Vector3 objectPosition = new Vector3(-1000,-1000,-1000);
	private PoolObject[] objectList;
	
	[MenuItem("Pattern/Patr")]
	public static void OpenPanel()
	{
		EditorWindow.GetWindow(typeof(PoolCreatePanel));
	}
	
	// Update is called once per frame
	void OnGUI () {
//		EditorGUILayout.LabelField("Your name" ,objectName);
		GUILayout.BeginVertical();
		
		//Enter the name
		objectName = EditorGUILayout.TextField("Name" ,objectName);
		//Enter init position
		objectPosition = EditorGUILayout.Vector3Field("Init position",objectPosition);
		//Button for creation
		CreateButton();
		//All object list
		AllObjectList();
		
		GUILayout.EndVertical();		
		
	}
	void OnHierarchyChange() {
		objectList = GameObject.FindObjectsOfType(typeof(PoolObject)) as PoolObject[];
	}
	
	
	
	private void AllObjectList()
	{
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		if(objectList != null)
		{
			
			int i=1;
			foreach(PoolObject o in objectList)
			{
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Pool-"+i ,o.name);
				if(GUILayout.Button("Select"))
				{
					Selection.objects = new GameObject[] { o.gameObject };
				}
				GUILayout.EndHorizontal();
				i++;
			}
		}
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();		
	}
	
	private void CreateButton()
	{
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("Create", GUILayout.Width(50)))
		{
			if(objectName == null || objectName.Equals(""))
			{
				this.ShowNotification(new GUIContent("Enter the name!"));
			}
			else
			{
				string[] dirs = Directory.GetDirectories("Assets","PatternTools"); //Get tool folder
				Debug.Log("dirs[0]: "+dirs[0]);
				Debug.Log("111: "+dirs[0].Replace(@"\","/")+"/Prefabs/PoolObject.prefab");
				
//				Object prefab = AssetDatabase.LoadAssetAtPath(dirs[0]+"/Prefabs/PoolObject.prefab", typeof(GameObject));
				Object prefab = AssetDatabase.LoadAssetAtPath("Assets/PatternTools/Prefabs/PoolObject.prefab", typeof(GameObject));
				GameObject clone = Instantiate(prefab, objectPosition, Quaternion.identity) as GameObject;
				clone.name = objectName;
					
				this.ShowNotification(new GUIContent("PoolObject \""+objectName+"\" created!"));
			}
		}
		
		GUILayout.EndHorizontal();
	}
}
