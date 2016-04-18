using StockVisAg.Data.Symbol;

namespace StockVisAg.Data.StockFile
{
    /// <summary>
    /// Defines a stock file. 
    /// </summary>
    public interface IStockFile
    {
        /// <summary>
        /// Raw file contents.
        /// </summary>
        byte[] Contents { get; }

        /// <summary>
        /// Ticker symbol used to create this stock file.
        /// </summary>
        ITickerSymbol Symbol { get; }
    }
}