using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg.Data.Symbol
{
    /// <summary>
    /// Constructs ticker symbols.
    /// </summary>
    public class TickerSymbolFactory
    {
        /// <summary>
        /// Construct a stock ticker symbol.
        /// </summary>
        /// <param name="symbolCode">Stock Ticker symbol code.</param>
        /// <returns>New stock ticker symbol.</returns>
        public ITickerSymbol GetSymbol(String symbolCode)
        {
            return new TickerSymbol(symbolCode);
        }

        /// <summary>
        /// Returns a list of default stock ticker symbols.
        /// </summary>
        /// <returns></returns>
        public List<ITickerSymbol> GetDefaultSymbols()
        {
            List<ITickerSymbol> symbolList = new List<ITickerSymbol>();

            //Read symbols from defaults file. 
            foreach (String symbol in Properties.Resources.DefaultStockTickerSymbols.Split(
                new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                symbolList.Add(GetSymbol(symbol));
            }

            return symbolList;
        }
    }
}
