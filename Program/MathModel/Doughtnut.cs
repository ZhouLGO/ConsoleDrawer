using System;
using System.Numerics;

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
        private Vector3 GetCirclePoint(float thetaRadians, out Vector3 vector)
        {
            float sinTheta = (float)Math.Sin(thetaRadians);
            float cosTheta = (float)Math.Cos(thetaRadians);

            vector = new Vector3(cosTheta, sinTheta, 0 );

            float x = R + r * cosTheta;
            float y = r * sinTheta;
            float z = 0;
            return new Vector3( x, y, z );
        }


        /// <summary>
        /// 距离原点R处有一圆环，圆环绕?轴旋转的参数方程(几何层面来看是一个静态的“甜甜圈”)
        /// </summary>
        /// <param name="rotationAxis">围绕某个轴的旋转矩阵</param>
        /// <param name="thetaRadians">θ角，环上某点和环心的连线与x轴的夹角大小(单位：弧度)</param>
        /// <param name="normal">甜甜圈上的位于该点的表面法线</param>
        /// <returns>返回一个一行三列的新矩阵，意为在指定θ角下的环上某点，在围绕?轴旋转ϕ角后的结果，也就是环上某点与旋转矩阵相乘的结果</returns>
        public Vector3 GetPointRotatingAroundAxis(Quaternion quaternion, float thetaRadians, out Vector3 normal)
        {

            Vector3 originPoint = GetCirclePoint(thetaRadians,out var vector);
            normal = Vector3.Transform(vector, quaternion);
            return Vector3.Transform(originPoint, quaternion);
        }
    }

}
