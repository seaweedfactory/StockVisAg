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

namespace StockVisAg
{
    public partial class GetStockListForm : Form
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public GetStockListForm()
        {
            InitializeComponent();
        }

        private TickerInfo ParseStockFile(String filename)
        {
            TickerInfo info = new TickerInfo();
            string line;
            using (var reader = File.OpenText(filename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(":"))
                    {
                        //Descriptor
                        String[] tokens = line.Split(new char[] { ':' });
                        if (tokens.Length > 0)
                        {
                            if (tokens[0].Equals("ticker"))
                            {
                                info.TickerSymbol = tokens[1];
                            }
                            else if (tokens[0].Equals("Company-Name"))
                            {
                                info.CompanyName = tokens[1];
                            }
                            else if (tokens[0].Equals("timezone"))
                            {
                                info.Timezone = tokens[1];
                            }
                        }
                    }
                    else
                    {
                        //Data point
                        String[] tokens = line.Split(new char[] { ',' });
                        if (tokens.Length == 6)
                        {
                            DateTime tempTime = UnixTimeToDateTime(tokens[0]);

                            //Ensure date range fits to avoid denormalizing outliers
                            if (tempTime >= startTime.Value && tempTime <= endTime.Value)
                            {
                                info.AddDataPoint(tempTime, Convert.ToDouble(tokens[1]), Convert.ToDouble(tokens[5]));
                            }
                        }
                    }
                }
            }
            return info;
        }

        private List<TickerInfo> ParseStocks()
        {
            List<TickerInfo> infoList = new List<TickerInfo>();

            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select a folder of stock files";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    String[] fileNames = Directory.GetFiles(dlg.SelectedPath);
                    foreach (String filename in fileNames)
                    {
                        infoList.Add(ParseStockFile(filename));
                    }
                }
            }

            return infoList;
        }

        private void GetStockList()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a list of stock ticker symbols, one on each line.";
            if (ofd.ShowDialog() != DialogResult.Cancel)
            {
                using (WebClient client = new WebClient())
                {
                    label1.Text = "Downloading stock files...";
                    string line;
                    using (var reader = File.OpenText(ofd.FileName))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            DownloadStockFile(client, line);
                        }
                    }
                    label1.Text = "Done.";
                }
            }
        }

        private bool DownloadStockFile(WebClient client, String company)
        {
            try
            {
                String startURL = @"http://chartapi.finance.yahoo.com/instrument/1.0/";
                String endURL = @"/chartdata;type=quote;range=2d/csv/";
                client.DownloadFile(startURL + company + endURL, company + ".txt");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static DateTime UnixTimeToDateTime(string text)
        {
            double seconds = double.Parse(text, CultureInfo.InvariantCulture);
            return Epoch.AddSeconds(seconds);
        }

        private List<TickerInfo> Normalize(List<TickerInfo> tickerList)
        {
            List<TickerInfo> newList = new List<TickerInfo>();

            DateTime maxTime = DateTime.MinValue;
            DateTime minTime = DateTime.MaxValue;
            double maxCloseValue = Int32.MinValue;
            double minCloseValue = Int32.MaxValue;
            double maxVolumeValue = Int32.MinValue;
            double minVolumeValue = Int32.MaxValue;
            double maxPercentChangeValue = Int32.MinValue;
            double minPercentChangeValue = Int32.MaxValue;
               
            #region Get max and min
            foreach (TickerInfo tickInfo in tickerList)
            {
                TickerInfo newInfo = new TickerInfo();
                foreach (TickerPoint tickPoint in tickInfo.Data)
                {
                    newInfo.AddDataPoint(tickPoint.Timestamp, tickPoint.CloseValue, tickPoint.VolumeValue);

                    if (tickPoint.Timestamp > maxTime)
                    {
                        maxTime = tickPoint.Timestamp;
                    }

                    if (tickPoint.Timestamp < minTime)
                    {
                        minTime = tickPoint.Timestamp;
                    }

                    if (tickPoint.CloseValue > maxCloseValue)
                    {
                        maxCloseValue = tickPoint.CloseValue;
                    }

                    if (tickPoint.CloseValue < minCloseValue)
                    {
                        minCloseValue = tickPoint.CloseValue;
                    }

                    if (tickPoint.VolumeValue > maxVolumeValue)
                    {
                        maxVolumeValue = tickPoint.VolumeValue;
                    }

                    if (tickPoint.VolumeValue < minVolumeValue)
                    {
                        minVolumeValue = tickPoint.VolumeValue;
                    }

                    if (tickPoint.PercentChange > maxPercentChangeValue)
                    {
                        maxPercentChangeValue = tickPoint.PercentChange;
                    }

                    if (tickPoint.PercentChange < minPercentChangeValue)
                    {
                        minPercentChangeValue = tickPoint.PercentChange;
                    }
                }
                newList.Add(newInfo);
            }
            #endregion

            double diffCloseValue = maxCloseValue - minCloseValue;
            double diffVolumeValue = maxVolumeValue - minVolumeValue;
            double diffPercentChange = maxPercentChangeValue - minPercentChangeValue;
            long diffTime = maxTime.Ticks - minTime.Ticks;
            int companyNumber = 0;
            foreach (TickerInfo tickInfo in newList)
            {
                TickerPoint startPoint = new TickerPoint();

                if (tickInfo.Data != null && tickInfo.Data.Count > 0)
                {
                    startPoint = tickInfo.Data[0];
                }

                foreach (TickerPoint tickPoint in tickInfo.Data)
                {
                    tickPoint.NormalCloseValue = (tickPoint.CloseValue - minCloseValue) / diffCloseValue;
                    tickPoint.NormalVolumeValue = (tickPoint.VolumeValue - minVolumeValue) / diffVolumeValue;
                    tickPoint.NormalTime = ((Double)tickPoint.Timestamp.Ticks - minTime.Ticks) / ((Double)diffTime);
                    tickPoint.NormalCompany = (((Double)companyNumber) / ((Double)newList.Count));
                    tickPoint.PercentChange = (tickPoint.PercentChange - minPercentChangeValue) / diffPercentChange;
                }
                companyNumber++;
            }

            return newList;
        }

        private void WriteToPointFile(List<TickerInfo> tickerList)
        {
            using(SaveFileDialog sfd = new SaveFileDialog())
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter myStreamWriter = new StreamWriter(sfd.FileName + ".csv");
                    myStreamWriter.WriteLine("Timestamp,PercentChange,CompanyNumber");
                    foreach (TickerInfo tickInfo in tickerList)
                    {
                        foreach (TickerPoint tickPoint in tickInfo.Data)
                        {
                            myStreamWriter.WriteLine(tickPoint.NormalTime.ToString() + "," + tickPoint.PercentChange.ToString() + "," + tickPoint.NormalCompany.ToString());
                        }
                    }
                    myStreamWriter.Close();

                    WriteToImage(tickerList, sfd.FileName + ".png");
                }
            }
        }

        private void WriteToImage(List<TickerInfo> tickerList, string filename)
        {
            int sideParam = tickerList.Count;
            Bitmap bmp = new Bitmap(sideParam, sideParam);
            using (var g = Graphics.FromImage(bmp))
            {
                tickerList = tickerList.OrderBy(x => x.AverageClose).ToList();

                foreach (TickerInfo tickInfo in tickerList)
                {
                    int i = 0;
                    int lx = 0;
                    int ly = 0;

                    tickInfo.Data = tickInfo.Data.OrderBy(x => x.Timestamp).ToList();

                    foreach (TickerPoint tickPoint in tickInfo.Data)
                    {
                        int x = (int)(tickPoint.NormalTime * (sideParam - 1));
                        int y = (int)(tickPoint.NormalCompany * (sideParam - 1));
                        int z = 128 + (int)(tickPoint.PercentChange * 128.0d);

                        if (i == 0)
                        {
                            lx = x;
                            ly = y;
                        }

                        z = z <= 255 ? z : 255;

                        using (Pen newPen = new Pen(Color.FromArgb(z, z, z)))
                        {
                            g.DrawLine(newPen, lx, ly, x, y);
                        }

                        lx = x;
                        ly = y;
                        i++;
                    }
                }
            }
            bmp.Save(filename, ImageFormat.Png);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetStockList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WriteToPointFile(Normalize(ParseStocks()));
        }
    }
}
