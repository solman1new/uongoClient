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
    public partial class AddRowObruch : Form
    {
        DBUtils db = null;
        string dbCurrent = null;

        public AddRowObruch(DBUtils db, string dbCurrent)
        {
            this.db = db;
            this.dbCurrent = dbCurrent;
            InitializeComponent();
        }

        public AddRowObruch()
        {
            InitializeComponent();
        }

        private void AddRowObruch_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string name = this.NameTextBox.Text.Trim();
            string address = this.AddressTextBox.Text.Trim();
            string city = this.CityTextBox.Text.Trim();
            string phone = this.PhoneTextBox.Text.Trim();

            if(name.Equals("") || address.Equals("") || city.Equals("") || phone.Equals("")) {
                MessageBox.Show("Не все поля заполнены");
                
            } else
            {
                if(db != null)
                {
                    string sql = "INSERT INTO `obruch`(name, address, city, phone) " +
                        "VALUES('" + name + "', '" + address + "', '" + city + "', '" + phone + "')";
                    int count = db.ExNonQuery(sql, dbCurrent);
                    if (count > 0)
                        MessageBox.Show("Успешно");
                    else MessageBox.Show("Не удалось добавить");

                } else
                {
                    MessageBox.Show("Нужно подключиться к базе");
                }
            }
        }
    }
}
