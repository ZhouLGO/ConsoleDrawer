using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ConsoleStage.Tools
{
    public class Light
    {
        public DenseMatrix LightDirection { get; private set; }
        public Light(float x, float y, float z)
        {
            LightDirection = new DenseMatrix(3,1,new double[] { x,y,z});
        }
    }
}
