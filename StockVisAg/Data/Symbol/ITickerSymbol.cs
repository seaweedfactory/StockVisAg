namespace StockVisAg.Data.Symbol
{
    /// <summary>
    /// Defines a stock ticker symbol.
    /// </summary>
    public interface ITickerSymbol
    {
        /// <summary>
        /// Stock ticker symbol code.
        /// </summary>
        string Symbol { get; }
    }
}