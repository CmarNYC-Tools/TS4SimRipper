using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Resources;
using s4pi.ImageResource;

namespace TS4SimRipper
{
    public partial class Form1 : Form
    {
                            //neutral           //heavy             //fit                   //lean              //bony
        static ulong[][] detailInstance = new ulong[][] { new ulong[] {0x0A11C0657FBDB54FU, 0, 0, 0, 0 }, //baby

            new ulong[] { 0xD19E353A4001EC4DU, 0xCB74D8715AACAEE5U, 0, 0xFFECB88D957AB9C8U, 0 }, //toddler

            new ulong[] { 0x9CB2C5C93E357C62U, 0xD35E44A00EC82DD2U, 0, 0xDCE1DE32790EEE2DU, 0 }, //child

            new ulong[] { 0x48F11375333EDB51U, 0xC5A686CB8DEAD669U, 0x050DA429AF8F8579U, 0x5B5168D56FB549CCU, 0x493919D5653D6D22U }, //teen male
            new ulong[] { 0x58F8275474E1AE00U, 0x8225CF86E9ADE5A8U, 0xF5A23C00099ADF1CU, 0xA6DF5710210EC357U, 0x1E40BE1064236B31U }, //YA male
            new ulong[] { 0x308855B3BFF0E848U, 0x0D2BCAD6902710D0U, 0x14F31CC55B6B8D94U, 0x37EF8E5A749B458FU, 0x2930155A6CFBC099U }, //adult male
            new ulong[] { 0x24DFF8E30DC7E5DCU, 0xF949199CADA33974U, 0xA64804A650DC3CB8U, 0x120B1D9B35364743U, 0x47F4E49B544D0695U }, //elder male

            new ulong[] { 0xA062AF087257C3AAU, 0x614DF350B2288202U, 0x9B76618343015672U, 0xA30CC4B09CB5AA9FU, 0x15CCD44E2A798561U }, //teen male overlay
            new ulong[] { 0xA3EC609A2DAB31D3U, 0xE6997ACD10E7ADFBU, 0x8A4EC419617EFB8FU, 0x983C2A528E708C94U, 0x4C97A925081CFE8AU }, //YA male overlay
            new ulong[] { 0x265B16FA4E7DA19BU, 0x25549E2A0EA2EA03U, 0x049310925E1F7507U, 0xBE003BD8D1E4F6CCU, 0x0FF4F3A205CF3792U }, //adult male overlay
            new ulong[] { 0x25EBBD9BED791D4FU, 0x15B4CE7F555B78E7U, 0x02749727C1E4936BU, 0x0DDA0E70371F1850U, 0xE4DC67D79FE3259EU }, //elder male overlay

            new ulong[] { 0x737A5FF0EB729888U, 0xCB60F0F987055510U, 0xD9BEAB29970846D4U, 0x3A8BC2ABBFEA0DCFU, 0x286649ABB5675FD9U }, //teen female
            new ulong[] { 0x36C865290B1F4E79U, 0x5A0156E11FEB7ED1U, 0x980C64FFD5139131U, 0x1A2B3CB6DF532C84U, 0x95006DB72556DFEAU }, //YA female
            new ulong[] { 0x59093C1074E2C911U, 0xB45360AD81F01829U, 0x404315C573F47F39U, 0x58B534842466818CU, 0x469DE58419F058E2U }, //adult female
            new ulong[] { 0x2356ABE32AC4C255U, 0xF6F5AF6EB76D95CDU, 0xD197EDA66965137DU, 0x33AAC3C4E5BB2680U, 0x65CEB4C5019CBDFEU }, //elder female

            new ulong[] { 0xF85FB112905485DBU, 0x983E38E671724943U, 0x87C948F0D0911447U, 0x24C0445548F4DD0CU, 0x769AFB69C35341D2U }, //teen female overlay
            new ulong[] { 0x0A136CA1147B1772U, 0xDA72BC0116BD2B2AU, 0xE2C41710B0F9748AU, 0x084BCF43E871B757U, 0xB356C29F445C7C69U }, //YA female overlay
            new ulong[] { 0x53F13B3669333A6AU, 0x6B5D4119F5F3E3C2U, 0x6E20B376A7440832U, 0x38E5BC3422C0E85FU, 0x4C23AB3892B08421U }, //adult female overlay
            new ulong[] { 0x1E1930AE6138725EU, 0xDFD33956DE308196U, 0xDA922B3518C2D1E6U, 0x718F473471CA0653U, 0x2433EE3BE4F044ADU } }; //elder female overlay


