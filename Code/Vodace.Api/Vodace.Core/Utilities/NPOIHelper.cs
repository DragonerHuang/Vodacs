using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Vodace.Core.Utilities.Log4Net;

namespace Vodace.Core.Utilities
{
    public class NPOIHelper
    {
        #region  -- 读取报价单Excel数据 --

        /// <summary>
        /// 读取Excel文件中所有sheet的指定列数据
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="columns">需要读取的列名</param>
        /// <returns>包含所有sheet数据的列表</returns>
        public static List<SheetDataResult> ReadAllSheetsWithSpecifiedColumns(string filePath, List<string> columns, Dictionary<string, List<string>> dicColumns)
        {
            var results = new List<SheetDataResult>();

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 根据文件扩展名创建对应的Workbook
                    IWorkbook workbook = null;
                    string fileExt = Path.GetExtension(filePath).ToLower();

                    if (fileExt == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }
                    else if (fileExt == ".xls")
                    {
                        workbook = new HSSFWorkbook(fileStream);
                    }
                    else
                    {
                        throw new Exception("Unsupported file format, only supports .xlsx and .xls files.");
                    }

                    // 遍历所有sheet
                    for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
                    {
                        //判断sheet是否隐藏
                        if (workbook.IsSheetHidden(sheetIndex)) continue;

                        var sheet = workbook.GetSheetAt(sheetIndex);
                        if (sheet == null) continue;
                      
                        var sheetResult = ReadSheetWithSpecifiedColumns(sheet, columns, dicColumns);
                        if (sheetResult != null && sheetResult.Data.Rows.Count > 0)
                        {
                            results.Add(sheetResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("NPOIHelper.ReadAllSheetsWithSpecifiedColumns", ex);
                return null;
            }

            return results;
        }

        /// <summary>
        /// 读取单个sheet的指定列数据
        /// </summary>
        /// <param name="sheet">Excel sheet</param>
        /// <param name="columns">需要读取的列名</param>
        /// <returns>包含sheet数据的结果对象</returns>
        public static SheetDataResult ReadSheetWithSpecifiedColumns(ISheet sheet, List<string> columns, Dictionary<string, List<string>> dicColumns)
        {
            var result = new SheetDataResult { SheetName = sheet.SheetName };
            var dataTable = new DataTable(sheet.SheetName);

            try
            {
                // 查找表头行位置
                int headerRowIndex = FindHeaderRowIndex(sheet, columns, dicColumns);
                if (headerRowIndex < 0)
                {
                    return null; // 未找到包含所有必需列的表头行
                }

                var headerRow = sheet.GetRow(headerRowIndex);
                if (headerRow == null) return null;

                // 创建列索引映射和DataTable列
                Dictionary<string, int> columnIndexMap = new Dictionary<string, int>();
                foreach (var columnName in columns)
                {
                    dataTable.Columns.Add(columnName, typeof(string));
                }

                // 查找每个必需列的索引位置
                for (int cellIndex = 0; cellIndex < headerRow.LastCellNum; cellIndex++)
                {
                    var cell = headerRow.GetCell(cellIndex);
                    if (cell != null)
                    {
                        string cellValue = GetCellValue(cell).Trim();
                        if (columns.Contains(cellValue))
                        {
                            columnIndexMap[cellValue] = cellIndex;
                        }
                        else
                        {
                            foreach (var key in dicColumns.Keys)
                            {
                                if (dicColumns[key].Contains(cellValue))
                                {
                                    columnIndexMap[key] = cellIndex;
                                    break;
                                }
                            }
                        }
                    }
                }

                // 验证是否找到所有必需列
                if (columnIndexMap.Count < columns.Count)
                {
                    return null; // 未找到所有必需列
                }

                // 读取数据行
                int startRowIndex = headerRowIndex + 1;
                for (int rowIndex = startRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    var dataRow = sheet.GetRow(rowIndex);
                    if (dataRow == null) continue;

                    // 检查这一行是否有数据
                    bool hasData = false;
                    foreach (var columnName in columns)
                    {
                        int cellIndex = columnIndexMap[columnName];                        
                        var cell = dataRow.GetCell(cellIndex);
                        if (cell != null && !string.IsNullOrWhiteSpace(GetCellValue(cell)))
                        {
                            hasData = true;
                            break;
                        }
                    }

                    if (!hasData) continue; // 跳过空行

                    // 创建DataTable行并填充数据
                    DataRow newRow = dataTable.NewRow();
                    foreach (var columnName in columns)
                    {
                        int cellIndex = columnIndexMap[columnName];
                        var cell = dataRow.GetCell(cellIndex);
                        newRow[columnName] = cell != null ? GetCellValue(cell) : string.Empty;
                    }
                    dataTable.Rows.Add(newRow);
                }

                result.Data = dataTable;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("NPOIHelper.ReadSheetWithSpecifiedColumns", ex);
                return null;
            }

            return result;
        }

        /// <summary>
        /// 查找包含指定列的表头行
        /// </summary>
        /// <param name="sheet">Excel sheet</param>
        /// <param name="columns">需要查找的列名</param>
        /// <returns>表头行索引，如果未找到返回-1</returns>
        public static int FindHeaderRowIndex(ISheet sheet, List<string> columns, Dictionary<string, List<string>> dicColumns)
        {
            // 最多检查前20行，防止表格有大量空行
            int maxRowsToCheck = Math.Min(20, sheet.LastRowNum + 1);

            for (int rowIndex = 0; rowIndex < maxRowsToCheck; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null) continue;

                int foundColumnsCount = 0;
                for (int cellIndex = 0; cellIndex < row.LastCellNum; cellIndex++)
                {
                    var cell = row.GetCell(cellIndex);
                    if (cell != null)
                    {
                        string cellValue = GetCellValue(cell).Trim();
                        if (columns.Contains(cellValue))
                        {
                            foundColumnsCount++;
                        }
                        else
                        {
                            foreach (var key in dicColumns.Keys)
                            {
                                if (dicColumns[key].Contains(cellValue))
                                {
                                    foundColumnsCount++;
                                    break;
                                }
                            }
                        }
                    }
                }

                // 如果找到所有必需列，返回当前行索引
                if (foundColumnsCount == columns.Count)
                {
                    return rowIndex;
                }
            }

            return -1; // 未找到包含所有必需列的行
        }

        /// <summary>
        /// 获取Excel单元格的值
        /// </summary>
        /// <param name="cell">Excel单元格</param>
        /// <returns>单元格的字符串值</returns>
        public static string GetCellValue_bak(ICell cell)
        {
            if (cell == null) return string.Empty;

            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue.ToString();
                    }
                    else
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    try
                    {
                        switch (cell.CachedFormulaResultType)
                        {
                            case CellType.String:
                                return cell.StringCellValue;
                            case CellType.Numeric:
                                return cell.NumericCellValue.ToString();
                            case CellType.Boolean:
                                return cell.BooleanCellValue.ToString();
                            default:
                                return cell.CellFormula;
                        }
                    }
                    catch
                    {
                        return cell.CellFormula;
                    }
                default:
                    return string.Empty;
            }
        }

