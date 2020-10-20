#include "Object.h"

Object::Object(std::string i, std::string p, Transform t)
{
	id = i;
	prefab = p;
	transform = t;
}

Object::Object(std::string i, std::string p, Vector3 pos, Vector3 rot, Vector3 siz)
{
	id = i;
	prefab = p;
	transform = Transform(pos, rot, siz);
}

std::string Object::GetId()
{
	return id;
}

std::string Object::ToString()
{
	std::string ret = "";

	ret += "\"" + id + "\"\n";
	ret += "{\n";

	ret += "\tprefab: \"" + prefab + "\"\n";
	ret += "\tposition: (" + std::to_string(transform.position.x) + "," + std::to_string(transform.position.y) + ", " + std::to_string(transform.position.z) + ")\"\n";
	ret += "\trotation: (" + std::to_string(transform.rotation.x) + "," + std::to_string(transform.rotation.y) + ", " + std::to_string(transform.rotation.z) + ")\"\n";
	ret += "\tscale: (" + std::to_string(transform.scale.x) + "," + std::to_string(transform.scale.y) + ", " + std::to_string(transform.scale.z) + ")\"\n";

	ret += "}\n";
	return ret;
}
