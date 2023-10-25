using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueData_DotNet;
using Newtonsoft.Json;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace TrueData_DotNetSample
{
    class Program
    {
        static TDWebSocket tDWebSocket;
        static TDHistory tDHistory;
        public string Output = string.Empty;
        static void Main(string[] args)
        {
            

            connectWebSocketRT();
            //connectRESTHistory();

        }
        private static void connectRESTHistory()
        {
            tDHistory = new TDHistory("your_id", "your_pwd");
            tDHistory.login();

            //string csvResponse = tDHistory.GetBarHistory("L%26TFH", DateTime.Today.AddHours(-1), DateTime.Now,false,  Constants.Interval_1Min, false);
            //Console.WriteLine("History 1min for Relaince");
            //Console.WriteLine(csvResponse);
            ////Console.ReadLine();

            //string csvTickData = tDHistory.GetTickHistory("ACC", false, DateTime.Now.AddHours(-0.5), DateTime.Now,false, true);
            //Console.WriteLine("History Tick for ACC");
            //Console.WriteLine(csvTickData);
            ////Console.ReadLine();

            //string csvLastNBar = tDHistory.GetLastNBars("HDFC", 1, false, "15min");
            //Console.WriteLine("Last 10 bar of HDFC");
            //Console.WriteLine(csvLastNBar);
            //Console.ReadLine();
            string data1 = tDHistory.GetBarHistory("NIFTY23083119300CE", new DateTime(2023, 08, 25, 9, 00, 0), 
                    new DateTime(2023, 08, 25, 16, 30, 0), true, Constants.Interval_3Min);
            Console.WriteLine(data1);
            Console.Read();

            string data2 = tDHistory.GetBarHistory("NIFTY23083119300CE", new DateTime(2023, 08, 25, 9, 00, 0),
                new DateTime(2023, 08, 25, 15, 30, 0), true, Constants.Interval_3Min);
            Console.WriteLine(data2);

            Console.Read();

            //int reqid = 0;
            //for (int i = 0; i < 300; i++)
            //{
            //   // (new Thread(() => {
                    
            //        string csvLastNTicks = tDHistory.GetLastNTicks("HDFC", true, 1, false, "EOD", true);
            //        Console.Write(++reqid + " " + DateTime.Now.ToString() + " ");
            //        //Console.WriteLine("Last 1 tick of HDFC");
            //        Console.WriteLine(csvLastNTicks);
                    
            //    //})).Start();
            //    //Thread.Sleep(100);


            //}
            //Console.ReadLine();

            //string strLTP = tDHistory.GetLTP("HDFC", true, true);
            //Console.WriteLine("LTP HDFC");
            //Console.WriteLine(strLTP);
            //Console.ReadLine();

            //string csvBhavCopy = tDHistory.GetBhavCopy("EQ", DateTime.Today, true);
            //Console.WriteLine("Today's Bhavcopy");
            //Console.WriteLine(csvBhavCopy);
            //Console.ReadLine();

            //string csvGainers = tDHistory.GetTopGainers("NSEEQ", true, 10);
            //Console.WriteLine("Today's Gainers");
            //Console.WriteLine(csvGainers);
            //Console.ReadLine();

            //string csvLosers = tDHistory.GetTopLosers("CASH", true, 10);
            //Console.WriteLine("Today's Losers");
            //Console.WriteLine(csvLosers);
            //Console.ReadLine();

            //string csvCorpAction = tDHistory.GetCorporateActions("AARTIIND", true);
            //Console.WriteLine("Corporate Actions");
            //Console.WriteLine(csvCorpAction);
            Console.ReadLine();
        }

        private static void connectWebSocketRT()
        {
            tDWebSocket = new TDWebSocket("wssand034", "vikrant034", "wss://push.truedata.in", 8086);

            tDWebSocket.OnConnect += TDWebSocket_OnConnect;
            tDWebSocket.OnDataArrive += TDWebSocket_OnDataArrive;
            tDWebSocket.OnClose += TDWebSocket_OnClose;
            tDWebSocket.OnError += TDWebSocket_OnError;
            //new Thread(() => {
            //    Thread.Sleep(5000);
            //    tDWebSocket.GetMarketStatus();
            //    tDWebSocket.UnSubscribe(new string[] { "NIFTY-I", "RELIANCE", "HDFC", "HDFCBANK", "ICICIBANK", "ZEEL", "MINDTREE", "NIFTY 50", "CRUDEOIL-I" });
            //    tDWebSocket.Logout();
            //}).Start();

            tDWebSocket.ConnectAsync();

        }

        private static void TDWebSocket_OnError(object sender, EventErrorArgs e)
        {
            Console.WriteLine(e.ErrorMsg);
            Console.Read();
        }

        private static void TDWebSocket_OnClose(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected");
            Console.Read();
        }

        private static void TDWebSocket_OnDataArrive(object sender, EventDataArgs e)
        {
            //Console.WriteLine(e.JsonMsg);
            if (e.JsonMsg.Contains("bidask"))
            {
                BidAsk b = new BidAsk();
                b = JsonConvert.DeserializeObject<BidAsk>(e.JsonMsg);
                Console.WriteLine(b.SymbolId + "-" + b.Timestamp + "-" + b.Bid + "-" + b.Ask + "-" + b.BidQty);
            }
            else if (e.JsonMsg.Contains("trade"))
            {
                SnapData t = new SnapData();
                t = JsonConvert.DeserializeObject<SnapData>(e.JsonMsg);
                Console.WriteLine(t.SymbolId + "-" + t.Timestamp + "-" + t.LTP + "-" + t.Volume + "-" + t.Open + "-" + t.High + "-" + t.Low + "-" + t.PrevClose);
                StoreDataInDatabase(t.SymbolId,t.Timestamp,t.LTP,t.Volume,t.Open,t.High,t.Low,t.PrevClose);
            }
            else if (e.JsonMsg.Contains("HeartBeat"))
            {
                HeartBeat hb = new HeartBeat();
                hb = JsonConvert.DeserializeObject<HeartBeat>(e.JsonMsg);
                Console.WriteLine("Heartbeat " + hb.timestamp + "-" + hb.message);
            }
            else if (e.JsonMsg.Contains("TrueData"))
            {
                Console.WriteLine("Connected");
                //tDWebSocket.Subscribe(new string[] { "NIFTY-I", "RELIANCE", "HDFC", "ACC",
                //    "IRCTC", "NUVOCO", "SUNPHARMA", "ADANIGREEN", "ADLABS", "ADSL", "AFFLE", "AIAENG",
                //    "AKZOINDIA", "ALLCARGO", "SUNPHARMA", "ALOKINDS", "AMARAJABAT", "AMBER", "AMRUTANJAN", "ANDHRABANK",
                //    "APLAPOLLO", "APLLTD", "APOLLOHOSP", "APOLLOTYRE", "APTECHT", "ARVIND", "ASHOKA", "ASHOKLEY",
                //    "ASIANPAINT", "NUVOCO", "SUNPHARMA", "ADANI,GREEN", "ADLABS", "ATUL", "ATULAUTO", "AIRAN",
                //    "AUBANK", "AUTOAXLES", "AXISBANK", "AXISCADES", "BAJAJ-AUTO", "BAJFINANCE", "BALKRISIND", "BALKRISHNA",
                //    "BAJAJHIND","BAJFINANCE","BALAMINES","BANDHANBNK","BANKINDIA","BATAINDIA","BEML","BFUTILITIE",
                //    "HDFCBANK", "ICICIBANK", "ZEEL", "MINDTREE", "NIFTY 50", "CRUDEOIL-I","USDINR-I",""});

                //tDWebSocket.Subscribe(new string[] { "AARTIIND23AUGFUT", "ABB23AUGFUT", "ABBOTINDIA23AUGFUT", "ABCAPITAL23AUGFUT", "ABFRL23AUGFUT", "ACC23AUGFUT", "ADANIENT23AUGFUT", "ADANIPORTS23AUGFUT", "ALKEM23AUGFUT", "AMBUJACEM23AUGFUT", "APOLLOHOSP23AUGFUT", "APOLLOTYRE23AUGFUT", "ASHOKLEY23AUGFUT", "ASIANPAINT23AUGFUT", "ASTRAL23AUGFUT", "ATUL23AUGFUT", "AUBANK23AUGFUT", "AUROPHARMA23AUGFUT", "AXISBANK23AUGFUT", "BAJAJ-AUTO23AUGFUT", "BAJAJFINSV23AUGFUT", "BAJFINANCE23AUGFUT", "BALKRISIND23AUGFUT", "BALRAMCHIN23AUGFUT", "BANDHANBNK23AUGFUT", "BANKBARODA23AUGFUT", "BATAINDIA23AUGFUT", "BEL23AUGFUT", "BERGEPAINT23AUGFUT", "BHARATFORG23AUGFUT", "BHARTIARTL23AUGFUT", "BHEL23AUGFUT", "BIOCON23AUGFUT", "BOSCHLTD23AUGFUT", "BPCL23AUGFUT", "BRITANNIA23AUGFUT", "BSOFT23AUGFUT", "CANBK23AUGFUT", "CANFINHOME23AUGFUT", "CHAMBLFERT23AUGFUT", "CHOLAFIN23AUGFUT", "CIPLA23AUGFUT", "COALINDIA23AUGFUT", "COFORGE23AUGFUT", "COLPAL23AUGFUT", "NIFTY23081019750CE", "NIFTY23081019750PE", "NIFTY23081019700CE", "NIFTY23081019700PE", "NIFTY23081019650CE", "NIFTY23081019650PE", "NIFTY23081019600CE", "NIFTY23081019600PE", "NIFTY23081019550CE", "NIFTY23081019550PE", "NIFTY23081019500CE", "NIFTY23081019500PE", "NIFTY23081019450CE", "NIFTY23081019450PE", "NIFTY23081019400CE", "NIFTY23081019400PE", "NIFTY23081019350CE", "NIFTY23081019350PE", "NIFTY23081019300CE", "NIFTY23081019300PE", "NIFTY23081019250CE", "NIFTY23081019250PE", "NIFTY23081019200CE", "NIFTY23081019200PE", "BANKNIFTY23081045700CE", "BANKNIFTY23081045700PE", "BANKNIFTY23081045800CE", "BANKNIFTY23081045800PE", "BANKNIFTY23081045900CE", "BANKNIFTY23081045900PE", "BANKNIFTY23081045600CE", "BANKNIFTY23081045600PE", "BANKNIFTY23081045500CE", "BANKNIFTY23081045500PE", "BANKNIFTY23081045400CE", "BANKNIFTY23081045400PE", "BANKNIFTY23081045300CE", "BANKNIFTY23081045300PE", "BANKNIFTY23081045200CE", "BANKNIFTY23081045200PE", "BANKNIFTY23081045100CE", "BANKNIFTY23081045100PE" });
                tDWebSocket.Subscribe(new string[] { "NIFTY-I", "TCS", "NIFTY 50", "CRUDEOIL-I" });

            }
            else
                Console.WriteLine(e.JsonMsg);
        }

        private static void TDWebSocket_OnConnect(object sender, EventArgs e)
        {
            //tDWebSocket.Send("{\"method\":\"addsymbol\",\"symbols\":[\"NIFTY-I\",\"RELIANCE\",\"HDFC\",\"HDFCBANK\",\"ICICIBANK\",\"ZEEL\",\"MINDTREE\",\"NIFTY 50\",\"CRUDEOIL-I\"]}");
        }
        static void StoreDataInDatabase(int SymbolId ,DateTime Timestamp ,float LTP,int Volume,float Open,float High,float Low,float PrevClose)
        {
            //ResHeartBeat sd = new ResHeartBeat();
            string connectionString = @"data source=DESKTOP-L528N01\SQLEXPRESS01;initial catalog=WebSocket;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework providerName = System.Data.SqlClient";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("InsertData", connection);
                cmd.CommandType = CommandType.StoredProcedure;  
                cmd.Parameters.AddWithValue("@SymbolId", SymbolId);
                cmd.Parameters.AddWithValue("@TimeStamp", Timestamp);
                cmd.Parameters.AddWithValue("@LTP", Math.Round(LTP,2));
                cmd.Parameters.AddWithValue("@Volume", Volume);
                cmd.Parameters.AddWithValue("@OpenValue", Math.Round(Open, 2));
                cmd.Parameters.AddWithValue("@HighValue", Math.Round(High, 2));
                cmd.Parameters.AddWithValue("@LowValue", Math.Round(Low, 2));
                cmd.Parameters.AddWithValue("@PrevClose", Math.Round(PrevClose,2));

                cmd.ExecuteNonQuery();
            }
        }
    }

    class BidAsk
    {
        [JsonIgnore]
        public int SymbolId { get; set; }
        [JsonIgnore]
        public DateTime Timestamp { get; set; }
        [JsonIgnore]
        public float Bid { get; set; }
        [JsonIgnore]
        public int BidQty { get; set; }
        [JsonIgnore]
        public float Ask { get; set; }
        [JsonIgnore]
        public float AskQty { get; set; }
        private string[] bidask1;
        [JsonProperty]
        public string[] bidask
        {
            get { return bidask1; }
            set
            {
                bidask1 = value;
                SymbolId = int.Parse(bidask1[0]);
                Timestamp = DateTime.Parse(bidask1[1]);
                Bid = float.Parse(bidask1[2]);
                BidQty = int.Parse(bidask1[3]);
                Ask = float.Parse(bidask1[4]);
                AskQty = int.Parse(bidask1[5]);
            }
        }


    }
    class SnapData
    {
        [JsonIgnore]
        public int SymbolId { get; set; }
        [JsonIgnore]
        public DateTime Timestamp { get; set; }
        [JsonIgnore]
        public float LTP { get; set; }
        [JsonIgnore]
        public float ATP { get; set; }
        [JsonIgnore]
        public int Volume { get; set; }
        [JsonIgnore]
        public long TotalVolume { get; set; }
        [JsonIgnore]
        public float Open { get; set; }
        [JsonIgnore]
        public float High { get; set; }
        [JsonIgnore]
        public float Low { get; set; }
        [JsonIgnore]
        public float PrevClose { get; set; }
        [JsonIgnore]
        public int OI { get; set; }
        [JsonIgnore]
        public int PrevOI { get; set; }
        [JsonIgnore]
        public double TurnOver { get; set; }
        [JsonIgnore]
        public string OHL { get; set; }
        [JsonIgnore]
        public int sequenceno { get; set; }
        [JsonIgnore]
        public float Bid { get; set; }
        [JsonIgnore]
        public int BidQty { get; set; }
        [JsonIgnore]
        public float Ask { get; set; }
        [JsonIgnore]
        public float AskQty { get; set; }
        private string[] tradelocal;
        [JsonProperty]
        public string[] trade
        {
            get { return tradelocal; }
            set
            {
                tradelocal = value;
                SymbolId = int.Parse(tradelocal[0]);
                Timestamp = DateTime.Parse(tradelocal[1]);
                LTP = float.Parse(tradelocal[2]);
                Volume = int.Parse(tradelocal[3]);
                ATP = float.Parse(tradelocal[4]);
                TotalVolume = long.Parse(tradelocal[5]);
                Open = float.Parse(tradelocal[6]);
                High = float.Parse(tradelocal[7]);
                Low = float.Parse(tradelocal[8]);
                PrevClose = float.Parse(tradelocal[9]);
                OI = int.Parse(tradelocal[10]);
                PrevOI = int.Parse(tradelocal[11]);
            }
        }
    }
    public class HeartBeat
    {
        public bool success { get; set; }
        public string message { get; set; }
        public DateTime timestamp { get; set; }
    }
    //public class ResHeartBeat
    //{
    //    public string message { get; set; }
    //    public DateTime timestamp { get; set; }
    //}

}
