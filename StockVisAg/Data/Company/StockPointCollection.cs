using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg.Data.Company
{
    /// <summary>
    /// Collection of stock points, with aggregation methods.
    /// </summary>
    public class StockPointCollection
    {
        private readonly List<IStockPoint> _points = new List<IStockPoint>();

        /// <summary>
        /// Stock points.
        /// </summary>
        public List<IStockPoint> Points
        {
            get
            {
                return _points;
            }
        }

        /// <summary>
        /// Earliest point in the collection.
        /// </summary>
        public IStockPoint EarliestPoint
        {
            get
            {
                return (_points != null && _points.Count > 0) ?
                    _points.OrderBy(x => x.Timestamp).FirstOrDefault(): 
                    null;
            }
        }

        /// <summary>
        /// Average close value of all points.
        /// </summary>
        public Decimal AverageClose
        {
            get
            {
                Decimal total = Decimal.Zero;
                foreach (StockPoint point in _points)
                {
                    total += point.CloseValue;
                }
                return (_points.Count > 0) ? total / (Decimal)_points.Count : Decimal.Zero;
            }
        }

        /// <summary>
        /// Average percent close changed from earliest point in collection.
        /// </summary>
        public Decimal AveragePercentCloseChanged
        {
            get
            {
                IStockPoint e = EarliestPoint;
                if(e != null)
                {
                    Decimal total = Decimal.Zero;
                    foreach (StockPoint point in _points)
                    {
                        total += point.PercentChangeInCloseFrom(e);
                    }
                    return (_points.Count > 0) ? total / (Decimal)_points.Count : Decimal.Zero;
                }
                else
                {
                    return Decimal.Zero;
                }
                
            }
        }
    }
}
