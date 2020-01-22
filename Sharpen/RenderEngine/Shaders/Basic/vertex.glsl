#version 400 core

in vec3 vertexCoordinates;
in vec2 textureCoordinates;

out vec2 vTextureCoordinates;

void main(void) 
{
	gl_Position = vec4(vertexCoordinates, 1.0);
	vTextureCoordinates = textureCoordinates;
}