using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleStage.Tools
{
    public class Canvas
    {
        public readonly int width;  //画布的宽度(单位：控制台界面的一个字符大小)
        public readonly int height;   //画布的高度(单位：控制台界面的一个字符大小)
        public readonly float eyesToScreen; //人眼与屏幕之间的距离(K1)
        public float sreenToObj;   //“3D物体”中心到屏幕的距离(K2)
        public float eyesToObj => eyesToScreen + sreenToObj;
        public double[,] zAxisProximityRatio;  //每个字符点的深度信息缓存，因为每次计算“甜甜圈”的采样点集是前后都算的，但是绘制的时候仅需将面对屏幕最近的一些点画出来
        public char[,] charBuffer;  //由width与height构成的画布的字符缓存

        public Canvas(int width, int height, float eyes2Screen, float sreenToObj)
        {
            this.width = width;
            this.height = height;
            this.eyesToScreen = eyes2Screen;
            this.sreenToObj = sreenToObj;

            zAxisProximityRatio = new double[height, width];
            charBuffer = new char[height, width];
        }

        /// <summary>
        /// 通过3D模型的数学模型上的采样点，计算其在显示器上的投影点(使用相似三角形求解)
        /// </summary>
        /// <param name="point">采样点(含有xyz轴坐标信息（一行三列的矩阵）)</param>
        /// <returns>返回其在屏幕上的投影点</returns>
        public DenseMatrix CalculateScreenPoint(DenseMatrix point)
        {
            double depthRatio = 1 / (point.At(0, 2) + eyesToObj); // 眼睛到实参点的z轴距离;
                                                                   //计算屏幕原点
            double x = eyesToScreen * point.At(0, 0) * depthRatio;
            double y = -(eyesToScreen * point.At(0, 1) * depthRatio); //因为屏幕坐标空间的原点在左上角,所以这里y的计算结果取负值了
            DenseMatrix screenP = new DenseMatrix(1, 3, new double[3] { x, y, depthRatio });//屏幕点依然携带着深度信息

            //因为屏幕坐标空间的原点在左上角，所以施加半个窗口大小的偏移，将“表现层的甜甜圈”的中心挪到屏幕中心上
            screenP += new DenseMatrix(1, 3, new double[3] { width / 2, 0, 0 });
            screenP += new DenseMatrix(1, 3, new double[3] { 0, height / 2, 0 }); ;

            return screenP;
        }


        /// <summary>
        /// 判断新来的点是否在之前缓存的点的前面(离屏幕更近)
        /// </summary>
        /// <param name="screenPoint">屏幕上的某点</param>
        /// <returns>该点是否在其他已计算的点的上层</returns>
        public bool CheckPointInFonrt(Matrix screenPoint)
        {
            if (0 <= screenPoint.At(0, 0) && screenPoint.At(0, 0) < width
             && 0 <= screenPoint.At(0, 1) && screenPoint.At(0, 1) < height)
            {
                int x_pixel = (int)screenPoint.At(0, 0);    //光标之前列的字符数
                int y_pixel = (int)screenPoint.At(0, 1);   //光标当前行的字符数
                double cachedDepthRatio = zAxisProximityRatio[y_pixel, x_pixel];//当前正在刷新的目标“像素点”的深度信息
                double currentDepthRatio = screenPoint.At(0, 2);
                if (currentDepthRatio > cachedDepthRatio)
                {
                    zAxisProximityRatio[y_pixel, x_pixel] = cachedDepthRatio;
                    return true;
                }
            }
            return false;
        }

        public void UpdatePixel(Matrix screenPoint, char value)
        {
            charBuffer[(int)screenPoint.At(0, 1), (int)screenPoint.At(0, 0)] = value;
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
