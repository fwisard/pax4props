#define TRACE
using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Media;
//using System.Windows.Media;
using System.Diagnostics;
using System.Globalization;
using AutoUpdaterDotNET;
using System.Data.SQLite;

//using System.Diagnostics;


/*
 * 
 *  Hey, that's my first C# project, also first time using Visual Studio, so please
 *  don't judge my mess of a code too harshly ^^
 * 
 */

namespace pax4props
{
#pragma warning disable IDE1006 // Naming Styles
   public static class db
#pragma warning restore IDE1006 // Naming Styles
    {
        public static string SaveDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\pax4props";
#if DEBUG
        const string _name = "debug-log.db";
#else
        const string _name = "log.db";
#endif
        public static void Init()
        {
            if (! File.Exists($@"{SaveDir}\{_name}"))
            {
                Directory.CreateDirectory(SaveDir);
            }
        }
        public static SQLiteConnection conn = new SQLiteConnection($@"Data Source={SaveDir}\{_name};Compress=True;");

    }
    public static class Globals
    {
        public static bool IsNoiseEnabled = true;
        public static bool IsHypoxemiaEnabled = true;
        public static bool IsDebugEnabled = false;
    }
    public enum DUMMYENUM
    {
        Dummy = 0
    }

    public enum Ouch
    {
        What,
        Am,
        I,
        Even,
        Doing
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Contains the list of all the SimConnect properties we will read, the unit is separated by coma by our own code.
        /// </summary>
        readonly Dictionary<int, string> simConnectProperties = new Dictionary<int, string>
        {
            {1,"PLANE BANK DEGREES,degree" },
            {2,"PLANE PITCH DEGREES,degree" },
            {3,"VERTICAL SPEED,ft/min" },
            {4,"PLANE ALTITUDE,feet" },
            {5,"GROUND VELOCITY,knots" },
            {6,"PLANE TOUCHDOWN NORMAL VELOCITY,ft/min" },
            {7,"PROP MAX RPM PERCENT:1,percent" },
            {8,"PLANE ALT ABOVE GROUND,feet" },
            {9, "G FORCE,Gforce" },
            {10, "ACCELERATION BODY Y,Gforce" },
           // {11, "CRASH FLAG,Enum" },
          //  {12, "ON ANY RUNWAY,Bool" },


        };

        public Dictionary<string, float[]> ComplainingAbout = new Dictionary<string, float[]>
        {
            {"bank", new float[] { 0, 0 } },
            {"pitch", new float[] { 0, 0 } },
            {"climb", new float[] { 0, 0 } },
            {"descent", new float[] { 0, 0 } },
            {"low alt", new float[] { 0, 0 } },
            {"oxygen", new float[] { 0, 0 } },
            {"landing", new float[] { 0, 0 } },
            {"noise", new float[] { 0, 0 } },
            {"g-force", new float[] { 0, 0 } },
            {"bumps", new float[] { 0, 0 } },
        };

