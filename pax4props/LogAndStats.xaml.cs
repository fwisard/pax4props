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
using System.Data;

namespace pax4props
{
    /// <summary>
    /// Interaction logic for LogAndStats.xaml
    /// </summary>
    public partial class LogAndStats : Window
    {
        public LogAndStats()
        {
            InitializeComponent();
            UpdateLogView();
            UpdateStats();
        }

        private void UpdateStats()
        {
            db.conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("select Score from Log where Score = (select max(Score) from Log)", db.conn);
            var _res = cmd.ExecuteScalar();
            lblBScore.Content = $"{_res:F1}";

            cmd.CommandText = "select MaxFpm from Log where MaxFpm = (select min(MaxFpm) from Log)";
            _res = cmd.ExecuteScalar();
            lblBLanding.Content = $"{_res:F1}";

            cmd.CommandText = "select sum(Pax) from Log";
            _res = cmd.ExecuteScalar();
            int _pax;
            int _satisfied;
            float _pct;
            try
            {
                _pax = Convert.ToInt32(_res);
            }
            catch (InvalidCastException)
            {
                _pax = 0;
            }

            cmd.CommandText = "select sum(Pax) from Log where Score > 75";
            _res = cmd.ExecuteScalar();
            try
            {
                _satisfied = Convert.ToInt32(_res);
                _pct = (float)_satisfied / (float)_pax * 100.0f;
            }
            catch (InvalidCastException)
            {
                _satisfied = 0;
                _pct = 0.0f;
            }
            lblSatisfied.Content = $"{_satisfied}/{_pax} ({_pct:F1}%)";

            db.conn.Close();
        }
        private void UpdateLogView()
        {
            db.conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("select rowid,* from Log order by rowid desc", db.conn);
            SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataGrid1.ItemsSource = ds.Tables[0].DefaultView;
            }
            db.conn.Close();
        }
    }
}
