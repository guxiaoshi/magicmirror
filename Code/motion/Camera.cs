
namespace motion
{
	using System;
	using System.Drawing;
	using System.Threading;
	using VideoSource;


	public class Camera
	{
		private IVideoSource	videoSource = null;
		private IMotionDetector	motionDetecotor = null;
		private Bitmap			lastFrame = null;


		private int		width = -1, height = -1;

		private double	alarmLevel = 0.005;

		//
		public event EventHandler	NewFrame;
		public event EventHandler	Alarm;


		public Bitmap LastFrame
		{
			get { return lastFrame; }
		}

		public int Width
		{
			get { return width; }
		}		public int Height
		{
			get { return height; }
		}

		public int FramesReceived
		{
			get { return ( videoSource == null ) ? 0 : videoSource.FramesReceived; }
		}

		public int BytesReceived
		{
			get { return ( videoSource == null ) ? 0 : videoSource.BytesReceived; }
		}

		public bool Running
		{
			get { return ( videoSource == null ) ? false : videoSource.Running; }
		}

		public IMotionDetector MotionDetector
		{
			get { return motionDetecotor; }
			set { motionDetecotor = value; }
		}

		public Camera( IVideoSource source ) : this( source, null )
		{ }
		public Camera( IVideoSource source, IMotionDetector detector )
		{
			this.videoSource = source;
			this.motionDetecotor = detector;
			videoSource.NewFrame += new CameraEventHandler( video_NewFrame );
		}

		public void Start( )
		{
			if ( videoSource != null )
			{
				videoSource.Start( );
			}
		}

		public void SignalToStop( )
		{
			if ( videoSource != null )
			{
				videoSource.SignalToStop( );
			}
		}

		public void WaitForStop( )
		{
			// lock
			Monitor.Enter( this );

			if ( videoSource != null )
			{
				videoSource.WaitForStop( );
			}

			Monitor.Exit( this );
		}


		public void Stop( )
		{

			Monitor.Enter( this );

			if ( videoSource != null )
			{
				videoSource.Stop( );
			}

			Monitor.Exit( this );
		}

		public void Lock( )
		{
			Monitor.Enter( this );
		}

		public void Unlock( )
		{
			Monitor.Exit( this );
		}

		private void video_NewFrame( object sender, CameraEventArgs e )
		{
			try
			{

				Monitor.Enter( this );

				if ( lastFrame != null )
				{
					lastFrame.Dispose( );
				}

				lastFrame = (Bitmap) e.Bitmap.Clone( );

				if ( motionDetecotor != null )
				{
					motionDetecotor.ProcessFrame( ref lastFrame );

					if (
						( motionDetecotor.MotionLevel >= alarmLevel ) &&
						( Alarm != null )
						)
					{
						Alarm( this, new EventArgs( ) );
					}
				}

				width = lastFrame.Width;
				height = lastFrame.Height;
			}
			catch ( Exception )
			{
			}
			finally
			{
				Monitor.Exit( this );
			}

			if ( NewFrame != null )
				NewFrame( this, new EventArgs( ) );
		}
	}
}
