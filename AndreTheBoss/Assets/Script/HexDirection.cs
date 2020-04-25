

public enum HexDirection
{
    NE,
    E,
    SE,
    SW,
    W,
    NW,
};

public static class HexDirectionExtension
{
    public static HexDirection Opposite(this HexDirection direction)
    {
        return ((int)direction >= 3) ? ((HexDirection)direction - 3) : ((HexDirection)direction + 3);
    }

    public static HexDirection PrevDirection(this HexDirection direction)
    {
        return (direction == HexDirection.NE) ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection NextDirection(this HexDirection direction)
    {
        return (direction == HexDirection.NW) ? HexDirection.NE : (direction + 1);
    }
}