        public static string GetCellValue(ICell cell)
        {
            if (cell == null) return string.Empty;

            switch (cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue.ToString();
                    }
                    else
                    {
                        // 使用DataFormatter来保留原始格式
                        var dataFormatter = new DataFormatter();
                        return dataFormatter.FormatCellValue(cell);
                    }
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Formula:
                    try
                    {
                        switch (cell.CachedFormulaResultType)
                        {
                            case CellType.String:
                                return cell.StringCellValue;
                            case CellType.Numeric:
                                // 使用DataFormatter处理公式结果
                                var dataFormatter = new DataFormatter();
                                return dataFormatter.FormatCellValue(cell);
                            case CellType.Boolean:
                                return cell.BooleanCellValue.ToString();
                            default:
                                return cell.CellFormula;
                        }
                    }
                    catch
                    {
                        return cell.CellFormula;
                    }
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region  -- 不同work的工种任务类型读取 --

        #region  -- 读取Site Survey，Sub-C.Work数据 --

        /// <summary>
        /// 专门读取Site Survey sheet的数据，以左边列为key，右边列为value的格式组织
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetIndex">Sheet索引</param>
        /// <param name="rowStart">获取第几行作为标题行开始，默认0</param>
        /// <returns>包含Site Survey数据的字典，key为左边列的值，value为右边列对应的值</returns>
        public static Dictionary<int, Dictionary<string, string>> ReadSiteSurveyAsKeyValuePairs(string filePath, int sheetIndex, int rowStart = 0)
        {
            var result = new Dictionary<int, Dictionary<string, string>>();

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 根据文件扩展名创建对应的Workbook
                    IWorkbook workbook = null;
                    string fileExt = Path.GetExtension(filePath).ToLower();

                    #region  -- 非空判断 --

                    if (fileExt == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }
                    else if (fileExt == ".xls")
                    {
                        workbook = new HSSFWorkbook(fileStream);
                    }
                    else
                    {
                        throw new Exception("Unsupported file format, only supports .xlsx and .xls files.");
                    }

                    var sheet = workbook.GetSheetAt(sheetIndex);
                    if (sheet == null)
                    {
                        Log4NetHelper.Warn("NPOIHelper.ReadSiteSurveyAsKeyValuePairs: Third sheet not found.");
                        return result;
                    }

                    // 检查sheet是否隐藏
                    if (workbook.IsSheetHidden(sheetIndex))
                    {
                        Log4NetHelper.Warn("NPOIHelper.ReadSiteSurveyAsKeyValuePairs: Third sheet is hidden.");
                        return result;
                    }

                    // 检查sheet名称是否为"Site Survey"
                    //if (!string.Equals(sheet.SheetName, "Site Survey", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    Log4NetHelper.Info($"NPOIHelper.ReadSiteSurveyAsKeyValuePairs: Third sheet name is '{sheet.SheetName}', not 'Site Survey'.");
                    //}

                    #endregion

                    int line_number = 1;

                    //表头索引集合
                    var cellTitle = new Dictionary<int, int>();

                    // 获取第rowStart行作为列标题行
                    var headerRow = sheet.GetRow(rowStart);
                    for (int cellIndex = 0; cellIndex < headerRow.LastCellNum; cellIndex++)
                    {
                        var keyCell = headerRow.GetCell(cellIndex);
                        var valueCell = headerRow.GetCell(cellIndex + 1);

                        if ((keyCell != null && !string.IsNullOrEmpty(keyCell.ToString())) && (valueCell != null && !string.IsNullOrEmpty(valueCell.ToString())))
                        {
                            cellTitle[cellIndex] = cellIndex + 1;
                        }
                    }

                    foreach (var item in cellTitle)
                    {
                        // 遍历所有行
                        for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                        {
                            var dataRow = sheet.GetRow(rowIndex);
                            if (dataRow == null || dataRow.LastCellNum < 2)
                                continue;

                            // 获取左边列(第一列)作为key
                            var rowKeyCell = dataRow.GetCell(item.Key);
                            string key = GetCellValue(rowKeyCell).Trim();

                            // 获取右边列(第二列)作为value
                            var rowValueCell = dataRow.GetCell(item.Value);
                            string value = rowValueCell != null ? GetCellValue(rowValueCell).Trim() : string.Empty;

                            if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(value))
                                continue;   //key-value为空时才需要跳过

                            value = value.Replace("□", "").Trim();

                            // 存储key-value对
                            result[line_number] = new Dictionary<string, string> { { key, value } };

                            line_number++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("NPOIHelper.ReadSiteSurveyAsKeyValuePairs", ex);
                return new Dictionary<int, Dictionary<string, string>>();
            }

            return result;
        }

        #endregion

        #region  -- 读取Site Work数据 --

        /// <summary>
        /// 专门读取Excel文件中第二个sheet "Site Work" 的所有内容
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetIndex">Sheet索引</param>
        /// <param name="rowStart">获取第几行作为标题行开始，默认0</param>
        /// <returns>包含Site Work sheet所有数据的结果对象</returns>
        public static SheetDataResult ReadSiteWorkSheet(string filePath, int sheetIndex, int rowStart, int append, List<string> columns, Dictionary<string, List<string>> dicColumns)
        {
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 根据文件扩展名创建对应的Workbook
                    IWorkbook workbook = null;
                    string fileExt = Path.GetExtension(filePath).ToLower();

                    if (fileExt == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }
                    else if (fileExt == ".xls")
                    {
                        workbook = new HSSFWorkbook(fileStream);
                    }
                    else
                    {
                        throw new Exception("Unsupported file format, only supports .xlsx and .xls files.");
                    }

                    // 获取第二个sheet (索引为1)
                    var sheet = workbook.GetSheetAt(sheetIndex);
                    if (sheet == null)
                    {
                        Log4NetHelper.Warn("NPOIHelper.ReadSiteWorkSheet: Second sheet not found.");
                        return null;
                    }

                    // 检查sheet名称是否为"Site Work"
                    if (!string.Equals(sheet.SheetName, "Site Work", StringComparison.OrdinalIgnoreCase))
                    {
                        Log4NetHelper.Info($"NPOIHelper.ReadSiteWorkSheet: Second sheet name is '{sheet.SheetName}', not 'Site Work'.");
                    }

                    // 检查sheet是否隐藏
                    if (workbook.IsSheetHidden(1))
                    {
                        Log4NetHelper.Warn("NPOIHelper.ReadSiteWorkSheet: Second sheet is hidden.");
                        return null;
                    }

                    // 使用现有的ReadSheetAllColumns方法读取所有内容
                    // 自动查找表头行
                    var result = ReadSheetAllColumns(sheet, rowStart, append, columns, dicColumns);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("NPOIHelper.ReadSiteWorkSheet", ex);
                return null;
            }
        }

