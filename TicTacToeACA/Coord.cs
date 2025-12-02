namespace TicTacToeACA;

public struct Coord
{
    public int X { get; set; }
    public int Y { get; set; }

    public Coord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Coord Clamp(Coord coord, Coord min, Coord max)
    {
        return new Coord(Math.Clamp(coord.X, min.X, max.X), Math.Clamp(coord.Y, min.Y, max.Y));
    }

    public static Coord operator +(Coord left, Coord right)
    {
        return new Coord(left.X + right.X, left.Y + right.Y);
    }

    public static Coord operator -(Coord left, Coord right)
    {
        return new Coord(left.X - right.X, left.Y - right.Y);
    }

    public static bool operator ==(Coord left, Coord right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    public static bool operator !=(Coord left, Coord right)
    {
        return !(left == right);
    }

    public static Coord Zero => new Coord(0, 0);
    public static Coord One => new Coord(1, 1);
    public static Coord Up => new Coord(0, -1);
    public static Coord Down => new Coord(0, 1);
    public static Coord Left => new Coord(-1, 0);
    public static Coord Right => new Coord(1, 0);
}