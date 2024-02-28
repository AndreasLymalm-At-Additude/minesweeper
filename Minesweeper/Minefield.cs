namespace Minesweeper;

public class Minefield
{
    private const int MaxHeight = 50;
    private const int MaxWidth = 50;

    public readonly int Height;
    public readonly int Width;
    public readonly int MineCount;
    private readonly bool[,] Mines;
    private bool IsFirstVisit = true;

    /// <summary>
    /// Represents the minefield with 
    /// -1 meaning a covered coordinate and
    /// 0-8 meaning a cleared coorinate surrounded by the number of mines shown by the digit.
    /// </summary>
    private int[,] Visited;

    public Minefield(int height, int width, int mineCount)
    {
        Height = Helper.Clamp(1, height, MaxHeight);
        Width = Helper.Clamp(1, width, MaxWidth);
        MineCount = Helper.Clamp(1, mineCount, Height * Width - 1);

        Mines = new bool[Height, Width];
        Visited = new int[Height, Width];

        Reset();
    }

    public void Reset()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Mines[y, x] = false;
                Visited[y, x] = -1;
            }
        }
        IsFirstVisit = true;
    }

    public GameState Visit(int x, int y)
    {
        if (IsFirstVisit)
        {
            PlaceMinesExcludingAroundCoordinate(x, y);
            IsFirstVisit = false;
        }

        if (Mines[y, x])
        {
            return GameState.Lose;
        }
        else
        {
            VisitAround(x, y);

            if (HasVisitedAll())
            {
                return GameState.Win;
            }
            return GameState.Ongoing;
        }
    }

    public bool IsCovered(int x, int y)
    {
        return Visited[y, x] == -1;
    }

    public int GetAdjacentMineCount(int x, int y)
    {
        return Visited[y, x];
    }

    private void PlaceMinesExcludingAroundCoordinate(int excludeX, int excludeY)
    {
        Random random = new();
        int x, y;

        for (int i = 0; i < MineCount; i++)
        {
            do
            {
                x = random.Next(0, Width);
                y = random.Next(0, Height);
            }
            while (Mines[y, x] || IsAround(excludeX, excludeY, x, y));

            Mines[y, x] = true;
        }
    }

    private bool HasVisitedAll()
    {
        int remainingCovered = 0;
        foreach (int visitedCoordinate in Visited)
        {
            if (visitedCoordinate == -1)
            {
                remainingCovered++;
            }
        }

        return remainingCovered == MineCount;
    }

    private void VisitAround(int x, int y)
    {
        Visited[y, x] = GetSurroundingMineCount(x, y);
        if (Visited[y, x] == 0)
        {
            for (int dx = Math.Max(0, x - 1); dx <= Math.Min(x + 1, Width - 1); dx++)
            {
                for (int dy = Math.Max(0, y - 1); dy <= Math.Min(y + 1, Height - 1); dy++)
                {
                    if (Visited[dy, dx] == -1)
                    {
                        VisitAround(dx, dy);
                    }
                }
            }
        }
    }

    private int GetSurroundingMineCount(int x, int y)
    {
        int reachableMineCount = 0;
        for (int dx = Math.Max(0, x - 1); dx <= Math.Min(x + 1, Width - 1); dx++)
        {
            for (int dy = Math.Max(0, y - 1); dy <= Math.Min(y + 1, Height - 1); dy++)
            {
                if (Mines[dy, dx])
                {
                    reachableMineCount++;
                }
            }
        }

        return reachableMineCount;
    }

    private bool IsAround(int xCenter, int yCenter, int xToCheck, int yToCheck)
    {
        // Handle special case, where there are too many mines
        if (MineCount >= Width * Height - 9)
        {
            return xToCheck == xCenter && yToCheck == yCenter;
        }

        for (int x = xCenter - 1; x <= xCenter + 1; x++)
        {
            for (int y = yCenter - 1; y <= yCenter + 1; y++)
            {
                if (xToCheck == x && yToCheck == y)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
