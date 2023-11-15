using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace Samsonite.Library.Utility
{
    public class EPPlusExcelHelper : IDisposable
    {
        public ExcelPackage excelPackage { get; private set; }
        private FileStream fs;

        public EPPlusExcelHelper(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fileinfo = new FileInfo(filePath);
                excelPackage = new ExcelPackage(fileinfo);
            }
            else
            {
                fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
                excelPackage = new ExcelPackage(fs);

            }
        }

        /// <summary>
        /// 将DataTable数据转成Excel
        /// </summary>
        /// <param name="sourceTable"></param>
        public void DataTableToExcel(DataTable sourceTable)
        {
            try
            {
                this.AppendSheetToWorkBook(sourceTable);
                excelPackage.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取Excel数据转成Table
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="colCount">总列数</param>
        /// <param name="startCol">第一列开始索引</param>
        /// <param name="startRow">第一行数据行号</param>
        /// <returns></returns>
        public DataTable ExcelToDataTable(string sheetName, int colCount = -1, int startCol = 1, int startRow = 1)
        {
            DataTable dataTable = new DataTable();
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

                if (excelPackage.Workbook.Worksheets.Count() > 0)
                {
                    ExcelWorksheet excelWorksheet = (!string.IsNullOrEmpty(sheetName)) ? excelPackage.Workbook.Worksheets[sheetName] : excelPackage.Workbook.Worksheets[0];
                    //如果等于-1,就表示需要自动读取Excel的总行数
                    if (colCount == -1)
                    {
                        colCount = excelWorksheet.Dimension.Columns;
                    }
                    //如果设定列数大于总列数,则已总列数为准
                    if (colCount > excelWorksheet.Dimension.Columns)
                    {
                        colCount = excelWorksheet.Dimension.Columns;
                    }
                    int rowCount = excelWorksheet.Dimension.Rows;

                    if (rowCount > 0 && colCount > 0)
                    {
                        //读取标题
                        //注:EPPlus是从1开始读
                        for (var col = startCol; col <= colCount; col++)
                        {
                            dataTable.Columns.Add(excelWorksheet.Cells[startRow, col].Value?.ToString() ?? string.Empty);
                        }
                    }
                    //读取内容
                    startRow = startRow + 1;
                    for (var row = startRow; row <= rowCount; row++)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        int _columnIndex = 0;
                        for (var col = startCol; col <= colCount; col++)
                        {
                            dataRow[_columnIndex] = excelWorksheet.Cells[row, col].Value?.ToString() ?? string.Empty; ;
                            _columnIndex++;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region excel操作类
        /// <summary>
        /// DataTable数据导出到Excel(xlsx)
        /// </summary>
        /// <param name="ExcelPackage">ExcelPackage</param>
        /// <param name="sourceTable">数据源</param>
        /// <param name="isDeleteSameNameSheet">是否删除同名的sheet</param>
        private void AppendSheetToWorkBook(DataTable sourceTable, bool isDeleteSameNameSheet = false)
        {
            try
            {
                //创建worksheet
                ExcelWorksheet ws = AddSheet(sourceTable.TableName, isDeleteSameNameSheet);
                //从单元格A1开始，将数据表加载到工作表中。第1行输出列名
                ws.Cells["A1"].LoadFromDataTable(sourceTable, true);
                //格式化Row
                //FromatRow(sourceTable.Rows.Count, sourceTable.Columns.Count, ws);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 导出列表到excel,已存在同名sheet将删除已存在的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ExcelPackage"></param>
        /// <param name="list">数据源</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="isDeleteSameNameSheet">是否删除已存在的同名sheet,false时将重命名导出的sheet</param>
        private void AppendSheetToWorkBook<T>(IEnumerable<T> list, string sheetName, bool isDeleteSameNameSheet = false)
        {
            try
            {
                ExcelWorksheet ws = AddSheet(sheetName, isDeleteSameNameSheet);
                ws.Cells["A1"].LoadFromCollection(list, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// List转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataTable ListToDataTable<T>(IEnumerable<T> data)
        {
            try
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                DataTable dataTable = new DataTable();
                for (int i = 0; i < properties.Count; i++)
                {
                    PropertyDescriptor property = properties[i];
                    dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                }
                object[] values = new object[properties.Count];
                foreach (T item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = properties[i].GetValue(item);
                    }

                    dataTable.Rows.Add(values);
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        private void Save()
        {
            excelPackage.Save();
        }

        /// <summary>
        /// 添加Sheet到ExcelPackage
        /// </summary>
        /// <param name="ExcelPackage">ExcelPackage</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="isDeleteSameNameSheet">如果存在同名的sheet是否删除</param>
        /// <returns></returns>
        private ExcelWorksheet AddSheet(string sheetName, bool isDeleteSameNameSheet)
        {
            if (isDeleteSameNameSheet)
            {
                DeleteSheet(sheetName);
            }
            else
            {
                while (excelPackage.Workbook.Worksheets.Any(i => i.Name == sheetName))
                {
                    sheetName = sheetName + "(1)";
                }
            }

            ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add(sheetName);
            return ws;
        }

        /// <summary>
        /// 格式化Excel
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="ws"></param>
        private void FromatRow(int rowCount, int colCount, ExcelWorksheet ws)
        {
            ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
            Color borderColor = Color.FromArgb(28, 177, 243);

            //格式化内容
            using (ExcelRange rng = ws.Cells[1, 1, rowCount + 1, colCount])
            {
                //rng.Style.Font.Name = "宋体";
                rng.Style.Font.Size = 12;
                //设置图案的背景为Solid
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));

                rng.Style.Border.Top.Style = borderStyle;
                rng.Style.Border.Top.Color.SetColor(borderColor);

                rng.Style.Border.Bottom.Style = borderStyle;
                rng.Style.Border.Bottom.Color.SetColor(borderColor);

                rng.Style.Border.Right.Style = borderStyle;
                rng.Style.Border.Right.Color.SetColor(borderColor);
            }

            //格式化标题行
            using (ExcelRange rng = ws.Cells[1, 1, 1, colCount])
            {
                rng.Style.Font.Bold = true;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(28, 177, 243));
                rng.Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
            }
        }

        /// <summary>
        /// 删除指定的sheet
        /// </summary>
        /// <param name="ExcelPackage"></param>
        /// <param name="sheetName"></param>
        private void DeleteSheet(string sheetName)
        {
            var sheet = excelPackage.Workbook.Worksheets.FirstOrDefault(i => i.Name == sheetName);
            if (sheet != null)
            {
                excelPackage.Workbook.Worksheets.Delete(sheet);
            }
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="values">行类容，一个单元格一个对象</param>
        /// <param name="rowIndex">插入位置，起始位置为1</param>
        private void InsertValues(string sheetName, List<object> values, int rowIndex)
        {
            var sheet = GetOrAddSheet(sheetName);
            sheet.InsertRow(rowIndex, 1);
            int i = 1;
            foreach (var item in values)
            {
                sheet.SetValue(rowIndex, i, item);
                i++;
            }
        }

        /// <summary>
        /// 获取sheet,没有则创建
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private ExcelWorksheet GetOrAddSheet(string sheetName)
        {
            ExcelWorksheet ws = excelPackage.Workbook.Worksheets.FirstOrDefault(i => i.Name == sheetName);
            if (ws == null)
            {
                ws = excelPackage.Workbook.Worksheets.Add(sheetName);
            }
            return ws;
        }

        ///// <summary>
        ///// 添加文字图片
        ///// </summary>
        ///// <param name="sheet"></param>
        ///// <param name="msg">要转换成图片的文字</param>
        //public void AddPicture(string sheetName, string msg)
        //{
        //    Bitmap img = GetPictureString(msg);

        //    var sheet = GetOrAddSheet(sheetName);
        //    var picName = "92FF5CFE-2C1D-4A6B-92C6-661BDB9ED016";
        //    var pic = sheet.Drawings.FirstOrDefault(i => i.Name == picName);
        //    if (pic != null)
        //    {
        //        sheet.Drawings.Remove(pic);
        //    }
        //    pic = sheet.Drawings.AddPicture(picName, img);

        //    pic.SetPosition(3, 0, 6, 0);
        //}

        /// <summary>
        /// 文字绘制图片
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static Bitmap GetPictureString(string msg)
        {
            var msgs = msg.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var maxLenght = msgs.Max(i => i.Length);
            var rowCount = msgs.Count();
            var rowHeight = 23;
            var fontWidth = 17;
            var img = new Bitmap(maxLenght * fontWidth, rowCount * rowHeight);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.Clear(Color.White);
                Font font = new Font("Arial", 12, (FontStyle.Bold));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), Color.Blue, Color.DarkRed, 1.2f, true);

                for (int i = 0; i < msgs.Count(); i++)
                {
                    g.DrawString(msgs[i], font, brush, 3, 2 + rowHeight * i);
                }
            }
            return img;
        }

        public void Dispose()
        {
            excelPackage.Dispose();
            if (fs != null)
            {
                fs.Dispose();
                fs.Close();
            }
        }
        #endregion
    }
}

