using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace CourseWork
{
    public partial class Document
    {
        public override string ToString()
        {
            return this.SigningDate.ToString("d") + " - " + this.Title;
        }
    }
    public partial class Meter
    {
        public override string ToString()
        {
            return this.ProductionId + " - " + this.Name;
        }
    }

    public partial class Parametr
    {
        public override string ToString()
        {
            return this.Name + " - " + this.Measure;
        }
    }

    public partial class Reading
    {
        public override string ToString()
        {
            return this.Value + " " + Meter.Type.Unit;
        }
    }

    public partial class Tariff
    {
        public override string ToString()
        {
            return this.Name;
        }
    }

    public partial class TimeSpan
    {
        public override string ToString()
        {
            return this.TimeStart.ToString("g") + " - " + this.TimeEnd.ToString("g");
        }
    }

    public partial class Type
    {
        public override string ToString()
        {
            return this.Name +" ("+this.Unit+")";
        }
    }

    public partial class User
    {
        public override string ToString()
        {
            return this.Login + " - " + this.FullName;
            
        }
    }


    public  abstract class Error
    {
        public static bool Show(string msg, string title)
        {
            System.Windows.MessageBox.Show(msg, title, MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }

    public abstract class ImageConverter
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(System.IntPtr hObject);

        public static System.Windows.Media.Imaging.BitmapSource convertBtmToBtmSource(System.Drawing.Bitmap btm)
        {
            using (System.Drawing.Bitmap bitmap = btm)
            {
                IntPtr hBitmap = bitmap.GetHbitmap();

                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(hBitmap);

                return bitmapSource;
            }
        }
    }

    public abstract class Export
    {
        // выгрузка в Эксель
        public static void Excel(params DataGrid[] dataGrids)
        {
            if (dataGrids == null) return;
            if (dataGrids.Length == 0) return;

            Mouse.SetCursor(Cursors.Wait);

            // создание экселя
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbooks books = excelApp.Workbooks;
            Excel._Workbook book = books.Add(System.Reflection.Missing.Value);
            Excel.Sheets sheets = book.Worksheets;
            Excel._Worksheet sheet = null;

            for (int i = 0; i < dataGrids.Length-1; i++)
            {
                sheets.Add();
            }

            for (int i = 0; i < dataGrids.Length; i++)
            {
                int headersCount = dataGrids[i].Items[0].GetType().GetProperties().Length;

                // заголовки
                string[] headers = new string[headersCount];
                for (int j = 0; j < headersCount; j++)
                    headers[j] = dataGrids[i].Items[0].GetType().GetProperties()[j].Name;


                // рабочий лист
                //sheets.Add();
                sheet = (Excel._Worksheet) (sheets.Item[i+1]);
                sheet.Name = dataGrids[i].Name;
                
                WriteData(sheet, dataGrids[i], headers);

                ReleaseObject(sheet);
            }

            // вывести на экран
            excelApp.Visible = true;

            // очистить объекты
            ReleaseObject(sheet);
            ReleaseObject(sheets);
            ReleaseObject(book);
            ReleaseObject(books);
            ReleaseObject(excelApp);

            Mouse.SetCursor(Cursors.Arrow);
        }

        // добавляет значения в указанный диапазон
        private static void AddExcelRows(Excel._Worksheet sheet, string startRange, int rowCount, int colCount, object values)
        {
            Excel.Range range = sheet.get_Range(startRange, System.Reflection.Missing.Value);
            range = range.get_Resize(rowCount, colCount);
            range.set_Value(System.Reflection.Missing.Value, values);
            ReleaseObject(range);
        }

        // автовыравнивания столбцов
        private static void AutoFitCollumns(Excel._Worksheet sheet, string startRange, int rowCount, int colCount)
        {
            Excel.Range range = sheet.get_Range(startRange, System.Reflection.Missing.Value);
            range = range.get_Resize(rowCount, colCount);
            range.Columns.AutoFit();
            ReleaseObject(range);
        }

        // выделение заголовков жирным
        private static void SetHeadersStyle(Excel._Worksheet sheet, string startRange,  int colCount)
        {
            Excel.Range range = sheet.get_Range(startRange, System.Reflection.Missing.Value);
            range = range.get_Resize(1, colCount);
            range.Font.Bold = true;
            ReleaseObject(range);
        }

        // заполнение
        private static void WriteData(Excel._Worksheet sheet,  DataGrid dg, string[] headers)
        {
            object[,] objData = new object[dg.Items.Count, headers.Length];

            for (int j = 0; j < dg.Items.Count; j++)
            {
                var item = dg.Items[j];
                for (int i = 0; i < headers.Length; i++)
                {
                    var y = dg.Items[0].GetType().InvokeMember
                        (headers[i], System.Reflection.BindingFlags.GetProperty, null, item, null);
                    objData[j, i] = y?.ToString() ?? "";
                }
            }

            // добавить на лист заголовки
            AddExcelRows(sheet, "A1", 1, headers.Length, headers);
            // добавить данные
            AddExcelRows(sheet, "A2", dg.Items.Count, headers.Length, objData);
            // выровнять столбцы
            AutoFitCollumns(sheet, "A1", dg.Items.Count + 1, headers.Length);
            // выделить заголовки
            SetHeadersStyle(sheet, "A1", headers.Length);
        }

        // очищение объектов
        private static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }

}
