using System;
using System.Text;
using System.Numerics;

namespace ConsoleStage.Tools
{
    public class Canvas
    {
        public readonly int width;  //画布的宽度(单位：控制台界面的一个字符大小)
        public readonly int height;   //画布的高度(单位：控制台界面的一个字符大小)
        public readonly float eyesToScreen; //人眼与屏幕之间的距离(K1)
        public float sreenToObj;   //“3D物体”中心到屏幕的距离(K2)
        public float eyesToObj => eyesToScreen + sreenToObj;
        public float[,] zAxisProximityRatio;  //每个字符点的深度信息缓存，因为每次计算“甜甜圈”的采样点集是前后都算的，但是绘制的时候仅需将面对屏幕最近的一些点画出来
        public char[,] charBuffer;  //由width与height构成的画布的字符缓存

        public Canvas(int width, int height, float eyes2Screen, float sreenToObj)
        {
            this.width = width;
            this.height = height;
            this.eyesToScreen = eyes2Screen;
            this.sreenToObj = sreenToObj;

            zAxisProximityRatio = new float[height, width];
            charBuffer = new char[height, width];
        }

        /// <summary>
        /// 通过3D模型的数学模型上的采样点，计算其在显示器上的投影点(使用相似三角形求解)
        /// </summary>
        /// <param name="point">采样点(含有xyz轴坐标信息（一行三列的矩阵）)</param>
        /// <returns>返回其在屏幕上的投影点</returns>
        public Vector3 CalculateScreenPoint(Vector3 point)
        {
            float depthRatio = 1 / (point.Z + eyesToObj); // 眼睛到实参点的z轴距离;
                                                                   //计算屏幕原点
            float x = eyesToScreen * point.X * depthRatio;
            float y = -(eyesToScreen * point.Y * depthRatio); //因为屏幕坐标空间的原点在左上角,所以这里y的计算结果取负值了
            Vector3 screenP = new Vector3( x, y, depthRatio );//屏幕点依然携带着深度信息

            //因为屏幕坐标空间的原点在左上角，所以施加半个窗口大小的偏移，将“表现层的甜甜圈”的中心挪到屏幕中心上
            screenP += new Vector3(width / 2, 0, 0 );
            screenP += new Vector3(0, height / 2, 0 );

            return screenP;
        }


        /// <summary>
        /// 判断新来的点是否在之前缓存的点的前面(离屏幕更近)
        /// </summary>
        /// <param name="screenPoint">屏幕上的某点</param>
        /// <returns>该点是否在其他已计算的点的上层</returns>
        public bool CheckPointInFonrt(Vector3 screenPoint)
        {
            if (0 <= screenPoint.X && screenPoint.X < width
             && 0 <= screenPoint.Y && screenPoint.Y < height)
            {
                int x_pixel = (int)screenPoint.X;    //光标之前列的字符数
                int y_pixel = (int)screenPoint.Y;   //光标当前行的字符数
                float cachedDepthRatio = zAxisProximityRatio[y_pixel, x_pixel];//当前正在刷新的目标“像素点”的深度信息
                float currentDepthRatio = screenPoint.Z;
                if (currentDepthRatio > cachedDepthRatio)
                {
                    zAxisProximityRatio[y_pixel, x_pixel] = cachedDepthRatio;
                    return true;
                }
            }
            return false;
        }

        public void UpdatePixel(Vector3 screenPoint, char value)
        {
            charBuffer[(int)screenPoint.Y, (int)screenPoint.X] = value;
        }

        public void Draw()
        {
            Console.Clear();

            StringBuilder resultBuilder = new StringBuilder();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    resultBuilder.Append($"{charBuffer[i, j]} ");//控制台行距比字距大，为了让画出来的甜甜圈不至于变扁，每个字符都追加一个空格
                };
                resultBuilder.AppendLine();
            };

            Console.Write(resultBuilder.ToString());
        }

        public void CleanCanvas()
        {
            charBuffer = new char[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    charBuffer[i, j] = ' ';
                }
            }
        }
    }
}
