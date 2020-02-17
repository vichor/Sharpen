using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using l = Serilog.Log;

namespace Sharpen.RenderEngine 
{
    /// <summary>Represents a shader program in the graphics pipeline.</summary>
    /// <remarks>
    ///     A Shader program is defined by the shader files found in the
    ///     supplied directory. This directory will be located in the <c>Shaders</c> 
    ///     directory (this is hardcoded). Each shader program stage will be loaded
    ///     from a file named by the stage name followed by the <c>.glsl</c> extension.
    ///     <para>
    ///         Currently only vertex and fragment shaders are supported, but the 
    ///         class is ready to be easily extended to support the full set of 
    ///         shaders.
    ///         To do so, just add them to the <c>shaders</c> variable in the constructor
    ///         the new shader stages. But keep in mind that the rest of the pipeline is
    ///         not ready for that, so you will need to modify other parts of Sharpen as well.
    ///         So, currently, the shader files are:
    ///         <list type="table">
    ///             <item> <term>Shader Stage</term><description>Filename</description></item>
    ///             <item> <term>Vertex shader</term><description>vertex.glsl</description></item>
    ///             <item> <term>Fragment shader</term><description>fragment.glsl</description></item>
    ///         </list>
    ///     </para>
    /// </remarks>
    internal class ShaderProgram : IDisposable
    {
        /// <value>Shader compiling and linking log message useful when there is any problem.</value>
        public string Log { get; private set; }
        /// <value>Indicates if the shader program is ready to be used.</value>
        public bool Ready { get; private set; }

        private int _program;
        private string _path;
        private bool _running;
        private readonly Dictionary<string, int> _uniformLocations; 


        /// <summary>Creates a <c>ShaderProgram</c> from the given path.</summary>
        /// <param name="path">Directory containing the shader files for each stage.</param>
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

        /// <summary>Clean-up method deleting the program from OpenGL internals.</summary>
        public void Dispose()
        {
            l.Information($"Disposing shader program {_path}");
            GL.DeleteProgram(_program);
        }

        /// <summary>Obtain the location of a shader attribute.</summary>
        /// <remarks>
        ///     In OpenGL context, an attribute are the inputs of a shader, like the vertex 
        ///     coordinates, and are stored in a VAO. So, this returns which position of
        ///     the VAO is using the requested attribute.
        /// </remarks>
        /// <param name="attribute">Name of the attribute to get.</param>
        /// <returns>The position of the attribute.</returns>
        public int GetAttributeLocation(string attribute)
        {
            return GL.GetAttribLocation(_program, attribute);
        }

        /// <summary>Sets value for an integer uniform</summary>
        /// <remarks>
        ///     In OpenGL context, a uniform is a value which keeps the same value
        ///     during the execution of the shader for all the inputs (so, all the vertex
        ///     are executed using the same uniform value).
        /// </remarks>
        public void SetInt(string name, int data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                // For debugging. Access to the dictionary will be done anyway 
                // below to force the exception
                l.Error($"Uniform {name} is not a uniform of shader {_program}");
            }
            GL.UseProgram(_program);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>Sets value for a float uniform</summary>
        /// <remarks>
        ///     In OpenGL context, a uniform is a value which keeps the same value
        ///     during the execution of the shader for all the inputs (so, all the vertex
        ///     are executed using the same uniform value).
        /// </remarks>
        public void SetFloat(string name, float data)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                // For debugging. Access to the dictionary will be done anyway 
                // below to force the exception
                l.Error($"Uniform {name} is not a uniform of shader {_program}");
            }
            GL.UseProgram(_program);
            GL.Uniform1(_uniformLocations[name], data);
        }

        /// <summary>Sets value for a 4x4 matrix uniform</summary>
        /// <remarks>
        ///     In OpenGL context, a uniform is a value which keeps the same value
        ///     during the execution of the shader for all the inputs (so, all the vertex
        ///     are executed using the same uniform value).
        ///     Typically, model/view and projection matrixes are loaded through uniforms.
        /// </remarks>
        public void SetMatrix4(string name, Matrix4 matrix)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                // For debugging. Access to the dictionary will be done anyway 
                // below to force the exception
                l.Error($"Uniform {name} is not a uniform of shader {_program}");
            }
            GL.UseProgram(_program);
            GL.UniformMatrix4(_uniformLocations[name], false, ref matrix); 
        }

        /// <summary>Engages the shader program (or uses the program in OpenGL language).</summary>
        public void Run()
        {
            if (Ready)
            {
                GL.UseProgram(_program);
                _running = true;
            }
        }

        /// <summary>Stops the shader program.</summary>
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