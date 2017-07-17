﻿//Code written by Camron Bartlow in Francis Tuttle CSP
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewAlgorithm;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
		private int buttonX;
		private int buttonY;
		private int numUpDown1X;
		private int numUpDown2X;
		private int numUpDownY;
		private int label1X;
		private int label2X;
		private int labelY;

		private bool firstRun = true;

		public Form1()
        {
            InitializeComponent();
        }

        public void Button1_Click(object sender, EventArgs e)
        {
			if (firstRun)
			{
				firstRun = false;
				buttonX = button1.Location.X;
				buttonY = button1.Location.Y;
				numUpDown1X = numericUpDown1.Location.X;
				numUpDown2X = numericUpDown2.Location.X;
				numUpDownY = numericUpDown1.Location.Y;
				label1X = label1.Location.X;
				label2X = label2.Location.X;
				labelY = label1.Location.Y;
            }

			if (!(numericUpDown2.Value <= 1 || numericUpDown1.Value <= 1 || numericUpDown2.Value > 20 || numericUpDown1.Value > 20))
			{
				button1.Location = new Point(buttonX, buttonY + Convert.ToInt32(25 * (numericUpDown2.Value + 2)));
				numericUpDown1.Location = new Point(numUpDown1X, numUpDownY + Convert.ToInt32(25 * (numericUpDown2.Value + 2)));
				numericUpDown2.Location = new Point(numUpDown2X, numUpDownY + Convert.ToInt32(25 * (numericUpDown2.Value + 2)));
				label1.Location = new Point(label1X, labelY + Convert.ToInt32(25 * (numericUpDown2.Value + 2)));
				label2.Location = new Point(label2X, labelY + Convert.ToInt32(25 * (numericUpDown2.Value + 2)));
			}

			NewAlgorithm.NewAlgorithm.NewAlg(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value));

            //PictureBox wow = new PictureBox();
            //...
            //wow.BackgroundImage = Properties.Resources.black;
            //this.Controls.Add(wow);
        }

		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
}
