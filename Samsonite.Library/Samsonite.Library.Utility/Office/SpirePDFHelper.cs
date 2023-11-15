using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using Spire.Pdf.Security;

namespace Samsonite.Library.Utility
{
    public class SpirePDFHelper
    {
        #region 安全性
        /// <summary>
        /// pdf加密
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="ownerPassword">权限密码</param>
        /// <param name="outputFileName"></param>
        public static void Encrypt(string inputFileName, string userPassword, string ownerPassword, string outputFileName)
        {
            try
            {
                //加载文档
                using (PdfDocument pdfDocument = new PdfDocument(inputFileName))
                {
                    //加密
                    pdfDocument.Security.Encrypt(userPassword, ownerPassword, PdfPermissionsFlags.Print | PdfPermissionsFlags.FillFields, PdfEncryptionKeySize.Key128Bit);
                    //保存文件
                    pdfDocument.SaveToFile(outputFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// pdf解密
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <param name="userPassword">用户密码</param>
        /// <param name="ownerPassword">权限密码</param>
        /// /// <param name="outputFileName"></param>
        public static void Decrypt(string inputFileName, string userPassword, string ownerPassword, string outputFileName)
        {
            try
            {
                //加载文档
                using (PdfDocument pdfDocument = new PdfDocument(inputFileName, userPassword))
                {
                    //解密
                    pdfDocument.Security.Encrypt("", "", PdfPermissionsFlags.Default, pdfDocument.Security.KeySize, ownerPassword);
                    //保存文件
                    pdfDocument.SaveToFile(outputFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除内容
        /// <summary>
        /// 删除pdf中的文字信息
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <param name="text"></param>
        /// <param name="outputFileName"></param>
        public static void RemoveText(string inputFileName, string text, string outputFileName)
        {
            try
            {
                //加载文档
                using (PdfDocument pdfDocument = new PdfDocument(inputFileName))
                {
                    for (int i = 0; i < pdfDocument.Pages.Count; i++)
                    {
                        //Searches "Spire.PDF for .NET" by ignoring case
                        PdfTextFindCollection collection = pdfDocument.Pages[i].FindText(text, TextFindParameter.IgnoreCase);
                        //Gets the bound of the found text in page
                        foreach (PdfTextFind find in collection.Finds)
                        {
                            foreach (var rec in find.TextBounds)
                            {
                                pdfDocument.Pages[i].Canvas.DrawRectangle(PdfBrushes.White, rec);
                            }
                        }
                    }
                    //保存文件
                    pdfDocument.SaveToFile(outputFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 固定矩形区域内的信息
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <param name="rectangleF"></param>
        /// <param name="outputFileName"></param>
        public static void RemoveRectangle(string inputFileName, RectangleF rectangleF, string outputFileName)
        {
            try
            {
                //加载文档
                using (PdfDocument pdfDocument = new PdfDocument(inputFileName))
                {
                    for (int i = 0; i < pdfDocument.Pages.Count; i++)
                    {
                        pdfDocument.Pages[i].Canvas.DrawRectangle(PdfBrushes.White, rectangleF);
                    }
                    //保存文件
                    pdfDocument.SaveToFile(outputFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 转换文档
        /// <summary>
        /// 转成图片
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="imageName"></param>
        /// <param name="imageFormat"></param>
        public static List<string> ConvertToImage(string inputFileName, string outputFilePath, string imageName, ImageFormat imageFormat)
        {
            List<string> _result = new List<string>();
            try
            {
                //加载文档
                using (PdfDocument pdfDocument = new PdfDocument(inputFileName))
                {
                    int pdfPage = pdfDocument.Pages.Count;
                    //Save to images
                    for (int i = 0; i < pdfPage; i++)
                    {
                        string fileName = $"{imageName}_{pdfPage}_{i}.{imageFormat}";
                        string filePath = Path.Combine(outputFilePath, fileName);
                        using (var image = pdfDocument.SaveAsImage(i))
                        {
                            //保存图片
                            image.Save(filePath, imageFormat);
                            //返回图片
                            _result.Add(fileName);
                        }
                    }
                }
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 转成图片
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="imageName"></param>
        /// <param name="imageFormat"></param>
        /// <param name="dpiX"></param>
        /// <param name="dpiY"></param>
        public static List<string> ConvertToImage(string inputFileName, string outputFilePath, string imageName, ImageFormat imageFormat, int dpiX = 300, int dpiY = 300)
        {
            List<string> _result = new List<string>();
            try
            {
                //加载文档
                using (PdfDocument pdfDocument = new PdfDocument(inputFileName))
                {
                    int pdfPage = pdfDocument.Pages.Count;
                    //Save to images
                    for (int i = 0; i < pdfPage; i++)
                    {
                        string fileName = $"{imageName}_{pdfPage}_{i}.{imageFormat}";
                        string filePath = Path.Combine(outputFilePath, fileName);
                        using (var image = pdfDocument.SaveAsImage(i, dpiX, dpiY))
                        {
                            //保存图片
                            image.Save(filePath, imageFormat);
                            //返回图片
                            _result.Add(fileName);
                        }
                    }
                }
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
