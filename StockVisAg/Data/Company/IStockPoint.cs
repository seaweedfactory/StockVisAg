using System;

namespace StockVisAg.Data.Company
{
    /// <summary>
    /// Defines a stock point.
    /// </summary>
    public interface IStockPoint
    {
        DateTime? Timestamp { get; }
        Decimal CloseValue { get; }
        Decimal VolumeValue { get; }
    }
}