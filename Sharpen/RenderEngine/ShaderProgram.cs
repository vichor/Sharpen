using System;
using System.IO;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using l = Serilog.Log;

namespace Sharpen.RenderEngine 
{
    public class ShaderProgram : IDisposable
    {
        public string Log { get; private set; }
        public bool Ready { get; private set; }

        private int _program;
        private string _path;
        private bool _running;
        private readonly Dictionary<string, int> _uniformLocations; 


        public ShaderProgram(string path)
        {
            Ready = false;
            _running = false;
            _path = Path.Combine("Shaders", path);

            _program = GL.CreateProgram();

            // Define supported shaders in program
            var shaders = new[] { 
                new { id = ShaderType.VertexShader,   fileName = "vertex.glsl"   },
                new { id = ShaderType.FragmentShader, fileName = "fragment.glsl" },
                };

            List<int> shadersIds = new List<int>();
            bool shadersWithoutErrors = true;
            Log = "";

            // Compile shaders of the program
            foreach (var shader in shaders)
            {
                string shaderPath = Path.Combine(_path, shader.fileName);
                if (File.Exists(shaderPath))
                {
                    var sourceCode = ReadSourceCode(shaderPath);
                    var shaderId = GL.CreateShader(shader.id);
                    GL.ShaderSource(shaderId, sourceCode);
                    shadersWithoutErrors = CompileShader(shaderId, out var errorLog);
                    if (!shadersWithoutErrors)
                    {
                        Log += $"\n[{_path}]: Compile error in shader [{shader.fileName}]\n" + errorLog;
                        GL.DeleteShader(shaderId);
                        continue;
                    }
                    GL.AttachShader(_program, shaderId);
                    shadersIds.Add(shaderId);
                }
            }                

            // Check at least two shaders (assumed they will be vertex and fragment)
            if (shadersIds.Count < 2)
            {
                Log += $"[{_path}]: Shader program shall contain at least two shaders (vertex and fragment).";
                l.Error(Log);
                throw new Exception(Log);
            }

            // Abort if compile error (added shaders less than available shaders)
            if (shadersIds.Count < shaders.Length)
            {
                GL.DeleteProgram(_program);
                l.Error(Log);
                throw new Exception(Log);
            }

            // Link program
            var linkSuccess = LinkProgram(out var linkErrorLog);
            foreach (var shaderId in shadersIds)
            {
                GL.DetachShader(_program, shaderId);
                GL.DeleteShader(shaderId);
            }

            // Abort if link error
            if (!linkSuccess)
            {
                GL.DeleteProgram(_program);
                Log += $"\n[{_path}]: Link error\n" + linkErrorLog;
                l.Error(Log);
                throw new Exception(Log);
            }

            // Validate
            GL.ValidateProgram(_program);
            GL.GetProgram(_program, GetProgramParameterName.ValidateStatus, out var validation);
            if (validation == 0)    // TODO: compare against 0 is ok?
            {
                Log += $"[{_path}]: Program failed validation.";
                l.Error(Log);
                throw new Exception(Log);
            }

            // Get uniforms
            GL.GetProgram(_program, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            _uniformLocations = new Dictionary<string, int>();
            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(_program, i, out _, out _);
                var location = GL.GetUniformLocation(_program, key);
                _uniformLocations.Add(key, location);
            }

            // Program is ready to be used
            l.Information($"Shader program [${_path}] loaded successfully.");
            Ready = true;
        }

        private string ReadSourceCode(string path)
        {
            using (var buffer = new StreamReader(path))
            {
                return buffer.ReadToEnd();
            }
        }

        private bool CompileShader(int id, out string @log)
        {
            bool compilationResult;
            log = "";
            GL.CompileShader(id);
            GL.GetShader(id, ShaderParameter.CompileStatus, out var code);
            compilationResult = (code == (int) All.True);
            if (!compilationResult)
            {
                log = GL.GetShaderInfoLog(id);
            }
            return compilationResult;
        }

        private bool LinkProgram(out string @log)
        {
            bool linkResult;
            log = "";
            GL.LinkProgram(_program);
            GL.GetProgram(_program, GetProgramParameterName.LinkStatus, out var code);
            linkResult = (code == (int)All.True);
            if (!linkResult)
            {
                log = GL.GetShaderInfoLog(_program);
            }
            return linkResult;
        }

        public void Dispose()
        {
            l.Information($"Disposing shader program {_path}");
            GL.DeleteProgram(_program);
        }

        public int GetAttributeLocation(string attribute)
        {
            return GL.GetAttribLocation(_program, attribute);
        }

        public void SetInt(string name, int data)
        {
            GL.UseProgram(_program);
            GL.Uniform1(_uniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            GL.UseProgram(_program);
            GL.Uniform1(_uniformLocations[name], data);
        }

        public void Run()
        {
            if (Ready)
            {
                GL.UseProgram(_program);
                _running = true;
            }
        }

        public void Stop()
        {
            if (_running)
            {
                GL.UseProgram(0);
                _running = false;
            }
        }

    }
}