        readonly string[] GoodMood = { "happy.", "enthusiastic.", "fine.", "in a good mood.", "cheerful.", "calm.", "relaxed.", "lighthearted." };
        readonly string[] MehMood = { "bored.", "OK.", "neutral.", "absorbed.", "pensive.", "withdrawn.", "absent." };
        readonly string[] BadMood = { "tired.", "twitchy.", "angry.", "tense.", "worried.", "bothered.", "concerned." };
        public float MaxLandingFPM = 0;
        public float LandingFPM = 0;
        public float Bank = 0;
        public float Pitch = 0;
        public int Altitude = 0;
        public int AltAboveGround = 0;
        public float PropRPM = 0;
        public float GForce = 0;
        public bool OnAnyRunway = false;
        public float VerticalSpeed = 0;
        public float GroundSpeed = 0;
        public int AirTime = 0;
        public double MissionTime = 0;
        public float Discomfort = 2;
        public double VerticalAccel = 0;
        public double PrevVerticalAccel = 0;
        public bool FlightInProgress = false;
        public int PAX = 0;
        public int StressPeak = 0; // 
        public float StressCounter = 0;
        public string LatestStress = "";
        public string PAXStatusPrefix = "No passenger on board yet.";
        private string CurrentMood = "";
        private string CalmedMood = "";
        private double PaxFac = 0;
        private bool Reach1500 = false;
        private int AGLComfortAlt = 200;
        private int OxyLimit1 = 8000;
        private int OxyLimit2 = 10000;
        private int OxyLimit3 = 14000;
        private int VSDownLimit1 = -1050;
        private int VSDownLimit2 = -1600;
        private int VSUpLimit = 1250;
        private readonly string BaseDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private readonly MediaPlayer UISound = new MediaPlayer();
        private int Nausea = 0;
        private DateTime dtLastSound = DateTime.Now;
        private MediaPlayer CurrentSound = new MediaPlayer();
        private int NoiseRPMPercent = 98;

        //TODO: Simrates?

        enum Shout
        {
            Wow,
            Complain,
            Panic,
            Vomit
        }

        

        private readonly Dictionary<Enum, string> DictPrefix = new Dictionary<Enum, string>
        {
            {Shout.Wow, "wow" },
            {Shout.Complain, "complain" },
            {Shout.Panic, "panic" },
            {Shout.Vomit, "vomit" }
        };

        private readonly Dictionary<Enum, MediaPlayer[]> SoloSoundFiles = new Dictionary<Enum, MediaPlayer[]>
        {
            { Shout.Wow, new MediaPlayer[] {new MediaPlayer()} },
            { Shout.Complain, new MediaPlayer[] {
                new MediaPlayer(), new MediaPlayer(), new MediaPlayer(), new MediaPlayer(), new MediaPlayer(), new MediaPlayer()
            } },
            { Shout.Panic, new MediaPlayer[] {
                new MediaPlayer(), new MediaPlayer(), new MediaPlayer(), new MediaPlayer(), new MediaPlayer(), new MediaPlayer()

            } },
            { Shout.Vomit, new MediaPlayer[]
            {
                new MediaPlayer(), new MediaPlayer(), new MediaPlayer()
            } }
        };

        private readonly Dictionary<Enum, MediaPlayer> CrowdSoundFile = new Dictionary<Enum, MediaPlayer>
        {
            {Shout.Wow, new MediaPlayer() },
            {Shout.Complain, new MediaPlayer() },
            {Shout.Panic,new MediaPlayer() },
            {Shout.Vomit, new MediaPlayer() }
        };

       
        private string[] GetSoundName(Enum V)
        {         
            string[] ListAll = Directory.GetFiles($@"{BaseDir}\sounds", $"{DictPrefix[V]}1-*.wav");
            
            string[] Uniques = new string[] { "", "", "", "","","" };
            var Rand = new Random();
            string Test;

            switch (V)
            {
                case Shout.Wow:
                    Uniques[0] = ListAll[Rand.Next(ListAll.Length)]; // get random file
                    return Uniques;

                default:
                    for (var i = 0; i < Uniques.Length; i++)
                    {
                        do
                        {
                            Test = ListAll[Rand.Next(ListAll.Length)];
                        }
                        while (Uniques.Contains(Test));
                        // out of the loop means Test is a new file
                        Uniques[i] = Test;

                    }
                    break;
            }
            
            return Uniques;
        }

        /// User-defined win32 event => put basically any number?
        public const int WM_USER_SIMCONNECT = 0x0402;

        SimConnect sim;

