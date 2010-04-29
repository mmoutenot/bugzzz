using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Bugzzz
{
    public sealed class MathFns
    {
        public struct rect2
        {
            public Vector2 C;
            public Vector2 S;
            public float ang;
        }

        public static float min_angle = 0f;
        public static float max_angle = (float)(2*Math.PI);


        public static float Hermite(float start, float end, float value)
        {
            return Lerp(start, end, value * value * (3.0f - 2.0f * value));
        }

        public static float Sinerp(float start, float end, float value)
        {
            return Lerp(start, end, (float) (Math.Sin(value * Math.PI * 0.5f)));
        }

        public static float Coserp(float start, float end, float value)
        {
            return Lerp(start, end, 1.0f - (float) (Math.Cos(value * Math.PI * 0.5f)));
        }

        /*
        public static float Berp(float start, float end, float value)
        {
            value = Math.Clamp01(value);
            value = (Math.Sin(value * Math.PI * (0.2f + 2.5f * value * value * value)) * Math.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }
        */
        /*
        public static float SmoothStep(float x, float min, float max)
        {
            x = Math.Clamp(x, min, max);
            float v1 = (x - min) / (max - min);
            float v2 = (x - min) / (max - min);
            return -2 * v1 * v1 * v1 + 3 * v2 * v2;
        }
        */
        public static float Lerp(float start, float end, float value)
        {
            return ((1.0f - value) * start) + (value * end);
        }

        public static float Clerp(float start, float end, float value)
        {
            float half = Math.Abs((max_angle - min_angle) / 2.0f);//half the distance between min and max
            float retval = 0.0f;
            float diff = 0.0f;

            //determine which direction to rotate
            if ((end - start) < -half)
            {
                diff = ((max_angle - start) + end) * value;
                retval = start + diff;
            }
            else if ((end - start) > half)
            {
                diff = -((max_angle - end) + start) * value;
                retval = start + diff;
            }
            else retval = start + (end - start) * value;

           //Console.WriteLine("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
           return retval;
        }

        public static float Distance(Vector2 A, Vector2 B)
        {
            return (float)Math.Sqrt(Math.Pow(A.X-B.X,2) + Math.Pow(A.Y-B.Y,2));
        }

        /// <summary>
        /// Calculates if a collision occurs between two rotated rectangles
        /// </summary>
        /// <param name="A">Rectangle 1</param>
        /// <param name="a">Rectangle 1's Rotation</param>
        /// <param name="B">Rectangle 2</param>
        /// <param name="b">Rectangle 2's Rotation</param>
        public static bool rotRectCollision(Rectangle A, float a, Rectangle B, float b)
        {
            //Create new rect structs for code
            rect2 A1 = new rect2();
            A1.ang = a;
            A1.C = new Vector2(A.X+A.Width/2,A.Y+A.Height/2);
            A1.S = new Vector2(A.Width/2, A.Height/2);
            rect2 B1 = new rect2();
            B1.ang = b;
            B1.C = new Vector2(B.X+B.Width/2,B.Y+B.Height/2);
            B1.S = new Vector2(B.Width/2, A.Height/2);

            return RotRectsCollision(A1,B1);
        }
        
        private static void AddVectors2D(Vector2 v1, Vector2 v2)
        { 
            v1.X += v2.X; 
            v1.Y += v2.Y; 
        }

        private static void SubVectors2D(Vector2 v1, Vector2 v2)
        {
            v1.X -= v2.X; 
            v1.Y -= v2.Y; 
        }
        
        public static void RotateVector2DClockwise(Vector2 v, float ang)
        {
            float t;
            t = v.X;
            v.X = (float)(t*Math.Cos(ang) + v.Y*Math.Sin(ang));
            v.Y = (float)(-t*Math.Sin(ang) + v.Y*Math.Cos(ang));
        }
        
        public static bool RotRectsCollision(rect2 rr1, rect2 rr2)
        {
         Vector2 A, B,   // vertices of the rotated rr2
	           C,      // center of rr2
	           BL, TR; // vertices of rr2 (bottom-left, top-right)

         float ang = rr1.ang - rr2.ang, // orientation of rotated rr1
               cosa = (float)(Math.Cos(ang)),           // precalculated trigonometic -
               sina = (float)(Math.Sin(ang));           // - values for repeated use

         float t, x, a;      // temporary variables for various uses
         float dx;           // deltaX for linear equations
         float ext1, ext2;   // min/max vertical values

         // move rr2 to make rr1 cannonic
         C = rr2.C;
         SubVectors2D(C, rr1.C);

         // rotate rr2 clockwise by rr2.ang to make rr2 axis-aligned
         RotateVector2DClockwise(C, rr2.ang);

         // calculate vertices of (moved and axis-aligned := 'ma') rr2
         TR = C + C/ 2;
         BL = C - C / 2;
         SubVectors2D(BL, rr2.S);
         AddVectors2D(TR, rr2.S);

         // calculate vertices of (rotated := 'r') rr1
         A.X = -(rr1.S.Y/2)*sina; 
         B.X = A.X; t = (rr1.S.X/2)*cosa; A.X += t; B.X -= t;
         A.Y =  (rr1.S.Y/2)*cosa; B.Y = A.Y; t = (rr1.S.X/2)*sina; A.Y += t; B.Y -= t;

         t = sina*cosa;

         // verify that A is vertical min/max, B is horizontal min/max
         if (t < 0)
         {
          t = A.X; A.X = B.X; B.X = t;
          t = A.Y; A.Y = B.Y; B.Y = t;
         }

         // verify that B is horizontal minimum (leftest-vertex)
         if (sina < 0) { B.X = -B.X; B.Y = -B.Y; }

         // if rr2(ma) isn't in the horizontal range of
         // colliding with rr1(r), collision is impossible
         if (B.X > TR.X || B.X > -BL.X) return false;

         // if rr1(r) is axis-aligned, vertical min/max are easy to get
         if (t == 0) {ext1 = A.Y; ext2 = -ext1; }
         // else, find vertical min/max in the range [BL.x, TR.x]
         else
         {
          x = BL.X-A.X; a = TR.X-A.X;
          ext1 = A.Y;
          // if the first vertical min/max isn't in (BL.x, TR.x), then
          // find the vertical min/max on BL.x or on TR.x
          if (a*x > 0)
          {
           dx = A.X;
           if (x < 0) { dx -= B.X; ext1 -= B.Y; x = a; }
           else       { dx += B.X; ext1 += B.Y; }
           ext1 *= x; ext1 /= dx; ext1 += A.Y;
          }
          
          x = BL.X+A.X; a = TR.X+A.X;
          ext2 = -A.Y;
          // if the second vertical min/max isn't in (BL.x, TR.x), then
          // find the local vertical min/max on BL.x or on TR.x
          if (a*x > 0)
          {
           dx = -A.X;
           if (x < 0) { dx -= B.X; ext2 -= B.Y; x = a; }
           else       { dx += B.X; ext2 += B.Y; }
           ext2 *= x; ext2 /= dx; ext2 -= A.Y;
          }
         }

         // check whether rr2(ma) is in the vertical range of colliding with rr1(r)
         // (for the horizontal range of rr2)
         return !((ext1 < BL.Y && ext2 < BL.Y) ||
	          (ext1 > TR.Y && ext2 > TR.Y));
        }
    }
}