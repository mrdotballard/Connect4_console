using System;

namespace HasWon
{
    class Program
    {
        //7 x 6
       static string[,] board = new string[7, 6];

        static void Main(string[] args)
        {
            string turn = "red";
            string input = "";
            bool result;
            PrintBoard();

            while (!input.Equals("exit"))
            {
                Console.WriteLine(turn + "'s turn...");
                Console.Write("Enter move coordinates as one number (e.g. 35):  ");
                input = Console.ReadLine();

                if (input.Equals("exit"))
                    break;

                char[] coord = input.ToCharArray();
                
                if(!int.TryParse(input, out _) || coord.Length < 2)
                {
                    Console.WriteLine("Invalid coordinates; try again.");
                    continue;
                }

                int x = (int)Char.GetNumericValue(coord[0]);
                int y = (int)Char.GetNumericValue(coord[1]);

                if((x < 0 || x > 6) || (y < 0 || y > 5) || (y > 0 && board[x, y-1] == null) || board[x, y] != null)
                {
                    Console.WriteLine("Invalid coordinates; try again.");
                    continue;
                }

                result = HasWon(x, y, turn);

                if (result)
                {
                    Console.Clear();
                    PrintBoard();
                    Console.WriteLine(turn + " won!");
                    break;
                }
                else
                {
                    turn = turn.Equals("red") ? "yellow" : "red";
                }
                Console.Clear();
                PrintBoard();
            }
        }


        static bool HasWon(int x, int y, string color)
        {
            board[x, y] = color;

            int leftBound = Math.Max(x - 3, 0),
                rightBound = Math.Min(x + 3, 6),
                upperBound = Math.Min(y + 3, 5),
                lowerBound = Math.Max(y - 3, 0),
                counter = 0;

            //horizontal = checking spaces left/right
            for(int i = leftBound; i <= rightBound; i++)
            {
                if (board[i, y] != color)
                    counter = 0;
                else
                    counter++;

                if(counter == 4)
                    return (true);
            }

            
            //vertical = checking spaces above/below
            counter = 0;
            for(int i = lowerBound; i <= upperBound; i++)
            {
                if (board[x, i] != color)
                    counter = 0;
                else
                    counter++;

                if (counter == 4)
                    return (true);
            }

            //diagonal forwards
            //if top left or bottom right corner don't check forward diagonal)
            int cornerCheck = Math.Abs(x - y);
            if (cornerCheck >= 3 || cornerCheck >= 4)
                goto skip1;

            counter = 0;
            int gridCheck1 = x - (y - lowerBound); //find X distance to left of Y axis
            int startCordX = Math.Max(gridCheck1, 0); //adjust for edge cases
            int startCordY = gridCheck1 < 0 ? lowerBound + Math.Abs(gridCheck1) : lowerBound;

            for (int i = startCordY; i <= upperBound; i++)
            {
                if (board[startCordX, i] != color)
                    counter = 0;
                else
                    counter++;
                if (counter == 4)
                    return (true);
                startCordX++;
                if (startCordX > rightBound)
                    break;
            }

        skip1:
            //diagonal backwards
            counter = 0;
            int gridCheck2 = x + (y - lowerBound); //find X distance to right of Y axis
            startCordX = Math.Min(gridCheck2, 6); //adjust for edge cases
            startCordY = gridCheck2 > 6 ? lowerBound + gridCheck2 - 6 : lowerBound;
            
            for (int i = startCordY; i <= upperBound; i++)
            {
                if (board[startCordX, i] != color)
                    counter = 0;
                else
                    counter++;
                if (counter == 4)
                    return (true);
                startCordX--;
                if (startCordX < leftBound)
                    break;
            }

            return false;
        }

        static void PrintBoard()
        {
            for (int i = 5; i >= 0; i--)
            {
                Console.Write(i + "\t");
                for (int j = 0; j < 7; j++)
                {
                    if (board[j, i] == "red")
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(board[j, i] + "\t");
                    Console.ResetColor();
                }
                Console.WriteLine(Environment.NewLine);
            }

            Console.Write("\t");
            for (int i = 0; i < 7; i++)
            {
                Console.Write(i + "\t");
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}
