using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg.Data.Company
{
    /// <summary>
    /// Stock point.
    /// </summary>
    public class StockPoint : IStockPoint
    {
        private DateTime? _Timestamp = null;
        private Decimal _CloseValue = Decimal.Zero;
        private Decimal _VolumeValue = Decimal.Zero;

        /// <summary>
        /// Timestamp of stock value.
        /// </summary>
        public DateTime? Timestamp
        {
            get
            {
                return _Timestamp;
            }
        }

        /// <summary>
        /// Closing value.
        /// </summary>
        public Decimal CloseValue
        {
            get
            {
                return _CloseValue;
            }
        }

        /// <summary>
        /// Volume value.
        /// </summary>
        public Decimal VolumeValue
        {
            get
            {
                return _VolumeValue;
            }
        }

        /// <summary>
        /// Create a new stock point.
        /// </summary>
        /// <param name="timestamp">Timestamp of stock value.</param>
        /// <param name="closeValue">Closing value.</param>
        /// <param name="volumeValue">Volume value.</param>
        public StockPoint(
            DateTime? timestamp, 
            Decimal closeValue,
            Decimal volumeValue)
        {
            _Timestamp = timestamp;
            _CloseValue = closeValue;
            _VolumeValue = volumeValue;
        }

        /// <summary>
        /// Percent change in close value from a given point
        /// </summary>
        /// <param name="startPoint">Start calculation from this point.</param>
        /// <returns>Percent changed , 1.0 = 100, 2.0 = 200, etc</returns>
        public Decimal PercentChangeInCloseFrom(IStockPoint startPoint)
        {
            return (startPoint != null) ?
                PercentChanged(startPoint.CloseValue, CloseValue) :
                Decimal.Zero;
        }

        /// <summary>
        /// Percent change in volume from a given point
        /// </summary>
        /// <param name="startPoint">Start calculation from this point.</param>
        /// <returns>Percent changed , 1.0 = 100, 2.0 = 200, etc</returns>
        public Decimal PercentChangeInVolumeFrom(IStockPoint startPoint)
        {
            return (startPoint != null) ?
                PercentChanged(startPoint.CloseValue, CloseValue) :
                Decimal.Zero;
        }

        /// <summary>
        /// Calculate percent changed as a value from 0.0 to 1.0.
        /// </summary>
        /// <remarks>Infinite change is represented as Decimal.MaxValue.</remarks>
        /// <param name="start">Starting value</param>
        /// <param name="end">Ending value</param>
        /// <returns>Percent changed , 1.0 = 100, 2.0 = 200, etc</returns>
        private Decimal PercentChanged(Decimal start, Decimal end)
        {
            return (start > Decimal.Zero) ? ((start - end) / start) : Decimal.MaxValue;
        }

    }
}
