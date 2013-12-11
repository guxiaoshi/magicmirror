
namespace motion
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;

	using AForge.Imaging;
	using AForge.Imaging.Filters;

	
	public class MotionDetector3Optimized : IMotionDetector
	{
		private byte[]	backgroundFrame = null;
		private byte[]	currentFrame = null;
		private byte[]	currentFrameDilatated = null;

		private int		counter = 0;

		private bool	calculateMotionLevel = false;
		private int		width;	// image width
		private int		height;	// image height
		private int		pixelsChanged;

		public bool MotionLevelCalculation
		{
			get { return calculateMotionLevel; }
			set { calculateMotionLevel = value; }
		}

		public double MotionLevel
		{
			get { return (double) pixelsChanged / ( width * height ); }
		}

		
		public MotionDetector3Optimized( )
		{
		}

		
		public void Reset( )
		{
			backgroundFrame = null;
			currentFrame = null;
			currentFrameDilatated = null;
			counter = 0;
		}

			public void ProcessFrame( ref Bitmap image )
		{
			width	= image.Width;
			height	= image.Height;

			int fW = ( ( ( width - 1 ) / 8 ) + 1 );
			int fH = ( ( ( height - 1 ) / 8 ) + 1 );
			int len = fW * fH;

			if ( backgroundFrame == null )
			{
				backgroundFrame = new byte[len];
				currentFrame = new byte[len];
				currentFrameDilatated = new byte[len];

			
				BitmapData imgData = image.LockBits(
					new Rectangle( 0, 0, width, height ),
					ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb );

				
				PreprocessInputImage( imgData, width, height, backgroundFrame );

				
				image.UnlockBits( imgData );

			
				return;
			}

			
			BitmapData data = image.LockBits(
				new Rectangle( 0, 0, width, height ),
				ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb );

			
			PreprocessInputImage( data, width, height, currentFrame );

			if ( ++counter == 2 )
			{
				counter = 0;

				
				for ( int i = 0; i < len; i++ )
				{
					int t = currentFrame[i] - backgroundFrame[i];
					if ( t > 0 )
						backgroundFrame[i]++;
					else if ( t < 0 )
						backgroundFrame[i]--;
				}
			}

			
			pixelsChanged = 0;
			for ( int i = 0; i < len; i++ )
			{
				int t = currentFrame[i] - backgroundFrame[i];
				if ( t < 0 )
					t = -t;

				if ( t >= 15 )
				{
					pixelsChanged++;
					currentFrame[i] = (byte) 255;
				}
				else
				{
					currentFrame[i] = (byte) 0;
				}
			}
			if ( calculateMotionLevel )
				pixelsChanged *= 64;
			else
				pixelsChanged = 0;

		
			for ( int i = 0; i < fH; i++ )
			{
				for ( int j = 0; j < fW; j++ )
				{
					int k = i * fW + j;
					int v = currentFrame[k];

					// left pixels
					if ( j > 0 )
					{
						v += currentFrame[k - 1];

						if ( i > 0 )
						{
							v += currentFrame[k - fW - 1];
						}
						if ( i < fH - 1 )
						{
							v += currentFrame[k + fW - 1];
						}
					}
					// right pixels
					if ( j < fW - 1 )
					{
						v += currentFrame[k + 1];

						if ( i > 0 )
						{
							v += currentFrame[k - fW + 1];
						}
						if ( i < fH - 1 )
						{
							v += currentFrame[k + fW + 1];
						}
					}
					// top pixel
					if ( i > 0 )
					{
						v += currentFrame[k - fW];
					}
					// right pixel
					if ( i < fH - 1 )
					{
						v += currentFrame[k + fW];
					}

					currentFrameDilatated[k] = (v != 0) ? (byte) 255 : (byte) 0;
				}
			}

			
			PostprocessInputImage( data, width, height, currentFrameDilatated );

		
			image.UnlockBits( data );
		}

		
		private void PreprocessInputImage( BitmapData data, int width, int height, byte[] buf )
		{
			int stride = data.Stride;
			int offset = stride - width * 3;
			int len = (int)( ( width - 1 ) / 8 ) + 1;
			int rem = ( ( width - 1 ) % 8 ) + 1;
			int[] tmp = new int[len];
			int i, j, t1, t2, k = 0;

			unsafe
			{
				byte * src = (byte *) data.Scan0.ToPointer( );

				for ( int y = 0; y < height; )
				{
					// collect pixels
					Array.Clear( tmp, 0, len );

					// calculate
					for ( i = 0; ( i < 8 ) && ( y < height ); i++, y++ )
					{
						// for each pixel
						for ( int x = 0; x < width; x++, src += 3 )
						{
							// grayscale value using BT709
							tmp[(int) ( x / 8 )] += (int)( 0.2125f * src[RGB.R] + 0.7154f * src[RGB.G] + 0.0721f * src[RGB.B] );
						}
						src += offset;
					}

					// get average values
					t1 = i * 8;
					t2 = i * rem;

					for ( j = 0; j < len - 1; j++, k++ )
						buf[k] = (byte)( tmp[j] / t1 );
					buf[k++] = (byte)( tmp[j] / t2 );
				}
			}
		}

	
		private void PostprocessInputImage( BitmapData data, int width, int height, byte[] buf )
		{
			int stride = data.Stride;
			int offset = stride - width * 3;
			int len = (int)( ( width - 1 ) / 8 ) + 1;
			int lenWM1 = len - 1;
			int lenHM1 = (int)( ( height - 1 ) / 8);
			int rem = ( ( width - 1 ) % 8 ) + 1;

			int i, j, k;
	
			unsafe
			{
				byte * src = (byte *) data.Scan0.ToPointer( );

				// for each line
				for ( int y = 0; y < height; y++ )
				{
					i = (y / 8);

					// for each pixel
					for ( int x = 0; x < width; x++, src += 3 )
					{
						j = x / 8;	
						k = i * len + j;

						// check if we need to highlight moving object
						if (buf[k] == 255)
						{
							// check for border
							if (
								( ( x % 8 == 0 ) && ( ( j == 0 ) || ( buf[k - 1] == 0 ) ) ) ||
								( ( x % 8 == 7 ) && ( ( j == lenWM1 ) || ( buf[k + 1] == 0 ) ) ) ||
								( ( y % 8 == 0 ) && ( ( i == 0 ) || ( buf[k - len] == 0 ) ) ) ||
								( ( y % 8 == 7 ) && ( ( i == lenHM1 ) || ( buf[k + len] == 0 ) ) )
								)
							{
								src[RGB.R] = 255;
							}
						}
					}
					src += offset;
				}
			}
		}
	}
}
