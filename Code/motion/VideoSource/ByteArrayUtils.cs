
namespace VideoSource
{
	using System;


	internal class ByteArrayUtils
	{
	
		public static bool Compare(byte[] array, byte[] needle, int startIndex)
		{
			int	needleLen = needle.Length;
			for (int i = 0, p = startIndex; i < needleLen; i++, p++)
			{
				if (array[p] != needle[i])
				{
					return false;
				}
			}
			return true;
		}


		public static int Find(byte[] array, byte[] needle, int startIndex, int count)
		{
			int	needleLen = needle.Length;
			int	index;

			while (count >= needleLen)
			{
				index = Array.IndexOf(array, needle[0], startIndex, count - needleLen + 1);

				if (index == -1)
					return -1;

				int i, p;
		
				for (i = 0, p = index; i < needleLen; i++, p++)
				{
					if (array[p] != needle[i])
					{
						break;
					}
				}

				if (i == needleLen)
				{
					
					return index;
				}

				count -= (index - startIndex + 1);
				startIndex = index + 1;
			}
			return -1;
		}
	}
}
