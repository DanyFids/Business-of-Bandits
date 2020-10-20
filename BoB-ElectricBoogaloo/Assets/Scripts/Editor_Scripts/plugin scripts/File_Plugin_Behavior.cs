using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class File_Plugin_Behavior : MonoBehaviour
{
	private const string PLUGIN_DLL = "BoB-FileHandler";
	private GameObject Map;

	public GameObject load_menu;
	private string load_map_selected = "";
	private int cur_load_page = 0;
	private int num_load_pages = 0;
	private const int NUM_DISPLAYED_MAPS = 18;

	public GameObject save_menu;
	public GameObject new_menu;

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
	private static extern System.IntPtr Get_Map_List(ref int num_files);


	[DllImport(PLUGIN_DLL)]
	private static extern void Update_Object(string id, Vector3 pos, Vector3 rot);

	[DllImport(PLUGIN_DLL)]
	private static extern int Get_Num_Objects();

	[DllImport(PLUGIN_DLL)]
	private static extern Object_Ret Get_Object(int id);

	[DllImport(PLUGIN_DLL)]
	private static extern void Create_Object(string id, string prefab, Transform transform);

	[DllImport(PLUGIN_DLL)]
	private static extern string Get_Map_Name();

	[DllImport(PLUGIN_DLL)]
	private static extern void Set_Map_Name(string name);

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

		ClearMap();

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
		for (int c = 0; c < Map.transform.childCount; c++)
		{
			GameObject tmp = Map.transform.GetChild(c).gameObject;
			string id = tmp.name;

			Vector3 pos, rot;

			pos.x = tmp.transform.position.x;
			pos.y = tmp.transform.position.y;
			pos.z = tmp.transform.position.z;

			rot.x = tmp.transform.localEulerAngles.x;
			rot.y = tmp.transform.localEulerAngles.y;
			rot.z = tmp.transform.localEulerAngles.z;

			Update_Object(id, pos, rot);
		}
	}

	public void SaveMap()
	{
		if (Get_Map_Name() != "")
		{
			UpdateMap();
			Save_Map();
		}
		else
			DisplaySavePrompt(true);

	}

	public void Save_Prompt(TMP_InputField input)
	{
		if (input.text != "")
		{
			Set_Map_Name(input.text);

			UpdateMap();
			Save_Map();

			save_menu.SetActive(false);
		}
	}

	public void DisplayNewPrompt(bool t) {
		new_menu.SetActive(t);
	}

	public void New_Prompt(TMP_InputField input)
	{
		if (input.text != "")
		{
			NewMap(input.text);

			new_menu.SetActive(false);
		}
	}

	public void DisplaySavePrompt(bool t)
	{
		save_menu.SetActive(t);
	}

	private void ClearMap()
	{
		// clear the current Map Unity Side
		int current_objs = Map.transform.childCount;
		for (int c = 0; c < current_objs; c++)
		{
			GameObject.Destroy(Map.transform.GetChild(c).gameObject);
		}
	}

	public void LoadSelectedMap()
	{
		if (load_map_selected != "")
		{
			ClearMap();

			Load_Map(load_map_selected);
			int num_obj = Get_Num_Objects();

			for (int c = 0; c < num_obj; c++)
			{
				Object_Ret tmp = Get_Object(c);

				GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + tmp.prefab));
				obj.transform.parent = Map.transform;
				obj.tag = "editor_obj";
				obj.layer = 10;
				obj.transform.position = new UnityEngine.Vector3(tmp.transform.pos.x, tmp.transform.pos.y, tmp.transform.pos.z);
				obj.transform.localRotation = Quaternion.Euler(tmp.transform.rot.x, tmp.transform.rot.y, tmp.transform.rot.z);
				obj.name = tmp.id;
			}

			load_map_selected = "";
			DisplayLoadMenu(false);
		}
	}

	public void SelectMapToLoad(string map_name)
	{
		load_map_selected = map_name;
	}

	public void DisplayLoadMenu(bool t)
	{
		if (t)
		{
			string raw_map_list = "";
			int num_maps = 0;

			raw_map_list = Marshal.PtrToStringAnsi(Get_Map_List(ref num_maps));

			string[] files = raw_map_list.Split(';');

			GameObject file_list = load_menu.transform.Find("Panel/File_List").gameObject;

			for (int c = 0; c < NUM_DISPLAYED_MAPS; c++)
			{
				GameObject file_btn = file_list.transform.Find("File" + (c + 1).ToString()).gameObject;

				if (c < num_maps)
				{
					file_btn.SetActive(true);
					file_btn.GetComponent<Button>().interactable = true;
					file_btn.GetComponent<Select_File_Load_Btn>().SetFile(files[c].Split('.')[0]);
				}
				else
				{
					file_btn.SetActive(false);
				}
			}

			cur_load_page = 0;
			num_load_pages = (num_maps / NUM_DISPLAYED_MAPS) + 1;

			file_list.transform.Find("prev_pg").gameObject.GetComponent<Button>().interactable = false;

			if (num_load_pages > 1)
				file_list.transform.Find("next_pg").gameObject.GetComponent<Button>().interactable = true;
			else
				file_list.transform.Find("next_pg").gameObject.GetComponent<Button>().interactable = false;
		}

		SetLoadBtn(false);

		load_menu.SetActive(t);
	}

	public void SetLoadBtn(bool t)
	{
		load_menu.transform.Find("load_btn").GetComponent<Button>().interactable = t;
	}

	public void ChangeLoadPg(bool frwd)
	{
		if (frwd)
			cur_load_page++;
		else
			cur_load_page--;

		string raw_map_list = "";
		int num_maps = 0;

		raw_map_list = Marshal.PtrToStringAnsi(Get_Map_List(ref num_maps));

		string[] files = raw_map_list.Split(';');

		GameObject file_list = load_menu.transform.Find("Panel/File_List").gameObject;

		for (int c = 0; c < NUM_DISPLAYED_MAPS; c++)
		{
			GameObject file_btn = file_list.transform.Find("File" + (c + 1).ToString()).gameObject;

			int rl_file_id = (NUM_DISPLAYED_MAPS * cur_load_page) + c;

			if (rl_file_id < num_maps)
			{
				file_btn.SetActive(true);
				file_btn.GetComponent<Button>().interactable = true;
				file_btn.GetComponent<Select_File_Load_Btn>().SetFile(files[rl_file_id].Split('.')[0]);
			}
			else
			{
				file_btn.SetActive(false);
			}
		}

		if(cur_load_page > 0)
			file_list.transform.Find("prev_pg").gameObject.GetComponent<Button>().interactable = true;
		else
			file_list.transform.Find("prev_pg").gameObject.GetComponent<Button>().interactable = false;

		if (cur_load_page < num_load_pages - 1)
			file_list.transform.Find("next_pg").gameObject.GetComponent<Button>().interactable = true;
		else
			file_list.transform.Find("next_pg").gameObject.GetComponent<Button>().interactable = false;

		SetLoadBtn(false);
	}

	// Start is called before the first frame update
	void Start()
    {
		Map = GameObject.Find("Map");
		NewMap("");

		if (load_menu != null)
			load_menu.SetActive(false);
		if (save_menu != null)
			save_menu.SetActive(false);
		if (new_menu != null)
			new_menu.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
