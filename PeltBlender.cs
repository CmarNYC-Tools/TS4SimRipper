/* TS4 SimRipper, a tool for creating custom content for The Sims 4,
   Copyright (C) 2014  C. Marinetti

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>. 
   The author may be contacted at modthesims.info, username cmarNYC. */

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
using ProtoBuf;
using TS4SaveGame= EA.Sims4.Persistence;
namespace TS4SimRipper
{
    public partial class Form1 : Form
    {
        private Image DisplayablePelt(List<TS4SaveGame.PeltLayerData> peltLayers, Image sculptOverlay, float[] physiqueWeights)
        {
            Bitmap details;
            if (currentOccult == SimOccult.Werewolf)
            {                                                           //neutral, heavy, fit, lean, bony
                ulong[] wolfDetailsInstance = new ulong[] { 0x1F52549DEC59E9DA, 0x59A47D8E3E13DEF2, 0x342ADDA798142502, 0x7DBF069EF4CC602F, 0xD26D60979C93ABD1 };
                float[][] alphaMatrix = {
                   new float[] {1, 0, 0, 0, 0},       // m00 = red scaling factor
                   new float[] {0, 1, 0, 0, 0},       // m11 = green scaling factor
                   new float[] {0, 0, 1, 0, 0},       // m22 = blue scaling factor
                   new float[] {0, 0, 0, 1, 0},       // m33 = alpha scaling factor
                   new float[] {0, 0, 0, 0, 1}        // increments for R, G, B, A
                };
                details = FetchGameTexture(new TGI((uint)ResourceTypes.LRLE, 0, wolfDetailsInstance[0]), -1, ref errorList, false);
                if (details == null) details = new Bitmap(currentSize.Width, currentSize.Height);
                if (physiqueWeights != null)
                {
                    using (Graphics gr = Graphics.FromImage(details))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (physiqueWeights[i] > 0)
                            {
                                alphaMatrix[3][3] = physiqueWeights[i];
                                ColorMatrix convert = new ColorMatrix(alphaMatrix);
                                ImageAttributes attributes = new ImageAttributes();
                                attributes.SetColorMatrix(convert, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                Bitmap physique = FetchGameTexture(new TGI((uint)ResourceTypes.LRLE, 0, wolfDetailsInstance[i + 1]), -1, ref errorList, false);
                                if (physique != null)
                                {
                                    gr.DrawImage(physique, new Rectangle(0, 0, details.Width, details.Height), 0, 0, physique.Width, physique.Height, GraphicsUnit.Pixel, attributes);
                                    physique.Dispose();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                details = currentSpecies switch{
                    Species.Cat =>Properties.Resources.CatSkin,
                    Species.Dog =>Properties.Resources.DogSkin,
                    Species.LittleDog =>Properties.Resources.DogSkin,
                    Species.Fox => Properties.Resources.DogSkin,
                    Species.Horse => Properties.Resources.HorseSkin,
                    _=>throw new Exception($"Unable to display pelt of unknown animal type.")
                };
            }
            if (details.Size != currentSize) details = new Bitmap(details, currentSize);

            using (Graphics g = Graphics.FromImage(details))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(sculptOverlay, 0, 0, details.Width, details.Height);
            }

            Bitmap pelt = null;
            for (int p = 0; p < peltLayers.Count; p++)
            {
                PeltLayer peltLayer = FetchGamePeltLayer(new TGI((uint)ResourceTypes.PeltLayer, 0, peltLayers[p].layer_id), ref errorList);
                Bitmap alpha = FetchGameImageFromRLE(new TGI((uint)ResourceTypes.RLE2, 0, peltLayer.TextureKey), -1, ref errorList);
                uint peltColor = peltLayers[p].color;
                Bitmap layer = new Bitmap(alpha.Width, alpha.Height);
                using (Graphics gc = Graphics.FromImage(layer))
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)peltColor)))
                    {
                        gc.FillRectangle(brush, 0, 0, layer.Width, layer.Height);
                    }
                }
                layer.SetAlphaFromImage(alpha);

                if (pelt == null)
                {
                    pelt = new Bitmap(layer);
                }
                else
                {
                    using (Graphics gr = Graphics.FromImage(pelt))
                    {
                        gr.DrawImage(layer, new Point(0, 0));
                    }
                }
                layer.Dispose();
            }

