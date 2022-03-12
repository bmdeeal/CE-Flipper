using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;

/*
	CE-Flipper -- a really, really basic animation program for Windows CE.
	(C) 2022 B.M.Deeal <brenden.deeal@gmail.com>
	Tested on a device running CE2.0 with the .NET Compact Framework installed.
	
	The file format is extremely simple:
	* a .anim file (eg, "my-anim.anim")
	* a set of .bmp format images (eg, "my-anim.anim-00001.bmp")
	All must be in the same folder. The drawing area is 240x120 -- larger images will be cropped.
	The .anim file contains no information inside and is just used to identify an image set as an animation.
	
	By default, animations run with about 330ms of delay between frames. This can be changed in the menu.
	
	This program was written for a particularly slow handheld system and is a quick and dirty hack.
	Faster CE devices can just use a real video player like TCPMP and real video formats.
	
	TODO:
	* attempt to speed up loading?
	* maybe add an alternate format where each image is made of a stack of sub-images? that actually seems like a good idea
	* the .anim file wouldn't be blank
*/
namespace ce_flipper
{
	/// <summary>
	/// Main form
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private string loaded="";
		private bool ready=false;
		private Bitmap current;
		private Bitmap previous;
		private bool loop=true;
		private int frame=1;

		private System.Windows.Forms.PictureBox pictureBoxArea;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItemLoop;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItemAbout;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem9;
		
		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.timer1 = new System.Windows.Forms.Timer();
			this.pictureBoxArea = new System.Windows.Forms.PictureBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItemLoop = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItemAbout = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 330;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// pictureBoxArea
			// 
			this.pictureBoxArea.Location = new System.Drawing.Point(8, 32);
			this.pictureBoxArea.Size = new System.Drawing.Size(240, 120);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.Add(this.menuItem1);
			this.mainMenu1.MenuItems.Add(this.menuItem6);
			this.mainMenu1.MenuItems.Add(this.menuItem10);
			// 
			// menuItem1
			// 
			this.menuItem1.MenuItems.Add(this.menuItem2);
			this.menuItem1.MenuItems.Add(this.menuItem5);
			this.menuItem1.MenuItems.Add(this.menuItem12);
			this.menuItem1.MenuItems.Add(this.menuItem4);
			this.menuItem1.MenuItems.Add(this.menuItem3);
			this.menuItem1.Text = "File";
			// 
			// menuItem2
			// 
			this.menuItem2.Text = "L&oad";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Text = "&Play (from start)";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Text = "Pause/&Stop";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Text = "E&xit";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.MenuItems.Add(this.menuItemLoop);
			this.menuItem6.MenuItems.Add(this.menuItem11);
			this.menuItem6.MenuItems.Add(this.menuItem8);
			this.menuItem6.MenuItems.Add(this.menuItem7);
			this.menuItem6.MenuItems.Add(this.menuItem9);
			this.menuItem6.Text = "Options";
			// 
			// menuItemLoop
			// 
			this.menuItemLoop.Checked = true;
			this.menuItemLoop.Text = "&Loop";
			this.menuItemLoop.Click += new System.EventHandler(this.menuItemLoop_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Text = "Delay 330ms (&normal)";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Text = "Delay 110ms (&fast)";
			this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Text = "Delay 660ms (&slow)";
			this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.MenuItems.Add(this.menuItemAbout);
			this.menuItem10.Text = "Help";
			// 
			// menuItemAbout
			// 
			this.menuItemAbout.Text = "&About";
			this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Text = "Delay 55ms (&very fast)";
			this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Text = "Pl&ay (from current)";
			this.menuItem12.Click += new System.EventHandler(this.menuItem12_Click);
			// 
			// Form1
			// 
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(258, 159);
			this.Controls.Add(this.pictureBoxArea);
			this.Menu = this.mainMenu1;
			this.Text = "CE-Flipper Animation Player";
			this.Load += new System.EventHandler(this.Form1_Load);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			timer1.Enabled=false;
			if (ready) 
			{
				try 
				{
					previous=current;
					current = new Bitmap(loaded+"-"+frame.ToString("D5")+".bmp");
					pictureBoxArea.Image = current;
					previous.Dispose();
					frame=frame+1;
					timer1.Enabled=true;
				}
				//deal with problems
				catch (Exception)
				{
					//if we have a problem on the first frame, complain loudly
					if (frame==1)  
					{
						MessageBox.Show("Error playing animation!");
						ready=false;
					}
					//if we're not looping, stop playback
					else if (!loop) 
					{
						ready=false;
					}
					//if we are looping, go back to the start
					else 
					{
						frame=1;
						timer1.Enabled=true;
					}
				}
			}
		}

		//load menu option
		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			//ask for the file
			OpenFileDialog dialog=new OpenFileDialog();
			dialog.Filter="CE-Flipper Animations (*.anim)|*.anim|All files (*.*)|*.*";
			dialog.ShowDialog();
			if (dialog.FileName!="") 
			{
				frame=1;
				loaded=dialog.FileName;
				timer1.Enabled=false;
				try 
				{
					current = new Bitmap(loaded+"-"+frame.ToString("D5")+".bmp");
					pictureBoxArea.Image=current;
				}
				catch (Exception)
				{
					ready=false;
					timer1.Enabled=false;
					MessageBox.Show("Error loading animation!");
				}
			}
		}

		//exit menu option
		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		//stop menu option
		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			timer1.Enabled=false;
			ready=false;
		}

		//play menu option
		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			frame=1;
			ready=true;
			timer1.Enabled=true;
		}

		//normal delay menu option
		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			timer1.Interval=330;
		}

		//fast delay menu option
		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			timer1.Interval=110;
		}

		//slow delay menu option
		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			timer1.Interval=660;
		}

		//toggle looping
		private void menuItemLoop_Click(object sender, System.EventArgs e)
		{
			loop=!loop;
			menuItemLoop.Checked=loop;
		}

		//credits
		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("CE-Flipper - (C) 2022 by B.M.Deeal\n<brenden.deeal@gmail.com>\nCE-Flipper is distributed under the ISC license, see license.txt for details.\nCheck readme.txt for usage info.");
		}

		//very fast delay menu option
		private void menuItem11_Click(object sender, System.EventArgs e)
		{
			timer1.Interval=55;
		}

		//continue playback from current frame
		private void menuItem12_Click(object sender, System.EventArgs e)
		{
			ready=true;
			timer1.Enabled=true;
		}
	}
}
