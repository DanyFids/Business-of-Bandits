#pragma once
#include <vector>
#include <memory>
#include <string>
#include "Object.h"
#include "PlugginSettings.h"

class PLUGIN_API Map
{
protected:
	static std::string FILE_DIR;
	static std::string FILE_EXT;

	std::string map_file;
	std::vector<Object> objects;

public:
	Map(std::string f = "");

	void NewMap(std::string f);
	void LoadMap(std::string f);
	void SaveMap();

	void ClearObjects();

	void CreateObject(std::string i, std::string p, Transform t);
	void UpdateObject(std::string id, Transform t);
	void RemoveObject(std::string i);

	std::string GetMapFile();
	void SetMapFile(std::string name);

	int FindObject(std::string id);
	Object* GetObject(int id);

	int GetNumObjects();

	void PrintMap();

	void Debug_AddObj(std::string d);

	static void GetMapList(std::vector<std::string>& map_names, int& num_maps);
};

