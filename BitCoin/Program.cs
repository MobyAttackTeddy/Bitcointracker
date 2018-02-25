using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;


namespace BitCoin
{

    

    public class btcObject
    {
        public double high;
        public double last;
        public double timestamp;
        public double bid;
        public double vwap;
        public double volume;
        public double low;
        public double ask;
        public double open;
    }

    public class Wallet
    {
        public double dollars;
        public double bitCoin;
        public btcObject lastturn = new btcObject();
        public btcObject secondlastturn = new btcObject();
        public string lastConjuction = "down";
        public double percent = 0;
        public btcObject startValue = new btcObject();

        public string ToString()
        {
            return "Wallet, BTC: " + bitCoin + "\nDollars: " + dollars + " $ \nCurrent Conjuction: " + lastConjuction + " \nLast turn: " + Program.UnixTimeStampToDateTime(lastturn.timestamp) + " \nPrice last market turn: " + this.lastturn.last;
        }
    }

    

    class Program
    {
        
        public static btcObject getBtcJson()
        {
            try
            {
                json = new WebClient().DownloadString("https://www.bitstamp.net/api/ticker/");
            }
            catch (Exception e)
            {

            }
            
            return JsonConvert.DeserializeObject<btcObject>(json);
        }
        static Thread t1 = new Thread(RunningThread);
        public static Wallet wallet = new Wallet();
        public static btcObject btc = new btcObject();
        static List<btcObject> btcList = new List<btcObject>();

        public static void RunningThread()
        {
            {
                int loadupCounter = 0;
                Model1Container model1Container = new Model1Container();
                
                do
                {
                    /// JUST A TEST works
                    wallet.secondlastturn = wallet.lastturn;

                    

                    btcObject btc = getBtcJson();
                    // DateTime shit = UnixTimeStampToDateTime(btc.timestamp);  /// Remove
                    if (wallet.startValue.last == 0)
                        wallet.startValue = btc;
                    btcList.Add(btc);

                    double Percent = btcList.LastOrDefault().last / wallet.lastturn.last;
                    
                    // REST Bitcoin
                    try
                    {
                        if (btcList.LastOrDefault().last < btcList.ElementAt(btcList.Count - 2).last)
                        {

                            Console.WriteLine("Down " + UnixTimeStampToDateTime(btcList.LastOrDefault().timestamp));
                            Console.WriteLine("@ " + btcList.LastOrDefault().last);
                            // wallet.secondlastturn = wallet.lastturn;
                            wallet.lastturn = btcList.LastOrDefault();
                            wallet.percent = Percent;
                            Console.WriteLine(Percent + " %");
                            
                            if (Percent < 0.99 && wallet.dollars == 0 && Double.IsInfinity(Percent) == false )
                            {
                                Console.WriteLine("Buying dollars");
                                Console.WriteLine(Percent + " % ");

                                wallet.lastConjuction = "down";

                                // Conversion                                
                                wallet.dollars = btc.bid * wallet.bitCoin;
                                wallet.bitCoin = 0;

                            }


                        }
                        else if (btcList.LastOrDefault().last > btcList.ElementAt(btcList.Count - 2).last)
                        {

                            Console.WriteLine("Up " + UnixTimeStampToDateTime(btcList.LastOrDefault().timestamp));
                            Console.WriteLine("@ " + btcList.LastOrDefault().last);
                            // wallet.secondlastturn = wallet.lastturn ;
                            wallet.lastturn = btcList.LastOrDefault() ;

                            wallet.percent = Percent;
                            Console.WriteLine(Percent + " %");
                            
                            if (Percent > 1.01 && wallet.bitCoin == 0 && Double.IsInfinity(Percent) == false )
                            {
                                Console.WriteLine(Percent + " %");
                                Console.WriteLine("Buying BTC");
                                wallet.lastturn = btcList.LastOrDefault();
                                wallet.lastConjuction = "up";
                                wallet.percent = Percent;
                                // Conversion
                                wallet.bitCoin = wallet.dollars / btc.last;
                                wallet.dollars = 0;
                            }
                        }

                        // Database
                        Market market = new Market() {
                            BitValue = btc.last,
                            DateStamp = UnixTimeStampToDateTime(btc.timestamp)
                        };

                        WalletSet w = new WalletSet() { Bitcoin = wallet.bitCoin, BitInDollar = btc.last * wallet.bitCoin, Dollar = wallet.dollars, DateStamp = UnixTimeStampToDateTime(btc.timestamp) };
                        model1Container.MarketSet.Add(market);
                        model1Container.WalletSetSet.Add(w);
                        model1Container.SaveChanges();



                        // Init up data
                        if (loadupCounter < 2)                       
                            System.Threading.Thread.Sleep( 5 * 60 * 1000);
                        else
                            System.Threading.Thread.Sleep(60* 60 * 1000);
                        loadupCounter++;
                    }
                    catch (Exception e)
                    {

                    }
                    if (btcList.Count > 9)
                    {
                        btcList.RemoveAt(1);
                    }

                    Console.WriteLine("###############################################");
                    Console.WriteLine(wallet.ToString());
                    Console.WriteLine("_______________________________________________");
                    try
                    {

                        Console.WriteLine("BTC Value: " + btc.last);
                    }
                    catch (Exception e)
                    {

                    }
                } while (true);
            } 
        
        }

        public static DateTime UnixTimeStampToDateTime(double? unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            if (unixTimeStamp != null)
            dtDateTime = dtDateTime.AddSeconds((double)unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string json;

        static void Main(string[] args)
        {

            wallet.bitCoin = 1;

            // Some help for the user
            Console.WriteLine("Type command 'quit', to exit. Type 'value' to get some stats, ");
            Console.WriteLine("'wallet' to see current value of portfolio. The command 'help' will show this text.");


            t1.Start();
            

            string command;
            do {
                Console.WriteLine(" ------------------------------------------------------------------------ :");
                command = Console.ReadLine();
                if (command == "value")
                {
                    Console.WriteLine("Start value of BTC: " + wallet.startValue.last + " $");
                    Console.WriteLine("Current value of BTC: " + btcList.LastOrDefault().last + " $");
                    double percentValue = btcList.LastOrDefault().last / wallet.startValue.last;
                    Console.WriteLine("Value change since software started: " + ((percentValue) - 1) + "%");

                    if (wallet.dollars != 0)
                    {
                        Console.WriteLine("Value change on wallet: " + ((wallet.dollars) - 1) + "%");

                    }
                    else if (wallet.bitCoin != 0)
                    {
                        var t = wallet.dollars * btcList.LastOrDefault().last;
                        Console.WriteLine("Value change on wallet: " + ((wallet.bitCoin * Program.getBtcJson().last ) - 1) + "%");
                    }
                }
                else if (command == "wallet")
                {
                    Console.WriteLine(wallet.dollars + " $");
                    Console.WriteLine(getBtcJson().last * wallet.bitCoin + " BTC in $");
                }
                else if (command == "help")
                {
                    Console.WriteLine("Type command 'quit', to exit. Type 'value' to get some stats, ");
                    Console.WriteLine("'wallet' to see current value of portfolio. The command 'help' will show this text.");
                }
                else if (command == "thread")
                {
                    Console.WriteLine(t1.ThreadState);
                }
                else if (command == "clear")
                {
                    Console.Clear();
                }
            } while (command != "quit");
            t1.Abort();           
    }
    }
}