        private Image DisplayableSkintone(TONE tone, float shift, int skinState, Bitmap tanLines, AgeGender age, AgeGender gender, float[] physiqueWeights,
            float pregnantShape, Image sculptOverlay, Image outfitOverlay)
        {
            float[][] alphaMatrix = { 
                   new float[] {1, 0, 0, 0, 0},       // m00 = red scaling factor
                   new float[] {0, 1, 0, 0, 0},       // m11 = green scaling factor
                   new float[] {0, 0, 1, 0, 0},       // m22 = blue scaling factor
                   new float[] {0, 0, 0, 1, 0},       // m33 = alpha scaling factor
                   new float[] {0, 0, 0, 0, 1}        // increments for R, G, B, A
            };

            float[][] maskMatrix = {
                   new float[] {1, 0, 0, 0, 0},       // m00 = red scaling factor
                   new float[] {0, 1, 0, 0, 0},       // m11 = green scaling factor
                   new float[] {0, 0, 1, 0, 0},       // m22 = blue scaling factor
                   new float[] {0, 0, 0, .15f, 0},    // m33 = alpha scaling factor
                   new float[] {0, 0, 0, 0, 1}        // increments for R, G, B, A
            };

            float[][] overlayMatrix = {
                   new float[] {1, 0, 0, 0, 0},       // m00 = red scaling factor
                   new float[] {0, 1, 0, 0, 0},       // m11 = green scaling factor
                   new float[] {0, 0, 1, 0, 0},       // m22 = blue scaling factor
                   new float[] {0, 0, 0, 1, 0},       // m33 = alpha scaling factor
                   new float[] {0, 0, 0, 0, 1}        // increments for R, G, B, A
            };
            //float C = 1.2f;
            //float T = 0.5f * (1f - C);
            //float[][] contrastMatrix = {
            //       new float[] {C, 0, 0, 0, 0},       
            //       new float[] {0, C, 0, 0, 0},       
            //       new float[] {0, 0, C, 0, 0},       
            //       new float[] {0, 0, 0, 1, 0},       
            //       new float[] {T, T, T, 0, 1}        
            //    };

            float contrast = 1.1f;
            float midpoint = 0.75f;
            int skinIndex = FindSetBit((uint)age);
            if (age == AgeGender.Infant) skinIndex = 1;     //temporary until I find the goddamn infant textures
            else if (age > AgeGender.Child && gender == AgeGender.Female) skinIndex = skinIndex + 8;
             
            Bitmap details = FetchGameTexture(detailInstance[skinIndex][0], -1, ref errorList, false);

            if (details == null) return null;

            using (Graphics gr = Graphics.FromImage(details))
            {
                if (age > AgeGender.Child && age != AgeGender.Infant)
                {
                    Bitmap overlay = FetchGameTexture(detailInstance[skinIndex + 4][0], -1, ref errorList, false);
                    if (overlay != null)
                    {
                        gr.DrawImage(overlay, new Rectangle(0, 0, details.Width, details.Height), 0, 0, overlay.Width, overlay.Height, GraphicsUnit.Pixel);
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (physiqueWeights[i] > 0)
                        {
                            ImageAttributes attributes = new ImageAttributes();
                            alphaMatrix[3][3] = physiqueWeights[i];
                            ColorMatrix convert = new ColorMatrix(alphaMatrix);
                            attributes.SetColorMatrix(convert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            Bitmap physique = FetchGameTexture(detailInstance[skinIndex][i + 1], -1, ref errorList, false);
                            if (physique != null)
                            {
                                gr.DrawImage(physique, new Rectangle(0, 0, details.Width, details.Height), 0, 0, physique.Width, physique.Height, GraphicsUnit.Pixel, attributes);
                                physique.Dispose();
                            }
                            Bitmap pOverlay = FetchGameTexture(detailInstance[skinIndex + 4][i + 1], -1, ref errorList, false);
                            if (pOverlay != null)
                            {
                                gr.DrawImage(pOverlay, new Rectangle(0, 0, details.Width, details.Height), 0, 0, pOverlay.Width, pOverlay.Height, GraphicsUnit.Pixel, attributes);
                                pOverlay.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i += 2)
                    {
                        if (physiqueWeights[i] > 0)
                        {
                            alphaMatrix[3][3] = physiqueWeights[i];
                            ColorMatrix convert = new ColorMatrix(alphaMatrix);
                            ImageAttributes attributes = new ImageAttributes();
                            attributes.SetColorMatrix(convert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            Bitmap physique = FetchGameTexture(new TGI((uint)ResourceTypes.LRLE, 0, detailInstance[skinIndex][i + 1]), -1, ref errorList, false);
                            if (physique != null)
                            {
                                gr.DrawImage(physique, new Rectangle(0, 0, details.Width, details.Height), 0, 0, physique.Width, physique.Height, GraphicsUnit.Pixel, attributes);
                                physique.Dispose();
                            }
                        }
                    }
                }
            }

            //using (Graphics g = Graphics.FromImage(details))
            //{
            //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    g.DrawImage(sculptOverlay, new Rectangle(0, 0, details.Width, details.Height));
            //    g.DrawImage(outfitOverlay, new Rectangle(0, 0, details.Width, details.Height));
            //}
            Bitmap img = new Bitmap(sculptOverlay);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(details, new Rectangle(0, 0, img.Width, img.Height));
            }
            details = img;
            using (Graphics g = Graphics.FromImage(details))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(outfitOverlay, new Rectangle(0, 0, details.Width, details.Height));
            }

            if (tone == null) return details;

            TONE.SkinSetDesc skinSet = tone.SkinSets[0];
            Bitmap skin = FetchGameTexture(tone.SkinSets[0].TextureInstance, -1, ref errorList, true);
            if (skin == null) return details;
            if (currentSkinSet > 0)
            {
                using (Graphics g = Graphics.FromImage(skin))
                {
                    //for (int i = 0; i < overlays.Count; i++)
                    //{
                    //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //    g.DrawImage(overlays[i], new Point(0, 0));
                    //}
                    Bitmap tan = FetchGameTexture(tone.SkinSets[currentSkinSet].TextureInstance, -1, ref errorList, true);
                    if (tan != null)
                    {
                        if (TanLines_checkBox.Checked && tanLines != null) tan.SetAlphaFromImage(new Bitmap(tanLines, tan.Width, tan.Height));
                        g.DrawImage(tan, new Point(0, 0));
                        tan.Dispose();
                    }

                    Bitmap mask = FetchGameTexture(tone.SkinSets[currentSkinSet].overlayInstance, -1, ref errorList, true);
                    if (mask != null)
                    {
                        ColorMatrix maskConvert = new ColorMatrix(maskMatrix);
                        ImageAttributes maskAttributes = new ImageAttributes();
                        maskAttributes.SetColorMatrix(maskConvert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        g.DrawImage(mask, new Rectangle(0, 0, skin.Width, skin.Height), 0, 0, skin.Width, skin.Height, GraphicsUnit.Pixel, maskAttributes);
                    }
                }
            }

            if (Math.Abs(shift) > 0.001 && SkinBlend1_radioButton.Checked)
            {
                ShiftTexture(skin, 0, 0, shift);
            }

            if (details.Width != skin.Width || details.Height != skin.Height)
            {
                skin = new Bitmap(skin, details.Width, details.Height);
            }

            float burnMultiplier = skinSet.OverlayMultiplier;
            ushort overlaidHue = tone.Hue;
            ushort overlaidSaturation = tone.Saturation;
            uint Pass2Opacity = tone.Opacity;

            Rectangle rect1 = new Rectangle(0, 0, skin.Width, skin.Height);
            System.Drawing.Imaging.BitmapData bmpData1 = skin.LockBits(rect1, ImageLockMode.ReadWrite, skin.PixelFormat);
            IntPtr ptr1;
            if (bmpData1.Stride > 0) ptr1 = bmpData1.Scan0;
            else ptr1 = bmpData1.Scan0 + bmpData1.Stride * (skin.Height - 1);
            int bytes1 = Math.Abs(bmpData1.Stride) * skin.Height;
            byte[] color = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, color, 0, bytes1);

            Rectangle rect2 = new Rectangle(0, 0, details.Width, details.Height);
            System.Drawing.Imaging.BitmapData bmpData2 = details.LockBits(rect2, ImageLockMode.ReadWrite, details.PixelFormat);
            IntPtr ptr2;
            if (bmpData2.Stride > 0) ptr2 = bmpData2.Scan0;
            else ptr2 = bmpData2.Scan0 + bmpData2.Stride * (details.Height - 1);
            int bytes2 = Math.Abs(bmpData2.Stride) * details.Height;
            byte[] detail = new byte[bytes2];
            System.Runtime.InteropServices.Marshal.Copy(ptr2, detail, 0, bytes2);

            // argbValues1[i] = blue
            // argbValues1[i + 1] = green
            // argbValues1[i + 2] = red
            // argbValues1[i + 3] = alpha

            // int cutoff = (skin.Height / 4) * skin.Width * 4;      //where to stop making transparent

            float pass2opacity = Pass2Opacity / 100f;
            byte[] rgbOver = GetRGB(overlaidHue, 127, 127);
            float overFactor = overlaidSaturation / 100;

            for (int i = 0; i < color.Length; i += 4)
            {
                if (SkinBlend1_radioButton.Checked)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        float tmp;      //first pass softlight, details over color (keeps color)
                        tmp = ((1f - 2f * (detail[i + j] / 255f)) * (color[i + j] / 255f) * (color[i + j] / 255f) + 2f * (detail[i + j] / 255f) * (color[i + j] / 255f)) * 255f;
                        tmp = (float)Math.Min(tmp * 1.2, 255.0);        //but it's too dark

                        float tmp2;
                        if (tmp > 128)           //second pass overlay blend, details over color
                            tmp2 = 255f - ((255f - 2f * (detail[i + j] - 128f)) * (255f - tmp) / 256f);
                        else
                            tmp2 = (2f * detail[i + j] * tmp) / 256f;

                        tmp = (tmp2 * pass2opacity) + (tmp * (1f - pass2opacity));          // blend using 2nd pass opacity

                        if (SkinOverlay_checkBox.Checked && overlaidSaturation > 0)
                        {
                            float tmp3 = ((tmp / 255f) * (tmp + ((2f * rgbOver[2 - j]) / 255f) * (255f - tmp)));  //3rd pass is soft light blend, color over all
                            tmp = (tmp3 * overFactor) + (tmp * (1f - overFactor));
                        }

                        // increase contrast slightly
                        tmp = ((((tmp / 255f) - midpoint) * contrast) + midpoint) * 255f;
                        if (tmp < 0f) tmp = 0;
                        if (tmp > 255) tmp = 255;

                        color[i + j] = (byte)tmp;
                    }
                }
                else if (SkinBlend2_radioButton.Checked)
                {
                    HSVColor hsvColor = new HSVColor(color[i + 2], color[i + 1], color[i]);
                    HSVColor hsvDetail = new HSVColor(detail[i + 2], detail[i + 1], detail[i]);
                    if (SkinOverlay_checkBox.Checked) hsvColor.Hue = ((overlaidHue / 240f) * overFactor) + (hsvColor.Hue * (1f - overFactor));
                    hsvColor.Value += (hsvDetail.Value - .40f) + shift;
                    byte[] tmp = hsvColor.ToRGBColor();
                    color[i + 2] = tmp[0];
                    color[i + 1] = tmp[1];
                    color[i] = tmp[2];
                }
                else
                {
                    HSLColor hslColor = new HSLColor(color[i + 2], color[i + 1], color[i]);
                    HSLColor hslDetail = new HSLColor(detail[i + 2], detail[i + 1], detail[i]);
                    if (SkinOverlay_checkBox.Checked) hslColor.Hue = ((overlaidHue / 240f) * overFactor) + (hslColor.Hue * (1f - overFactor));
                    hslColor.Luminosity += ((hslDetail.Luminosity - .40f) * .60f) + shift;
                    byte[] tmp = hslColor.ToRGBColor();
                    color[i + 2] = tmp[0];
                    color[i + 1] = tmp[1];
                    color[i] = tmp[2];
                }

                // if (i < cutoff) color[i + 3] = 0;
            }

            System.Runtime.InteropServices.Marshal.Copy(color, 0, ptr1, bytes1);
            skin.UnlockBits(bmpData1);
            details.UnlockBits(bmpData2);
            // details.Dispose();

            using (Graphics gr = Graphics.FromImage(skin))
            {
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                ulong overlayInstance = tone.GetOverlayInstance(age & gender);
                if (overlayInstance > 0)
                {
                    Bitmap skinOverlay = FetchGameTexture(overlayInstance, -1, ref errorList, true);
                    if (skinOverlay != null) gr.DrawImage(skinOverlay, new Rectangle(0, 0, skin.Width, skin.Height), 0, 0, skinOverlay.Width, skinOverlay.Height, GraphicsUnit.Pixel);
                }
                Bitmap mouthOverlay = Properties.Resources.HeadMouthColor;
                if (mouthOverlay.Size != humanTextureSize) mouthOverlay = new Bitmap(mouthOverlay, humanTextureSize);
                gr.DrawImage(mouthOverlay, new Rectangle(0, 0, skin.Width, skin.Height), 0, 0, mouthOverlay.Width, mouthOverlay.Height, GraphicsUnit.Pixel);
            }
          //  AddDetailToSkinDelta(skin, outfitOverlay);

            return skin;
        }

