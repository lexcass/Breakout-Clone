using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout
{
    public static class LevelLoader
    {
        private const int MAX_BLOCK_COUNT = 128;
        private const int ROWS = 8;
        private const int COLS = 16;

        public static List<Block> Load(string levelName, Texture2D blockTex)
        {
            List<Block> blocks = new List<Block>();

            // Read level data from text file
            StreamReader fileReader = new StreamReader("Content/levels/" + levelName + ".txt");
            string[] delimiter = { ",", System.Environment.NewLine };
            string[,] colors = new string[ROWS, COLS];
            int row = 0;

            try
            {
                do
                {
                    string line = fileReader.ReadLine();
                    string[] data = line.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                    if (data.Length < COLS) throw new Exception("Every row must have 16 columns. To represent the absence of a block, use 'x'.");
                    if (row > ROWS) throw new Exception("There is only a max of 8 rows. Please remove some rows.");

                    for (int c = 0; c < COLS; c++)
                    {
                        colors[row, c] = data[c];
                    }

                    row++;
                } while (fileReader.Peek() != -1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
            finally
            {
                fileReader.Close();
            }

            // Create the blocks based on this data
            int cols = 16;
            int width = blockTex.Width;
            int height = blockTex.Height;

            if (colors.Length > MAX_BLOCK_COUNT)
            {
                throw new Exception("Number of blocks in a level can't exceed " + MAX_BLOCK_COUNT);
            }
            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < COLS; c++)
                {
                    // Red by default
                    Color blockColor = Color.Red;

                    if (colors[r, c] == "r")
                    {
                        blockColor = Color.Red;
                    }
                    else if (colors[r, c] == "b")
                    {
                        blockColor = Color.Blue;
                    }
                    else if (colors[r, c] == "g")
                    {
                        blockColor = Color.Green;
                    }
                    else if (colors[r, c] == "y")
                    {
                        blockColor = Color.Yellow;
                    }
                    else
                    {
                        continue;
                    }

                    int x = c * width;
                    int y = r * height;

                    blocks.Add(new Block(x, y, blockTex, blockColor));
                }
            }

            return blocks;
        }
    }
}
