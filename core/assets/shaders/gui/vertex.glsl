#version 430

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec2 vTexture;

out vec2 vertexTexture;

uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

void main()
{
	vertexTexture = vTexture;
	gl_Position = vec4(vPosition, 1.0) * modelMatrix * viewMatrix * projectionMatrix;
	gl_Position = vec4(1.0, 1.0, 1.0, 1.0);
}