        private int FindSetBit(uint b)
        {
            for (int i = 0; i < 32; i++)
            {
                if ((b & (1 << i)) != 0) return i;
            }
            return 0;
        }

        /// <summary>
        /// Converts hue, saturation, and luminance to RGB. 
        /// </summary>
        /// <param name="hue">Hue (0 - 239)</param>
        /// <param name="saturation">Saturation (0 - 240)</param>
        /// <param name="luminance">Luminance (0 - 240)</param>
        /// <returns>return byte array: RGB</returns>
        private byte[] GetRGB(ushort hue, ushort saturation, ushort luminance)
        {
            float l = luminance / 240f;
            if (l > 1f) l = 1f;
            if (saturation == 0) { byte r = (byte)(255f * l); return new byte[] { r, r, r }; }
            float s = saturation / 240f;
            float tmp1 = 0, tmp2 = 0;
            if (l < .5) tmp1 = l * (1f + s);
            else tmp1 = (l + s) - (l * s);
            tmp2 = 2f * l - tmp1;
            float H = hue / 239f;
            byte R = CalcChannel(H + .333f, tmp1, tmp2);
            byte G = CalcChannel(H, tmp1, tmp2);
            byte B = CalcChannel(H - .333f, tmp1, tmp2);
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

        /// <summary>
        /// Returns hue (0 - 239) in element 0, saturation (0 - 240) in element 1, luminance (0 - 240) in element 2
        /// </summary>
        /// <param name="Red">Red component</param>
        /// <param name="Green">Green component</param>
        /// <param name="Blue">Blue component</param>
        /// <returns></returns>
        private ushort[] GetHSL(byte Red, byte Green, byte Blue)
        {
            float R = Red / 255f;
            float G = Green / 255f;
            float B = Blue / 255f;

            bool rMax = false, gMax = false, bMax = false;
            float maxVal = 0, minVal = 0;

            if (R >= G && R >= B) { rMax = true; maxVal = R; }
            else if (G >= R && G >= B) { gMax = true; maxVal = G; }
            else if (B >= G && B >= R) { bMax = true; maxVal = B; }
            if (R <= G && R <= B) { minVal = R; }
            else if (G <= R && G <= B) { minVal = G; }
            else if (B <= G && B <= R) { minVal = B; }

            // float L = (R + G + B) / 3f;
            // float L = (float)Math.Sqrt((R * R * .241) + (G * G * .691) + (B * B * .068));
            float L = (float)Math.Sqrt((R * R * .299) + (G * G * .587) + (B * B * .114));

            if (maxVal == minVal) return new ushort[] { 0, 0, (ushort)((L * 240f) + 0.5f) };

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
            S = S * 240f;
            L = L * 240f;

            float H = 0;
            if (rMax) H = ((G - B) / (maxVal - minVal)) * 60f;
            else if (gMax) H = (2f + (B - R) / (maxVal - minVal)) * 60f;
            else if (bMax) H = (4f + (R - G) / (maxVal - minVal)) * 60f;
            if (H < 0) H += 360f;
            if (H > 360f) H -= 360f;
            H = (H / 360f) * 239f;

            return new ushort[] { (ushort)(H + 0.5f), (ushort)(S + 0.5f), (ushort)(L + 0.5f) };
        }

        /// <summary>
        /// Return float array of hue, saturation, value
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public static float[] GetHSV(byte red, byte green, byte blue)
        {
            int max = Math.Max(red, Math.Max(green, blue));
            int min = Math.Min(red, Math.Min(green, blue));

            float hue = Color.FromArgb(red, green, blue).GetHue();
            float saturation = (float)((max == 0) ? 0 : 1d - (1d * min / max));
            float value = (float)(max / 255d);
            return new float[] { hue, saturation, value };
        }

        public static byte[] GetRGB(float[] hsv)
        {
            int hi = Convert.ToInt32(Math.Floor(hsv[0] / 60)) % 6;
            double f = hsv[0] / 60 - Math.Floor(hsv[0] / 60);

            double value = hsv[2] * 255;
            byte v = (byte)Convert.ToInt32(value);
            byte p = (byte)Convert.ToInt32(value * (1 - hsv[1]));
            byte q = (byte)Convert.ToInt32(value * (1 - f * hsv[1]));
            byte t = (byte)Convert.ToInt32(value * (1 - (1 - f) * hsv[1]));

            if (hi == 0)
                return new byte[] { v, t, p };
            else if (hi == 1)
                return new byte[] { q, v, p };
            else if (hi == 2)
                return new byte[] { p, v, t };
            else if (hi == 3)
                return new byte[] { p, q, v };
            else if (hi == 4)
                return new byte[] { t, p, v };
            else
                return new byte[] { v, p, q };
        }

        private Image AddDetailToSkinDelta(Image skinImage, Image detailImage)
        {
            if (detailImage == null) return skinImage;
            Bitmap skin = (Bitmap)skinImage;
            Bitmap details = (Bitmap)detailImage;

            byte midpoint = 0xD0;

            Rectangle rect1 = new Rectangle(0, 0, skin.Width, skin.Height);
            System.Drawing.Imaging.BitmapData bmpData1 = skin.LockBits(rect1, ImageLockMode.ReadWrite,
                skin.PixelFormat);
            IntPtr ptr1;
            if (bmpData1.Stride > 0) ptr1 = bmpData1.Scan0;
            else ptr1 = bmpData1.Scan0 + bmpData1.Stride * (skin.Height - 1);
            int bytes1 = Math.Abs(bmpData1.Stride) * skin.Height;
            byte[] argbValues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, argbValues1, 0, bytes1);

            Rectangle rect2 = new Rectangle(0, 0, details.Width, details.Height);
            System.Drawing.Imaging.BitmapData bmpData2 = details.LockBits(rect2, ImageLockMode.ReadWrite,
                details.PixelFormat);
            IntPtr ptr2;
            if (bmpData2.Stride > 0) ptr2 = bmpData2.Scan0;
            else ptr2 = bmpData2.Scan0 + bmpData2.Stride * (details.Height - 1);
            int bytes2 = Math.Abs(bmpData2.Stride) * details.Height;
            byte[] argbValues2 = new byte[bytes2];
            System.Runtime.InteropServices.Marshal.Copy(ptr2, argbValues2, 0, bytes2);

            for (int i = 0; i < argbValues1.Length; i += 4)
            {
                float alpha = argbValues2[i + 3] / 255f;
                if (alpha < .01) continue;
                float[] hsvTmp1 = GetHSV(argbValues1[i + 2], argbValues1[i + 1], argbValues1[i]);
                int deltaLuminance = argbValues2[i + 1] - midpoint;
                float deltaLum = deltaLuminance / 255f;
                hsvTmp1[2] += deltaLum;
                byte[] rgb = GetRGB(hsvTmp1);
                argbValues1[i] = (byte)((rgb[2] * alpha) + (argbValues1[i] * (1f - alpha)));
                argbValues1[i + 1] = (byte)((rgb[1] * alpha) + (argbValues1[i + 1] * (1f - alpha)));
                argbValues1[i + 2] = (byte)((rgb[0] * alpha) + (argbValues1[i + 2] * (1f - alpha)));
            }
            System.Runtime.InteropServices.Marshal.Copy(argbValues1, 0, ptr1, bytes1);
            skin.UnlockBits(bmpData1);
            details.UnlockBits(bmpData2);
            details.Dispose();

            return skin;
        }


