using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomRotate
{
    struct Rotation
    {
        //A: [0, 2Pi)
        //B: [0, Pi], not linear
        //C: [0, 2Pi)
        public float A, B, C;

        public void RotateAtom(ref AtomData data)
        {
            //first rotate around z for A degree
            RotateTwo(ref data.X, ref data.Y, A);
            //then rotate around x for B degree
            RotateTwo(ref data.Y, ref data.Z, B);
            //finally rotate around z again for C degree
            RotateTwo(ref data.X, ref data.Y, C);
        }

        private static void RotateTwo(ref float x, ref float y, float degree)
        {
            float ox = x;
            float oy = y;
            x = (float)(ox * Math.Cos(degree) - oy * Math.Sin(degree));
            y = (float)(ox * Math.Sin(degree) + oy * Math.Cos(degree));
        }
    }

    static class RandomExt
    {
        public static Rotation NextRotation(this Random r)
        {
            Rotation ret = new Rotation();
            ret.A = (float)(r.NextDouble() * 2 * Math.PI);

            const int NumberB = 10000000;
            var xB = r.Next(-NumberB, NumberB + 1) / (double)(NumberB);
            var xBa = Math.Abs(xB);
            var yB = Math.Sqrt(1 - xB * xB);
            ret.B = (float)Math.Atan2(yB, xBa);
            if (xB < 0)
            {
                ret.B = (float)(Math.PI - ret.B);
            }

            ret.C = (float)(r.NextDouble() * 2 * Math.PI);

            return ret;
        }

        private static void Main()
        {
            AtomData a = new AtomData();
            a.X = 0;
            a.Y = 1;
            a.Z = 0;

            Random rand = new Random();
            float x = 0, y = 0, z = 0;

            const int Loop = 50;
            for (int i = 0; i < Loop; ++i)
            {
                AtomData b = a;
                rand.NextRotation().RotateAtom(ref b);
                x += b.X;
                y += b.Y;
                z += b.Z;
            }

            x /= Loop;
            y /= Loop;
            z /= Loop;
        }
    }
}
