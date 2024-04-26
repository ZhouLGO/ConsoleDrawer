using System;

namespace ConsoleStage.Tools
{
    public static class MathExtendsion
    {
        public static float Clamp(float x,float min,float max)
        {
            return Math.Max(min, Math.Min(x, max));
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static float Unlerp(float a, float b, float v)
        {
            if (a == b) return 0; // 防止除以0的异常
            return (v - a) / (b - a);
        }

        public static float Remap(float t,float source1,float source2,float target1, float target2)
        {
            return Lerp(target1, target2, Unlerp(source1, source2, t));
        }

        public static float ToRadians(float angle)
        {
            return angle * (float)Math.PI / 180;
        }
    }
}