        //private Image AddDetailToSkinDelta(Image skinImage, Image detailImage)
        //{
        //    if (detailImage == null) return skinImage;
        //    Bitmap skin = (Bitmap)skinImage;
        //    Bitmap details = (Bitmap)detailImage;

        //    byte midpoint = 0xD0;

        //    Rectangle rect1 = new Rectangle(0, 0, skin.Width, skin.Height);
        //    System.Drawing.Imaging.BitmapData bmpData1 = skin.LockBits(rect1, ImageLockMode.ReadWrite,
        //        skin.PixelFormat);
        //    IntPtr ptr1;
        //    if (bmpData1.Stride > 0) ptr1 = bmpData1.Scan0;
        //    else ptr1 = bmpData1.Scan0 + bmpData1.Stride * (skin.Height - 1);
        //    int bytes1 = Math.Abs(bmpData1.Stride) * skin.Height;
        //    byte[] argbValues1 = new byte[bytes1];
        //    System.Runtime.InteropServices.Marshal.Copy(ptr1, argbValues1, 0, bytes1);

        //    Rectangle rect2 = new Rectangle(0, 0, details.Width, details.Height);
        //    System.Drawing.Imaging.BitmapData bmpData2 = details.LockBits(rect2, ImageLockMode.ReadWrite,
        //        details.PixelFormat);
        //    IntPtr ptr2;
        //    if (bmpData2.Stride > 0) ptr2 = bmpData2.Scan0;
        //    else ptr2 = bmpData2.Scan0 + bmpData2.Stride * (details.Height - 1);
        //    int bytes2 = Math.Abs(bmpData2.Stride) * details.Height;
        //    byte[] argbValues2 = new byte[bytes2];
        //    System.Runtime.InteropServices.Marshal.Copy(ptr2, argbValues2, 0, bytes2);

