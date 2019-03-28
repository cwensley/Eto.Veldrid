﻿using Eto.Forms;
using Eto.Gl;
using Eto.Gl.Mac;
using Eto.VeldridSurface;
using OpenTK;
using System;
using Veldrid;

namespace PlaceholderName
{
	public class MacVeldridSurfaceHandler : VeldridSurfaceHandler
	{
		public override void InitializeGraphicsApi()
		{
			if (Widget.Backend == GraphicsBackend.Metal)
			{
				Widget.GraphicsDevice = GraphicsDevice.CreateMetal(new GraphicsDeviceOptions());
			}
			else
			{
				string message;
				if (!Enum.IsDefined(typeof(GraphicsBackend), Widget.Backend))
				{
					message = "Unrecognized backend!";
				}
				else
				{
					message = "Specified backend not supported on this platform!";
				}

				throw new ArgumentException(message);
			}

			var source = SwapchainSource.CreateNSView(Control.NativeHandle);
			Widget.Swapchain = Widget.GraphicsDevice.ResourceFactory.CreateSwapchain(
				new SwapchainDescription(source, (uint)Widget.Width, (uint)Widget.Height, null, false));
		}
	}

	public static class MainClass
	{
		[STAThread]
		public static void Main(string[] args)
		{
			GraphicsBackend backend = VeldridSurface.PreferredBackend;

			if (backend == GraphicsBackend.OpenGL)
			{
				Toolkit.Init(new ToolkitOptions { Backend = PlatformBackend.PreferNative });
			}

			var platform = new Eto.Mac.Platform();

			if (backend == GraphicsBackend.OpenGL)
			{
				platform.Add<GLSurface.IHandler>(() => new MacGLSurfaceHandler());
			}

			platform.Add<VeldridSurface.IHandler>(() => new MacVeldridSurfaceHandler());

			new Application(platform).Run(new MainForm(backend));
		}
	}
}
