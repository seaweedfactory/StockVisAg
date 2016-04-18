using StockVisAg.Data.Company;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StockVisAg.Visualize
{
    /// <summary>
    /// Visual the given companies as a square, for the given date range.
    /// Sorts by company name on the vertical axis.
    /// </summary>
    public class StockSquareVisualizer
    {
        /// <summary>
        /// Visual the given companies as a square, for the given date range.
        /// </summary>
        /// <param name="companies">Company list.</param>
        /// <param name="startTime">Start time.</param>
        /// <param name="endTime">End time.</param>
        /// <param name="desiredWidth">Optional width of output image. If null, uses number of companies.</param>
        /// <returns>Image of visualization.</returns>
        public Image Visualize(
            List<CompanyStockGroup> companies, 
            DateTime startTime, 
            DateTime endTime,
            int? desiredWidth = null)
        {
            int width = (desiredWidth != null) ? (int)desiredWidth : companies.Count;
            Bitmap bmp = new Bitmap(width, companies.Count);
            using (var g = Graphics.FromImage(bmp))
            {
                //Examine each company
                int companyCount = 0; //Company counter for normalization.
                foreach (CompanyStockGroup company in companies.OrderBy(x => x.CompanyName).ToList())
                {
                    //Get points in range
                    List<IStockPoint> rangePoints =
                        company.Data.Points
                        .Where(x => x.Timestamp != null
                        && ((DateTime)x.Timestamp) >= startTime
                        && ((DateTime)x.Timestamp) <= endTime)
                        .OrderBy(x => x.Timestamp)
                        .ToList();

                    if (rangePoints != null && rangePoints.Count > 0)
                    {
                        //Get normalization values
                        DateTime minimumTimestamp = (DateTime)rangePoints.Where(x => x.Timestamp != null).Min(x => x.Timestamp);
                        DateTime maximumTimestamp = (DateTime)rangePoints.Where(x => x.Timestamp != null).Max(x => x.Timestamp);

                        Decimal minimumClose = rangePoints.Min(x => x.CloseValue);
                        Decimal maximumClose = rangePoints.Max(x => x.CloseValue);

                        int i = 0;
                        int lx = 0;
                        int y = (int)(((Decimal)companyCount / (Decimal)(companies.Count - 1)) * (Decimal)(companies.Count - 1));

                        foreach (IStockPoint tickPoint in rangePoints)
                        {
                            //Normalize values for this point
                            Decimal normalizedPointTime = (maximumTimestamp - minimumTimestamp).Ticks != 0 ?
                                (Decimal)((((DateTime)tickPoint.Timestamp) - minimumTimestamp).Ticks) / (Decimal)((maximumTimestamp - minimumTimestamp).Ticks) :
                                Decimal.Zero;

                            Decimal normalizedPointClose = (maximumClose - minimumClose) != 0 ?
                                (Decimal)(tickPoint.CloseValue - minimumClose) / (Decimal)(maximumClose - minimumClose) :
                                Decimal.Zero;

                            int x = (int)(normalizedPointTime * (Decimal)(width));
                            int z = 128 + (int)(normalizedPointClose * new Decimal(128));

                            if (i == 0)
                            {
                                lx = x;
                            }

                            //Clamp in range
                            z = z <= 255 ? z : 255;

                            using (Pen newPen = new Pen(Color.FromArgb(z, z, z)))
                            {
                                g.DrawLine(newPen, lx, y, x, y);
                            }

                            lx = x;
                            i++;
                        }
                    }
                    else
                    {
                        int y = (int)(((Decimal)companyCount / (Decimal)(width)) * (Decimal)(width));
                        using (Pen newPen = new Pen(Color.FromArgb(0, 0, 0)))
                        {
                            g.DrawLine(newPen, 0, y, (width), y);
                        }
                    }

                    //Increment company counter for normalization
                    companyCount++;
                }
            }

            return bmp;
        }
    }
}
