using System;
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
            int currentRow = tableLayoutPanel1.GetRow(activePiece);
            int currentCol = tableLayoutPanel1.GetColumn(activePiece);

            if (e.KeyCode == Keys.Left & currentCol > 0)
            {
                textBox1.Text = "Left"; //Debug

                Control oldSquare = tableLayoutPanel1.GetControlFromPosition(currentCol, currentRow);
                Control newSquare = tableLayoutPanel1.GetControlFromPosition(currentCol - 1, currentRow);

                if (newSquare.BackColor != Color.Red)
                {
                    textBox1.Text = "Down"; //Debug

                    oldSquare.BackColor = Color.White;
                    newSquare.BackColor = Color.Red;

                    activePiece = newSquare;
                }
            }
            else if (e.KeyCode == Keys.Right & currentCol < 9)
            {
                textBox1.Text = "Right"; //Debug

                Control oldSquare = tableLayoutPanel1.GetControlFromPosition(currentCol, currentRow);
                Control newSquare = tableLayoutPanel1.GetControlFromPosition(currentCol + 1, currentRow);

                if (newSquare.BackColor != Color.Red)
                {
                    textBox1.Text = "Down";

                    oldSquare.BackColor = Color.White;
                    newSquare.BackColor = Color.Red;

                    activePiece = newSquare;
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                textBox1.Text = "Up"; //Debug
            }
            else if (e.KeyCode == Keys.Down & currentRow < 9)
            {
                Control oldSquare = tableLayoutPanel1.GetControlFromPosition(currentCol, currentRow);
                Control newSquare = tableLayoutPanel1.GetControlFromPosition(currentCol, currentRow + 1);

                if (newSquare.BackColor != Color.Red)
                {
                    textBox1.Text = "Down"; //Debug

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

            int currentRow = tableLayoutPanel1.GetRow(activePiece);
            int currentCol = tableLayoutPanel1.GetColumn(activePiece);

            if (currentRow < 9)
            {
                Control oldSquare = tableLayoutPanel1.GetControlFromPosition(currentCol, currentRow);
                Control newSquare = tableLayoutPanel1.GetControlFromPosition(currentCol, currentRow + 1);

                if (newSquare.BackColor == Color.Red)
                {
                    dropNewPiece();
                }
                else
                {
                    //Move piece down
                    oldSquare.BackColor = Color.White;
                    newSquare.BackColor = Color.Red;

                    activePiece = newSquare;

                    textBox2.Text = "Row: " + tableLayoutPanel1.GetRow(activePiece).ToString();
                    textBox3.Text = "Col: " + tableLayoutPanel1.GetColumn(activePiece).ToString();
                }

            }
            else if (currentRow == 9)
            {
                dropNewPiece();
            }


        }
    }
}
