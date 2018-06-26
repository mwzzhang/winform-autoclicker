using System;
using System.Drawing;
using System.Windows.Forms;

namespace winformautoclicker
{
	public class MainWindow : Form
	{
		public static void Main()
		{
			Application.Run (new MainWindow ());
		}

		public MainWindow()
		{
			Button b = new Button ();
			b.Text = "Click Me!";
			b.Click += new EventHandler (Button_Click);
			Controls.Add (b);
		}

		private void Button_Click(object sender, EventArgs e)
		{
			MessageBox.Show ("Button Clicked!");
		}
	}
}

