using MathNet.Numerics.LinearAlgebra.Double;

namespace ConsoleStage.Tools
{
    public class Shader
    {
        public readonly static string lightList = ".,-~:;=!*#$@";//光强列表

        public bool CalculateLight(Matrix normal, Light light, out char pixelChar)
        {
            double lightValue = (normal * light.LightDirection).At(0, 0);//点乘的代数表达
            int normOfNormal = 1;//不用求normal.FrobeniusNorm()，因为环上法线的模长:（Sin(a)^2 + Cos(a)^2）^(1/2) = 1，一系列旋转运算并不会改变其大小
            double normOfLightDir = light.LightDirection.FrobeniusNorm();//光强的模长
            double extremum = normOfNormal * normOfLightDir;

            double level = MathExtendsion.Remap(lightValue, -extremum, extremum, 0, lightList.Length);

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
