using StockVisAg.Data.Symbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg.Data.Company
{
    /// <summary>
    /// Represents aggregated stock data from a single company.
    /// </summary>
    public class CompanyStockGroup
    {
        private readonly String _CompanyName;
        private readonly String _Timezone;
        private readonly ITickerSymbol _Symbol;
        private readonly StockPointCollection _PointsCollection;
        
        /// <summary>
        /// Full name of company.
        /// </summary>
        public String CompanyName
        {
            get
            {
                return _CompanyName;
            }
        }

        /// <summary>
        /// Raw timezone token.
        /// </summary>
        public String Timezone
        {
            get
            {
                return _Timezone;
            }
        }

        /// <summary>
        /// Ticker symbol for company.
        /// </summary>
        public ITickerSymbol Symbol
        {
            get
            {
                return _Symbol;
            }
        }

        /// <summary>
        /// Stock data points.
        /// </summary>
        public StockPointCollection Data
        {
            get
            {
                return _PointsCollection;
            }
        }

        /// <summary>
        /// Represents aggregated stock data from a single company.
        /// </summary>
        /// <param name="companyName">Full name of company.</param>
        /// <param name="timezone">Raw Timezone token.</param>
        /// <param name="symbol">Ticker symbol for company.</param>
        /// <param name="pointsCollection">Optional. Collection of stock points for this company.</param>
        public CompanyStockGroup(
            String companyName,
            String timezone,
            ITickerSymbol symbol,
            StockPointCollection pointsCollection = null
            )
        {
            _CompanyName = companyName;
            _Timezone = timezone;
            _Symbol = symbol;
            _PointsCollection = (pointsCollection != null) ?
                pointsCollection :
                new StockPointCollection();
        }
    }
}