        /// <summary>
        ///  Direct reference to the window pointer
        /// </summary>
        /// <returns></returns>
        protected HwndSource GetHWinSource()
        {
            return PresentationSource.FromVisual(this) as HwndSource;
        }

        
        public MainWindow()
        {
            InitializeComponent();

            // Starts our connection and poller
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick; ;
            timer.Start();
            
            try
            {
                db.Init();
                db.conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(db.conn);
                cmd.CommandText = @"create table if not exists Log (
                    TimeLog TEXT PRIMARY KEY NOT NULL,
                    Pax INT NOT NULL,
                    FlightType TEXT NOT NULL,
                    Score REAL NOT NULL,
                    Remarks TEXT,
                    BankS INT, BankP REAL, PitchS INT, PitchP REAL, ClimbS INT, ClimbP REAL,
                    DescentS INT, DescentP REAL, LowS INT, LowP REAL, OxygenS INT, OxygenP REAL,
                    LandingS INT, LandingP REAL, NoiseS INT, NoiseP REAL, GForceS INT, GForceP REAL,
                    BumpsS INT, BumpsP REAL,
                    MaxFpm REAL );";
                cmd.ExecuteScalar();
                cmd.CommandText = "select count(*) from Log;";
                Console.WriteLine($"Flights: {cmd.ExecuteScalar()}");
                db.conn.Close();
                Console.WriteLine("conn.close");
            }
            catch (Exception Ex)
            {
                System.Windows.MessageBox.Show(this, Ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            if (Properties.Settings.Default.Bush)
            {
                CbBush.IsChecked = true; 
                imgFlight.Source = new BitmapImage(new Uri($@"{BaseDir}\bushflight.png"));
            }
            else
            {
                CbRegular.IsChecked = true;
                imgFlight.Source = new BitmapImage(new Uri($@"{BaseDir}\regularflight.png"));
            }

            try
            {
                tbPAX.Text = Properties.Settings.Default.LastPAXNum;
            }
            catch (Exception Ex)
            {
                System.Windows.MessageBox.Show(this, Ex.Message);
            }
            
            BtnHelp.Content = " ? ";

            Uri TempUri = new Uri(GetSoundName(Shout.Wow)[0]);
            float _SoloVol = Properties.Settings.Default.SoloVolume / 100.0f;
            float _GroupVol = Properties.Settings.Default.GroupVolume / 100.0f;
            
            foreach (Enum e in DictPrefix.Keys) // load all reaction sounds
            {
                string[] TempNames = GetSoundName(e);
                for (var i = 0; i < SoloSoundFiles[e].Length; i++)
                {
                    SoloSoundFiles[e][i].Open(new Uri(TempNames[i]));
                    SoloSoundFiles[e][i].Volume = _SoloVol;
                }

            }

           
            foreach (var _k in DictPrefix.Keys)
            {
                string[] _ListAll = Directory.GetFiles($@"{BaseDir}\sounds", $"{DictPrefix[_k]}3-*.wav");
                CrowdSoundFile[_k].Open(new Uri(_ListAll[r.Next(_ListAll.Length)]));
                CrowdSoundFile[_k].Volume = _GroupVol;

            }
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            AutoUpdater.HttpUserAgent = "pax4props AutoUpdater";
            if (Properties.Settings.Default.CheckForUpdates.Equals(true))
            {
                AutoUpdater.Start("http://github.com/fwisard/pax4props/pax4props/updater.xml"); 
            }

#if DEBUG
            tbDebug.Visibility = Visibility.Visible;
            Globals.IsDebugEnabled = true;
#else
            tbDebug.Visibility = Visibility.Hidden;
#endif

        }

        private void PlayRandomVoice(Enum V)
        {
            var Rand = new Random();
            TimeSpan ts = DateTime.Now - dtLastSound;

            void _PlaySound(bool bCut = true)
            {
                var _m = SoloSoundFiles[V][Rand.Next(SoloSoundFiles[V].Length)];
                if (bCut)
                {
                    CurrentSound.Stop();
                    CurrentSound = _m;
                }
                _m.Play(); 
                
                dtLastSound = DateTime.Now;
            }

            if (V.Equals(Shout.Panic) )
            {
                if (Nausea < 10 || Rand.NextDouble() < 0.1)
                {
                    _PlaySound();
                }
            }
            else if (V.Equals(Shout.Vomit) && ts.TotalMilliseconds > 2100)
            {
                _PlaySound();
                Nausea /= 3;
            }
            else if (ts.TotalMilliseconds > 5100) // clearer than a complex IF statement with ORs
            {
                _PlaySound(); // Wow or complaint
            }
     
            if (PAX > 1)
            {
                if (Rand.Next(PAX * 10) > 12 && ts.TotalMilliseconds > 2000)
                {
                    _PlaySound(false);
                }
                if (PAX > 7 && ts.TotalMilliseconds > 5000)
                {
                    CrowdSoundFile[V].Play();
                }
            }
            
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            if (sim == null) // We are not connected, let's try to connect
                Connect();
            else // We are connected, let's try to grab the data from the Sim
            {
                if (! FlightInProgress)
                {
                    return; // not interested in anything since we're not officially flying
                }
                try
                {
                    foreach (var toConnect in simConnectProperties)
                        sim.RequestDataOnSimObjectType((DUMMYENUM)toConnect.Key, (DUMMYENUM)toConnect.Key, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
                }
                catch
                {
                    Disconnect();
                }
                if (StressCounter > 1)
                {
                    StressCounter--;
                    if (StressCounter > 300)
                    {
                        CurrentMood = "to be panicking!";
                    }
                    else if (StressCounter > 200)
                    {
                        CurrentMood = "extremely concerned.";
                    }
                    else if (StressCounter > 100)
                    {
                        CurrentMood = "quite anxious.";
                    }
                    else
                    {
                        CurrentMood = CalmedMood;
                    }

                }
                else if (StressCounter == 1) // the crisis has passed, back to a normal mood
                {
                    if (StressPeak == 2)
                    {
                        CurrentMood = BadMood[r.Next(0, BadMood.Length)];
                    }
                    else if (StressPeak == 1)
                    {
                        CurrentMood = MehMood[r.Next(0, MehMood.Length)];
                        Nausea /= 2;
                    }
                    else
                    {
                        CurrentMood = GoodMood[r.Next(0, GoodMood.Length)];
                        Nausea /= 4; // does high nausea AND low stress even happen?
                    }
                    StressCounter = -1;
                }
                tbPAXStatus.Text = PAXStatusPrefix + CurrentMood;
#if DEBUG
                tbDebug.Text = "";
                foreach (var key in ComplainingAbout.Keys)
                {
                    tbDebug.Text += $"{key}:{ComplainingAbout[key][1]:F1} ";
                }
                tbDebug.Text += $"\nStress:{StressCounter}, Discomfort:{Discomfort:F1}, Nausea:{Nausea}";
                
#endif
            }
        }

        /// <summary>
        /// We received a disconnection from SimConnect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void Sim_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            lblStatus.Content = "Disconnected";
        }

        /// <summary>
        /// We received a connection from SimConnect.
        /// Let's register all the properties we need.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void Sim_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            lblStatus.Content = "Connected";

            foreach (var toConnect in simConnectProperties)
            {
                var values = toConnect.Value.Split(new char[] { ',' });
                /// Define a data structure
                sim.AddToDataDefinition((DUMMYENUM)toConnect.Key, values[0], values[1], SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);

                /// IMPORTANT: Register it with the simconnect managed wrapper marshaller
                /// If you skip this step, you will only receive a uint in the .dwData field.
                sim.RegisterDataDefineStruct<double>((DUMMYENUM)toConnect.Key);
            }
        }

