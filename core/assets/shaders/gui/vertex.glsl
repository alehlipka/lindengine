#version 430

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexture;

out vec2 vTexture;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

void main()
{
	vTexture = aTexture;
	gl_Position = vec4(aPosition, 1.0) * modelMatrix * viewMatrix * projectionMatrix;
}
