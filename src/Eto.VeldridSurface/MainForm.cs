﻿using Eto.Drawing;
using Eto.Forms;
using System;
using Veldrid;

namespace PlaceholderName
{
	public partial class MainForm : Form
	{
		VeldridSurface Surface;

		VeldridDriver Driver;

		OVPSettings ovpSettings;

		private bool _veldridReady = false;
		public bool VeldridReady
		{
			get { return _veldridReady; }
			private set
			{
				_veldridReady = value;

				SetUpVeldrid();
			}
		}

		private bool _formReady = false;
		public bool FormReady
		{
			get { return _formReady; }
			set
			{
				_formReady = value;

				SetUpVeldrid();
			}
		}

		public MainForm(GraphicsBackend backend)
		{
			InitializeComponent();

			Shown += (sender, e) => FormReady = true;

			Surface = new VeldridSurface(backend);
			Surface.VeldridInitialized += (sender, e) => VeldridReady = true;
			Surface.Draw += (sender, e) => Driver.Draw();

			Content = Surface;

			ovpSettings = new OVPSettings();

			PointF[] testPoly = new PointF[6];
			testPoly[0] = new PointF(2.0f, 2.0f);
			testPoly[1] = new PointF(15.0f, 12.0f);
			testPoly[2] = new PointF(8.0f, 24.0f);
			testPoly[3] = new PointF(8.0f, 15.0f);
			testPoly[4] = new PointF(3.0f, 2.0f);
			testPoly[5] = new PointF(2.0f, 2.0f);

			ovpSettings.addPolygon(testPoly, Color.FromArgb(255, 0, 0), 1.0f, true);

			Driver = new VeldridDriver(ref ovpSettings, ref Surface);
		}

		private void SetUpVeldrid()
		{
			if (!(FormReady && VeldridReady))
			{
				return;
			}

			Driver.SetUpVeldrid();
			Driver.Clock.Start();
		}
	}
}
