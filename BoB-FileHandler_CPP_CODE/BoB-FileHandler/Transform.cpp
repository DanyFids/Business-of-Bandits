#include "Transform.h"

Transform::Transform()
{
	position = {0.0f, 0.0f, 0.0f};
	rotation = { 0.0f, 0.0f, 0.0f };
	scale = { 0.0f, 0.0f, 0.0f };
}

Transform::Transform(Vector3 p, Vector3 r, Vector3 s)
{
	position = p;
	rotation = r;
	scale = s;
}