        //    for (int i = 0; i < argbValues1.Length; i += 4)
        //    {
        //        float alpha = argbValues2[i + 3] / 255f;
        //        if (alpha < .01) continue;
        //        ushort[] hslTmp1 = GetHSL(argbValues1[i + 2], argbValues1[i + 1], argbValues1[i]);
        //        int deltaLuminance = argbValues2[i + 1] - midpoint;
        //        float deltaLum = deltaLuminance / 255f;
        //        ushort adjustedLum = (ushort)(hslTmp1[2] + (deltaLum * 240f));
        //        byte[] rgb = GetRGB(hslTmp1[0], hslTmp1[1], adjustedLum);
        //        argbValues1[i] = (byte)((rgb[2] * alpha) + (argbValues1[i] * (1f - alpha)));
        //        argbValues1[i + 1] = (byte)((rgb[1] * alpha) + (argbValues1[i + 1] * (1f - alpha)));
        //        argbValues1[i + 2] = (byte)((rgb[0] * alpha) + (argbValues1[i + 2] * (1f - alpha)));
        //    }
        //    System.Runtime.InteropServices.Marshal.Copy(argbValues1, 0, ptr1, bytes1);
        //    skin.UnlockBits(bmpData1);
        //    details.UnlockBits(bmpData2);
        //    details.Dispose();

