using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace winformautoclicker
{
	public class AutoClick
	{
		/// <summary>
		/// Struct repersenting a point.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct POINT
		{
			public int x;
			public int y;
		}

		[DllImport("user32.dll", CallingConvention = CallingConvention.Winapi)]
		private static extern void mouse_event (uint dwFlags, int dx, int dy, uint cButtons, int dwExtraInfo);

		[DllImport("user32.dll")]
		private static extern bool GetCursorPos (out POINT lpPoint);

		public static void click() {
			
			POINT cursor;

			while (true) {
				
				GetCursorPos (out cursor);
				Console.WriteLine ("X: " + cursor.x + "; Y: " + cursor.y);
				mouse_event ((uint)MouseFlags.LeftDown | (uint)MouseFlags.LeftUp, cursor.x, cursor.y, 0, 0);

				Thread.Sleep (100);
			}
		}

		[Flags]
		private enum MouseFlags : uint
		{
			Move = 0x0001, 
			LeftDown = 0x0002, 
			LeftUp = 0x0004, 
			RightDown = 0x0008,
			RightUp = 0x0010, 
			MiddleDown = 0x0020, 
			MiddleUp = 0x0040, 
			XDown = 0x0080, 
			XUp = 0x0100, 
			Wheel = 0x0800, 
			HWheel = 0x1000, 
			Absolute = 0x8000
		}
	}
}

