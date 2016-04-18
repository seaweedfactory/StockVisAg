using StockVisAg.Data.Symbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg.Data.StockFile
{
    public class StockFileFactory
    {
        /// <summary>
        /// Get a stock file for the given symbol using the provided handler.
        /// </summary>
        /// <param name="symbol">Ticker symbol to retrieve.</param>
        /// <param name="formatHandler">Handles format specifics.</param>
        /// <returns>Stock file containing raw data.</returns>
        public IStockFile GetStockFile(ITickerSymbol symbol, IStockFileFormatHandler formatHandler)
        {
            //Check that symbol and format handler are both present.
            if (symbol == null || formatHandler == null)
            {
                return null;
            }

            //Get file data
            byte[] fileData = formatHandler.GetFileContents(symbol);

            return new StockFile(fileData, symbol);
        }

        /// <summary>
        /// Get a list of stock files form a list of ticker symbols.
        /// </summary>
        /// <param name="symbols">List of ticker symbols.</param>
        /// <param name="formatHandler">Handles format specifics.</param>
        /// <returns>List of raw stock files.</returns>
        public List<IStockFile> GetStockFiles(List<ITickerSymbol> symbols, IStockFileFormatHandler formatHandler)
        {
            List<IStockFile> stockFiles = new List<IStockFile>();
            foreach(ITickerSymbol symbol in symbols)
            {
                stockFiles.Add(GetStockFile(symbol, formatHandler));
            }
            return stockFiles;
        }
    }
}
