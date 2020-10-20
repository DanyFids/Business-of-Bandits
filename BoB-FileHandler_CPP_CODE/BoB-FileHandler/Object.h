#pragma once
#include <string>
#include "Transform.h"
#include "PlugginSettings.h"

struct PLUGIN_API Object_Ret {
	char* id;
	char* prefab;
	Transform transform;
};

class PLUGIN_API Object
{
private:
	std::string id;
public:
	std::string prefab;
	Transform transform;

	Object(std::string i, std::string p, Transform t);
	Object(std::string i, std::string p, Vector3 pos = {0.0f, 0.0f, 0.0f}, Vector3 rot = {0.0f, 0.0f, 0.0f}, Vector3 siz = { 1.0f, 1.0f, 1.0f });

	std::string GetId();

	std::string ToString();
};

