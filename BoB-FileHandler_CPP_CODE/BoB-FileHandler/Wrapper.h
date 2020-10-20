#pragma once

#include "Map.h"

#ifdef __cplusplus
extern "C"{
#endif
	PLUGIN_API void New_Map(char* name);
	PLUGIN_API void Save_Map();
	PLUGIN_API void Load_Map(char* name);

	PLUGIN_API char* Get_Map_Name();
	PLUGIN_API void Set_Map_Name(char* name);
	
	PLUGIN_API void Create_Object(char* id, char* prefab, Transform transform);
	PLUGIN_API void Update_Object(char* id, Vector3 pos, Vector3 rot);
	PLUGIN_API void Remove_Object(char* id);
	PLUGIN_API Object_Ret Get_Object(int id);
	
	PLUGIN_API int Get_Num_Objects();

	PLUGIN_API char* Get_Map_List(int& num_maps);
#ifdef __cplusplus
}
#endif

