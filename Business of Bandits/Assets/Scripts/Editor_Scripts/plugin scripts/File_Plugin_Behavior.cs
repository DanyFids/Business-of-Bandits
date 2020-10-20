using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class File_Plugin_Behavior : MonoBehaviour
{
	private const string PLUGIN_DLL = "BoB-FileHandler";
	private GameObject Map;

	[StructLayout(LayoutKind.Sequential)]
	struct Vector3
	{
		public float x, y, z;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct Transform
	{
		public Vector3 pos, rot, scl;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct Object_Ret{
		public string id;
		public string prefab;
		public Transform transform;
	}

	[DllImport(PLUGIN_DLL)]
	private static extern void New_Map(string name);

	[DllImport(PLUGIN_DLL)]
	private static extern void Save_Map();

	[DllImport(PLUGIN_DLL)]
	private static extern void Load_Map(string name);

	[DllImport(PLUGIN_DLL)]
	private static extern void Get_Map_List(ref string names, ref int num_files);

	[DllImport(PLUGIN_DLL)]
	private static extern void Update_Object(string id, Vector3 pos, Vector3 rot);

	[DllImport(PLUGIN_DLL)]
	private static extern int Get_Num_Objects();

	[DllImport(PLUGIN_DLL)]
	private static extern Object_Ret Get_Object(int id);

	[DllImport(PLUGIN_DLL)]
	private static extern void Create_Object(string id, string prefab, Transform transform);

	public void AddObject(GameObject obj)
	{
		string id = obj.name;
		string pref = obj.GetComponent<prefab_info>().Prefab_Name;

		UnityEngine.Transform transform = obj.transform;
		Transform temp;

		temp.pos.x = transform.position.x;
		temp.pos.y = transform.position.y;
		temp.pos.z = transform.position.z;
		temp.rot.x = transform.localRotation.eulerAngles.x;
		temp.rot.y = transform.localRotation.eulerAngles.y;
		temp.rot.z = transform.localRotation.eulerAngles.z;

		temp.scl.x = 1.0f;
		temp.scl.y = 1.0f;
		temp.scl.z = 1.0f;

		Create_Object(id, pref, temp);
	}

	public void NewMap(string name)
	{
		New_Map(name);

		int current_objs = Map.transform.childCount;
		for (int c = 0; c < current_objs; c++)
		{
			GameObject.Destroy(Map.transform.GetChild(c).gameObject);
		}

		GameObject start_player = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player_Template"));
		start_player.transform.parent = Map.transform;
		start_player.tag = "editor_obj";
		start_player.layer = 10;
		start_player.transform.position += new UnityEngine.Vector3(0.0f, 0.5f, 0.0f);
		start_player.name = Name_Generator.Run_Unique(Map);

		AddObject(start_player);
	}

	public void UpdateMap()
	{

	}

	public void SaveMap()
	{
		Save_Map();
	}

	public void LoadMap(string map_name)
	{

	}

	// Start is called before the first frame update
	void Start()
    {
		Map = GameObject.Find("Map");
		NewMap("untitled");
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
