using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockVisAg.Data.Symbol;
using System.Net;
using StockVisAg.Data.Company;
using System.IO;
using System.Globalization;

namespace StockVisAg.Data.StockFile.FormatHandler.YahooFinance
{
    /// <summary>
    /// Handles data from Yahoo Finance.
    /// </summary>
    public class YahooFinanceFormatHandler : IStockFileFormatHandler, IDisposable
    {
        public readonly DateTime _Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private WebClient _Client = new WebClient();

        /// <summary>
        /// Download the file through Yahoo Finance's API.
        /// </summary>
        /// <param name="symbol">Symbol to download.</param>
        /// <returns>Byte array of file contents.</returns>
        public byte[] GetFileContents(ITickerSymbol symbol)
        {
            byte[] buffer = null;
            try
            {
                String startURL = @"http://chartapi.finance.yahoo.com/instrument/1.0/";
                String endURL = @"/chartdata;type=quote;range=2d/csv/";
                String fileURI = String.Join("", startURL, symbol.Symbol, endURL);
                buffer = _Client.DownloadData(fileURI);
            }
            catch(Exception)
            {
                return null;
            }

            return buffer;
        }

        /// <summary>
        /// Parse a stock file produced by the import routine.
        /// </summary>
        /// <param name="file">File to parse, should have been produced by import routine.</param>
        /// <returns>Company stock group.</returns>
        public CompanyStockGroup ParseStockFile(IStockFile file)
        {
            //Check for contents to parse
            if(file?.Contents == null)
            {
                return null;
            }

            String delimiter = ":";

            //Parse file
            using (var stream = new MemoryStream(file.Contents))
            using (var streamReader = new StreamReader(stream))
            {
                String companyName = null;
                String timeZone = null;
                ITickerSymbol symbol = file.Symbol;
                StockPointCollection points = new StockPointCollection();

                TickerSymbolFactory symbolFactory = new TickerSymbolFactory();

                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Contains(delimiter))
                    {
                        //Descriptor
                        String[] tokens = line.Split(new char[] { delimiter[0] });
                        if (tokens.Length > 0)
                        {
                            if (String.Equals(tokens[0],"ticker", StringComparison.InvariantCultureIgnoreCase))
                            {
                                //Ticker symbol
                                //Read ticker from content data if missing in stock file
                                if (symbol == null)
                                {
                                    symbol = symbolFactory.GetSymbol(tokens[1]);
                                }
                            }
                            else if (String.Equals(tokens[0],"Company-Name", StringComparison.InvariantCultureIgnoreCase))
                            {
                                //Company name
                                companyName = tokens[1];
                            }
                            else if (String.Equals(tokens[0], "timezone", StringComparison.InvariantCultureIgnoreCase))
                            {
                                //Timezone
                                timeZone = tokens[1];
                            }
                        }
                    }
                    else
                    {
                        //Data point
                        String[] tokens = line.Split(new char[] { ',' });
                        if (tokens.Length == 6)
                        {
                            //Get timestamp
                            DateTime? tempTimestamp = UnixTimeToDateTime(tokens[0]);

                            //Get close value
                            Decimal tempCloseValue = Decimal.Zero;
                            if (tokens[1] == null || !Decimal.TryParse(tokens[1], out tempCloseValue))
                            {
                                tempCloseValue = Decimal.Zero;
                            }

                            //Get volume value
                            Decimal tempVolumeValue = Decimal.Zero;
                            if (tokens[5] == null || !Decimal.TryParse(tokens[5], out tempVolumeValue))
                            {
                                tempVolumeValue = Decimal.Zero;
                            }

                            //Add data point to collection
                            points.Points.Add(
                                new StockPoint(
                                    tempTimestamp,
                                    tempCloseValue,
                                    tempVolumeValue));
                        }
                    }
                }

                //Create Commpany stock group out of data collected from the file, if possible
                if (companyName != null && symbol != null)
                {
                    return new CompanyStockGroup(
                        companyName,
                        timeZone,
                        symbol,
                        points);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Convert a UNIX date time to a standard date time.
        /// </summary>
        /// <param name="unixDateString">Date string in UNIX format.</param>
        /// <returns>Standard datetime.</returns>
        private DateTime? UnixTimeToDateTime(string unixDateString)
        {
            double seconds = 0;
            if(!double.TryParse(unixDateString, out seconds))
            {
                return null;
            }
            return _Epoch.AddSeconds(seconds);
        }

        /// <summary>
        /// Clean up web access object.
        /// </summary>
        public void Dispose()
        {
            if (_Client != null)
            {
                _Client.Dispose();
            }
        }

    }
}
