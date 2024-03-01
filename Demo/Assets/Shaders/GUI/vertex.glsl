#version 460 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexture;
uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
out vec2 vTexture;

void main()
{
	vTexture = aTexture;
	// Not proj * view * model * pos
	// That's because OpenTK use row-major matrices (they are transposed)
	gl_Position = vec4(aPosition, 1.0) * modelMatrix * viewMatrix * projectionMatrix;
}
