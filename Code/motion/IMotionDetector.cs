namespace motion
{
	using System;
	using System.Drawing;

	/// <summary>
	/// IMotionDetector interface
	/// </summary>
	public interface IMotionDetector
	{
	
		bool MotionLevelCalculation{ set; get; }

		double MotionLevel{ get; }

		void ProcessFrame( ref Bitmap image );

		void Reset( );
	}
}