        private void Sim_OnRecvEvent(SimConnect sender, SIMCONNECT_RECV_EVENT WhatAmIDoing)
        {
            // crash detected...
            if (WhatAmIDoing.uEventID == 1) //can't believe it worked on the first try
            {
                btnFlight.Content = "Start flight";
                lblFlightStatus.Content = "Press \"Start flight\" when ready.";
                FlightInProgress = false;
                tbPAX.IsEnabled = true;
                SPnlFlightType.IsEnabled = true;
                StressCounter = 0;
                Discomfort = 0;
                tbPAXStatus.Text = "No passenger on board yet.";
                System.Windows.MessageBox.Show(this, "Crash detected. Your flight has been reset. Let's pretend it was just a bad dream.");
            }
        }
        /// <summary>
        /// Try to connect to the Sim, and in case of success register the hooks
        /// </summary>
        private void Connect()
        {
            /// The constructor is similar to SimConnect_Open in the native API
            try
            {
                // Pass the self defined ID which will be returned on WndProc
                
                sim = new SimConnect(this.Title, GetHWinSource().Handle, WM_USER_SIMCONNECT, null, 0);
                sim.OnRecvOpen += Sim_OnRecvOpen;
                sim.OnRecvQuit += Sim_OnRecvQuit;
                sim.OnRecvEvent += Sim_OnRecvEvent;
                sim.SubscribeToSystemEvent(Ouch.Am, "Crashed");
                sim.OnRecvSimobjectDataBytype += Sim_OnRecvSimobjectDataBytype;
            }
            catch
            {
                sim = null;
            }
        }

 
        /// <summary>
        /// Received data from SimConnect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        /// 
        private void Stress(float Seconds)
        {
            if (Seconds > StressCounter) // incoming stress is higher than current stress 
            {
                StressCounter = Seconds;
                if (Seconds >= 120 && Seconds <= 300) // moderate stress
                {
                    if (StressPeak < 1)
                    {
                        StressPeak = 1;
                    }
                    PlayRandomVoice(Shout.Complain);
                }
                else if (Seconds > 300) //panic
                {
                    if (StressPeak < 2)
                    {
                        StressPeak = 2;
                    
                    }
                    PlayRandomVoice(Shout.Panic);
                    /*
                    if (DateTime.UtcNow.Second % 2 != 0)
                    {
                        PlayRandomVoice(Shout.Panic);
                    }
                    */
                }
    }
            
        }
        
