using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainWindow : Form
    {
        // Display gray preview of hard drop position
        // Needs cleanup
        private void DrawGhost()
        {
            // Ghost2 list is test position of Ghost
            // Ghost list is actual ghost position, after testing
            Control[] Ghost2 = { null, null, null, null };
            bool ghostFound = false;

            // Erase previous Ghost
            foreach (Control x in Ghost)
            {
                if (x != null)
                {
                    if (x.BackColor == Color.LightGray)
                    {
                        x.BackColor = Color.White;
                    }
                }
            }

            // Copy activePiece to Ghost2
            for (int x = 0; x < 4; x++)
            {
                Ghost2[x] = activePiece2[x];
            }

            // Test Ghost2 in each row
            for (int x = 21; x > 1; x--)
            {
                // Get position of test Ghost2, starting at bottom row
                if (currentPiece == 0) //I piece
                {
                    if (rotations == 0)
                    {
                        if (x == 2)
                        {
                            Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        }
                        else
                        {
                            Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                            Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 2);
                            Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 3);
                        }
                    }
                    else if (rotations == 1)
                    {
                        if (x == 2) //ignore
                        {
                            Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        }

                        else //problem
                        {
                            Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                            Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                            Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                            Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                        }
                    }
                }
                else if (currentPiece == 1) // L piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 2);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                    else if (rotations == 2)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 2);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 2);
                    }
                    else if (rotations == 3)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 1);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                }
                else if (currentPiece == 2) // J piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 2);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 1);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 2)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 2);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 2);
                    }
                    else if (rotations == 3)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                }
                else if (currentPiece == 3) // S piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 1);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 2);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                }
                else if (currentPiece == 4) // Z piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 1);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 2);
                    }
                }
                else if (currentPiece == 5) // O piece
                {
                    Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                    Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                    Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                    Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                }
                else if (currentPiece == 6) //T piece
                {
                    if (rotations == 0)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                    else if (rotations == 1)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 2);
                    }
                    else if (rotations == 2)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x - 1);
                    }
                    else if (rotations == 3)
                    {
                        Ghost2[0] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[0]), x - 1);
                        Ghost2[1] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[1]), x - 1);
                        Ghost2[2] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[2]), x - 2);
                        Ghost2[3] = grid.GetControlFromPosition(grid.GetColumn(Ghost2[3]), x);
                    }
                }

                //If no valid ghost stored
                if (ghostFound == false)
                {
                    // If all squares in test Ghost2 are white,
                    if (
                        (Ghost2[0].BackColor == Color.White | activePiece.Contains(Ghost2[0])) &
                        (Ghost2[1].BackColor == Color.White | activePiece.Contains(Ghost2[1])) &
                        (Ghost2[2].BackColor == Color.White | activePiece.Contains(Ghost2[2])) &
                        (Ghost2[3].BackColor == Color.White | activePiece.Contains(Ghost2[3]))
                        )
                    {

                        // Store Ghost
                        ghostFound = true;
                        for (int y = 0; y < 4; y++)
                        {
                            Ghost[y] = Ghost2[y];
                        }
                    }

                    // If not all white (and nothing stored) check the next row up
                    else
                    {
                        continue;
                    }
                }

                //valid ghost already stored
                else if (ghostFound == true)
                {

                    //Not all squares white
                    if (Ghost2[0].BackColor != Color.White | Ghost2[1].BackColor != Color.White | Ghost2[2].BackColor != Color.White | Ghost2[3].BackColor != Color.White)
                    {

                        //Is falling piece below x?
                        if (grid.GetRow(activePiece[0]) >= x | grid.GetRow(activePiece[1]) >= x | grid.GetRow(activePiece[2]) >= x | grid.GetRow(activePiece[3]) >= x)
                        {
                            continue;
                        }


                        //Reset
                        ghostFound = false;
                        for (int y = 0; y < 4; y++)
                        {
                            Ghost[y] = null;
                        }
                        continue;
                    }
                }
            }

            //Draw ghost
            if (ghostFound == true)
            {
                for (int x = 0; x < 4; x++)
                {
                    Ghost[x].BackColor = Color.LightGray;
                }
            }
        }
    }
}