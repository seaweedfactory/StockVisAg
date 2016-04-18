using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg.Data.Symbol
{
    /// <summary>
    /// Stock ticker symbol.
    /// </summary>
    public class TickerSymbol : ITickerSymbol
    {
        private readonly String _Symbol = null;
        
        /// <summary>
        /// Stock ticker symbol code.
        /// </summary>
        public String Symbol
        {
            get
            {
                return _Symbol;
            }
        }

        /// <summary>
        /// Stock ticker symbol.
        /// </summary>
        /// <param name="symbol">Symbol code.</param>
        public TickerSymbol(String symbol)
        {
            _Symbol = symbol;
        }

        /// <summary>
        /// String is symbol code.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Symbol;
        }
    }
}
