//Code written by Camron Bartlow in a single day
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

        static void NAlgorithm(int x, int y)
        {
            Random rand = new Random();
            bool[,] gridTable;
            int changeX;
            int changeY;

            //Sprite[] sprites = (Sprite[])Resources.LoadAll<Sprite>("GridSprites");

            double emptyRoomPerc = Math.Round(Convert.ToDouble(rand.Next(30, 51)));
            double numOfRooms = (x * y) - Math.Round(x * y * (emptyRoomPerc / 100));

            //Debug.Log ("numOfRooms: " + numOfRooms);
            gridTable = new bool[x + 2, y + 2];
            //Set center cell to always be a room
            gridTable[Convert.ToInt32(Math.Floor(Convert.ToDouble((x) / 2))), Convert.ToInt32(Math.Floor(Convert.ToDouble((y) / 2)))] = true;
            for (int i = 0; i < numOfRooms - 1; i++)
            {
                int randX = 0;
                int randY = 0;
                int randDir = 0;
                int moveX = 0;
                int moveY = 0;
                int instances = 0;
                do
                {
                    do
                    {
                        do
                        {
                            randX = Convert.ToInt32(Math.Round(Convert.ToDouble(rand.Next(1, x + 1))));
                            randY = Convert.ToInt32(Math.Round(Convert.ToDouble(rand.Next(1, y + 1))));
                            randDir = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rand.Next(2, 6))));
                            moveX = 0;
                            moveY = 0;
                            if (randDir == 2)
                            {
                                moveX = 1;
                            }
                            else if (randDir == 3)
                            {
                                moveX = -1;
                            }
                            else if (randDir == 4)
                            {
                                moveY = 1;
                            }
                            else if (randDir == 5)
                            {
                                moveY = -1;
                            }
                            changeX = randX + moveX;
                            changeY = randY + moveY;

                            while (changeX == 0 || changeX == x + 1 || changeY == 0 || changeY == y + 1)
                            {
                                randX = Convert.ToInt32(Math.Round(Convert.ToDouble(rand.Next(1, x))));
                                randY = Convert.ToInt32(Math.Round(Convert.ToDouble(rand.Next(1, y))));
                                randDir = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(rand.Next(2, 6))));
                                moveX = 0;
                                moveY = 0;
                                if (randDir == 2)
                                {
                                    moveX = 1;
                                }
                                else if (randDir == 3)
                                {
                                    moveX = -1;
                                }
                                else if (randDir == 4)
                                {
                                    moveY = 1;
                                }
                                else if (randDir == 5)
                                {
                                    moveY = -1;
                                }
                                changeX = randX + moveX;
                                changeY = randY + moveY;
                            }

                            instances++;
                        } while (!gridTable[randX, randY]);// && (randX+moveX) != 0 && (randX+moveX) != 9 && (randY+moveY) != 0 && (randY+moveY) != 9);
                    } while (gridTable[changeX, changeY]);
                } while (RoomOnAllSides(gridTable, randX, randY) && !RoomAvailableInDir(gridTable, moveX, moveY, randX, randY) && IsBorderClear(gridTable, x, y));
                gridTable[changeX, changeY] = true;

            }
            VisualizeArray(gridTable, x + 2, y + 2);
        }
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
                    PictureBox box = new PictureBox();
                    box.Size = new System.Drawing.Size(25, 25);
                    box.Location = new System.Drawing.Point(xStart, yStart);
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