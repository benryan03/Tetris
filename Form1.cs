﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        bool pieceIsActive;
        Control activePiece;
        int timeElapsed = 0;

        public Form1()
        {
            InitializeComponent();
            dropNewPiece();
            timer1.Start();
        }

        public void dropNewPiece()
        {
            pieceIsActive = true;
            activePiece = pictureBox6;
            activePiece.BackColor = Color.Red;
        }
            
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int currentRow = grid.GetRow(activePiece);
            int currentCol = grid.GetColumn(activePiece);
            Control oldSquare = grid.GetControlFromPosition(currentCol, currentRow);

            if (e.KeyCode == Keys.Left & currentCol > 0)
            {
                Control newSquare = grid.GetControlFromPosition(currentCol - 1, currentRow);

                if (newSquare.BackColor != Color.Red)
                {
                    oldSquare.BackColor = Color.White;
                    newSquare.BackColor = Color.Red;
                    activePiece = newSquare;
                }
            }
            else if (e.KeyCode == Keys.Right & currentCol < 9)
            {
                Control newSquare = grid.GetControlFromPosition(currentCol + 1, currentRow);

                if (newSquare.BackColor != Color.Red)
                {
                    oldSquare.BackColor = Color.White;
                    newSquare.BackColor = Color.Red;
                    activePiece = newSquare;
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                //Rotate
            }
            else if (e.KeyCode == Keys.Down & currentRow < 9)
            {
                Control newSquare = grid.GetControlFromPosition(currentCol, currentRow + 1);

                if (newSquare.BackColor != Color.Red)
                {
                    oldSquare.BackColor = Color.White;
                    newSquare.BackColor = Color.Red;
                    activePiece = newSquare;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Update timer
            timeElapsed++;
            textBox1.Text = timeElapsed.ToString();

            int currentRow = grid.GetRow(activePiece);
            int currentCol = grid.GetColumn(activePiece);
            Control oldSquare = grid.GetControlFromPosition(currentCol, currentRow);
            Control newSquare = grid.GetControlFromPosition(currentCol, currentRow + 1);

            if (currentRow == 9)
            {
                dropNewPiece();
            }
            else if (currentRow < 9 & newSquare.BackColor == Color.Red)
            {
                dropNewPiece();
            }
            else if (currentRow < 9 & newSquare.BackColor != Color.Red)
            {
                //Move piece down
                oldSquare.BackColor = Color.White;
                newSquare.BackColor = Color.Red;
                activePiece = newSquare;
            }
        }
    }
}
