﻿using System;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ClassicalSharp.GraphicsAPI {

	public partial class OpenGLApi {
		
		public int CreateProgram( string vertexShader, string fragShader ) {
			int vertexShaderId = LoadShader( vertexShader, ShaderType.VertexShader );
			int fragShaderId = LoadShader( fragShader, ShaderType.FragmentShader );
			int program = GL.CreateProgram();
			
			GL.AttachShader( program, vertexShaderId );
			GL.AttachShader( program, fragShaderId );
			GL.LinkProgram( program );
			
			string errorLog = GL.GetProgramInfoLog( program );
			Console.WriteLine( "Program error for {0}:", program );
			Console.WriteLine( errorLog );
			
			GL.DeleteShader( vertexShaderId );
			GL.DeleteShader( fragShaderId );
			return program;
		}
		
		unsafe static int LoadShader( string source, ShaderType type ) {
			int shaderId = GL.CreateShader( type );
			int len = source.Length;
			GL.ShaderSource( shaderId, 1, new [] { source }, &len );
			GL.CompileShader( shaderId );
			
			string errorLog = GL.GetShaderInfoLog( shaderId );
			Console.WriteLine( "Shader error for {0} ({1}):", shaderId, type );
			Console.WriteLine( errorLog );
			return shaderId;
		}
		
		public void UseProgram( int program ) {
			GL.UseProgram( program );
		}
		
		public void EnableVertexAttribF( int attribLoc, int numComponents, int stride, int offset ) {
			GL.EnableVertexAttribArray( attribLoc );
			GL.VertexAttribPointer( attribLoc, numComponents, VertexAttribPointerType.Float, false, stride, new IntPtr( offset ) );
		}
		
		public void EnableVertexAttribF( int attribLoc, int numComponents, VertexAttribType type, bool normalise, int stride, int offset ) {
			GL.EnableVertexAttribArray( attribLoc );
			GL.VertexAttribPointer( attribLoc, numComponents, attribMappings[(int)type], normalise, stride, new IntPtr( offset ) );
		}
		
		VertexAttribPointerType[] attribMappings = {
			VertexAttribPointerType.Float, VertexAttribPointerType.UnsignedByte,
			VertexAttribPointerType.UnsignedShort, VertexAttribPointerType.UnsignedInt,
			VertexAttribPointerType.Byte, VertexAttribPointerType.Short,
			VertexAttribPointerType.Int,
		};	
		
		public void DisableVertexAttrib( int attribLoc ) {
			GL.DisableVertexAttribArray( attribLoc );
		}
		
		// Booooo, no direct state access for OpenGL 2.0.	
		public void SetUniform( int uniformLoc, float value ) {
			GL.Uniform1( uniformLoc, value );
		}
		
		public void SetUniform( int uniformLoc, ref Vector2 value ) {
			GL.Uniform2( uniformLoc, ref value );
		}
		
		public void SetUniform( int uniformLoc, ref Vector3 value ) {
			GL.Uniform3( uniformLoc, ref value );
		}
		
		public void SetUniform( int uniformLoc, ref Vector4 value ) {
			GL.Uniform4( uniformLoc, ref value );
		}
		
		public unsafe void SetUniform( int uniformLoc, ref Matrix4 value ) {
			fixed( float* ptr = &value.Row0.X )
				GL.UniformMatrix4( uniformLoc, 1, false, ptr );
		}
		
		public int GetAttribLocation( int program, string name ) {
			return GL.GetAttribLocation( program, name );
		}
		
		public int GetUniformLocation( int program, string name ) {
			return GL.GetUniformLocation( program, name );
		}
		
		public unsafe void PrintAllAttribs( int program ) {
			int numAttribs;
			GL.GetProgram( program, ProgramParameter.ActiveAttributes, &numAttribs );
			for( int i = 0; i < numAttribs; i++ ) {
				int len, size;
				StringBuilder builder = new StringBuilder( 64 );
				ActiveAttribType type;
				GL.GetActiveAttrib( program, i, 32, &len, &size, &type, builder );
				Console.WriteLine( i + " : " + type + ", " + builder );
			}
		}
		
		public unsafe void PrintAllUniforms( int program ) {
			int numUniforms;
			GL.GetProgram( program, ProgramParameter.ActiveUniforms, &numUniforms );
			for( int i = 0; i < numUniforms; i++ ) {
				int len, size;
				StringBuilder builder = new StringBuilder( 64 );
				ActiveUniformType type;
				GL.GetActiveUniform( program, i, 32, &len, &size, &type, builder );
				Console.WriteLine( i + " : " + type + ", " + builder );
			}
		}
		
		public void DrawModernVb( DrawMode mode, int id, int startVertex, int verticesCount ) {
			GL.BindBuffer( BufferTarget.ArrayBuffer, id );
			GL.DrawArrays( modeMappings[(int)mode], startVertex, verticesCount );
		}
		
		public void DrawModernIndexedVb( DrawMode mode, int vb, int ib, int indicesCount,
		                                        int startVertex, int startIndex ) {
			GL.BindBuffer( BufferTarget.ArrayBuffer, vb );
			GL.BindBuffer( BufferTarget.ElementArrayBuffer, ib );
			
			int offset = startVertex * VertexPos3fTex2fCol4b.Size;
			GL.DrawElements( modeMappings[(int)mode], indicesCount, indexType, new IntPtr( startIndex * 2 ) );
		}
	}
	
	public enum VertexAttribType : byte {
		Float32 = 0,
		UInt8 = 1,
		UInt16 = 2,
		UInt32 = 3,
		Int8 = 4,
		Int16 = 5,
		Int32 = 6,
	}
}