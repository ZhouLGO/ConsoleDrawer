using MathNet.Numerics.LinearAlgebra.Double;
using System;

namespace ConsoleStage.MathModel
{
    public class Doughnut
    {

        public float R; //The eyesToScreen between doughnut`s center whit circle`s center
        public float r; //the radius of circle

        public Doughnut(float R, float r)
        {
            this.R = R;
            this.r = r;
        }


        /// <summary>
        /// 距离原点R处有一圆环，圆环的参数方程(几何层面来看是一个静态的圆环)
        /// </summary>
        /// <param name="thetaRadians">θ角，环上某点和环心的连线与x轴的夹角大小(单位：弧度)</param>
        /// <param name="vector">从环心指向环上该点的一个一行三列矩阵</param>
        /// <returns>返回一个一行三列的新矩阵，意为在指定角度下的环上某点</returns>
        private DenseMatrix GetCirclePoint(double thetaRadians, out DenseMatrix vector)
        {
            double sinTheta = Math.Sin(thetaRadians);
            double cosTheta = Math.Cos(thetaRadians);

            vector = new DenseMatrix(1, 3, new double[] { cosTheta, sinTheta, 0 });

            double x = R + r * cosTheta;
            double y = r * sinTheta;
            double z = 0;
            return new DenseMatrix(1, 3, new double[] { x, y, z });
        }


        /// <summary>
        /// 距离原点R处有一圆环，圆环绕?轴旋转的参数方程(几何层面来看是一个静态的“甜甜圈”)
        /// </summary>
        /// <param name="rotationAxis">围绕某个轴的旋转矩阵</param>
        /// <param name="thetaRadians">θ角，环上某点和环心的连线与x轴的夹角大小(单位：弧度)</param>
        /// <param name="normal">甜甜圈上的位于该点的表面法线</param>
        /// <returns>返回一个一行三列的新矩阵，意为在指定θ角下的环上某点，在围绕?轴旋转ϕ角后的结果，也就是环上某点与旋转矩阵相乘的结果</returns>
        public DenseMatrix GetPointRotatingAroundAxis(DenseMatrix rotationAxis, double thetaRadians, out DenseMatrix normal)
        {

            DenseMatrix originPoint = GetCirclePoint(thetaRadians,out var vector);
            normal = vector * rotationAxis;
            return originPoint * rotationAxis;
        }
    }

}
