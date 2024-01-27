#version 430

in vec2 vertexTexture;

out vec4 outputColor;

uniform sampler2D texture0;

void main()
{
	outputColor = texture(texture0, vertexTexture);
}
