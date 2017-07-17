//By Camron Bartlow with influence by ideas of random algorithms such as the not-so randomness of Bruteforcing and Prim's Algorithm
//TODO: Create a full image and a way to output and save it, rather than use a series of PictureBoxes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;

namespace NewAlgorithm
{
    public class NewAlgorithm
    {
        public static PictureBox[,] boxArr = null;

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

			//1x1 doesn't look visually appealing and over 20 takes awhile to load
            if (y <= 1 || x <= 1 || y > 20 || x > 20)
            {
                MessageBox.Show("X and Y cannot be less than 2 or greater than 20, please input another number", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (boxArr != null) {
                    for (int i = 0; i < boxArr.GetLength(1); i++) {
                        for (int j = 0; j < boxArr.GetLength(0); j++) {
                            form.Controls.Remove(boxArr[j, i]);
                        }
                    }
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

            double emptyRoomPerc = Math.Round(Convert.ToDouble(rand.Next(30, 51)));
            double numOfRooms = (x * y) - Math.Round(x * y * (emptyRoomPerc / 100));

            gridTable = new bool[x + 2, y + 2];
            //Set center cell to always be a room
            gridTable[Convert.ToInt32(Math.Floor(Convert.ToDouble((x) / 2))), Convert.ToInt32(Math.Floor(Convert.ToDouble((y) / 2)))] = true;
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
                } while (RoomOnAllSides(gridTable, randX, randY) && !RoomAvailableInDir(gridTable, moveX, moveY, randX, randY));
                gridTable[changeX, changeY] = true;

            }
            VisualizeArray(gridTable, x + 2, y + 2);
        }

		/// <summary>
		/// Function made to abstract checks for Initilized tiles on all 4 sides(Up, Down, Left, and Right)
		/// </summary>
		/// <param name="table">2D Boolean Array of the map</param>
		/// <param name="xCheck"></param>
		/// <param name="yCheck"></param>
		/// <returns></returns>
        static bool[] RoomOnAllSidesArr(bool[,] table, int xCheck, int yCheck)
        {
            bool roomRight = false;
            bool roomLeft = false;
            bool roomUp = false;
            bool roomDown = false;

			//In place as it can check null areas
            if (table[xCheck + 1, yCheck] == true)
            {
                roomRight = true;
            }
            if (table[xCheck - 1, yCheck] == true)
            {
                roomLeft = true;
            }
            if (table[xCheck, yCheck + 1] == true)
            {
                roomDown = true;
            }
            if (table[xCheck, yCheck - 1] == true)
            {
                roomUp = true;
            }

            bool[] onAllSides = { roomRight, roomLeft, roomUp, roomDown };

            return onAllSides;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="xCheck"></param>
		/// <param name="yCheck"></param>
		/// <returns></returns>
        static bool RoomOnAllSides(bool[,] table, int xCheck, int yCheck)
        {
            bool[] testTable = RoomOnAllSidesArr(table, xCheck, yCheck);
            if (testTable[0] || testTable[1] || testTable[2] || testTable[3])
            {
				return testTable[0] == testTable[1] && testTable[0] == testTable[2] && testTable[0] == testTable[3];
            }
            else
            {
                return false;
            }
        }

        static bool RoomAvailableInDir(bool[,] table, int moveInX, int moveInY, int pointerX, int pointerY)
        {
            if (moveInX != 0 && moveInY == 0)
            {
				return !table[pointerX + moveInX, pointerY];
			}
            else if (moveInX == 0 && moveInY != 0)
            {
				return !table[pointerX, pointerY + moveInY];
			}
            else
            {
                return false;
            }
        }

		/// <summary>
		/// Outputs the 2D Boolean array to the Form by creating PictureBoxes
		/// </summary>
		/// <param name="array"></param>
		/// <param name="xOfArray"></param>
		/// <param name="yOfArray"></param>
        static void VisualizeArray(bool[,] array, int xOfArray, int yOfArray)
        {
            boxArr = new PictureBox[xOfArray, yOfArray];

            var form = Form.ActiveForm;

			int xStart = 0;
            int yStart = 25;
            for (int i = 0; i < yOfArray; i++)
            {
                for (int j = 0; j < xOfArray; j++)
                {
                    xStart += 25;
					PictureBox box = new PictureBox()
					{
						Size = new System.Drawing.Size(25, 25),
						Location = new System.Drawing.Point(xStart, yStart)
					};
					boxArr[j, i] = box;
                    if (array[j, i])
                    {
                        box.BackgroundImage = WindowsFormsApplication1.Properties.Resources.white;
                    }

					//Uncomment if you want to see the first initialized room
					/*else if (j == Convert.ToInt32(Math.Floor(Convert.ToDouble((xOfArray) / 2))) && i == Convert.ToInt32(Math.Floor(Convert.ToDouble((yOfArray) / 2))))
					{
						box.BackgroundImage = WindowsFormsApplication1.Properties.Resources.red;
					}*/

                    else
                    {
                        box.BackgroundImage = WindowsFormsApplication1.Properties.Resources.black;
                    }
                    form.Controls.Add(box);
                }
                xStart = 0;
                yStart += 25;
            }
        }
    }//Class
}//Namespace