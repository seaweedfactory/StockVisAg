# StockVisAg
Collapse stock data from Yahoo! Finance into a grayscale visualization.

It was originally created to build heightmaps out of stock data. Data is sourced from the Yahoo! Financial API.

Currently, the project is set to show the percent change throughout the day as a grayscale value. Other values are available in the TickerPoint object which can also be used to build the visualization.

A basic UI is provided by GetStockListForm, which can grab the data and build the visualization according to a date range.

# Get Data
Get the raw stock data by clicking "Get Stock Files". This will prompt for a stock ticker symbol list. The list is simply text file with each line containing one stock ticker symbol, like so:

TFSCU
TFSCW
PIH
FLWS
FCTY
FCCY
SRCE
VNET

A sample list of tickers tracked by the Yahoo Financial API is included as stock_symbols.txt in the project.

After the file is selected, the program will begin downloading data about each symbol into text files, one for each symbol.

Here is an example file for the TFSCU symbol:

uri:/instrument/1.0/TFSCU/chartdata;type=quote;range=2d/csv/
ticker:tfscu
Company-Name:1347 Capital Corp.
Exchange-Name:NCM
unit:MIN
timezone:EDT
currency:USD
gmtoffset:-14400
previous_close:10.0500
range:20160314,1457962200,1457985600
range:20160323,1458739800,1458763200
Timestamp:1457962200,1458763200
labels:1457964000,1457967600,1457971200,1457974800,1457978400,1457982000,1457985600,1458741600,1458745200,1458748800,145875240,1458756000,1458759600,1458763200
values:Timestamp,close,high,low,open,volume
close:10.0500,10.2500
high:10.0500,10.2500
low:10.0500,10.2500
open:10.0500,10.2500
volume:1000,34000
1457970371,10.0500,10.0500,10.0500,10.0500,1000
1458744296,10.2500,10.2500,10.2500,10.2500,34000

The last two lines of this file contain data points which will be transformed into TickerPoint objects. It is important to note that the first token in these lines is a timestamp in epoch format. All available data from the API is collected regardless of date.

These are collected in a TickerInfo object, which contain the general information about each symbol.

For the next step, it is best to collect the downloaded data into its own folder.

# Make Visualization

To make the visualization, first pick a start and an end date and time to show. Usually, the dates captured from the API only extend back about a week. Some tickers will not have data for a given date. Remeber to pick appropriate times as well, for example, 8 00 00 to 17 00 00 should get most of the trading day. It is best to use dates and times that fall within the same day.

Click Parse Stock Files and a prompt will appear for a folder containing the data. Choose the folder from the first step. Note that the same data can be used to produce different results depending on the date range entered.

The data will be passed through a normalizaing function.

A prompt will appear to save the output. Both a CSV containing the normalized data and a PNG image file will be created with the given name. The WriteToImage function determines what data point in the TickerPoint will be used for the image. It also does additional normalization to produce the image.
