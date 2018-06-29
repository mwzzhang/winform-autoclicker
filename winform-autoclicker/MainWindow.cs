using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace winformautoclicker
{
	public class MainWindow : Form
	{
		private KeyboardHook hook = new KeyboardHook();
		private Thread autoclickthread = new Thread(new ThreadStart(AutoClick.click));

		public static void Main()
		{
			Application.Run (new MainWindow ());
		}

		public MainWindow()
		{
			/*
			Button b = new Button ();
			b.Text = "Click Me!";
			b.Click += new EventHandler (Button_Click);
			Controls.Add (b);
			*/
			// register the event that is fired after the key press.
			hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);

			// register the control + alt + numpad 5 combination as hot key.
			hook.RegisterHotKey(winformautoclicker.ModifierKeys.Control | winformautoclicker.ModifierKeys.Alt, Keys.NumPad5);
			/*
			// try to register ctrl + alt + numpad 8
			hook.RegisterHotKey(winformautoclicker.ModifierKeys.Control | winformautoclicker.ModifierKeys.Alt, Keys.NumPad8);
			*/
		}

		void hook_KeyPressed(object sender, KeyPressedEventArgs e) {
			if (autoclickthread.IsAlive) {
				autoclickthread.Abort ();
				Console.WriteLine ("Stop");
			} else {
				autoclickthread = new Thread (new ThreadStart (AutoClick.click));
				autoclickthread.Start ();
				Console.WriteLine ("Start");
			}
		}
	}
}
