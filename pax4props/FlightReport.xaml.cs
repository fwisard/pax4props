using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace pax4props
{
    /// <summary>
    /// Interaction logic for FlightReport.xaml
    /// </summary>
    public partial class FlightReport : Window
    {
        public string[] Summary = { "ERROR", "Unprofessional", "Uncomfortable", "OK", "Good", "Excellent"};
        public int NumberOfStars = 0;
        private readonly Random Rnd = new Random();

        private readonly string[] BadReview =
        { 
            "Who gave that pilot a license?", "Unacceptable. Will never fly with them ever again.", "I was scared the whole flight.",
            "My worst experience ever.", "Incompetent pilot!", "Don't recommend this company.", "Disappointed.", "Awful, awful, awful.",
            "A real nightmare.", "I want to file a formal complaint.", "My lawyer will contact you.", "I soiled myself.",
            "I want to talk to your manager.", "No.", "Thumbs down.", "Avoid this company at all cost.", "They just suck.", 
            "Feel bad but can't honestly recommend.", "My blind grandma is a better pilot.", "AAAaaaaaaargh!", "This flight gave me PTSD",
            "I can't believe I'm still alive!", "I need therapy.", "No. Flying. Skill. Whatsoever.", "Terrible experience.", "Not reliable.",
            "Do not even thinking about booking with them. Just don't.", "Never again.", "Only use if you really have no alternative.",
            "Very poor.","Below expectations", "An absolute joke.", "Not worth it.", "Horrible!", "Stressful.", "Definitely not a fan.",
            "Would rather walk", "Go anywhere else.", "Seriously?", "Fail.", "Hated every second of it", "Catastrophic flight", 
            "would rather be eaten by zombies", "Wish I'd stay home", "Bad idea", "NOPE", "Never been so happy to leave a negative review",
            "so bad it's probably criminal", "I'd felt safer juggling dynamite in a volcano pit", "I'm gonna sue them!", "Bloodcurling experience",
            "liars and frauds", "any good review you read about them is a lie", "won't trust them ever again", "can't find worse",
            "I WANT TO SPEAK TO YOUR MANAGER", "zero stars or even less", "so glad I'm outta that thing", "They're bad. Like REALLY bad.",
            "Failure is only a stepping stone to improvement.", "They don't seem to care for their passsengers", "I'm outraged. Seriously.",
            "I will not be flying with them anymore", "That wasn't the worst ever, but that was bad.", "Ouch. That was awful.",
            "How is the plane still in one piece? ", "Someone please call the police.",
        };

        private readonly string[] OKReview =
        {
            "Whatever.", "Meh.", "I was unimpressed.", "The view was nice.", "Not my worst flight, not my best either.", "A few scares here and there.",
            "Might fly with them again...", "Well, it might have been the weather, but it was kind of rough tbh.", "Not bad.", 
            "You can't always have the best I guess.", "Not what I expected.", "An honest company.", "Average flight.", "Huh... OK?", "No problems.",
            "Acceptable travel.", "Ordinary flight.", "Pleasant but not perfect.", "A mixed experience.", "Not thrilled.", "Could have been worse.",
            "Fine.", "Not actually as bad as feared.", "Nothing special.", "Uneventful.", "Typical flight.", "Better than walking, I guess.",
            "Kind of average.", "No probs.", "Sure, why not.", "Maybe.", "Ask someone else.", "Nothing extraordinary.", "I wish they had snacks",
            "I'm bored", "Slightly scary at times", "not the most peaceful flight", "tiring but not in the worst way", "OK-ish", "I had a flight",
            "Nothing to say", "thank you I guess", "see you next time maybe", "a bit cramped but otherwise ok", "I don't know what to tell you",
            "Hmmmm...", "jets are faster", "I've seen better.", "What does PAX mean, anyway?", "Not great, not terrible.","It will have to do.", "You did the job.",

        };

        private readonly string[] GoodReview =
        {
            "Best flight of my life.", "Great company.", "Nice plane, nice pilot, great skills.", "Strongly recommend.", "YES!", "Superb.",
            "I had a great time flying with them.", "Wow.", "Perfection", "I liked it a lot.", "Very professional.", "My apprehension was far from justified.",
            "That was a smooth ride.", "They sure know what they do.", "Blissful.", "A very pleasing adventure overall.", "Thank you for a job well done.",
            "You can trust them.", "That was cool.", "Great time.", "Best pilot ever.", "See you soon.", "Can't wait to fly with them again.",
            "If flying were an olympic sport, this pilot would get a few gold medals.", "Thumbs up!", "Keep up the good work.", "Can't recommend enough.",
            "It's actually good.", "These guys are awesome.", "Outstanding service.", "Friendly and competent.", "Will use again.", "Highly recommended.",
            "Great flight.", "Fun and cozy.", "Elite skills.", "Good flight.", "They're so good!", "Amazing.", "Very nice travel.",
            "Wonderful flight!", "Glad I chose them.", "They are my preferred ones.", "Everything went well", "Thank you so much", "Great plane, great company",
            "First choice", "Wish all my flights were this good", "safe, comfy flight", "So glad I flew with you", "No regrets", "a smile on my face the whole time",
            "I want no one else", "I will never fly with another company ever again", "the absolute best!", "very very good", "nice trip", "best company in the world",
            "A+", "5 stars aren't enough, they need a few more", "More comfortable than any jet I've been in", "The pilot took off and landed like it was the easiest thing in the world",
            "I'm your number one fan", "Excellent indeed.","Wow, what a great flight.", "That was a perfect flight.", "You're the best.", "Smooth as butter.",
            "Amazing job!"


        };

        private readonly string[] BadLow =
        {
            "I thought we were going to crash into a tree or something.", "Are you supposed to fly that low for that long?", 
            "This pilot sure loves to fly close to the ground.", "I don't like flying so close to the ground.", "Is that pilot afraid of heights?",
            "I don't think it's safe to spend so much time at very low altitudes.", "We almost hit a power line.", "I hate seeing the ground going by so fast",
            "too low for too long", "flying low scares me", "I still shiver when I think how close to the terrain we were."
        };

        private readonly string[] BadLanding =
        {
            "Landing was too rough.", "We hit the ground HARD.", "Was it a landing or a crash?", "Not a smooth landing.", "Worst landing ever.",
            "The pilot sure was eager to land.", "Hard landing.", "So glad we survived that landing.", "More a fall than a landing.", "Scary landing.",
            "I hurt myself during landing.", "Fell like a stone.", "Runway hit us hard.", "Unprofessional landing.", "Landed like a brick would."
        };

        private readonly string[] BadNoise =
        {
            "It was way too loud.", "Not comfortable AT ALL", "What a racket!", "Unbearingly noisy", "My ears still hurt from all that noise", "Are they supposed to be that loud?",
            "Deafening", "Oh that noise was something", "Ears still ringing", "Pretty sure this flight permanently damaged my hearing.", "My ears!", "Too noisy",
            "Very loud environment", "The noise was very uncomfortable", "SO LOUD", "high noise levels", "louder than any concert I've been to", 
            "Wish I had some kind of hearing protection.", "My ears are buzzing", "I have tinnitus now"
        };

        
        private string Star(int Num)
        {
            switch (Num)
            {
                case 1:
                    return "★☆☆☆☆";

                case 2:
                    return "★★☆☆☆";

                case 3:
                    return "★★★☆☆";
                case 4:
                    return "★★★★☆";
                default:
                    return "★★★★★";
            }
        }
        private string Signature()
        {
            //65-90
            return "\n\t -- "+(char)Rnd.Next(65, 91) + ". from " + (char)Rnd.Next(65, 91) + ".\n";
        }
        public FlightReport(int PAX, float Discomfort, Dictionary<string,float[]> ComplainingAbout, float MaxLandingFPM)
        {
            InitializeComponent();
            tbQuotes.Text = "";
            tbPAX.Text = PAX.ToString();


            if (Properties.Settings.Default.Bush)
            {
                tbFlightType.Text = "BUSH";
            }
            else
            {
                tbFlightType.Text = "REGULAR";
            }

            lblFlightReportNr.Content = $"#{Properties.Settings.Default.FlightReportNumber:D8}";
            Properties.Settings.Default.FlightReportNumber++;
            Properties.Settings.Default.Save();

#if DEBUG
            if (Discomfort == 0)
            {
                Discomfort = Rnd.Next(20, 70);
                if (Discomfort > 40)
                {
                    ComplainingAbout["g-force"][0] = Rnd.Next(30);
                    ComplainingAbout["g-force"][1] = ComplainingAbout["g-force"][0] * 0.5f;
                    ComplainingAbout["low alt"][0] = Rnd.Next(120);
                    ComplainingAbout["low alt"][1] = Rnd.Next(14,30);
                    ComplainingAbout["descent"][0] = Rnd.Next(10);
                    ComplainingAbout["descent"][1] = ComplainingAbout["descent"][0];
                    MaxLandingFPM = Rnd.Next(250, 500);
                }
                ComplainingAbout["noise"][0] = Rnd.Next(255);
                ComplainingAbout["noise"][1] = 0.017f * ComplainingAbout["noise"][0];
                MaxLandingFPM = Rnd.Next(55, 200);
                ComplainingAbout["landing"][0] = Rnd.Next(1, 4);
                ComplainingAbout["landing"][1] = MaxLandingFPM / 10.0f; 
                if (ComplainingAbout["landing"][0] > 1)
                {
                    ComplainingAbout["landing"][1] *= ComplainingAbout["landing"][0] * 0.7f;
                }
            }
#endif
            if (Discomfort <= 30)
            {
                NumberOfStars = 5;
            }
            else if (Discomfort <= 40)
            {
                NumberOfStars = 4;
            }
            else if (Discomfort <= 55 )
            {
                NumberOfStars = 3;
            }
            else if (Discomfort <= 65 )
            {
                NumberOfStars = 2;
            }
            else
            {
                NumberOfStars = 1;
            }
            tbStars.Text = Star(NumberOfStars);
            RotateTransform RTStars = new RotateTransform();
            tbStars.RenderTransform = RTStars;
            RTStars.Angle = (Rnd.NextDouble() * 14.0) - 7.0;
            tbSummary.Text = string.Format("{0} (score: {1:F1})", Summary[NumberOfStars],125 - Discomfort);

            //tbReportDetails.Text = Intro[NumberOfStars][Rnd.Next(1, Intro[NumberOfStars].Length)];
            tbReportDetails.Text = "";
            if (ComplainingAbout["bank"][0] > 0)
            {
                tbReportDetails.Text += $"Watch your bank angle: you spent {ComplainingAbout["bank"][0]}s in steep turn";
                tbReportDetails.Text += $" (-{ComplainingAbout["bank"][1]:F1} pts).\n\n";
            }
            if (ComplainingAbout["pitch"][0] > 0)
            {
                tbReportDetails.Text += $"Don't pitch that much (you spent {ComplainingAbout["pitch"][0]}s with a steep attitude)";
                tbReportDetails.Text += $" (-{ComplainingAbout["pitch"][1]:F1} pts).\n\n";
            }
            if (ComplainingAbout["climb"][0] > 0)
            {
                tbReportDetails.Text += $"When you climb too fast, your passengers may feel unwell (you spent {ComplainingAbout["climb"][0]}s in steep climb)";
                tbReportDetails.Text += $" (-{ComplainingAbout["climb"][1]:F1} pts).\n\n";
            }
            if (ComplainingAbout["descent"][0] > 0)
            {
                tbReportDetails.Text += $"Fast descent rates scare your passengers and may cause discomfort (you spent {ComplainingAbout["descent"][0]}s in steep descent";
                tbReportDetails.Text += $" (-{ComplainingAbout["descent"][1]:F1} pts).\n\n";
            }
            if (ComplainingAbout["oxygen"][0] > 0)
            {
                tbReportDetails.Text += $"Your aircraft is unpressurized, so don't fly too high. You spent {ComplainingAbout["oxygen"][0]}s in low oxygen zones";
                tbReportDetails.Text += $" (-{ComplainingAbout["oxygen"][1]:F1} pts).\n\n";
            }
            if (ComplainingAbout["noise"][0] > 180)
            {
                tbReportDetails.Text += $"Propellers running at high speeds are very noisy and may hurt your PAX's ears. You spent {ComplainingAbout["noise"][0]/60.0:F1} minutes at high noise levels";
                tbReportDetails.Text += $" (-{ComplainingAbout["noise"][1]:F1} pts).\n\n";
            }
            /*if (ComplainingAbout["braking"] > 0)
            {
                tbReportDetails.Text += $"For your passengers' comfort, brake carefully. ";
            }*/
            if ((ComplainingAbout["g-force"][1] + ComplainingAbout["bumps"][1]) > 1.0f)
            {
                tbReportDetails.Text += $"PAX want smooth rides but you spent {ComplainingAbout["g-force"][0] + ComplainingAbout["bumps"][0] }s in turbulence or rough maneuvers";
                tbReportDetails.Text += $" (-{ComplainingAbout["g-force"][1] + ComplainingAbout["bumps"][1]:F1} pts).\n\n";
            }
            if (ComplainingAbout["low alt"][1] > 14.0f) // check limit
            {
                tbReportDetails.Text += $"Fast and low (or on the ground) is scary and you spent {ComplainingAbout["low alt"][0]/60.0:F1}min in that situation";
                tbReportDetails.Text += $" (-{ComplainingAbout["low alt"][1]:F1} pts).\n\n";
            }

            tbReportDetails.Text += $"You landed at {MaxLandingFPM:F1}fpm ";
            if (ComplainingAbout["landing"][0] > 1)
            {
                tbReportDetails.Text += $"but bounced {ComplainingAbout["landing"][0] - 1} time(s)";
            }
            tbReportDetails.Text += $" (-{ComplainingAbout["landing"][1]:F1} pts).\n\n";

            var Num = 1; // at least one review
            for (int i = 1; i < PAX; i++) 
            {
                if (Rnd.Next(100) < 25) // maybe more reviews
                {
                    Num++;
                }
            }
            if (Num > 3) // max 3 reviews
            {
                Num = 3;
            }

            if (NumberOfStars > 3)
            {
                for (int i = 0; i < Num; i++)
                {
                    tbQuotes.Text += GoodReview[Rnd.Next(0, GoodReview.Length)] + Signature() + "\n";
                }
            }
            else if (NumberOfStars < 3)
            {
                for (int i = 0; i < Num; i++)
                {
                    
                    if (ComplainingAbout["low alt"][1] > 14.0f && Rnd.Next(100) < 15) 
                    {
                        tbQuotes.Text += BadLow[Rnd.Next(0, BadLow.Length)];
                    }
                    else if (MaxLandingFPM > 300 && Rnd.Next(100) < 15)
                    {
                        tbQuotes.Text += BadLanding[Rnd.Next(0, BadLanding.Length)];
                    }
                    else if (ComplainingAbout["noise"][0] > 240 && Discomfort - ComplainingAbout["noise"][1] < 25) //240 instead of 300 because some PAX have lower limits than others.
                    {
                        tbQuotes.Text += BadNoise[Rnd.Next(0, BadNoise.Length)];
                    }
                    else
                    {
                        tbQuotes.Text += BadReview[Rnd.Next(0, BadReview.Length)];
                    }
                    tbQuotes.Text += Signature() + "\n";

                }
            }
            else // 3 stars, meh
            {
                tbQuotes.Text += OKReview[Rnd.Next(0, OKReview.Length)] + Signature() + "\n";
                if (Num > 1 && Rnd.Next(100) <50)
                {
                    if (ComplainingAbout["low alt"][1] > 14.0f && Rnd.Next(100) < 25)
                    {
                        tbQuotes.Text += BadLow[Rnd.Next(0, BadLow.Length)];
                    }
                    else if (MaxLandingFPM > 300 && Rnd.Next(100) < 55)
                    {
                        tbQuotes.Text += BadLanding[Rnd.Next(0, BadLanding.Length)];
                    }
                    else if (ComplainingAbout["noise"][0] > 280 && Rnd.Next(100) < 40)
                    {
                        tbQuotes.Text += BadNoise[Rnd.Next(0, BadNoise.Length)];
                    }
                    else
                    {
                        tbQuotes.Text = OKReview[Rnd.Next(0, OKReview.Length)];
                    }
                    tbQuotes.Text += Signature() + "\n";
                }
            }
            try // log the flight
            {
                db.conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(db.conn);
                cmd.CommandText = @"insert into Log (TimeLog, Pax, FlightType, Score, Banks, Bankp,
                    PitchS, PitchP, ClimbS, ClimbP, DescentS, DescentP, LowS, LowP, OxygenS, OxygenP, LandingS, LandingP,
                    NoiseS, NoiseP, GForceS, GForceP, BumpsS, BumpsP, MaxFpm) values (datetime('now', 'localtime'),@pax, @ft, @sc, 
                    @bks, @bkp, @ptcs, @ptcp, @clbs, @clbp, @dss, @dsp, @las, @lap, @os, @op, @lds, @ldp,
                    @nss, @nsp, @gfs, @gfp, @bms, @bmp, @max)";
                cmd.Parameters.AddWithValue("pax", PAX);
                cmd.Parameters.AddWithValue("ft", tbFlightType.Text);
                cmd.Parameters.AddWithValue("sc", 125.0f - Discomfort); // score
                cmd.Parameters.AddWithValue("bks", ComplainingAbout["bank"][0]);
                cmd.Parameters.AddWithValue("bkp", ComplainingAbout["bank"][1]);
                cmd.Parameters.AddWithValue("ptcs", ComplainingAbout["pitch"][0]);
                cmd.Parameters.AddWithValue("ptcp", ComplainingAbout["pitch"][1]);
                cmd.Parameters.AddWithValue("clbs", ComplainingAbout["climb"][0]);
                cmd.Parameters.AddWithValue("clbp", ComplainingAbout["climb"][1]);
                cmd.Parameters.AddWithValue("dss", ComplainingAbout["descent"][0]);
                cmd.Parameters.AddWithValue("dsp", ComplainingAbout["descent"][1]);
                cmd.Parameters.AddWithValue("las", ComplainingAbout["low alt"][0]);
                cmd.Parameters.AddWithValue("lap", ComplainingAbout["low alt"][1]);
                cmd.Parameters.AddWithValue("os", ComplainingAbout["oxygen"][0]);
                cmd.Parameters.AddWithValue("op", ComplainingAbout["oxygen"][1]);
                cmd.Parameters.AddWithValue("lds", ComplainingAbout["landing"][0]);
                cmd.Parameters.AddWithValue("ldp", ComplainingAbout["landing"][1]);
                cmd.Parameters.AddWithValue("nss", ComplainingAbout["noise"][0]);
                cmd.Parameters.AddWithValue("nsp", ComplainingAbout["noise"][1]);
                cmd.Parameters.AddWithValue("gfs", ComplainingAbout["g-force"][0]);
                cmd.Parameters.AddWithValue("gfp", ComplainingAbout["g-force"][1]);
                cmd.Parameters.AddWithValue("bms", ComplainingAbout["bumps"][0]);
                cmd.Parameters.AddWithValue("bmp", ComplainingAbout["bumps"][1]);
                cmd.Parameters.AddWithValue("max", MaxLandingFPM);
                
                cmd.ExecuteNonQuery();

                db.conn.Close(); //TODO: add profile name to db & prefs

            }
            catch (Exception Ex)
            {
                System.Windows.MessageBox.Show(this, Ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (tbxRemarks.Text.Length > 0)
            {
                db.conn.Open();
                SQLiteCommand cmd = new SQLiteCommand("update Log set Remarks = @rem where rowid = (select max(rowid) from Log)", db.conn);
                cmd.Parameters.AddWithValue("rem", tbxRemarks.Text);
                cmd.ExecuteNonQuery();
                db.conn.Close();
            }
        }
    }
}
