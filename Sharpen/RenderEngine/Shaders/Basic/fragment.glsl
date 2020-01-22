#version 400 core

uniform sampler2D textureSampler;

in vec2 vTextureCoordinates;

out vec4 fColor;

void main(void) 
{
	fColor = texture(textureSampler, vTextureCoordinates);
}