        //    return skin;
        //}

        private Image AddDetailToSkin(Image skinImage, Image detailImage)   
        {
            Bitmap skin = (Bitmap)skinImage;
            Bitmap details = (Bitmap)detailImage;

            Rectangle rect1 = new Rectangle(0, 0, skin.Width, skin.Height);
            System.Drawing.Imaging.BitmapData bmpData1 = skin.LockBits(rect1, ImageLockMode.ReadWrite,
                skin.PixelFormat);
            IntPtr ptr1;
            if (bmpData1.Stride > 0) ptr1 = bmpData1.Scan0;
            else ptr1 = bmpData1.Scan0 + bmpData1.Stride * (skin.Height - 1);
            int bytes1 = Math.Abs(bmpData1.Stride) * skin.Height;
            byte[] argbValues1 = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, argbValues1, 0, bytes1);

            Rectangle rect2 = new Rectangle(0, 0, details.Width, details.Height);
            System.Drawing.Imaging.BitmapData bmpData2 = details.LockBits(rect2, ImageLockMode.ReadWrite,
                details.PixelFormat);
            IntPtr ptr2;
            if (bmpData2.Stride > 0) ptr2 = bmpData2.Scan0;
            else ptr2 = bmpData2.Scan0 + bmpData2.Stride * (details.Height - 1);
            int bytes2 = Math.Abs(bmpData2.Stride) * details.Height;
            byte[] argbValues2 = new byte[bytes2];
            System.Runtime.InteropServices.Marshal.Copy(ptr2, argbValues2, 0, bytes2);

            for (int i = 0; i < argbValues1.Length; i += 4)
            {
                float alpha = argbValues2[i + 3] / 255f;
                if (alpha < .01) continue;
                ushort[] hslTmp1 = GetHSL(argbValues1[i + 2], argbValues1[i + 1], argbValues1[i]);

                // ushort[] hslTmp2 = GetHSL(UnPreMult(argbValues2[i + 2], alpha), UnPreMult(argbValues2[i + 1], alpha), UnPreMult(argbValues2[i], alpha));
                // byte[] rgb = GetRGB(hslTmp1[0], hslTmp1[1], hslTmp2[2]);

                //float[] hsvTmp1 = GetHSV(argbValues1[i + 2], argbValues1[i + 1], argbValues1[i]);
                //float[] hsvTmp2 = GetHSV(UnPreMult(argbValues2[i + 2], alpha), UnPreMult(argbValues2[i + 1], alpha), UnPreMult(argbValues2[i], alpha));
                //hsvTmp1[2] = hsvTmp2[2];
                //byte[] rgb = GetRGB(hsvTmp1);

                byte[] rgb = GetRGB(hslTmp1[0], hslTmp1[1], (ushort)(UnPreMult(argbValues2[i + 1], alpha) / 255f * 240f));
                argbValues1[i] = (byte)((rgb[2] * alpha) + (argbValues1[i] * (1f - alpha)));
                argbValues1[i + 1] = (byte)((rgb[1] * alpha) + (argbValues1[i + 1] * (1f - alpha)));
                argbValues1[i + 2] = (byte)((rgb[0] * alpha) + (argbValues1[i + 2] * (1f - alpha)));
            }
            System.Runtime.InteropServices.Marshal.Copy(argbValues1, 0, ptr1, bytes1);
            skin.UnlockBits(bmpData1);
            details.UnlockBits(bmpData2);
            details.Dispose();

            return skin;
        }

        private byte UnPreMult(byte v, float a)
        {
            return (byte)(Math.Min(((v / 255f) / a * 255f), 255));
        }

        private byte Luminance(byte r, byte g, byte b)
        {
            return (byte)Math.Sqrt((r * r * .299) + (g * g * .587) + (b * b * .114));
        }