        private void Sim_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            int iRequest = (int)data.dwRequestID;
            double dValue = (double)data.dwData[0];
            
            switch (iRequest)
            {
                case 1: // bank angle
                    Bank = (float)Math.Abs(dValue);
                    if (Bank > 30)
                    {
                        float _dis = (float)PaxFac;
                        NauseaIncrease();
                        Stress(300);
                        if (Bank > 50)
                        {
                            _dis = 10.0f * (float)PaxFac;
                            Stress(310);
                            NauseaIncrease();
                        }
                        Discomfort += _dis;
                        ComplainingAbout["bank"][0] += 1;
                        ComplainingAbout["bank"][1] += _dis;
                    }

                    break;

                case 2: //pitch angle
                    Pitch = (float)Math.Abs(dValue);
                    if (Pitch > 20)
                    {
                        float _dis = (float)PaxFac;
                        NauseaIncrease();
                        Stress(300);
                        if (Pitch > 30)
                        {
                            _dis = 10.0f * (float)PaxFac ;
                            Stress(310);
                        }
                        Discomfort += _dis;
                        ComplainingAbout["pitch"][0] += 1;
                        ComplainingAbout["pitch"][1] += _dis;
                    }

                    break;

                case 3: // vertical speed
                    VerticalSpeed = (float)dValue;
                    if (VerticalSpeed <= VSUpLimit && VerticalSpeed >= VSDownLimit1)
                    {
                        break;
                    }
                    else
                    {
                        float _dis;
                        if (VerticalSpeed < VSDownLimit1)
                        {
                            if (VerticalSpeed < - 2500)
                            {
                                _dis = 5.0f * (float)PaxFac ; // < -2500
                                Stress(310);
                            }
                            else if (VerticalSpeed < VSDownLimit2)
                            {
                                 _dis = (float)PaxFac; // -1600 > vs > -2500
                                Stress(300);
                            }
                            else
                            {
                                _dis = 0.3f * (float)PaxFac ;  // -1050 > vs > -1600
                                Stress(150);
                            }
                            ComplainingAbout["descent"][0] += 1;
                            ComplainingAbout["descent"][1] += _dis;
                        }
                        else // vs > 1250
                        {
                            _dis = 0.2f * (float)PaxFac ;
                            ComplainingAbout["climb"][0] += 1;
                            ComplainingAbout["climb"][1] += _dis;
                            Stress(240);
                        }
                        Discomfort += _dis;
                        
                    }
                   
                    break;

                case 4: // AMSL
                    Altitude = (int)dValue;
                    if (Globals.IsHypoxemiaEnabled)
                    {
                        if (Altitude > OxyLimit1)
                        {
                            float _dis;
                            _dis = 0.0017f * (float)PaxFac; // 0.1 per min
                            if (Altitude > OxyLimit2)
                            {
                                Stress(140);
                                _dis = 0.1f * (float)PaxFac;
                                if (Altitude > OxyLimit3)
                                {
                                    _dis = 2.0f * (float)PaxFac;
                                    Stress(300);
                                    if (Altitude >= 18000)
                                    {
                                        _dis = 10.0f * (float)PaxFac;
                                        Stress(300); // "In space nobody can hear you scream..."
                                    }
                                }
                            }
                            Discomfort += _dis;
                            ComplainingAbout["oxygen"][0] += 1;
                            ComplainingAbout["oxygen"][1] += _dis;
                        }
                    }
                    break;

                case 5: // Ground speed
                    GroundSpeed = (float)dValue;
                    
                    break;

                case 6: // landing fpm
                    if (LandingFPM != (float)dValue) //new landing value
                    {
                        LandingFPM = (float)dValue;
                        float _dis;
                        ComplainingAbout["landing"][0] += 1; // landing AND bounces
                        if (LandingFPM > MaxLandingFPM)
                        {
                            MaxLandingFPM = LandingFPM;
                        }
                        
                        _dis = (LandingFPM / 10.0f);
                        if (LandingFPM > 200)
                        {
                            Stress(140);
                        }
                        Discomfort += _dis;
                        ComplainingAbout["landing"][1] += _dis;

                    }
                    break;

                case 7: // propeller rpm
                    PropRPM = (float)dValue;
                    if (PropRPM > NoiseRPMPercent)
                    {
                        float _dis = 0.017f * (float)PaxFac; // 1/min
                        Discomfort += _dis;
                        ComplainingAbout["noise"][0] += 1;
                        ComplainingAbout["noise"][1] += _dis;
                        Stress(60);
                    }
                    
                    break;

                case 8: // AGL
                    AltAboveGround = (int)dValue;                   
                    if (AltAboveGround < AGLComfortAlt)
                    {
                        if (GroundSpeed > 20 )
                        {

                            float _dis= 0.1f * (GroundSpeed / 30.0f);
                            Stress(90);
                            Discomfort += _dis;
                            ComplainingAbout["low alt"][0] += 1;
                            ComplainingAbout["low alt"][1] += _dis;
                        }
                        
                    }
                    else if (! Reach1500 && AltAboveGround > 1500)
                    {
                        Reach1500 = true;
                        Discomfort -= ((float) PaxFac -0.8f) * 10.0f;
                        PlayRandomVoice(Shout.Wow);
                    }
                    break;

                case 9: // G force
                    GForce = (float)dValue;

                    if (GForce > 1.35f || GForce < 0.65f)
                    {
                        float _dis = (float)PaxFac;
                        Discomfort += _dis;
                        ComplainingAbout["g-force"][0] += 1;
                        ComplainingAbout["g-force"][1] += _dis;
                        NauseaIncrease();
                        Stress(110);
                    }
                    break;

                case 10: // vertical accel
                    VerticalAccel = dValue;
                    if (Math.Abs(PrevVerticalAccel - VerticalAccel) > 0.3) 
                    {
                        float _dis = 0.1f * (float)PaxFac;
                        Discomfort += _dis;
                        ComplainingAbout["bumps"][0] += 1;
                        ComplainingAbout["bumps"][1] += _dis;
                        Stress(60);
                    }
                    PrevVerticalAccel = VerticalAccel;
                    break;
                
                case 11:// inop
                    break;

                case 12: //inop
                    
                    break;
                default:
                    break;
            }

