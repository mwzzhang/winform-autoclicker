using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

// https://stackoverflow.com/a/27309185

namespace winformautoclicker
{
	public sealed class KeyboardHook : IDisposable
	{
		// Registers a hot key with Windows.
		[DllImport ("user32.dll", SetLastError = true)]
		private static extern bool RegisterHotKey (IntPtr hWnd, int id, uint fsModifiers, uint vk);
		// Unregisters the hot key with Windows.
		[DllImport ("user32.dll", SetLastError = true)]
		private static extern bool UnregisterHotKey (IntPtr hWnd, int id);

		/// <summary>
		/// Represents the window that is used internally to get the messages.
		/// </summary>
		private class Window : NativeWindow, IDisposable
		{
			private static int WM_HOTKEY = 0x0312;

			public Window ()
			{
				// create the handle for the window.
				this.CreateHandle (new CreateParams ());
			}

			/// <summary>
			/// Overridden to get the notifications.
			/// </summary>
			/// <param name="m"></param>
			protected override void WndProc (ref Message m)
			{
				base.WndProc (ref m);

				if (m.Msg == WM_HOTKEY) {
					// get the keys.
					Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
					ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

					// invoke the event to notify the parent.
					if (KeyPressed != null) {
						KeyPressed (this, new KeyPressedEventArgs (modifier, key));
					}
				}
			}

			public event EventHandler<KeyPressedEventArgs> KeyPressed;

			#region IDisposable Members

			public void Dispose ()
			{
				this.DestroyHandle ();
			}

			#endregion

		}

		private Window window_ = new Window ();
		private int currentId_;

		public KeyboardHook ()
		{
			// register the event of the inner native window.
			window_.KeyPressed += delegate(object sender, KeyPressedEventArgs e) {
				if (KeyPressed != null) {
					KeyPressed (this, e);
				}
			};
		}

		/// <summary>
		/// Registers a hot key in the system.
		/// </summary>
		/// <param name="modifier">The modifiers that are associated with the hot key.</param>
		/// <param name="key">The key itself that is associated with the hot key.</param>
		public void RegisterHotKey (ModifierKeys modifier, Keys key)
		{
			// increment the counter.
			currentId_++;

			// register the hot key.
			if (!RegisterHotKey (window_.Handle, currentId_, (uint)modifier, (uint)key)) {
				throw new InvalidOperationException ("Couldn't register the hot key.");
			}
		}

		/// <summary>
		/// A hot key has been pressed.
		/// </summary>
		public event EventHandler<KeyPressedEventArgs> KeyPressed;

		#region IDisposable Members

		public void Dispose ()
		{
			// unregister all the registered hot keys.
			for (int i = currentId_; i > 0; i--) {
				UnregisterHotKey (window_.Handle, i);
			}

			// dispose the inner native window.
			window_.Dispose ();
		}

		#endregion
	}

	/// <summary>
	/// Event Args for the event that is fired after the hot key has been pressed.
	/// </summary>
	public class KeyPressedEventArgs : EventArgs
	{
		private ModifierKeys modifier_;
		private Keys key_;

		internal KeyPressedEventArgs(ModifierKeys modifier, Keys key) {
			modifier_ = modifier;
			key_ = key;
		}

		public ModifierKeys Modifier {
			get { return modifier_; }
		}

		public Keys Key {
			get { return key_; }
		}
	}

	/// <summary>
	/// The enumeration of possible modifiers.
	/// </summary>
	[Flags]
	public enum ModifierKeys : uint
	{
		Alt = 1, 
		Control = 2, 
		Shift = 4, 
		Windows = 8,
		NoRepeat = 0x4000
	}
}
