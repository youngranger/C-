using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.Util;
using NPOI.XSSF;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace YrUti.Office
{
     static class Tools
    {
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="_filePath">文件全路径</param>
        /// <returns>文件扩展名，不含"."</returns>
        public static String GetExName(String _filePath)
        {
            if (!String.IsNullOrEmpty(_filePath))
            {
                if (_filePath.Contains("."))
                {
                    return _filePath.Substring(_filePath.LastIndexOf(".") + 1);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
    public class Excel
    {

        public String filePath { get; }
        /// <summary>
        /// excel的版本，默认是03
        /// </summary>
        private Boolean is07 = false;//默认为03版
        /// <summary>
        /// 传入文件名
        /// </summary>
        /// <param name="filePath">文件的完整路径，包含完整文件名和扩展名</param>
        public Excel(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                _IWorkbook = CreateWorkbook(fileStream, Tools.GetExName(filePath));
                this.filePath = filePath;
            }
        }

        /// <summary>
        /// 工作薄
        /// </summary>
        private IWorkbook _IWorkbook;

        /// <summary>
        /// excel文件中含有几个sheet
        /// </summary>
        public int NumberOfSheets
        {
            get { if (_IWorkbook != null) { return _IWorkbook.NumberOfSheets; } else { return 0; } }
        }

        /// <summary>
        /// 创建工作簿对象
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private IWorkbook CreateWorkbook(Stream stream, String exName)
        {
            if (exName.Equals("xls"))
            {
                return new HSSFWorkbook(stream); //03
            }
            else if (exName.Equals("xlsx"))
            {
                this.is07 = true;
                return new XSSFWorkbook(stream); //07

            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 把Sheet中的数据转换为DataTable,由于各行的列并不是固定的，所以跟DataTable并不一致，会出各种问题
        /// 只能在很窄范围内使用，比如行列格式固定的excel表
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="hasTitleRow"></param>
        /// <returns></returns>
        private DataTable ExportToDataTable(ISheet sheet, Boolean hasTitleRow)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < sheet.GetRow(0).LastCellNum; i++)
            {
                dt.Columns.Add("column" + i.ToString(), System.Type.GetType("System.String"));
            }
            int offSet = 0;

            if (hasTitleRow)
            {
                //如果有标题行，则第一行是字段
                IRow headRow = sheet.GetRow(0);

                //设置datatable字段
                for (int i = headRow.FirstCellNum, len = headRow.LastCellNum; i < len; i++)
                {
                    dt.Columns.Add(headRow.Cells[i].StringCellValue);
                }

                offSet = 1;
            }


            //遍历数据行
            for (int i = (sheet.FirstRowNum + offSet), len = sheet.LastRowNum + offSet; i < len; i++)
            {
                IRow tempRow = sheet.GetRow(i);

                DataRow dataRow = dt.NewRow();

                //遍历一行的每一个单元格
                for (int r = 0, j = tempRow.FirstCellNum, len2 = tempRow.LastCellNum; j < len2; j++, r++)
                {

                    ICell cell = tempRow.GetCell(j);

                    if (cell != null)
                    {
                        dataRow[r] = this.GetCellValue(cell);
                        //switch (cell.CellType)
                        //{
                        //    case CellType.String:
                        //        dataRow[r] = cell.StringCellValue;
                        //        break;
                        //    case CellType.Numeric:
                        //        dataRow[r] = cell.NumericCellValue;
                        //        break;
                        //    case CellType.Boolean:
                        //        dataRow[r] = cell.BooleanCellValue;
                        //        break;
                        //    case CellType.Blank: //空白
                        //        dataRow[r] = "";
                        //        break;
                        //    default:
                        //        dataRow[r] = cell.StringCellValue;
                        //        break;
                        //}
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// Sheet中的数据转换为List集合
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private IList<T> ExportToList<T>(ISheet sheet, string[] fields) where T : class, new()
        {
            IList<T> list = new List<T>();

            //遍历每一行数据
            for (int i = sheet.FirstRowNum + 1, len = sheet.LastRowNum + 1; i < len; i++)
            {
                T t = new T();
                IRow row = sheet.GetRow(i);

                for (int j = 0, len2 = fields.Length; j < len2; j++)
                {
                    ICell cell = row.GetCell(j);
                    object cellValue = this.GetCellValue(cell);

                    //switch (cell.CellType)
                    //{
                    //    case CellType.String: //文本
                    //        cellValue = cell.StringCellValue;
                    //        break;
                    //    case CellType.Numeric: //数值
                    //        cellValue = Convert.ToInt32(cell.NumericCellValue);//Double转换为int
                    //        break;
                    //    case CellType.Boolean: //bool
                    //        cellValue = cell.BooleanCellValue;
                    //        break;
                    //    case CellType.Blank: //空白
                    //        cellValue = "";
                    //        break;
                    //    default: cellValue = cell.StringCellValue;
                    //        break;
                    //}

                    typeof(T).GetProperty(fields[j]).SetValue(t, cellValue, null);
                }
                list.Add(t);
            }

            return list;
        }

        /// <summary>
        /// 获取单元格的值
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>值</returns>
        private object GetCellValue(ICell cell)
        {
            object cellValue = null;

            switch (cell.CellType)
            {

                case CellType.String: //文本
                    cellValue = cell.StringCellValue;
                    break;
                case CellType.Numeric: //数值,可能是日期，或者数字
                    if (DateUtil.IsCellDateFormatted(cell))//日期类型,有时候无法判断
                    {
                        cellValue = cell.DateCellValue;
                    }
                    else//其他数字类型
                    {
                        cellValue = cell.NumericCellValue;
                    }
                    //cellValue = Convert.ToInt32(cell.NumericCellValue).ToString();//Double转换为int
                    break;
                case CellType.Boolean: //bool
                    cellValue = cell.BooleanCellValue;
                    break;
                case CellType.Blank: //空白
                    cellValue = "";
                    break;
                case CellType.Unknown: //空白
                    cellValue = "内容未知";
                    break;
                case CellType.Formula: //公式，可能是日期，或者其他数字

                    IFormulaEvaluator eva;
                    if (is07)
                    {
                        eva = new XSSFFormulaEvaluator(_IWorkbook);
                    }
                    else
                    {
                        eva = new HSSFFormulaEvaluator(_IWorkbook);
                    }
                    cellValue = eva.Evaluate(cell).StringValue;
                    if (eva == null)
                    {
                        return cellValue.ToString();
                    }
                    break;
                case CellType.Error: //空白
                    cellValue = "单元格格式错误";
                    break;
                default:
                    cellValue = "单元格类型不明，无法读取";
                    break;
            }
            return cellValue;
        }
        /// <summary>
        /// 获取Sheet的某个单元格的值转换后的字符串,如果行列索引越界，返回为空
        /// </summary>
        /// <param name="_sheetIndex">sheet的索引，从0开始</param>
        /// <param name="rowIndex">单元格行的索引，从0开始</param>
        /// <param name="columnIndex">单元格列的索引，从0开始</param>
        /// <returns></returns>
        public string GetCellValue(int _sheetIndex, int rowIndex, int columnIndex)
        {
            ISheet sheet = _IWorkbook.GetSheetAt(_sheetIndex);
            if (rowIndex >= sheet.LastRowNum)
            {
                return null;
            }
            IRow row = sheet.GetRow(rowIndex);

            if (columnIndex >= row.LastCellNum)
            {
                return null;
            }

            ICell cell = row.GetCell(columnIndex);
            return this.GetCellValue(cell).ToString();

            //switch (cell.CellType)
            //{
            //    case CellType.String: //文本
            //        cellValue = cell.StringCellValue;
            //        break;
            //    case CellType.Numeric: //数值,可能是日期，或者数字
            //        if (HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
            //        {
            //            cellValue = cell.DateCellValue.ToString();
            //        }
            //        else//其他数字类型
            //        {
            //            cellValue = cell.NumericCellValue.ToString();
            //        }
            //        //cellValue = Convert.ToInt32(cell.NumericCellValue).ToString();//Double转换为int
            //        break;
            //    case CellType.Boolean: //bool
            //        cellValue = cell.BooleanCellValue.ToString();
            //        break;
            //    case CellType.Blank: //空白
            //        cellValue = "";
            //        break;
            //    case CellType.Unknown: //空白
            //        cellValue = "内容未知";
            //        break;
            //    case CellType.Formula: //公式，可能是日期，或者其他数字
            //        HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(_IWorkbook);
            //        cellValue = eva.Evaluate(cell).StringValue;
            //        break;
            //    case CellType.Error: //空白
            //        cellValue = "";
            //        break;
            //    default:
            //        cellValue ="类型不明，无法读取";
            //        break;
            //}
            //return cellValue;
        }

        public int GetSheetFirstRowNumber(int sheetIndex)
        {
            ISheet sheet = _IWorkbook.GetSheetAt(sheetIndex);
            return sheet.FirstRowNum;
        }

        public int GetSheetLastRowNumber(int sheetIndex)
        {
            ISheet sheet = _IWorkbook.GetSheetAt(sheetIndex);
            return sheet.LastRowNum;
        }

        /// <summary>
        /// 获取Excel文件中某个sheet中的一行的所有数据
        /// </summary>
        /// <param name="sheetIndex">sheet的索引，从0开始</param>
        /// <param name="rowIndex">行的索引，从0开始</param>
        /// <returns></returns>
        public string[] GetCells(int sheetIndex, int rowIndex)
        {
            List<string> list = new List<string>();

            ISheet sheet = _IWorkbook.GetSheetAt(sheetIndex);

            IRow row = sheet.GetRow(rowIndex);

            for (int i = 0, len = row.LastCellNum; i < len; i++)
            {
                list.Add(row.GetCell(i).StringCellValue);//这里没有考虑数据格式转换，会出现bug
            }
            return list.ToArray();
        }


        /// <summary>
        /// 将Excel文件中的一个Sheet中的数据，转换为DataTable
        /// </summary>
        /// <param name="sheetIndex">Sheet的索引，从0开始</param>
        /// <param name="hasTitleRow">Sheet第一行是否是标题行</param>
        /// <returns></returns>
        public DataTable ExportSheetToDataTable(int sheetIndex, Boolean hasTitleRow)
        {
            return ExportToDataTable(_IWorkbook.GetSheetAt(sheetIndex), hasTitleRow);
        }


        /// <summary>
        /// Excel中默认第一张Sheet导出到集合
        /// </summary>
        /// <param name="fields">Excel各个列，依次要转换成为的对象字段名称</param>
        /// <returns></returns>
        public IList<T> ExcelToList<T>(string[] fields) where T : class, new()
        {
            return ExportToList<T>(_IWorkbook.GetSheetAt(0), fields);
        }

        /// <summary>
        /// Excel中指定的Sheet导出到集合
        /// </summary>
        /// <param name="sheetIndex">第几张Sheet,从1开始</param>
        /// <param name="fields">Excel各个列，依次要转换成为的对象字段名称</param>
        /// <returns></returns>
        public IList<T> ExcelToList<T>(int sheetIndex, string[] fields) where T : class, new()
        {
            return ExportToList<T>(_IWorkbook.GetSheetAt(sheetIndex - 1), fields);
        }

        /// <summary>
        /// 判断一个cell横跨了几行几列，即存在单元格合并现象
        /// </summary>
        /// <param name="sheetIndex">sheet的索引值,从0开始</param>
        /// <param name="rowIndex">cell的行索引，从0开始</param>
        /// <param name="colIndex">cell的列索引，从0开始</param>
        /// <param name="rowSpan">cell行跨度，如果没有跨行，则值为1</param>
        /// <param name="colSpan">cell列跨度，如果没有跨列，则值为1</param>
        /// <returns></returns>
        private bool IsMergeCell(int sheetIndex, int rowIndex, int colIndex, out int rowSpan, out int colSpan)
        {
            ISheet sheet = _IWorkbook.GetSheetAt(sheetIndex);
            bool result = false;
            rowSpan = 0;
            colSpan = 0;
            if ((rowIndex < 0) || (colIndex < 0)) return result;
            int regionsCount = sheet.NumMergedRegions;
            rowSpan = 1;
            colSpan = 1;
            for (int i = 0; i < regionsCount; i++)
            {
                CellRangeAddress range = sheet.GetMergedRegion(i);
                sheet.IsMergedRegion(range);
                if (range.FirstRow == rowIndex && range.FirstColumn == colIndex)
                {
                    rowSpan = range.LastRow - range.FirstRow + 1;
                    colSpan = range.LastColumn - range.FirstColumn + 1;
                    break;
                }
            }
            try
            {
                result = sheet.GetRow(rowIndex).GetCell(colIndex).IsMergedCell;
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 判断一个cell是否跨行跨列
        /// </summary>
        /// <param name="sheetIndex">sheet的索引，从0开始</param>
        /// <param name="rowIndex">cell的行索引，从0开始</param>
        /// <param name="colIndex">cell的列索引，从0开始</param>
        /// <returns>是否跨行跨列</returns>
        public Boolean IsMergedCell(int sheetIndex, int rowIndex, int colIndex)
        {
            ISheet sheet = _IWorkbook.GetSheetAt(sheetIndex);
            bool result = false;
            if ((rowIndex < 0) || (colIndex < 0)) return result;
            try
            {
                result = sheet.GetRow(rowIndex).GetCell(colIndex).IsMergedCell;
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 获取一个cell的跨行，跨列数
        /// </summary>
        /// <param name="sheetIndex">sheet的索引，从0开始</param>
        /// <param name="rowIndex">行索引，从0开始</param>
        /// <param name="colIndex">列索引，从0开始</param>
        /// <returns>整形数组，第一个为跨行的数目，没有跨行，为1，第二个为跨列的数目，没有跨列，为1,行列号越界，则返回0</returns>
        public int[] GetCellSpan(int sheetIndex, int rowIndex, int colIndex)
        {
            ISheet sheet = _IWorkbook.GetSheetAt(sheetIndex);

            int rowSpan = 0;
            int colSpan = 0;
            if ((rowIndex < 0) || (colIndex < 0) || rowIndex >= sheet.LastRowNum || colIndex >= sheet.GetRow(rowIndex).LastCellNum)
            {
                return new int[] { rowSpan, colSpan };
            }


            int regionsCount = sheet.NumMergedRegions;
            rowSpan = 1;
            colSpan = 1;
            for (int i = 0; i < regionsCount; i++)
            {
                CellRangeAddress range = sheet.GetMergedRegion(i);
                sheet.IsMergedRegion(range);
                if (range.FirstRow == rowIndex && range.FirstColumn == colIndex)
                {
                    rowSpan = range.LastRow - range.FirstRow + 1;
                    colSpan = range.LastColumn - range.FirstColumn + 1;
                    break;
                }
            }

            return new int[] { rowSpan, colSpan };
        }
    }

}