            void NauseaIncrease()
            {
                Nausea++;
                if (Nausea > 3)
                {
                    if (r.Next(Nausea) > 2)
                    {
                        PlayRandomVoice(Shout.Vomit);
                    }
                }
            }
        }

        public void ReceiveSimConnectMessage()
        {
            sim?.ReceiveMessage();
        }

        /// <summary>
        /// Let's disconnect from SimConnect
        /// </summary>
        public void Disconnect()
        {
            if (sim != null)
            {
                sim.Dispose();
                sim = null;
                lblStatus.Content = "Disconnected";
                Console.WriteLine("--> Disconnected");
            }
        }

        /// <summary>
        /// Handles Windows events directly, for example to grab the SimConnect connection
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="iMsg"></param>
        /// <param name="hWParam"></param>
        /// <param name="hLParam"></param>
        /// <param name="bHandled"></param>
        /// <returns></returns>
        private IntPtr WndProc(IntPtr hWnd, int iMsg, IntPtr hWParam, IntPtr hLParam, ref bool bHandled)
        {
            try
            {
                if (iMsg == WM_USER_SIMCONNECT)
                    ReceiveSimConnectMessage();
            }
            catch
            {
                Disconnect();
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Once the window is loaded, let's hook to the WinProc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var windowsSource = GetHWinSource();
            windowsSource.AddHook(WndProc);

            Connect();
        }

        /// <summary>
        /// Called while the window is closed, dispose SimConnect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }

        readonly Random r = new Random();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PAX = Int32.Parse(tbPAX.Text);
            }
            catch (Exception)
            {
                PAX = 0;
            }
            if (PAX < 1 || PAX > 50) // passengers limit?
            {
                MessageBox.Show(this, "Not a valid number of passengers.\nPlease enter a number between 1 and 50");
                return;
            }
            else if (PAX == 1)
            {
                PAXStatusPrefix = "Your passenger seems ";
                PaxFac = 0.9;
            }
            else
            {
                PAXStatusPrefix = "Your passengers seem ";
                if (PAX < 4)
                { 
                    PaxFac = 1;
                }
                else if (PAX < 10)
                {
                    PaxFac = 1.1;
                }
                else
                {
                    PaxFac = 1.2;
                }
            }
            if (FlightInProgress) // stop the flight
            {
#if !DEBUG
                if (MaxLandingFPM < 0)
                {
                    System.Windows.MessageBox.Show(this, "Flight aborted.", "Alert",MessageBoxButton.OK,MessageBoxImage.Stop);
                }
                else
#endif
                {
                    // show flight report
                    UISound.Open(new Uri(BaseDir + "\\sounds\\printer.wav"));
                    UISound.Play();
                    FlightReport Report = new FlightReport(PAX, Discomfort, ComplainingAbout, MaxLandingFPM);
                    Report.ShowDialog();
                }
                btnFlight.Content = "Start flight";
                lblFlightStatus.Content = "Press \"Start flight\" when ready";
                FlightInProgress = false;
                tbPAX.IsEnabled = true;
                SPnlFlightType.IsEnabled = true;
                tbPAXStatus.Text = "No passenger on board.";

            }
            else //start flight
            {
                MaxLandingFPM = -1;
                UISound.Open(new Uri(BaseDir + "\\sounds\\beltclick.wav"));
                UISound.Play();
                Nausea = 0;
                StressCounter = 0;
                SPnlFlightType.IsEnabled = false;
                if (Globals.IsNoiseEnabled)
                {
                    NoiseRPMPercent = 98;
                }
                else
                {
                    NoiseRPMPercent = 9999;
                }
                if (Globals.IsDebugEnabled)
                {
                    tbDebug.Visibility = Visibility.Visible;
                }
                else
                {
                    tbDebug.Visibility = Visibility.Hidden;
                }
                if ((bool) CbBush.IsChecked)
                {
                    Properties.Settings.Default.Bush = true;
                    AGLComfortAlt = 50;
                    OxyLimit1 = 12000;
                    OxyLimit2 = 14000;
                    OxyLimit3 = 15500; // barely 1000ft over the highest airport in the world
                    VSUpLimit = 1400;
                    VSDownLimit1 = -1400;
                    VSDownLimit2 = -2000;
                    Discomfort = 15;
                }
                else
                {
                    Properties.Settings.Default.Bush = false;
                    AGLComfortAlt = 200;
                    OxyLimit1 = 8000;
                    OxyLimit2 = 10000;
                    OxyLimit3 = 14000;
                    VSUpLimit = 1250;
                    VSDownLimit1 = -1050;
                    VSDownLimit2 = -1600;
                    Discomfort = 0;
                }
                Properties.Settings.Default.LastPAXNum = PAX.ToString();
                CurrentMood = GoodMood[r.Next(0, GoodMood.Length)];
                CalmedMood = MehMood[r.Next(0, MehMood.Length)];
                tbPAXStatus.Text = PAXStatusPrefix + CurrentMood;
                tbPAX.IsEnabled = false;
                foreach (var key in ComplainingAbout.Keys.ToList())
                {
                    ComplainingAbout[key][0] = 0;
                    ComplainingAbout[key][1] = 0;
                }
                lblFlightStatus.Content = "Flight in progress...";
                btnFlight.Content = "End flight";
                FlightInProgress = true;
                Reach1500 = false;
            }
            
