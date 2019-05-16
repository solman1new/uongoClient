using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uongoClient
{
    public partial class AddRowTimes : Form
    {
        DBUtils db = null;
        string dbCurrent = null;

        public AddRowTimes(DBUtils db, string dbCurrent)
        {
            this.db = db;
            this.dbCurrent = dbCurrent;
            InitializeComponent();
        }

        public AddRowTimes()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void AddRowTimes_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int type = Convert.ToInt32(this.comboBox1.SelectedIndex);
                int year = Convert.ToInt32(this.YearTextBox.Text.Trim());
                int month = Convert.ToInt32(this.MonthTextBox.Text.Trim());
                int day = Convert.ToInt32(this.DayTextBox.Text.Trim());
                int hours = Convert.ToInt32(this.HoursTextBox.Text.Trim());
                int minuts = Convert.ToInt32(this.MinuntsTextBox.Text.Trim());

                string result = year.ToString();


                if (month < 10)
                    result += "-0" + month.ToString();
                else result += "-" + month.ToString();

                if (day < 10)
                    result += "-0" + day.ToString();
                else result += "-" + day.ToString();

                if (hours < 10)
                    result += " 0" + hours.ToString();
                else result += " " + hours.ToString();

                if (minuts < 10)
                    result += ":0" + minuts.ToString();
                else result += ":" + hours.ToString();

                result += ":00";

                if (db != null)
                {
                    string sql = "INSERT INTO `times`(dt, used, type) " +
                        "VALUES('" + result +"', '0', '" + type + "')";
                    int count = db.ExNonQuery(sql, dbCurrent);
                    if (count > 0)
                        MessageBox.Show("Успешно");
                    else MessageBox.Show("Не удалось добавить");

                }
                else
                {
                    MessageBox.Show("Нужно подключиться к базе");
                }

            }
            catch(FormatException exc)
            {
                MessageBox.Show("Ошибка формата");
            }
        }
    }
}
