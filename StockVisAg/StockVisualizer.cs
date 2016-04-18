using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Globalization;
using System.Drawing.Imaging;
using StockVisAg.Data.Symbol;
using StockVisAg.Data.StockFile;
using StockVisAg.Data.Company;
using StockVisAg.Data.StockFile.FormatHandler.YahooFinance;
using StockVisAg.Visualize;

namespace StockVisAg
{
    public partial class StockVisualizer : Form
    {
        //Holds all company data
        private List<CompanyStockGroup> companies = new List<CompanyStockGroup>();
        
        public StockVisualizer()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Get all available data.
        /// </summary>
        private void GetData()
        {
            //Holds all company data
            companies = new List<CompanyStockGroup>();

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
        }

        /// <summary>
        /// Create an image with data from the given dates.
        /// </summary>
        private void Export()
        {
            //Create a new visualizer
            StockSquareVisualizer visualizer = new StockSquareVisualizer();

            //Create the image
            Image img = visualizer.Visualize(companies,
                startTime.Value,
                endTime.Value);

            //Get the file name
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Title = "Save image to file.";
            ofd.FileName = "stock_" + startTime.Value.ToString("MM_dd_yyyy") + ".png";
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                //Save image file to disk
                img.Save(ofd.FileName, ImageFormat.Png);
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            //Start getting data.
            btnGetData.Enabled = false;
            btnGetData.Text = "Getting Data...";

            //Start getting data
            bwGetData.RunWorkerAsync();
        }
        
        private void btnMakeImage_Click(object sender, EventArgs e)
        {
            //Export to image
            Export();
        }

        private void bwGetData_DoWork(object sender, DoWorkEventArgs e)
        {
            //Get available data
            GetData();
        }

        private void bwGetData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Enable get data button
            btnGetData.Enabled = true;
            btnGetData.Text = "Get Data";

            //Enable image file controls
            startTime.Enabled = true;
            endTime.Enabled = true;
            btnMakeImage.Enabled = true;
        }
    }
}
