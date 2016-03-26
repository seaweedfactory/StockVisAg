using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockVisAg
{
    public class TickerInfo
    {
        List<TickerPoint> m_data = new List<TickerPoint>();

        public String CompanyName
        {
            get;
            set;
        }

        public String Timezone
        {
            get;
            set;
        }

        public String TickerSymbol
        {
            get;
            set;
        }

        public Double AverageClose
        {
            get
            {
                Double total = 0.0d;
                foreach (TickerPoint tp in m_data)
                {
                    total += tp.CloseValue;
                }
                return (m_data.Count > 0) ? total / (Double)m_data.Count : 0.0d;
            }
        }

        public List<TickerPoint> Data
        {
            get
            {
                return m_data;
            }

            set
            {
                m_data = value;
            }
        }

        public void AddDataPoint(DateTime timestamp, Double closeValue, Double volumeValue)
        {
            TickerPoint p = new TickerPoint();
            p.Timestamp = timestamp;
            p.CloseValue = closeValue;
            p.VolumeValue = volumeValue;

            //Percent change
            TickerPoint s = new TickerPoint();
            if (m_data != null && m_data.Count > 0)
            {
                s = m_data[0];
                p.PercentChange = ((s.CloseValue - p.CloseValue) / s.CloseValue);
            }
            else
            {
                p.PercentChange = 0.0d;
            }

            p.Info = this;
            m_data.Add(p);
        }
    }
}
