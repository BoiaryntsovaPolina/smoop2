using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5._2
{
    public enum MoveDirection { Left, Right, Up, Down }

    public class GameManager
    {
        public int Size { get; }
        public int[,] Board { get; private set; }
        public int Score { get; private set; }
        private Random rnd = new Random();

        public event EventHandler? BoardChanged;
        public event EventHandler? GameOver;

        public GameManager(int size = 4)
        {
            Size = size;
            NewGame();
        }

        public void NewGame()
        {
            Board = new int[Size, Size];
            Score = 0;
            // Дві початкові плитки
            SpawnRandomTile();
            SpawnRandomTile();
            BoardChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool Move(MoveDirection dir)
        {
            bool moved = false;
            int[,] old = (int[,])Board.Clone();

            for (int i = 0; i < Size; i++)
            {
                int[] line = GetLine(i, dir);
                int[] merged = MergeLine(line);
                SetLine(i, merged, dir);
            }

            // Перевіримо чи змінились
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (old[r, c] != Board[r, c]) moved = true;

            if (moved)
            {
                SpawnRandomTile();
                BoardChanged?.Invoke(this, EventArgs.Empty);
                if (!CanMove()) GameOver?.Invoke(this, EventArgs.Empty);
            }

            return moved;
        }

        private int[] GetLine(int index, MoveDirection dir)
        {
            int[] line = new int[Size];
            for (int i = 0; i < Size; i++)
            {
                switch (dir)
                {
                    case MoveDirection.Left: line[i] = Board[index, i]; break;
                    case MoveDirection.Right: line[i] = Board[index, Size - 1 - i]; break;
                    case MoveDirection.Up: line[i] = Board[i, index]; break;
                    case MoveDirection.Down: line[i] = Board[Size - 1 - i, index]; break;
                }
            }
            return line;
        }

        private void SetLine(int index, int[] line, MoveDirection dir)
        {
            for (int i = 0; i < Size; i++)
            {
                switch (dir)
                {
                    case MoveDirection.Left: Board[index, i] = line[i]; break;
                    case MoveDirection.Right: Board[index, Size - 1 - i] = line[i]; break;
                    case MoveDirection.Up: Board[i, index] = line[i]; break;
                    case MoveDirection.Down: Board[Size - 1 - i, index] = line[i]; break;
                }
            }
        }

        private int[] MergeLine(int[] oldLine)
        {
            List<int> list = new List<int>();
            // Стиснути (compress) — прибрати нулі
            foreach (var v in oldLine) if (v != 0) list.Add(v);

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] == list[i + 1])
                {
                    list[i] = list[i] + list[i + 1];
                    Score += list[i];
                    list.RemoveAt(i + 1);
                }
            }

            while (list.Count < Size) list.Add(0);
            return list.ToArray();
        }

        private void SpawnRandomTile()
        {
            var empty = new List<(int r, int c)>();
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (Board[r, c] == 0) empty.Add((r, c));

            if (empty.Count == 0) return;

            var pick = empty[rnd.Next(empty.Count)];
            // 90% для 2, 10% для 4
            Board[pick.r, pick.c] = (rnd.NextDouble() < 0.9) ? 2 : 4;
        }

        public bool CanMove()
        {
            // Якщо є пусті — можна рухатись
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (Board[r, c] == 0) return true;

            // Перевірити сусідні однакові для можливості злиття
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                {
                    int v = Board[r, c];
                    if (r < Size - 1 && Board[r + 1, c] == v) return true;
                    if (c < Size - 1 && Board[r, c + 1] == v) return true;
                }
            return false;
        }
    }
}
