using StockVisAg.Data.Company;
using StockVisAg.Data.Symbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg.Data.StockFile
{
    /// <summary>
    /// Handles importing and parsing data from a source.
    /// </summary>
    public interface IStockFileFormatHandler
    {
        /// <summary>
        /// Given the ticker symbol, locate data and store in byte array.
        /// </summary>
        /// <param name="symbol">Find dat for this ticker symbol</param>
        /// <returns>Byte arrya of raw file data</returns>
        byte[] GetFileContents(ITickerSymbol symbol);

        /// <summary>
        /// Parse raw file data retrieved by GetFileContents.
        /// </summary>
        /// <param name="file">Stock file to parse</param>
        /// <returns>Company stock group produced from stock file data</returns>
        CompanyStockGroup ParseStockFile(IStockFile file);
    }
}
