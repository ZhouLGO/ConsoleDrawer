using System;

namespace ConsoleStage.Tools
{
    public static class MathExtendsion
    {
        public static double Clamp(double x,double min,double max)
        {
            return Math.Max(min, Math.Min(x, max));
        }

        public static double Lerp(double a, double b, double t)
        {
            return a + (b - a) * t;
        }

        public static double Unlerp(double a, double b, double v)
        {
            if (a == b) return 0; // 防止除以0的异常
            return (v - a) / (b - a);
        }

        public static double Remap(double t,double source1,double source2,double target1, double target2)
        {
            return Lerp(target1, target2, Unlerp(source1, source2, t));
        }

    }
}
