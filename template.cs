﻿using System;
using System.IO;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace template
{
	public class OpenTKApp : GameWindow
	{
        public static int VIEW_WIDTH = 512;
        public static int VIEW_HEIGHT = 512;
        public static int DEBUG_WIDTH = VIEW_HEIGHT;
        public static int DEBUG_HEIGHT = VIEW_HEIGHT;
        public static int APP_WIDTH = VIEW_WIDTH + DEBUG_WIDTH;
        public static int APP_HEIGHT = VIEW_HEIGHT;

		static int screenID;
		static Game game;
		static bool terminated = false;
		protected override void OnLoad( EventArgs e )
		{
			// called upon app init
			GL.ClearColor( Color.Black );
			GL.Enable( EnableCap.Texture2D );
			GL.Disable( EnableCap.DepthTest );
			GL.Hint( HintTarget.PerspectiveCorrectionHint, HintMode.Nicest );
            ClientSize = new Size(APP_WIDTH, APP_HEIGHT );
			game = new Game();
			game.screen = new Surface( Width, Height );
			Sprite.target = game.screen;
			screenID = game.screen.GenTexture();
			game.Init();
		}
		protected override void OnUnload( EventArgs e )
		{
			// called upon app close
			GL.DeleteTextures( 1, ref screenID );
			Environment.Exit( 0 ); // bypass wait for key on CTRL-F5
		}
		protected override void OnResize( EventArgs e )
		{
			// called upon window resize
			GL.Viewport(0, 0, Width, Height);
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			GL.Ortho( -1.0, 1.0, -1.0, 1.0, 0.0, 4.0 );
		}
		protected override void OnUpdateFrame( FrameEventArgs e )
		{
			// called once per frame; app logic
			var keyboard = OpenTK.Input.Keyboard.GetState();
			if (keyboard[OpenTK.Input.Key.Escape]) this.Exit();
		}
		protected override void OnRenderFrame( FrameEventArgs e )
		{
			// called once per frame; render
			game.Tick();
			if (terminated) 
			{
				Exit();
				return;
			}
			// convert Game.screen to OpenGL texture
			GL.BindTexture( TextureTarget.Texture2D, screenID );
			GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 
						   game.screen.width, game.screen.height, 0, 
						   OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
						   PixelType.UnsignedByte, game.screen.pixels 
						 );
			// clear window contents
			GL.Clear( ClearBufferMask.ColorBufferBit );
			// setup camera
			GL.MatrixMode( MatrixMode.Modelview );
			GL.LoadIdentity();
			GL.MatrixMode( MatrixMode.Projection );
			GL.LoadIdentity();
			// draw screen filling quad
			GL.Begin( PrimitiveType.Quads );
			GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex2( -1.0f, -1.0f );
			GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex2(  1.0f, -1.0f );
			GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex2(  1.0f,  1.0f );
			GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex2( -1.0f,  1.0f );
			GL.End();
			// tell OpenTK we're done rendering
			SwapBuffers();
		}
		public static void Main( string[] args ) 
		{
            // entry point
            using (OpenTKApp app = new OpenTKApp()) { app.Run(); }
        }
	}
}