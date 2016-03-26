using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg
{
    public class TickerPoint
    {
        public TickerInfo Info
        {
            get;
            set;
        }

        public Double NormalTime
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get;
            set;
        }

        public Int32 UnixTimestamp
        {
            get
            {
                return (Int32)(Timestamp.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            }
        }

        public Double CloseValue
        {
            get;
            set;
        }

        public Double NormalCloseValue
        {
            get;
            set;
        }

        public Double VolumeValue
        {
            get;
            set;
        }

        public Double NormalVolumeValue
        {
            get;
            set;
        }

        public Double NormalCompany
        {
            get;
            set;
        }

        public Double PercentChange
        {
            get;
            set;
        }
    }
}
