
namespace VideoSource
{
	using System;
	using System.Drawing.Imaging;


	public delegate void CameraEventHandler(object sender, CameraEventArgs e);

	/// <summary>
	/// Camera event arguments
	/// </summary>
	public class CameraEventArgs : EventArgs
	{
		private System.Drawing.Bitmap bmp;


		public CameraEventArgs(System.Drawing.Bitmap bmp)
		{
			this.bmp = bmp;
		}


		public System.Drawing.Bitmap Bitmap
		{
			get { return bmp; }
		}
	}
}