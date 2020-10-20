#include "Map.h"
#include<iostream>
#include<fstream>
#include<sys/stat.h>
#include<filesystem>

std::string Map::FILE_DIR = "map_saves/";
std::string Map::FILE_EXT = ".dmap";

Map::Map(std::string f)
{
	std::string file_path = Map::FILE_DIR + f + Map::FILE_EXT;
	struct stat results;

	if (stat(file_path.c_str(), &results) == 0) {
		LoadMap(f);
	}
	else {
		NewMap(f);
	}
}

void Map::NewMap(std::string f)
{
	map_file = f;
	ClearObjects();

	std::cout << "New Map: \"" + f + "\" created." << std::endl;
}

void Map::LoadMap(std::string f)
{
	std::string file_path = Map::FILE_DIR + f + Map::FILE_EXT;
	int file_size = -1;

	// Get File Size
	struct stat results;

	if (stat(file_path.c_str(), &results) == 0) {
		file_size = results.st_size;
	}
	else {
		std::cout << "Error Reading File." << std::endl;
		return;
	}

	if (file_size != -1) {
		map_file = f;
		ClearObjects();

		std::ifstream file(file_path, std::ios::in | std::ios::binary);

		int num_obj;
		file.read((char*)&num_obj, sizeof(num_obj));

		for (int c = 0; c < num_obj; c++) {
			size_t id_size;
			size_t pf_size;

			std::string id, prefab;

			Transform trans;

			file.read((char*)& id_size, sizeof(id_size));
			id.resize(id_size);
			file.read((char*)& id[0], id_size);

			file.read((char*)& pf_size, sizeof(pf_size));
			prefab.resize(pf_size);
			file.read((char*)& prefab[0], pf_size);

			file.read((char*)& trans, sizeof(trans));

			Object temp(id, prefab, trans);
			
			objects.push_back(temp);
		}

		file.close();
	}
	else {
		std::cout << "Error Reading File." << std::endl;
		return;
	}

	std::cout << "File: '" + file_path + "' Loaded Successfully!" << std::endl;
}

void Map::SaveMap()
{
	if (!std::filesystem::exists(Map::FILE_DIR))
		std::filesystem::create_directories(Map::FILE_DIR);


	std::string file_path = Map::FILE_DIR + map_file + Map::FILE_EXT;
	std::ofstream write_file(file_path, std::ios::out | std::ios::binary);

	if (!write_file.is_open()) {
		std::cout << "Error opening file: '" + file_path + "'" << std::endl;
		return;
	}

	int num_obj = objects.size();
	int size_obj = sizeof(Object);

	write_file.write((char*)&num_obj, sizeof(num_obj));

	for (int c = 0; c < num_obj; c++){
		Object temp = objects.data()[c];
		size_t id_size = temp.GetId().size();
		size_t pf_size = temp.prefab.size();

		//write prefab
		write_file.write((char*)&id_size, sizeof(id_size));
		write_file.write((char*)&temp.GetId()[0], id_size);

		//write prefab
		write_file.write((char*)& pf_size, sizeof(pf_size));
		write_file.write((char*)&temp.prefab[0], pf_size);

		//write transform
		write_file.write((char*)& temp.transform, sizeof(temp.transform));
	}

	//write_file.write((char*)objects.data(), size_obj * num_obj);

	write_file.close();

	std::cout << "File saved successfully." << std::endl;
}

void Map::ClearObjects()
{
	std::vector<Object> temp;

	objects.swap(temp);

	temp.clear();
}

void Map::CreateObject(std::string i, std::string p, Transform t)
{
	objects.push_back(Object(i, p, t));
}

void Map::UpdateObject(std::string id, Transform t)
{
	int i = FindObject(id);

	if (i != -1) {
		objects.at(i).transform = t;
	}
}

void Map::RemoveObject(std::string id)
{
	int i = FindObject(id);

	if (i != -1)
		objects.erase(objects.begin() + i);
}

std::string Map::GetMapFile()
{
	return map_file;
}

void Map::SetMapFile(std::string name)
{
	map_file = name;
}

int Map::FindObject(std::string id)
{
	int ret = -1;

	for (int c = 0; c < objects.size(); c++) {
		if (id.compare(objects.at(c).GetId()) == 0)
			ret = c;
	}

	return ret;
}

Object* Map::GetObject(int id)
{
	if (id < 0 || id >= objects.size()) {
		return NULL;
	}
	else
		return &objects.at(id);
}

int Map::GetNumObjects()
{
	return objects.size();
}

void Map::PrintMap()
{
	std::cout << map_file << std::endl << "{" << std::endl;

	for (int c = 0; c < objects.size(); c++) {
		std::cout << std::to_string(c + 1) + ": " + objects.at(c).ToString();
	}

	std::cout << "}" << std::endl << std::endl;
}

void Map::Debug_AddObj(std::string d)
{
	objects.push_back(Object(d, "Test_Prefab"));
}

void Map::GetMapList(std::vector<std::string>& map_names, int& num_maps)
{
	std::vector<std::string> files;
	for (const auto& entry : std::filesystem::directory_iterator(Map::FILE_DIR))
		files.push_back(entry.path().filename().string());

	num_maps = files.size();
	map_names = files;
}