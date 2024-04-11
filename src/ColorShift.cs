using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TS4SimRipper
{
    public class HSVColor
    {
        private float h, s, v;
        public float Hue
        {
            get { return this.h; }
            set
            {
                this.h = value;
                while (this.h > 1)
                {
                    this.h -= 1f;
                }
                while (this.h < 0)
                {
                    this.h += 1f;
                }
            }
        }
        public float Saturation
        {
            get { return this.s; }
            set
            {
                this.s = value;
                if (this.s > 1) this.s = 1;
                if (this.s < 0) this.s = 0;
            }
        }
        public float Value
        {
            get { return this.v; }
            set
            {
                this.v = value;
                if (this.v > 1) this.v = 1;
                if (this.v < 0) this.v = 0;
            }
        }

        public HSVColor(float hue, float saturation, float value)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Value = value;
        }

        /// <summary>
        /// Create HSV color from red, green, blue values
        /// </summary>
        /// <param name="red">0-255</param>
        /// <param name="green">0-255</param>
        /// <param name="blue">0-255</param>
        /// <returns></returns>
        public HSVColor(byte red, byte green, byte blue)
        {
            double val1 = (double)red / (double)byte.MaxValue;
            double val2_1 = (double)green / (double)byte.MaxValue;
            double val2_2 = (double)blue / (double)byte.MaxValue;
            double num1 = Math.Min(Math.Min(val1, val2_1), val2_2);
            double num2 = Math.Max(Math.Max(val1, val2_1), val2_2);
            double h = 0.0;
            if (num1 != num2)
            {
                if (val1 == num2 && val2_1 >= val2_2)
                    h = 60.0 * ((val2_1 - val2_2) / (num2 - num1)) + 0.0;
                else if (val1 == num2 && val2_1 < val2_2)
                    h = 60.0 * ((val2_1 - val2_2) / (num2 - num1)) + 360.0;
                else if (val2_1 == num2)
                    h = 60.0 * ((val2_2 - val1) / (num2 - num1)) + 120.0;
                else if (val2_2 == num2)
                    h = 60.0 * ((val1 - val2_1) / (num2 - num1)) + 240.0;
            }
            this.Hue = (float)h / 360f;
            this.Value = (float)num2;
            if (num2 == 0.0)
                this.Saturation = 0.0f;
            else
                this.Saturation = (float)((num2 - num1) / num2);
        }

        /// <summary>
        /// Returns byte array of red (0-255), green (0-255), blue (0-255)
        /// </summary>
        public byte[] ToRGBColor()
        {
            double num1 = this.h * 360f;
            while (num1 < 0.0)
                num1 += 360.0;
            while (num1 >= 360.0)
                num1 -= 360.0;
            double num2;
            double num3;
            double num4;
            if (this.v <= 0.0)
            {
                double num5;
                num2 = num5 = 0.0;
                num3 = num5;
                num4 = num5;
            }
            else if (this.s <= 0.0)
            {
                double num5;
                num2 = num5 = this.v;
                num3 = num5;
                num4 = num5;
            }
            else
            {
                double d = num1 / 60.0;
                int num5 = (int)Math.Floor(d);
                double num6 = d - (double)num5;
                double num7 = this.v * (1.0 - this.s);
                double num8 = this.v * (1.0 - this.s * num6);
                double num9 = this.v * (1.0 - this.s * (1.0 - num6));
                switch (num5)
                {
                    case -1:
                        num4 = this.v;
                        num3 = num7;
                        num2 = num8;
                        break;
                    case 0:
                        num4 = this.v;
                        num3 = num9;
                        num2 = num7;
                        break;
                    case 1:
                        num4 = num8;
                        num3 = this.v;
                        num2 = num7;
                        break;
                    case 2:
                        num4 = num7;
                        num3 = this.v;
                        num2 = num9;
                        break;
                    case 3:
                        num4 = num7;
                        num3 = num8;
                        num2 = this.v;
                        break;
                    case 4:
                        num4 = num9;
                        num3 = num7;
                        num2 = this.v;
                        break;
                    case 5:
                        num4 = this.v;
                        num3 = num7;
                        num2 = num8;
                        break;
                    case 6:
                        num4 = this.v;
                        num3 = num9;
                        num2 = num7;
                        break;
                    default:
                        double num10;
                        num2 = num10 = this.v;
                        num3 = num10;
                        num4 = num10;
                        break;
                }
            }
            return new byte[] { Clamp((int)(num4 * (double)byte.MaxValue)), Clamp((int)(num3 * (double)byte.MaxValue)), Clamp((int)(num2 * (double)byte.MaxValue)) };
        }

        private static byte Clamp(int i)
        {
            if (i < 0)
                return 0;
            if (i > byte.MaxValue)
                return byte.MaxValue;
            return (byte)i;
        }

        public static HSVColor operator +(HSVColor hsv1, HSVColor hsv2)
        {
            return new HSVColor(hsv1.h + hsv2.h, hsv1.s + hsv2.s, hsv1.v + hsv2.v);
        }

        public static HSVColor operator -(HSVColor hsv1, HSVColor hsv2)
        {
            return new HSVColor(hsv1.h - hsv2.h, hsv1.s - hsv2.s, hsv1.v - hsv2.v);
        }
    }

    public class HSLColor
    {
        private float h, s, l;
        public float Hue
        {
            get { return this.h; }
            set
            {
                this.h = value;
                while (this.h > 1)
                {
                    this.h -= 1f;
                }
                while (this.h < 0)
                {
                    this.h += 1f;
                }
            }
        }
        public float Saturation
        {
            get { return this.s; }
            set
            {
                this.s = value;
                if (this.s > 1) this.s = 1;
                if (this.s < 0) this.s = 0;
            }
        }
        public float Luminosity
        {
            get { return this.l; }
            set
            {
                this.l = value;
                if (this.l > 1) this.l = 1;
                if (this.l < 0) this.l = 0;
            }
        }

        public HSLColor(float hue, float saturation, float luminosity)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Luminosity = luminosity;
        }

        /// <summary>
        /// Create HSL color from red, green, blue values
        /// </summary>
        /// <param name="red">0-255</param>
        /// <param name="green">0-255</param>
        /// <param name="blue">0-255</param>
        /// <returns>A new HSL color</returns>
        public HSLColor(byte red, byte green, byte blue)
        {
            float R = red / 255f;
            float G = green / 255f;
            float B = blue / 255f;

            bool rMax = false, gMax = false, bMax = false;
            float maxVal = 0, minVal = 0;

            if (R >= G && R >= B) { rMax = true; maxVal = R; }
            else if (G >= R && G >= B) { gMax = true; maxVal = G; }
            else if (B >= G && B >= R) { bMax = true; maxVal = B; }
            if (R <= G && R <= B) { minVal = R; }
            else if (G <= R && G <= B) { minVal = G; }
            else if (B <= G && B <= R) { minVal = B; }

            // float L = (R + G + B) / 3f;
            float L = (float)Math.Sqrt((R * R * .241) + (G * G * .691) + (B * B * .068));

            if (maxVal == minVal)
            {
                this.Hue = 0;
                this.Saturation = 0;
                this.Luminosity = L;
            }
            else
            {
                float S = 0;
                if (L < 0.5)
                {
                    S = (maxVal - minVal) / (maxVal + minVal);
                }
                else
                {
                    S = (maxVal - minVal) / (2.0f - maxVal - minVal);
                }
                if (S < 0) S = 0f;
                if (S > 1f) S = 1f;
                this.Saturation = S;
                this.Luminosity = L;

                float H = 0;
                if (rMax) H = ((G - B) / (maxVal - minVal)) * 60f;
                else if (gMax) H = (2f + (B - R) / (maxVal - minVal)) * 60f;
                else if (bMax) H = (4f + (R - G) / (maxVal - minVal)) * 60f;
                if (H < 0) H += 360f;
                if (H > 360f) H -= 360f;
                this.Hue = H / 360f;
            }
        }

        /// <summary>
        /// Returns byte array of red (0-255), green (0-255), blue (0-255)
        /// </summary>
        public byte[] ToRGBColor()
        {
            double v;
            double r, g, b;
            
            r = l;   // default to gray
            g = l;
            b = l;

            v = (l <= 0.5) ? (l * (1.0 + s)) : (l + s - l * s);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;
                m = l + l - v;
                sv = (v - m) / v;
                double hue = h * 6.0;
                sextant = (int)hue;
                fract = hue - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return new byte[] { Clamp((int)(r * (double)byte.MaxValue)), Clamp((int)(g * (double)byte.MaxValue)), Clamp((int)(b * (double)byte.MaxValue)) };
        }

        public byte[] GetRGB()
        {
            if (l > 1f) l = 1f;
            if (s == 0) { byte r = (byte)(255f * l); return new byte[] { r, r, r }; }
            float tmp1 = 0, tmp2 = 0;
            if (l < .5) tmp1 = l * (1f + s);
            else tmp1 = (l + s) - (l * s);
            tmp2 = 2f * l - tmp1;
            byte R = CalcChannel(h + .333f, tmp1, tmp2);
            byte G = CalcChannel(h, tmp1, tmp2);
            byte B = CalcChannel(h - .333f, tmp1, tmp2);
            return new byte[] { R, G, B };
        }

        private byte CalcChannel(float value, float adjust1, float adjust2)
        {
            if (value < 0f) value += 1f;
            else if (value > 1f) value -= 1f;
            float C = 0;
            if ((6f * value) < 1f) C = adjust2 + ((adjust1 - adjust2) * 6f * value);
            else if ((2f * value) < 1f) C = adjust1;
            else if ((3f * value) < 2f) C = adjust2 + ((adjust1 - adjust2) * (.666f - value) * 6f);
            else C = adjust2;
            C = C * 255f;
            if (C < 0f) C = 0f;
            if (C > 255f) C = 255f;
            return (byte)(C + 0.5f);
        }

        private static byte Clamp(int i)
        {
            if (i < 0)
                return 0;
            if (i > byte.MaxValue)
                return byte.MaxValue;
            return (byte)i;
        }
    }

    public class ShiftMatrix
    {
        internal float[][] matrix;

        public float[][] Matrix { get { return this.matrix; } }

        public static float[][] Multiply(float[][] f1, float[][] f2)
        {
            float[][] X = new float[5][];
            for (int d = 0; d < 5; d++)
                X[d] = new float[5];
            int size = 5;
            float[] column = new float[5];
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    column[k] = f1[k][j];
                }
                for (int i = 0; i < 5; i++)
                {
                    float[] row = f2[i];
                    float s = 0;
                    for (int k = 0; k < size; k++)
                    {
                        s += row[k] * column[k];
                    }
                    X[i][j] = s;
                }
            }
            return X;
        }
    }

    public class HueMatrix : ShiftMatrix        //shifts ok but doesn't keep luminance
    {
        public HueMatrix(float hueShift)
        {
            double theta = (hueShift * 2 * Math.PI);         //Degrees --> Radians
            double c = Math.Cos(theta);
            double s = Math.Sin(theta);

            float A00 = (float)(0.213 + 0.787 * c - 0.213 * s);
            float A01 = (float)(0.213 - 0.213 * c + 0.413 * s);
            float A02 = (float)(0.213 - 0.213 * c - 0.787 * s);

            float A10 = (float)(0.715 - 0.715 * c - 0.715 * s);
            float A11 = (float)(0.715 + 0.285 * c + 0.140 * s);
            float A12 = (float)(0.715 - 0.715 * c + 0.715 * s);

            float A20 = (float)(0.072 - 0.072 * c + 0.928 * s);
            float A21 = (float)(0.072 - 0.072 * c - 0.283 * s);
            float A22 = (float)(0.072 + 0.928 * c + 0.072 * s);

            this.matrix = new float[][] {
                  new float[] { A00, A01, A02, 0, 0 },
                  new float[] { A10, A11, A12, 0, 0 },
                  new float[] { A20, A21, A22, 0, 0 },
                  new float[] { 0, 0, 0, 1, 0 },
                  new float[] { 0, 0, 0, 0, 1 }
            };
        }
    }

    public partial class Form1 : Form
    {
        /// <summary>
        /// Applies hue, saturation, and brightness shifts to a texture
        /// </summary>
        /// <param name="texture">Bitmap to be modified</param>
        /// <param name="hueShift">Hue shift, -1 to +1</param>
        /// <param name="saturationShift">Saturation shift, -1 to 1</param>
        /// <param name="brightnessShift">Brightness shift, -1 to 1</param>
        public void ShiftTexture(Bitmap texture, float hueShift, float saturationShift, float brightnessShift)
        {
            Rectangle rect1 = new Rectangle(0, 0, texture.Width, texture.Height);
            System.Drawing.Imaging.BitmapData bmpData1 = texture.LockBits(rect1, ImageLockMode.ReadWrite, texture.PixelFormat);
            IntPtr ptr1;
            if (bmpData1.Stride > 0) ptr1 = bmpData1.Scan0;
            else ptr1 = bmpData1.Scan0 + bmpData1.Stride * (texture.Height - 1);
            int bytes1 = Math.Abs(bmpData1.Stride) * texture.Height;
            byte[] color = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, color, 0, bytes1);

            // argbValues1[i] = blue
            // argbValues1[i + 1] = green
            // argbValues1[i + 2] = red
            // argbValues1[i + 3] = alpha

            for (int i = 0; i < color.Length; i += 4)
            {
                HSVColor hsv = new HSVColor(color[i + 2], color[i + 1], color[i]);
                hsv.Hue += hueShift;
                hsv.Saturation += saturationShift;
                hsv.Value += brightnessShift;
                byte[] tmp = hsv.ToRGBColor();
                color[i + 2] = tmp[0];
                color[i + 1] = tmp[1];
                color[i] = tmp[2];
            }

            System.Runtime.InteropServices.Marshal.Copy(color, 0, ptr1, bytes1);
            texture.UnlockBits(bmpData1);
        }
    }
}

