#version 460 core

in vec2 vTexture;
uniform sampler2D texture0;
out vec4 outputColor;

void main()
{
    outputColor = texture(texture0, vTexture);
}
