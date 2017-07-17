//By Camron Bartlow with influence by ideas of random algorithms such as the not-so randomness of Bruteforcing and Prim's Algorithm
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
                } while (RoomOnAllSides(gridTable, randX, randY) && !RoomAvailableInDir(gridTable, moveX, moveY, randX, randY) && IsBorderClear(gridTable, x, y));
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
            if (table[xCheck + 1, yCheck] == true)
            {
                roomRight = true;
            }
            else if (table[xCheck - 1, yCheck] == true)
            {
                roomLeft = true;
            }
            else if (table[xCheck, yCheck + 1] == true)
            {
                roomDown = true;
            }
            else if (table[xCheck, yCheck - 1] == true)
            {
                roomUp = true;
            }

            bool[] onAllSides = { roomRight, roomLeft, roomUp, roomDown };

            return onAllSides;
        }


        static bool RoomOnAllSides(bool[,] table, int xCheck, int yCheck)
        {
            bool[] testTable = RoomOnAllSidesArr(table, xCheck, yCheck);
            if (testTable[0] || testTable[1] || testTable[2] || testTable[3])
            {
                if (testTable[0] == testTable[1] && testTable[0] == testTable[2] && testTable[0] == testTable[3])
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
                if (table[pointerX + moveInX, pointerY])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (moveInX == 0 && moveInY != 0)
            {
                if (table[pointerX, pointerY + moveInY])
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
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
                    if (array[j, i] /*&& !(j == Convert.ToInt32(Math.Floor(Convert.ToDouble((xOfArray) / 2))) && i == Convert.ToInt32(Math.Floor(Convert.ToDouble((yOfArray) / 2))))*/)
                    {
                        box.BackgroundImage = WindowsFormsApplication1.Properties.Resources.white;
                    }
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
        static bool IsBorderClear(bool[,] table, int x, int y)
        {
            bool borderClear = true;

            int rightBorder = x + 1;
            int downwardBorder = y + 1;

            for (int i = 0; i < table.GetLength(0); i++)
            {
                if (table[i, 0])
                {
                    borderClear = false;
                }
                if (table[i, downwardBorder])
                {
                    borderClear = false;
                }
            }
            for (int j = 0; j < table.GetLength(1); j++)
            {
                if (table[0, j])
                {
                    borderClear = false;
                }
                if (table[rightBorder, j])
                {
                    borderClear = false;
                }
            }
            return borderClear;
        }
    }
}