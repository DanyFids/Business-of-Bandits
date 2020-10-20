#include <iostream>
#include "Map.h"

// Get_Map_List debugging
//#include "Wrapper.h"
//
//
//void main() {
//	int num_maps;
//
//	char* test = Get_Map_List(num_maps);
//
//	system("pause");
//}


// Basic Testing
Map test_map;

void main() {
	system("cls");
	//Display List of Maps
	std::vector<std::string> maps;
	int num_maps;
	Map::GetMapList(maps, num_maps);

	std::cout << "Available Maps: " << std::endl;
	for (int c = 0; c < num_maps; c++)
		std::cout << "\t" + maps[c] << std::endl;
	
	system("pause");
	system("cls");

	// create/load map
	test_map = Map("Testing");
	system("pause");
	system("cls");

	//display start map
	test_map.PrintMap();
	system("pause");
	system("cls");

	// add new obj and display map
	int num = test_map.GetNumObjects();
	test_map.Debug_AddObj("obj_" + std::to_string(++num));
	test_map.Debug_AddObj("obj_" + std::to_string(++num));
	test_map.CreateObject("reaaaaaaaallllly loooooooooooooong", "long long maaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaan", Transform());
	test_map.PrintMap();
	system("pause");
	system("cls");

	// save map
	test_map.SaveMap();
	system("pause");
}