        /// <summary>
        /// 读取Sheet的所有列数据
        /// </summary>
        /// <param name="sheet">Excel sheet</param>
        /// <param name="headerRowIndex">表头行索引，-1表示自动查找</param>
        /// <param name="append">是否追加列的数据读取？0-不追加；正整数-往后追加多少index；负整数-往前追加多少个index（暂不开放）</param>
        /// <returns>包含sheet数据的结果对象</returns>
        public static SheetDataResult ReadSheetAllColumns(ISheet sheet, int headerRowIndex, int append, List<string> columns, Dictionary<string, List<string>> dicColumns)
        {
            var result = new SheetDataResult { SheetName = sheet.SheetName };
            var dataTable = new DataTable(sheet.SheetName);

            try
            {
                #region  -- 条件判断 --

                // 如果没有指定表头行索引，自动查找
                if (headerRowIndex < 0)
                {
                    // 简单的查找，假设第一行有数据的行就是表头
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row != null && row.LastCellNum > 0)
                        {
                            headerRowIndex = i;
                            break;
                        }
                    }

                    if (headerRowIndex < 0)
                    {
                        return null; // 未找到表头行
                    }
                }

                var headerRow = sheet.GetRow(headerRowIndex);
                if (headerRow == null)
                    return null;

                // 创建列索引映射和DataTable列
                Dictionary<string, int> columnIndexMap = new Dictionary<string, int>();
                foreach (var columnName in columns)
                {
                    dataTable.Columns.Add(columnName, typeof(string));
                }

