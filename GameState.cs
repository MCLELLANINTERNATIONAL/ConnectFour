namespace ConnectFour;

public class GameState
{
    public enum WinState
    {
        No_Winner,
        Player1_Wins,
        Player2_Wins,
        Tie
    }

    public int PlayerTurn { get; private set; } = 1;
    public int CurrentTurn { get; private set; } = 0;

    private int[] board = new int[42];

    public void ResetBoard()
    {
        PlayerTurn = 1;
        CurrentTurn = 0;
        board = new int[42];
    }

    public int PlayPiece(byte col)
    {
        if (CheckForWin() != WinState.No_Winner)
        {
            throw new ArgumentException("Game is over");
        }

        for (int row = 6; row >= 1; row--)
        {
            int index = (row - 1) * 7 + col;

            if (board[index] == 0)
            {
                board[index] = PlayerTurn;

                int landingRow = row;
                CurrentTurn++;

                PlayerTurn = PlayerTurn == 1 ? 2 : 1;

                return landingRow;
            }
        }

        throw new ArgumentException("Column is full");
    }

    public WinState CheckForWin()
    {
        int[,] directions =
        {
            { 1, 0 },  // horizontal
            { 0, 1 },  // vertical
            { 1, 1 },  // diagonal down-right
            { 1, -1 }  // diagonal up-right
        };

        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                int player = board[row * 7 + col];

                if (player == 0)
                    continue;

                for (int d = 0; d < 4; d++)
                {
                    int count = 1;

                    for (int step = 1; step < 4; step++)
                    {
                        int newCol = col + directions[d, 0] * step;
                        int newRow = row + directions[d, 1] * step;

                        if (newCol < 0 || newCol >= 7 || newRow < 0 || newRow >= 6)
                            break;

                        if (board[newRow * 7 + newCol] == player)
                            count++;
                        else
                            break;
                    }

                    if (count == 4)
                    {
                        return player == 1
                            ? WinState.Player1_Wins
                            : WinState.Player2_Wins;
                    }
                }
            }
        }

        return CurrentTurn >= 42 ? WinState.Tie : WinState.No_Winner;
    }
}