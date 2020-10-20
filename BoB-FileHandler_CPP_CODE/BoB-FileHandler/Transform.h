#pragma once
#include "PlugginSettings.h"

struct PLUGIN_API Vector3 {
	float x, y, z;
};

struct PLUGIN_API Transform
{
	Vector3 position = {0.0f, 0.0f, 0.0f};
	Vector3 rotation = {0.0f, 0.0f, 0.0f};
	Vector3 scale = {1.0f, 1.0f, 1.0f};

	Transform();
	Transform(Vector3 p, Vector3 r, Vector3 s);
};