        private Image DisplayableSkintone37(TONE tone, int skinState, Bitmap tanLines, AgeGender age, AgeGender gender, float[] physiqueWeights,
                        float pregnantShape, Image sculptOverlay, Image outfitOverlay)
        {
            float[][] alphaMatrix = {
                   new float[] {1, 0, 0, 0, 0},       // m00 = red scaling factor
                   new float[] {0, 1, 0, 0, 0},       // m11 = green scaling factor
                   new float[] {0, 0, 1, 0, 0},       // m22 = blue scaling factor
                   new float[] {0, 0, 0, 1, 0},       // m33 = alpha scaling factor
                   new float[] {0, 0, 0, 0, 1}        // increments for R, G, B, A
            };

            float[][] maskMatrix = {
                   new float[] {1, 0, 0, 0, 0},       // m00 = red scaling factor
                   new float[] {0, 1, 0, 0, 0},       // m11 = green scaling factor
                   new float[] {0, 0, 1, 0, 0},       // m22 = blue scaling factor
                   new float[] {0, 0, 0, .15f, 0},    // m33 = alpha scaling factor
                   new float[] {0, 0, 0, 0, 1}        // increments for R, G, B, A
            };
            //float C = 1.2f;
            //float T = 0.5f * (1f - C);
            //float[][] contrastMatrix = {
            //       new float[] {C, 0, 0, 0, 0},       
            //       new float[] {0, C, 0, 0, 0},       
            //       new float[] {0, 0, C, 0, 0},       
            //       new float[] {0, 0, 0, 1, 0},       
            //       new float[] {T, T, T, 0, 1}        
            //    };

            float contrast = 1.1f;
            float midpoint = 0.75f;
            int skinIndex = FindSetBit((uint)age);
            if (age == AgeGender.Infant) skinIndex = 1;
            else if (age > AgeGender.Child && gender == AgeGender.Female) skinIndex = skinIndex + 8;

            Bitmap details = FetchGameTexture(detailInstance[skinIndex][0], -1, ref errorList, false);
            if (details == null) return null;
            using (Graphics gr = Graphics.FromImage(details))
            {
                if (age > AgeGender.Child && age != AgeGender.Infant)
                {
                    Bitmap overlay = FetchGameTexture(detailInstance[skinIndex + 4][0], -1, ref errorList, false);
                    if (overlay != null)
                    {
                        gr.DrawImage(overlay, new Rectangle(0, 0, details.Width, details.Height), 0, 0, overlay.Width, overlay.Height, GraphicsUnit.Pixel);
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (physiqueWeights[i] > 0)
                        {
                            ImageAttributes attributes = new ImageAttributes();
                            alphaMatrix[3][3] = physiqueWeights[i];
                            ColorMatrix convert = new ColorMatrix(alphaMatrix);
                            attributes.SetColorMatrix(convert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            Bitmap physique = FetchGameTexture(detailInstance[skinIndex][i + 1], -1, ref errorList, false);
                            if (physique != null)
                            {
                                gr.DrawImage(physique, new Rectangle(0, 0, details.Width, details.Height), 0, 0, physique.Width, physique.Height, GraphicsUnit.Pixel, attributes);
                                physique.Dispose();
                            }
                            Bitmap pOverlay = FetchGameTexture(detailInstance[skinIndex + 4][i + 1], -1, ref errorList, false);
                            if (pOverlay != null)
                            {
                                gr.DrawImage(pOverlay, new Rectangle(0, 0, details.Width, details.Height), 0, 0, pOverlay.Width, pOverlay.Height, GraphicsUnit.Pixel, attributes);
                                pOverlay.Dispose();
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i += 2)
                    {
                        if (physiqueWeights[i] > 0)
                        {
                            alphaMatrix[3][3] = physiqueWeights[i];
                            ColorMatrix convert = new ColorMatrix(alphaMatrix);
                            ImageAttributes attributes = new ImageAttributes();
                            attributes.SetColorMatrix(convert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            Bitmap physique = FetchGameTexture(new TGI((uint)ResourceTypes.LRLE, 0, detailInstance[skinIndex][i + 1]), -1, ref errorList, false);
                            if (physique != null)
                            {
                                gr.DrawImage(physique, new Rectangle(0, 0, details.Width, details.Height), 0, 0, physique.Width, physique.Height, GraphicsUnit.Pixel, attributes);
                                physique.Dispose();
                            }
                        }
                    }
                }
            }

            //using (Graphics g = Graphics.FromImage(details))
            //{
            //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    g.DrawImage(sculptOverlay, new Point(0, 0));
            //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    g.DrawImage(outfitOverlay, new Point(0, 0));
            //}
            Bitmap img = new Bitmap(sculptOverlay);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(details, new Rectangle(0, 0, img.Width, img.Height));
            }
            details = img;
            using (Graphics g = Graphics.FromImage(details))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(outfitOverlay, new Rectangle(0, 0, details.Width, details.Height));
            }

            if (tone == null) return details;

            TONE.SkinSetDesc skinSet = tone.SkinSets[0];
            Bitmap skin = FetchGameTexture(tone.SkinSets[0].TextureInstance, -1, ref errorList, true);
            if (skin == null) return details;
            if (currentSkinSet > 0)
            {
                using (Graphics g = Graphics.FromImage(skin))
                {
                    //for (int i = 0; i < overlays.Count; i++)
                    //{
                    //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    //    g.DrawImage(overlays[i], new Point(0, 0));
                    //}
                    Bitmap tan = FetchGameTexture(tone.SkinSets[currentSkinSet].TextureInstance, -1, ref errorList, true);
                    if (tan != null)
                    {
                        if (TanLines_checkBox.Checked && tanLines != null) tan.SetAlphaFromImage(new Bitmap(tanLines, tan.Width, tan.Height));
                        g.DrawImage(tan, new Point(0, 0));
                        tan.Dispose();
                    }

                    Bitmap mask = FetchGameTexture(tone.SkinSets[currentSkinSet].overlayInstance, -1, ref errorList, true);
                    if (mask != null)
                    {
                        ColorMatrix maskConvert = new ColorMatrix(maskMatrix);
                        ImageAttributes maskAttributes = new ImageAttributes();
                        maskAttributes.SetColorMatrix(maskConvert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        g.DrawImage(mask, new Rectangle(0, 0, skin.Width, skin.Height), 0, 0, skin.Width, skin.Height, GraphicsUnit.Pixel, maskAttributes);
                    }
                }
            }

            if (details.Width != skin.Width || details.Height != skin.Height)
            {
                skin = new Bitmap(skin, details.Width, details.Height);
            }

            float burnMultiplier = skinSet.OverlayMultiplier;
            ushort overlaidHue = tone.Hue;
            ushort overlaidSaturation = tone.Saturation;
            uint Pass2Opacity = tone.Opacity;

            Rectangle rect1 = new Rectangle(0, 0, skin.Width, skin.Height);
            System.Drawing.Imaging.BitmapData bmpData1 = skin.LockBits(rect1, ImageLockMode.ReadWrite, skin.PixelFormat);
            IntPtr ptr1;
            if (bmpData1.Stride > 0) ptr1 = bmpData1.Scan0;
            else ptr1 = bmpData1.Scan0 + bmpData1.Stride * (skin.Height - 1);
            int bytes1 = Math.Abs(bmpData1.Stride) * skin.Height;
            byte[] color = new byte[bytes1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, color, 0, bytes1);

            Rectangle rect2 = new Rectangle(0, 0, details.Width, details.Height);
            System.Drawing.Imaging.BitmapData bmpData2 = details.LockBits(rect2, ImageLockMode.ReadWrite, details.PixelFormat);
            IntPtr ptr2;
            if (bmpData2.Stride > 0) ptr2 = bmpData2.Scan0;
            else ptr2 = bmpData2.Scan0 + bmpData2.Stride * (details.Height - 1);
            int bytes2 = Math.Abs(bmpData2.Stride) * details.Height;
            byte[] detail = new byte[bytes2];
            System.Runtime.InteropServices.Marshal.Copy(ptr2, detail, 0, bytes2);

            float pass2opacity = Pass2Opacity / 100f;
            byte[] rgbOver = GetRGB(overlaidHue, overlaidSaturation, 100);
            //  float overAlpha = overlaidSaturation / 100f;

            // argbValues1[i] = blue
            // argbValues1[i + 1] = green
            // argbValues1[i + 2] = red
            // argbValues1[i + 3] = alpha

            // int cutoff = (skin.Height / 4) * skin.Width * 4;      //where to stop making transparent

            for (int i = 0; i < color.Length; i += 4)
            {
                for (int j = 0; j < 3; j++)
                {
                    float tmp;      //first pass softlight, details over color (keeps color)
                    tmp = ((1f - 2f * (detail[i + j] / 255f)) * (color[i + j] / 255f) * (color[i + j] / 255f) + 2f * (detail[i + j] / 255f) * (color[i + j] / 255f)) * 255f;
                    tmp = (float)Math.Min(tmp * 1.2, 255.0);        //but it's too dark

                    float tmp2;
                    if (tmp > 128)           //second pass overlay blend, details over color
                        tmp2 = 255f - ((255f - 2f * (detail[i + j] - 128f)) * (255f - tmp) / 256f);
                    else
                        tmp2 = (2f * detail[i + j] * tmp) / 256f;

                    tmp = ((tmp2 * pass2opacity) + (tmp * (1f - pass2opacity)));          // blend using 2nd pass opacity

                    if (overlaidSaturation <= 100)
                        tmp = ((tmp / 255f) * (tmp + ((2f * rgbOver[2 - j]) / 255f) * (255f - tmp)));  //3rd pass is soft light blend, color over all

                    // increase contrast slightly
                    tmp = ((((tmp / 255f) - midpoint) * contrast) + midpoint) * 255f;
                    if (tmp < 0f) tmp = 0;
                    if (tmp > 255) tmp = 255;

                    color[i + j] = (byte)tmp;
                }
                // if (i < cutoff) color[i + 3] = 0;
            }
            System.Runtime.InteropServices.Marshal.Copy(color, 0, ptr1, bytes1);
            skin.UnlockBits(bmpData1);
            details.UnlockBits(bmpData2);
            // details.Dispose();

            //if (mask != null)
            //{
            //    alphaMatrix[3][3] = .2f;
            //    ColorMatrix maskConvert = new ColorMatrix(alphaMatrix);
            //    ImageAttributes maskAttributes = new ImageAttributes();
            //    maskAttributes.SetColorMatrix(maskConvert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            //    using (Graphics gr = Graphics.FromImage(skin))
            //    {
            //        gr.DrawImage(mask, new Rectangle(0, 0, skin.Width, skin.Height), 0, 0, mask.Width, mask.Height, GraphicsUnit.Pixel, maskAttributes);
            //    }
            //}

            using (Graphics gr = Graphics.FromImage(skin))
            {
                ulong overlayInstance = tone.GetOverlayInstance(age & gender);
                if (overlayInstance > 0)
                {
                    Bitmap skinOverlay = FetchGameTexture(overlayInstance, -1, ref errorList, true);
                    if (skinOverlay != null) gr.DrawImage(skinOverlay, new Rectangle(0, 0, skin.Width, skin.Height), 0, 0, skinOverlay.Width, skinOverlay.Height, GraphicsUnit.Pixel);
                }
                Bitmap mouthOverlay = Properties.Resources.HeadMouthColor;
                if (mouthOverlay.Size != humanTextureSize) mouthOverlay = new Bitmap(mouthOverlay, humanTextureSize);
                gr.DrawImage(mouthOverlay, new Rectangle(0, 0, skin.Width, skin.Height), 0, 0, mouthOverlay.Width, mouthOverlay.Height, GraphicsUnit.Pixel);
            }

            return skin;
        }
    }
}
