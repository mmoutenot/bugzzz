using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

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

        public static Vector2 newPoint(Vector2 center, Vector2 original, float angle)
        {
            float x = (float)(center.X - (original.X - center.X) * Math.Cos(angle) + (original.Y - center.Y) * Math.Sin(angle));
            float y = (float)(center.Y + (original.X - center.X) * Math.Sin(angle) + (original.Y - center.Y) * Math.Cos(angle));
            Vector2 ret = new Vector2(x, y);
            return ret;

        }

        public static bool broadPhaseCollision(Rectangle a, float angleA, Rectangle b, float angleB)
        {
            Vector2 CenterA = new Vector2(a.Center.X, a.Center.Y);
            Vector2 CenterB = new Vector2(b.X, b.Y);

            Vector2 UL1 = new Vector2(a.Left, a.Top);
            UL1 = newPoint(CenterA, UL1, angleA);
            //s.Draw(l, UL1, Color.White);
            Vector2 BL1 = new Vector2(a.Left,a.Bottom);
            BL1 = newPoint(CenterA, BL1, angleA);
            Vector2 UR1 = new Vector2(a.Right,a.Top);
            UR1 = newPoint(CenterA, UR1, angleA);
            Vector2 BR1 = new Vector2(a.Right,a.Bottom);
            BR1 = newPoint(CenterA, BR1, angleA);

            //calculate new corners for rect b
            Vector2 UL2 = new Vector2(b.Left-b.Width/2, b.Top-b.Height/2);
            UL2 = newPoint(CenterB, UL2, angleB);
            Vector2 BL2 = new Vector2(b.Left-b.Width/2, b.Top+b.Height/2);
            BL2 = newPoint(CenterB, BL2, angleB);
            Vector2 UR2 = new Vector2(b.X+b.Width/2, b.Y-b.Height/2);
            UR2 = newPoint(CenterB, UR2, angleB);
            Vector2 BR2 = new Vector2(b.X+b.Width/2, b.Y+b.Height/2);
            BR2 = newPoint(CenterB, BR2, angleB);

            //find max and mins for Rect 1
            #region
            float maxX1 = UL1.X, maxY1 = UL1.Y, minX1 = UL1.X, minY1 = UL1.Y;
            if (BL1.X > maxX1)
                maxX1 = BL1.X;
            if (BL1.X < minX1)
                minX1 = BL1.X;
            if (BL1.Y > maxY1)
                maxY1 = BL1.Y;
            if (BL1.Y < minY1)
                minY1 = BL1.Y;

            if (UR1.X > maxX1)
                maxX1 = UR1.X;
            if (UR1.X < minX1)
                minX1 = UR1.X;
            if (UR1.Y > maxY1)
                maxY1 = UR1.Y;
            if (UR1.Y < minY1)
                minY1 = UR1.Y;

            if (BR1.X > maxX1)
                maxX1 = BR1.X;
            if (BR1.X < minX1)
                minX1 = BR1.X;
            if (BR1.Y > maxY1)
                maxY1 = BR1.Y;
            if (BR1.Y < minY1)
                minY1 = BR1.Y;
            #endregion
            //find max and minx for Rect 2
            #region
            float maxX2 = UL2.X, maxY2 = UL2.Y, minX2 = UL2.X, minY2 = UL2.Y;
            if (BL2.X > maxX2)
                maxX2 = BL2.X;
            if (BL2.X < minX2)
                minX2 = BL2.X;
            if (BL2.Y > maxY2)
                maxY2 = BL2.Y;
            if (BL2.Y < minY2)
                minY2 = BL2.Y;

            if (UR2.X > maxX2)
                maxX2 = UR2.X;
            if (UR2.X < minX2)
                minX2 = UR2.X;
            if (UR2.Y > maxY2)
                maxY2 = UR2.Y;
            if (UR2.Y < minY2)
                minY2 = UR2.Y;

            if (BR2.X > maxX2)
                maxX2 = BR2.X;
            if (BR2.X < minX2)
                minX2 = BR2.X;
            if (BR2.Y > maxY2)
                maxY2 = BR2.Y;
            if (BR2.Y < minY2)
                minY2 = BR2.Y;
            #endregion

            //Console.Out.WriteLine(minX1 + " " + maxX1);
            //Console.Out.WriteLine(b.X);
            Rectangle r1 = new Rectangle((int)minX1, (int)minY1, (int)(maxX1 - minX1), (int)(maxY1- minY1));
            Rectangle r2 = new Rectangle((int)minX2, (int)minY2, (int)(maxX2 - minX2), (int)(maxY2- minY2));
            return r1.Intersects(r2);



        }
        public static bool narrowPhaseCollision(Rectangle a, float angleA, Rectangle b, float angleB)
        {
            Vector2 CenterA = new Vector2(a.Center.X, a.Center.Y);
            Vector2 CenterB = new Vector2(b.X, b.Y);

            Vector2 UL1 = new Vector2(a.Left, a.Top);
            UL1 = newPoint(CenterA, UL1, angleA);
            //s.Draw(l, UL1, Color.White);
            Vector2 BL1 = new Vector2(a.Left, a.Bottom);
            BL1 = newPoint(CenterA, BL1, angleA);
            Vector2 UR1 = new Vector2(a.Right, a.Top);
            UR1 = newPoint(CenterA, UR1, angleA);
            Vector2 BR1 = new Vector2(a.Right, a.Bottom);
            BR1 = newPoint(CenterA, BR1, angleA);

            //calculate new corners for rect b
            Vector2 UL2 = new Vector2(b.Left - b.Width / 2, b.Top - b.Height / 2);
            UL2 = newPoint(CenterB, UL2, angleB);
            Vector2 BL2 = new Vector2(b.Left - b.Width / 2, b.Top + b.Height / 2);
            BL2 = newPoint(CenterB, BL2, angleB);
            Vector2 UR2 = new Vector2(b.X + b.Width / 2, b.Y - b.Height / 2);
            UR2 = newPoint(CenterB, UR2, angleB);
            Vector2 BR2 = new Vector2(b.X + b.Width / 2, b.Y + b.Height / 2);
            BR2 = newPoint(CenterB, BR2, angleB);

            Vector2 Axis = UR1 - UL1;
            if (!nPCHelpers(UR1, UR2, UL1, UL2, BL1, BL2, BR1, BR2, Axis))
                return false;

            Axis = UR1 - BR1;
            if (!nPCHelpers(UR1, UR2, UL1, UL2, BL1, BL2, BR1, BR2, Axis))
                return false;

            Axis = UL2 - BL2;
            if (!nPCHelpers(UR1, UR2, UL1, UL2, BL1, BL2, BR1, BR2, Axis))
                return false;

            Axis = UL2 - UR2;
            if (!nPCHelpers(UR1, UR2, UL1, UL2, BL1, BL2, BR1, BR2, Axis))
                return false;

            return true;
        }
        public static bool nPCHelpers(Vector2 UR1, Vector2 UR2, Vector2 UL1, Vector2 UL2, Vector2 BL1, Vector2 BL2,Vector2 BR1,Vector2 BR2,Vector2 Axis)
        {

            //calculate vector projections on axis
            Vector2 pUR1 = project(Axis, UR1);
            Vector2 pUR2 = project(Axis, UR2);
            Vector2 pUL1 = project(Axis, UL1);
            Vector2 pUL2 = project(Axis, UL2);
            Vector2 pBL1 = project(Axis, BL1);
            Vector2 pBL2 = project(Axis, BL2);
            Vector2 pBR1 = project(Axis, BR1);
            Vector2 pBR2 = project(Axis, BR2);

            //calculate dot products (scalar values)
            float urv1, urv2, ulv1, ulv2, blv1, blv2, brv1, brv2;
            urv1 = DP(Axis, pUR1);
            urv2 = DP(Axis, pUR2);
            ulv1 = DP(Axis, pUL1);
            ulv2 = DP(Axis, pUL2);
            brv1 = DP(Axis, pBR1);
            brv2 = DP(Axis, pBR2);
            blv1 = DP(Axis, pBL1);
            blv2 = DP(Axis, pBL2);

            //Calculate min values on axis
            float maxA=urv1, minA=urv1, maxB=urv2, minB=urv2;
            if (brv1 > maxA)
                maxA = brv1;
            else if (brv1 < minA)
                minA = brv1;
            if (blv1 > maxA)
                maxA = blv1;
            else if (blv1 < minA)
                minA = blv1;
            if (ulv1 > maxA)
                maxA = ulv1;
            else if (ulv1 < minA)
                minA = ulv1;

            if (brv2 > maxA)
                maxA = brv2;
            else if (brv2 < minA)
                minA = brv2;
            if (blv2 > maxA)
                maxA = blv2;
            else if (blv2 < minA)
                minA = blv2;
            if (ulv2 > maxA)
                maxA = ulv2;
            else if (ulv2 < minA)
                minA = ulv2;

            if (minA < minB && minB < maxA)
                return true;
            if (minA < maxB && maxB < maxA)
                return true;
            if (minB < minA && minA < maxB)
                return true;
            if (minB < maxA && maxA < maxB)
                return true;

            return false;
        }

        public static Vector2 project(Vector2 Axis, Vector2 pt)
        {
            Vector2 ret = new Vector2();
            ret.X = (float)((pt.X * Axis.X + pt.Y * Axis.Y) * Axis.X / (Math.Pow(Axis.X, 2) + Math.Pow(Axis.Y, 2)));
            ret.Y = (float)((pt.X * Axis.X + pt.Y * Axis.Y) * Axis.Y / (Math.Pow(Axis.X, 2) + Math.Pow(Axis.Y, 2)));
            return ret;
        }

        public static float DP(Vector2 A, Vector2 B)
        {
            return A.X * B.X + A.Y * B.Y;
        }
    }
}