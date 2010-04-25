using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugzzz
{
    public sealed class MathFns
    {
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
    }
}