                int lastIndex = 0;
                // 查找每个必需列的索引位置
                for (int cellIndex = 0; cellIndex < headerRow.LastCellNum; cellIndex++)
                {
                    var cell = headerRow.GetCell(cellIndex);
                    if (cell != null)
                    {
                        string cellValue = GetCellValue(cell).Trim();
                        if (columns.Contains(cellValue))
                        {
                            columnIndexMap[cellValue] = cellIndex;
                        }
                        else
                        {
                            foreach (var key in dicColumns.Keys)
                            {
                                if (dicColumns[key].Contains(cellValue))
                                {
                                    columnIndexMap[key] = cellIndex;
                                    break;
                                }
                            }
                        }
                        lastIndex = cellIndex;
                    }
                }

                if (append > 0)
                {
                    string colKeyName = string.Empty;
                    //往后追加
                    for (var i = 0; i < append; i++)
                    {
                        colKeyName = "Columns_" + i;
                        dataTable.Columns.Add(colKeyName, typeof(string));
                        columnIndexMap[colKeyName] = lastIndex;
                        lastIndex++;
                    }
                }
                else if (append < 0)
                {
                    //往前追加
                    //暂不开放
                }

                #endregion

                // 读取数据行
                int startRowIndex = headerRowIndex + 1;
                for (int rowIndex = startRowIndex; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    var dataRow = sheet.GetRow(rowIndex);
                    if (dataRow == null)
                        continue;

                    // 检查这一行是否有数据
                    bool hasData = false;
                    for (int cellIndex = 0; cellIndex < dataTable.Columns.Count; cellIndex++)
                    {
                        var cell = dataRow.GetCell(cellIndex);
                        if (cell != null && !string.IsNullOrWhiteSpace(GetCellValue(cell)))
                        {
                            hasData = true;
                            break;
                        }
                    }

                    if (!hasData)
                        continue; // 跳过空行

                    // 创建DataTable行并填充数据
                    DataRow newRow = dataTable.NewRow();
                    foreach (var item in columnIndexMap)
                    {
                        var cell = dataRow.GetCell(item.Value);
                        //newRow[item.Key] = cell != null ? GetCellValue(cell) : string.Empty;
                        newRow[item.Key] = cell != null ? GetCellValue(cell) : string.Empty;
                    }

                    dataTable.Rows.Add(newRow);
                }

