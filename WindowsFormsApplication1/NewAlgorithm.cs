//By Camron Bartlow with influence by ideas of random algorithms such as the not-so randomness of Bruteforcing and Prim's Algorithm
//TODO: Create a full image and a way to output and save it, rather than use a series of PictureBoxes
using System;
using System.Windows.Forms;
using System.Drawing;

namespace NewAlgorithm
{
    public class NewAlgorithm
    {
        public static Image[,] boxArr = null;
		public static Bitmap bitmap;
		public static PictureBox box = null;
		public static CheckState checkBox;
		public static int[] onBox = new int[2];
		public static int[] onBoxCheck = new int[2];


		/// <summary>
		/// Runs NAlgorithm and error checks when button in form is pressed
		/// </summary>
		/// <param name="formX">Integer value for width from 2-20</param>
		/// <param name="formY">Integer value for height from 2-20</param>
		public static void NewAlg(int formX, int formY)
        {
            int x = formX;
            int y = formY;

            var form = Form.ActiveForm;

			//1x1, throws crash on 2x2 doesn't look visually appealing and over 20 takes awhile to load
            if (y <= 2 || x <= 2 || y > 20 || x > 20)
            {
                MessageBox.Show("X and Y cannot be less than 3 or greater than 20, please input another number", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (boxArr != null) {
					form.Controls.Remove(box);
                }
                NAlgorithm(x, y);
            }
        }

		/// <summary>
		/// Algorithm that creates a map with a given x and y, initializes a center box and surrounds that initialized box by initializing boxes around it
		/// </summary>
		/// <param name="x">Integer value for width from 2-20</param>
		/// <param name="y">Integer value for height from 2-20</param>
		static void NAlgorithm(int x, int y)
        {
            Random rand = new Random();
            bool[,] gridTable;
            int changeX;
            int changeY;

			//Takes a random Percentage from 30-50 and multiplies it by the dimensions and leads to the number of rooms
			//or how many times the below for loop runs(as it fills the rooms w/ do while loops)
            double emptyRoomPerc = Math.Round(Convert.ToDouble(rand.Next(30, 51)));
            double numOfRooms = (x * y) - Math.Round(x * y * (emptyRoomPerc / 100));

			//gives gridTable a border aka a tile row/column on each side
            gridTable = new bool[x + 2, y + 2];

			//Set center cell to always be a room
			gridTable[Convert.ToInt32(Math.Ceiling(Convert.ToDouble((x+2) / 2))), Convert.ToInt32(Math.Ceiling(Convert.ToDouble((y+2) / 2)))] = true;
            for (int i = 0; i < numOfRooms - 1; i++)
            {
                //Keep these unchanged, 0 needs to be their starting state to work
                int randX = 0;
                int randY = 0;
                int randDir = 0;
                int moveX = 0;
                int moveY = 0;

                //TODO: pls clean these do-whiles, I'm getting an aneurysm working with them
                do
                {
                    do
                    {
						do
						{
							randX = Convert.ToInt32(Math.Round(Convert.ToDouble(rand.Next(1, x))));
							randY = Convert.ToInt32(Math.Round(Convert.ToDouble(rand.Next(1, y))));
							randDir = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rand.Next(2, 6))));
							moveX = 0;
							moveY = 0;

							//Chosen random direction
							switch (randDir)
							{
								//Right
								case 2:
									moveX = 1;
									break;
								//Left
								case 3:
									moveX = -1;
									break;
								//Down
								case 4:
									moveY = 1;
									break;
								case 5:
									//Up
									moveY = -1;
									break;
							}
							changeX = randX + moveX;
							changeY = randY + moveY;
						} while (changeX == 0 || changeX == x + 1 || changeY == 0 || changeY == y + 1);

                    } while (!gridTable[randX, randY]);// && (randX+moveX) != 0 && (randX+moveX) != 9 && (randY+moveY) != 0 && (randY+moveY) != 9);
                } while (gridTable[changeX, changeY]);
                gridTable[changeX, changeY] = true;

            }
            VisualizeArray(gridTable, x + 2, y + 2);
        }

		/// <summary>
		/// Outputs the 2D Boolean array to the Form by creating PictureBoxes
		/// </summary>
		/// <param name="array">2D Boolean array of the map</param>
		/// <param name="xOfArray">Width of map plus 2 for the border</param>
		/// <param name="yOfArray">Height of map plus 2 for the border</param>
		static void VisualizeArray(bool[,] array, int xOfArray, int yOfArray)
        {
            boxArr = new Image[xOfArray, yOfArray];

            var form = Form.ActiveForm;

			int xStart = 0;
            int yStart = 0;

			int width = xOfArray * 17;
			int height = yOfArray * 17;

			bitmap = new Bitmap(width, height);

			//Draws a PictureBox for every tile in the given 2D Boolean Array map
			//Black = false or uninitialized
			//White = true or initialized
			for (int i = 0; i < yOfArray; i++)
            {
                for (int j = 0; j < xOfArray; j++)
                {
					if (array[j, i]) {
						boxArr[j, i] = WindowsFormsApplication1.Properties.Resources.white;

						//For if you want to see the first initialized room
						if (j == Convert.ToInt32(Math.Ceiling(Convert.ToDouble((xOfArray) / 2))) && i == Convert.ToInt32(Math.Ceiling(Convert.ToDouble((yOfArray) / 2))))
						{
							if (checkBox == CheckState.Checked)
							{
								boxArr[j, i] = WindowsFormsApplication1.Properties.Resources.red;
							}
							else
							{
								boxArr[j, i] = WindowsFormsApplication1.Properties.Resources.white;
							}
						}
					}
					else
					{
						boxArr[j, i] = WindowsFormsApplication1.Properties.Resources.black;
					}
					
					using (Graphics g = Graphics.FromImage(bitmap))
					{
						g.DrawImage(boxArr[j, i], xStart, yStart);
					}

					xStart += 17;
                }
                xStart = 0;
                yStart += 17;
            }

			box = new PictureBox()
			{
				Size = new Size((int)Math.Floor((decimal)bitmap.Width * 1.47058824m), (int)Math.Floor((decimal)bitmap.Height * 1.47058824m)),
				Location = new Point(25, 25)
			};

			Bitmap resized = new Bitmap((Image)bitmap, new Size((int)Math.Floor((decimal)bitmap.Width * 1.47058824m), (int)Math.Floor((decimal)bitmap.Height * 1.47058824m)));
			using (Graphics g = Graphics.FromImage(resized))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

				g.DrawImage(bitmap, 0, 0, (int)Math.Floor((decimal)bitmap.Width * 1.47058824m), (int)Math.Floor((decimal)bitmap.Height * 1.47058824m));
			}
			bitmap = resized;
			box.BackgroundImage = bitmap;

			form.Controls.Add(box);
		}

		public static void SaveImg()
		{
			if (bitmap != null) {
				SaveFileDialog f = new SaveFileDialog()
				{
					DefaultExt = "png",
					AddExtension = true,
					Filter = "BMP files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*",
					FilterIndex = 5,
					RestoreDirectory = true,
					FileName = "map"
				};
				if (f.ShowDialog() == DialogResult.OK)
				{
					bitmap.Save(f.FileName);
				}
			}
			else
			{
				MessageBox.Show("The Algorithm must be run at least once.", "Error",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}//Class
}//Namespace
