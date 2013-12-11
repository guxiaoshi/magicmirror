
namespace VideoSource
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Threading;

	using Tiger.Video.VFW;

	/// <summary>
	/// VideoFileSource
	/// </summary>
	public class VideoFileSource : IVideoSource
	{
		private string	source;
		private object	userData = null;
		private int		framesReceived;

		private Thread	thread = null;
		private ManualResetEvent stopEvent = null;

	
		public event CameraEventHandler NewFrame;

		public virtual string VideoSource
		{
			get { return source; }
			set { source = value; }
		}
	
		public string Login
		{
			get { return null; }
			set { }
		}
	
		public string Password
		{
			get { return null; }
			set { }
		}
		
		public int FramesReceived
		{
			get
			{
				int frames = framesReceived;
				framesReceived = 0;
				return frames;
			}
		}
		
		public int BytesReceived
		{
			get { return 0; }
		}
		
		public object UserData
		{
			get { return userData; }
			set { userData = value; }
		}
		public bool Running
		{
			get
			{
				if (thread != null)
				{
					if (thread.Join(0) == false)
						return true;
					Free();
				}
				return false;
			}
		}



		public VideoFileSource()
		{
		}
	public void Start()
		{
			if (thread == null)
			{
				framesReceived = 0;

				stopEvent	= new ManualResetEvent(false);
				thread = new Thread(new ThreadStart(WorkerThread));
				thread.Name = source;
				thread.Start();
			}
		}
        public void SignalToStop()
		{
				if (thread != null)
			{
				
				stopEvent.Set();
			}
		}

		
		public void WaitForStop()
		{
			if (thread != null)
			{
				
				thread.Join();

				Free();
			}
		}

		
		public void Stop()
		{
			if (this.Running)
			{
				thread.Abort();
				WaitForStop();
			}
		}

		
		private void Free()
		{
			thread = null;

			
			stopEvent.Close();
			stopEvent = null;
		}

		
		public void WorkerThread()
		{
			AVIReader	aviReader = new AVIReader();

			try
			{
			
				aviReader.Open(source);

				while (true)
				{
				
					DateTime	start = DateTime.Now;

					Bitmap	bmp = aviReader.GetNextFrame();

					framesReceived++;

				   if (stopEvent.WaitOne(0, false))
						break;

					if (NewFrame != null)
						NewFrame(this, new CameraEventArgs(bmp));

					
					bmp.Dispose();
				
					TimeSpan	span = DateTime.Now.Subtract(start);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("exception : " + ex.Message);
			}

			aviReader.Dispose();
			aviReader = null;
		}
	}
}
