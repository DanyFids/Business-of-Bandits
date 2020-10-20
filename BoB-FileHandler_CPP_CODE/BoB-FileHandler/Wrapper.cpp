#include "Wrapper.h"

Map loaded_map;

PLUGIN_API void New_Map(char* name)
{
	loaded_map.NewMap(name);
}

PLUGIN_API void Save_Map()
{
	loaded_map.SaveMap();
}

PLUGIN_API void Load_Map(char* name)
{
	loaded_map.LoadMap(name);
}

PLUGIN_API void Set_Map_Name(char* name) {
	loaded_map.SetMapFile(name);
}

PLUGIN_API char* Get_Map_Name()
{
	std::string map_name = loaded_map.GetMapFile();

	char* ret = new char[map_name.length() + 1];

	for (int c = 0; c < map_name.length() + 1; c++) {
		ret[c] = map_name.c_str()[c];
	}

	return ret;
}

PLUGIN_API void Create_Object(char* id, char* prefab, Transform transform)
{
	loaded_map.CreateObject(id, prefab, transform);
}

PLUGIN_API void Update_Object(char* id, Vector3 pos, Vector3 rot)
{
	loaded_map.UpdateObject(id, { pos, rot, {1.0f, 1.0f, 1.0f} });
}

PLUGIN_API void Remove_Object(char* id)
{
	loaded_map.RemoveObject(id);
}

PLUGIN_API Object_Ret Get_Object(int id)
{
	Object* temp = loaded_map.GetObject(id);
	Object_Ret ret;

	if (temp != NULL) {
		ret.id = new char[temp->GetId().length() +1];
		for (int c = 0; c < temp->GetId().length() + 1; c++) {
			ret.id[c] = temp->GetId().c_str()[c];
		}
		
		ret.prefab = new char[temp->prefab.length() + 1];
		for (int c = 0; c < temp->prefab.length() + 1; c++) {
			ret.prefab[c] = temp->prefab.c_str()[c];
		}

		ret.transform = temp->transform;
	}

	return ret;
}

PLUGIN_API int Get_Num_Objects()
{
	return loaded_map.GetNumObjects();
}

PLUGIN_API char* Get_Map_List(int& num_maps)
{
	std::vector<std::string> maps;

	Map::GetMapList(maps, num_maps);

	std::string maps_ret = "";

	for (int c = 0; c < maps.size(); c++) {
		maps_ret += maps[c] + ";";
	}

	char* ret = new char[maps_ret.length() + 1];

	for (int c = 0; c < maps_ret.length() + 1; c++) {
		ret[c] = maps_ret.c_str()[c];
	}

	return ret;
}
