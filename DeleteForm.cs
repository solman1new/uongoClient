using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace uongoClient
{
    public partial class DeleteForm : Form
    {
        DBUtils db = null;
        string dbCurrent = null;

        public DeleteForm(DBUtils db, string dbCurrent)
        {
            this.db = db;
            this.dbCurrent = dbCurrent;
            InitializeComponent();
        }

        public DeleteForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(this.textBox1.Text.Trim()); 

                if (db != null)
                {
                    if (dbCurrent != null || dbCurrent != "")
                    {
                        string sql = "DELETE FROM `" + dbCurrent + "` WHERE `" + dbCurrent +"`.`id`=" + id.ToString();
                        int count = db.ExNonQuery(sql, dbCurrent);
                        if (count > 0)
                            MessageBox.Show("Успешно");
                        else MessageBox.Show("Возможно, такого id не существует");
                    } else
                    {
                        MessageBox.Show("Нужно выбрать бд");
                    }
                } else
                {
                    MessageBox.Show("Нет подключения к бд");
                }
            } catch(FormatException exc)
            {
                MessageBox.Show("Неверный формат");
            }
        }
    }
}