            //WriteImage("Save details", details, "");
            //WriteImage("Save pelt", pelt, "");

            Rectangle rect1 = new Rectangle(0, 0, pelt.Width, pelt.Height);
            System.Drawing.Imaging.BitmapData bmpData1 = pelt.LockBits(rect1, ImageLockMode.ReadWrite, pelt.PixelFormat);
            IntPtr ptr1;
            if (bmpData1.Stride > 0) ptr1 = bmpData1.Scan0;
            else ptr1 = bmpData1.Scan0 + bmpData1.Stride * (pelt.Height - 1);
            int bytes1 = Math.Abs(bmpData1.Stride) * pelt.Height;
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

            //float contrast = 1.2f;
            //float midpoint = 0.5f;

            // argbValues1[i] = blue
            // argbValues1[i + 1] = green
            // argbValues1[i + 2] = red
            // argbValues1[i + 3] = alpha

            for (int i = 0; i < color.Length; i += 4)
            {
                HSVColor hsvColor = new HSVColor(color[i + 2], color[i + 1], color[i]);
                HSVColor hsvDetail = new HSVColor(detail[i + 2], detail[i + 1], detail[i]);
                hsvColor.Value *= 0.9f;
                hsvColor.Value += ((hsvDetail.Value - 0.5f) * 1.1f) * (hsvColor.Value * 0.25f + 1f);
                byte[] rgb = hsvColor.ToRGBColor();
                color[i + 2] = rgb[0];
                color[i + 1] = rgb[1];
                color[i] = rgb[2];

                //HSLColor hslColor = new HSLColor(color[i + 2], color[i + 1], color[i]);
                //HSLColor hslDetail = new HSLColor(detail[i + 2], detail[i + 1], detail[i]);
                //hslColor.Luminosity *= 0.9f;
                //hslColor.Luminosity += (hslDetail.Luminosity - 0.5f) * 1.1f;
                //byte[] rgb = hslColor.ToRGBColor();
                //color[i + 2] = rgb[0];
                //color[i + 1] = rgb[1];
                //color[i] = rgb[2];

                //for (int j = 0; j < 3; j++)
                //{
                //    float tmp;

                //first pass softlight, details over color(keeps color)
                //tmp = ((1f - 2f * (detail[i + j] / 255f)) * (color[i + j] / 255f) * (color[i + j] / 255f) + 2f * (detail[i + j] / 255f) * (color[i + j] / 255f)) * 255f;
                // tmp = (float)Math.Min(tmp * 1.2, 255.0);        //but it's too dark

                //first pass softlight, color over details (keeps detail)
                //tmp = ((1f - 2f * (color[i + j] / 255f)) * (detail[i + j] / 255f) * (detail[i + j] / 255f) + 2f * (color[i + j] / 255f) * (detail[i + j] / 255f)) * 255f;

                //color[i + j] = (byte)tmp;

                //if (color[i + j] > 128)           //hard light blend, color over details
                //    tmp = 255 - ((255f - 2f * (color[i + j] - 128f)) * (255f - detail[i + j]) / 256f);
                //else
                //    tmp = (2f * color[i + j] * detail[i + j]) / 256f;

                //if (detail[i + j] > 128)           //hard light blend, details over color
                //    tmp = 255 - ((255f - 2f * (detail[i + j] - 128f)) * (255f - color[i + j]) / 256f);
                //else
                //    tmp = (2f * detail[i + j] * color[i + j]) / 256f;

                // increase contrast slightly
                //tmp = ((((tmp / 255f) - midpoint) * contrast) + midpoint) * 255f;
                //if (tmp < 0f) tmp = 0;
                //if (tmp > 255) tmp = 255;

                //color[i + j] = (byte)tmp;
                //}
                //}
            }
            System.Runtime.InteropServices.Marshal.Copy(color, 0, ptr1, bytes1);
            pelt.UnlockBits(bmpData1);
            details.UnlockBits(bmpData2);
            details.Dispose();

            if (currentPaintedCoatInstance > 0)
            {
                Bitmap paint = FetchPaintedCoat(new TGI(0xF8E1457A, 0x00800000, currentPaintedCoatInstance), ref errorList);
                using (Graphics gr = Graphics.FromImage(pelt))
                {
                    gr.DrawImage(paint, new Point(0, 0));
                }
            }

            return pelt;
        }
    }
}
