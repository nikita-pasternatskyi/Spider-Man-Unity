public static class FloatExtensions
{
    public static bool SameSign(this System.Single a, float b)
    {
        return a * b >= 0f;
    }
}