                result.Data = dataTable;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("NPOIHelper.ReadSheetAllColumns", ex);
                return null;
            }

            return result;
        }

        #endregion

        #region  -- 读取Pre Work数据 --

        /// <summary>
        /// 专门读取Pre Work sheet的数据，以左边列为key，右边列为value的格式组织
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetIndex">Sheet索引</param>
        /// <param name="rowStart">获取第几行作为标题行开始，默认0</param>
        /// <returns>包含Site Survey数据的字典，key为左边列的值，value为右边列对应的值</returns>
        public static (List<Dictionary<string, string>>, List<Dictionary<string, string>>) ReadPreWorkAsKeyValuePairs(string filePath, int sheetIndex, int rowStart = 0, int colStart = 0)
        {
            var resultParent = new List<Dictionary<string, string>>();
            var resultChild = new List<Dictionary<string, string>>();

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // 根据文件扩展名创建对应的Workbook
                    IWorkbook workbook = null;
                    string fileExt = Path.GetExtension(filePath).ToLower();

                    #region  -- 非空判断 --

                    if (fileExt == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }
                    else if (fileExt == ".xls")
                    {
                        workbook = new HSSFWorkbook(fileStream);
                    }
                    else
                    {
                        throw new Exception("Unsupported file format, only supports .xlsx and .xls files.");
                    }

                    var sheet = workbook.GetSheetAt(sheetIndex);
                    if (sheet == null)
                    {
                        Log4NetHelper.Warn("NPOIHelper.ReadSiteSurveyAsKeyValuePairs: Third sheet not found.");
                        return (null, null);
                    }

                    // 检查sheet是否隐藏
                    if (workbook.IsSheetHidden(sheetIndex))
                    {
                        Log4NetHelper.Warn("NPOIHelper.ReadSiteSurveyAsKeyValuePairs: Third sheet is hidden.");
                        return (null, null);
                    }

                    // 检查sheet名称是否为"Pre Work"
                    //if (!string.Equals(sheet.SheetName, "Pre Work", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    Log4NetHelper.Info($"NPOIHelper.ReadSiteSurveyAsKeyValuePairs: Third sheet name is '{sheet.SheetName}', not 'Pre Work'.");
                    //}

                    #endregion

                    int line_number = 1;

                    //表头索引集合
                    var cellTitleList = new List<Dictionary<int, int>>();

                    // 获取第rowStart行作为列标题行
                    var headerRow = sheet.GetRow(rowStart);
                    for (int cellIndex = colStart; cellIndex < headerRow.LastCellNum; cellIndex++)
                    {
                        var keyCell = headerRow.GetCell(cellIndex - 1);
                        var valueCell = headerRow.GetCell(cellIndex);

                        if ((valueCell != null && !string.IsNullOrEmpty(valueCell.ToString())))
                        {
                            cellTitleList.Add(new Dictionary<int, int> { { cellIndex - 1, cellIndex } });
                        }
                    }

                    for(var i = 0; i < cellTitleList.Count; i++)
                    {
                        foreach (var item in cellTitleList[i])
                        {
                            for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
                            {
                                var dataRow = sheet.GetRow(rowIndex);
                                if (dataRow == null || dataRow.LastCellNum < 2)
                                    continue;

                                // 获取左边列(第一列)作为key
                                var rowKeyCell = dataRow.GetCell(item.Key);
                                //if (rowKeyCell == null)
                                //    continue;

                                string key = GetCellValue(rowKeyCell).Trim();

                                // 获取右边列(第二列)作为value
                                var rowValueCell = dataRow.GetCell(item.Value);
                                string value = rowValueCell != null ? GetCellValue(rowValueCell).Trim() : string.Empty;

                                if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(value))
                                    continue;   //key-value为空时才需要跳过

                                value = value.Replace("□", "").Trim();

                                if (i == 0) //作为主级数据
                                {
                                    // 存储key-value对
                                    resultParent.Add(new Dictionary<string, string> { { key, value } });
                                }
                                else        //作为子级数据
                                {
                                    // 存储key-value对
                                    resultChild.Add(new Dictionary<string, string> { { key, value } });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("NPOIHelper.ReadSiteSurveyAsKeyValuePairs", ex);
                return (null, null);
            }

            return (resultParent, resultChild);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Sheet数据结果类
    /// </summary>
    public class SheetDataResult
    {
        public string SheetName { get; set; }
        public DataTable Data { get; set; }
    }

    /// <summary>
    /// Sheet读取类型枚举
    /// </summary>
    public enum SheetReadType
    {
        /// <summary>
        /// 读取指定列
        /// </summary>
        SpecifiedColumns,
        /// <summary>
        /// 读取所有列
        /// </summary>
        AllColumns,
        /// <summary>
        /// 读取自定义行范围
        /// </summary>
        CustomRange
    }

    /// <summary>
    /// Sheet读取配置类
    /// </summary>
    public class SheetReadConfiguration
    {
        /// <summary>
        /// 读取类型
        /// </summary>
        public SheetReadType ReadType { get; set; }

        /// <summary>
        /// 必需的列名列表（当ReadType为SpecifiedColumns时使用）
        /// </summary>
        public List<string> RequiredColumns { get; set; }

        /// <summary>
        /// 列别名映射（当ReadType为SpecifiedColumns时使用）
        /// </summary>
        public Dictionary<string, List<string>> AliasColumns { get; set; }

        /// <summary>
        /// 表头行索引（当ReadType为AllColumns或CustomRange时使用）
        /// </summary>
        public int HeaderRowIndex { get; set; }

        /// <summary>
        /// 数据开始行索引（当ReadType为CustomRange时使用）
        /// </summary>
        public int DataStartRow { get; set; }

        /// <summary>
        /// 数据结束行索引，-1表示读取到最后一行（当ReadType为CustomRange时使用）
        /// </summary>
        public int DataEndRow { get; set; }

        /// <summary>
        /// 是否包含隐藏的sheet
        /// </summary>
        public bool IncludeHiddenSheets { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SheetReadConfiguration()
        {
            RequiredColumns = new List<string>();
            AliasColumns = new Dictionary<string, List<string>>();
            HeaderRowIndex = 0;
            DataStartRow = 1;
            DataEndRow = -1;
            IncludeHiddenSheets = false;
        }
    }
}