            //sim.AddToDataDefinition((DUMMYENUM)12, "DESIGN SPEED VS0", "knots", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);

            /// IMPORTANT: Register it with the simconnect managed wrapper marshaller
            /// If you skip this step, you will only receive a uint in the .dwData field.
           // sim.RegisterDataDefineStruct<double>((DUMMYENUM)12);
            //sim.RequestDataOnSimObjectType((DUMMYENUM)12, (DUMMYENUM)12, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            //PrintIssues(r.Next().ToString());
           // PrintIssues("vs0 " + VS0 + " Vapp " + VApp);
        }


   
        

     
        private void Window_Deactivated(object sender, EventArgs e)
        {
            /*if ((bool)cbAlwaysOnTop.IsChecked)
            {
                Window window = (Window)sender;
                window.Topmost = true;
            }
            else
            {
                Window window = (Window)sender;
                window.Topmost = false;
            }*/
            return;
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpAbout HelpAbout = new HelpAbout();
            HelpAbout.ShowDialog();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
#if DEBUG

            PlayRandomVoice(Shout.Complain);
#endif
        }

        private void Image_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
            PlayRandomVoice(Shout.Panic);
#endif
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                Process.Start("readme.html"); // TODO: safer way to show help
            }
        }

        private void CbRegular_Checked(object sender, RoutedEventArgs e)
        {
            imgFlight.Source = new BitmapImage(new Uri($@"{BaseDir}\regularflight.png"));
        }

        private void CbBush_Checked(object sender, RoutedEventArgs e)
        {
            imgFlight.Source = new BitmapImage(new Uri($@"{BaseDir}\bushflight.png"));
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            LogAndStats LogAndStats = new LogAndStats();
            LogAndStats.Show();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings Settings = new Settings();
            Settings.ShowDialog();
            
        }
    }
}
