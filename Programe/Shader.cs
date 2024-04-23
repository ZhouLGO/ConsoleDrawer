using System;
using System.Numerics;

namespace ConsoleStage.Tools
{
    public class Shader
    {
        public readonly static string lightList = ".,-~:;=!*#$@";//光强列表

        public bool CalculateLight(Vector3 normal, Light light, out char pixelChar)
        {
            float lightValue = (normal * light.LightDirection).X;//点乘的代数表达
            int normOfNormal = 1;//不用求normal.FrobeniusNorm()，因为环上法线的模长:（Sin(a)^2 + Cos(a)^2）^(1/2) = 1，一系列旋转运算并不会改变其大小
            float normOfLightDir = light.LightDirection.Length();//光强的模长
            float extremum = normOfNormal * normOfLightDir;

            float level = MathExtendsion.Remap(lightValue, -extremum, extremum, 0, lightList.Length);

            if (level > 0)
            {
                int index = (int)MathExtendsion.Clamp(level, 0, lightList.Length - 1);
                pixelChar = lightList[index];
                return true;
            }
            else
            {
                pixelChar = ' ';
                return false;
            }
        }

    }
}
