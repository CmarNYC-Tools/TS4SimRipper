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
using s4pi.Package;
using s4pi.Interfaces;

namespace TS4SimRipper
{
    public partial class Form1 : Form
    {
        public Bitmap FetchPaintedCoat(TGI tgi, ref string errorMsg)
        {
            if (currentSaveGame == null)
            {
                errorMsg += "Current save game is null, can't read painted coat!" + Environment.NewLine;
                return null;
            }
            if (tgi.Instance == 0UL)
            {
                errorMsg += "Painted coat tgi invalid, can't read painted coat!" + Environment.NewLine;
                return null;
            }
            Predicate<IResourceIndexEntry> pred = r => r.ResourceType == tgi.Type & r.ResourceGroup == tgi.Group & r.Instance == tgi.Instance;
            try
            {
                IResourceIndexEntry irie = currentSaveGame.Find(pred);
                Stream s = currentSaveGame.GetResource(irie);
                if (s != null)
                {
                        MemoryStream msa = new MemoryStream();
                        MemoryStream msi = new MemoryStream();
                        s.Position = 0;
                        int i = 0;
                    do
                    {
                        i = s.ReadByte();
                        long soi = 0;
                        if (i == 0xFF)                  //search for start of image
                        {
                            soi = s.Position - 1;
                            if (s.ReadByte() != 0xD8) continue;
                            if (s.ReadByte() != 0xFF) continue;
                            if (s.ReadByte() != 0xE0) continue;

                            byte[] buffer = new byte[20];
                            s.Position = soi;
                            s.Read(buffer, 0, 20);    //copy APP0 marker for alpha
                            msa.Write(buffer, 0, 20);
                            s.Position += 12;        //skip proprietary ALFJ APP0 marker
                            s.Read(buffer, 0, 20);   //copy APP0 marker for RGB image
                            msi.Write(buffer, 0, 20);

                            ushort eoi = 0xFFD9;            //end of image marker
                            ushort testbuff = 0;
                            do
                            {                               //copy alpha data
                                i = s.ReadByte();
                                if (i == 0xFF)
                                {
                                    i = s.ReadByte();
                                    if (i == 0xE0)
                                    {
                                        s.Position += 6;    //skip subsequent proprietary ALFX APP0 markers
                                        i = s.ReadByte();
                                    }
                                    else
                                    {
                                        s.Position -= 1;     //back up
                                        i = 0xFF;
                                    }
                                }
                                testbuff = (ushort)((testbuff << 8) + (byte)i);
                                msa.WriteByte((byte)i);
                            } while (testbuff != eoi);
                            testbuff = 0;
                            do
                            {                               //copy rgb image data
                                i = s.ReadByte();
                                testbuff = (ushort)((testbuff << 8) + (byte)i);
                                msi.WriteByte((byte)i);
                            } while (testbuff != eoi);

                            Bitmap alpha = new Bitmap(msa);
                            Bitmap rgb = new Bitmap(msi);
                            rgb = ApplyAlpha(rgb, alpha);
                            if (rgb.Size == currentSize) return rgb;
                            else return new Bitmap(rgb, currentSize);
                            //coatImage.Add(ApplyAlpha(rgb, alpha));
                            //msa = new MemoryStream();
                            //msi = new MemoryStream();
                        }

                    } while (i != -1);
                }
                errorMsg += "Could not extract painted coat " + tgi.ToString() + Environment.NewLine;
                return null;
            }
            catch (Exception ex)
            {
                errorMsg += "Could not read painted coat " + tgi.ToString() + ". Original error: " + ex.Message + Environment.NewLine;
                return null;
            }
        }

        public static Bitmap ApplyAlpha(Bitmap rgb, Bitmap alpha)
        {
            Bitmap tmp = new Bitmap(rgb);

            // Use LockBits to make this faster
            Rectangle rect = new Rectangle(0, 0, tmp.Width, tmp.Height);
            BitmapData bmpData = tmp.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            IntPtr ptr = bmpData.Scan0;

            // Alpha image
            Rectangle rectA = new Rectangle(0, 0, alpha.Width, alpha.Height);
            BitmapData bmpDataA = alpha.LockBits(rectA, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            IntPtr ptrA = bmpDataA.Scan0;

            int bytesPerPixel = 4;
            int numBytes = rgb.Width * rgb.Height * bytesPerPixel;
            byte[] argbValues = new byte[numBytes];
            byte[] alphaValues = new byte[numBytes];

            // Copy values into arrays
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numBytes);
            System.Runtime.InteropServices.Marshal.Copy(ptrA, alphaValues, 0, numBytes);

            // Copy alpha values into argb array
            for (int counter = 0; counter < argbValues.Length; counter += bytesPerPixel)
            {
                // argbValues is in format BGRA (Blue, Green, Red, Alpha)
                argbValues[counter + bytesPerPixel - 1] = alphaValues[counter];     //copy blue channel of alpha to alpha of argb
            }

            // Copy the ARGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numBytes);

            // Unlock the bits.
            tmp.UnlockBits(bmpData);
            alpha.UnlockBits(bmpDataA);

            return tmp;
        }
    }
}
