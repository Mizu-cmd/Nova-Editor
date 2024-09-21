using System;

namespace Nova_Engine.Util.Gizmos;

public class MathUtils
{
    public static float DotProduct(float x1, float y1, float x2, float y2) => x1 * x2 + y1 * y2;
    public static double DotProduct(double x1, double y1, double x2, double y2) => x1 * x2 + y1 * y2;
    public static double Magnitude(double x1, double y1, double x2, double y2) => Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    public static float Magnitude(float x1, float y1, float x2, float y2) => (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
}