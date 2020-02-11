#version 400 core

layout(location = 0) in vec3 vertexCoordinates;
layout(location = 1) in vec2 textureCoordinates;

uniform mat4 modelview;
uniform mat4 projection;

out vec2 vTextureCoordinates;

void main(void) 
{
	gl_Position = projection * modelview * vec4(vertexCoordinates, 1.0);
	vTextureCoordinates = textureCoordinates;
}