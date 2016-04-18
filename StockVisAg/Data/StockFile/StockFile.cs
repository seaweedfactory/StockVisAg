using StockVisAg.Data.Symbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg.Data.StockFile
{
    /// <summary>
    /// Holds raw stock ticker data.
    /// </summary>
    public class StockFile : IStockFile
    {
        private readonly byte[] _Contents;
        private readonly ITickerSymbol _Symbol;

        /// <summary>
        /// Raw file contents.
        /// </summary>
        public byte[] Contents
        {
            get
            {
                return _Contents;
            }
        }

        /// <summary>
        /// Ticker symbol used to create this stock file.
        /// </summary>
        public ITickerSymbol Symbol
        {
            get
            {
                return _Symbol;
            }
        }

        /// <summary>
        /// Holds raw stock ticker data.
        /// </summary>
        /// <param name="contents">Stock file data.</param>
        /// <param name="symbol">Ticker used to create stock file.</param>
        public StockFile(byte[] contents, ITickerSymbol symbol)
        {
            _Contents = contents;
            _Symbol = symbol;
        }
    }
}
