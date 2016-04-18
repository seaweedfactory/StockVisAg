using StockVisAg.Data.Company;
using StockVisAg.Data.StockFile;
using StockVisAg.Data.StockFile.FormatHandler.YahooFinance;
using StockVisAg.Data.Symbol;
using StockVisAg.Visualize;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace StockVisAg
{
    /// <summary>
    /// Download all available data, then export each day as an image.
    /// </summary>
    public class ExportAllDataAsImage
    {
        public static void Main()
        {
            //Holds all company data
            List<CompanyStockGroup> companies = new List<CompanyStockGroup>();
            
            //Build a symbol factory
            TickerSymbolFactory tsf = new TickerSymbolFactory();

            //Get default symbols from resources
            List<ITickerSymbol> tickerList = tsf.GetDefaultSymbols();
            
            //Use the Yahoo Finance handler to get stock files, then parse those stock files
            using (YahooFinanceFormatHandler fileFormatHandler = new YahooFinanceFormatHandler())
            {
                //Hold list of stock files
                List<IStockFile> stockFiles = new List<IStockFile>();

                //Build stock file factory
                StockFileFactory sff = new StockFileFactory();

                //Download all stock file data and store in memmory
                stockFiles = sff.GetStockFiles(tickerList, fileFormatHandler);

                //Parse each stock file to produce a company file
                foreach (IStockFile stockFile in stockFiles)
                {
                    CompanyStockGroup companyStockGroup = fileFormatHandler.ParseStockFile(stockFile);
                    if (companyStockGroup != null)
                    {
                        companies.Add(companyStockGroup);
                    }
                }
            }

            //Get the date range of the available data
            DateTime minDate = (DateTime)companies.Min(x => x.Data.Points.Min(y => y.Timestamp));
            DateTime maxDate = (DateTime)companies.Max(x => x.Data.Points.Max(y => y.Timestamp));

            //Create a new visualizer
            StockSquareVisualizer visualizer = new StockSquareVisualizer();

            //Step through each day in the sequence and find data between the hours of 8 and 17.
            TimeSpan t = (maxDate - minDate).Add(new TimeSpan(1,0,0,0));
            for (int i = 0; i < t.Days; i++)
            {
                DateTime stepDate = minDate.AddDays(i);
                Image img = visualizer.Visualize(companies,
                    new DateTime(stepDate.Year, stepDate.Month, stepDate.Day, 8, 0, 0),
                    new DateTime(stepDate.Year, stepDate.Month, stepDate.Day, 17, 0, 0));

                //Save image file to disk
                img.Save("stock_" + stepDate.ToString("MM_dd_yyyy") + ".png", ImageFormat.Png);
            }
        }
    }
}
