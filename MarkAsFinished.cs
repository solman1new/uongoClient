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
    public partial class MarkAsFinished : Form
    {
        DBUtils db = null;

        public MarkAsFinished(DBUtils db)
        {
            this.db = db;
            InitializeComponent();
        }

        public MarkAsFinished()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            if (this.textBox1.Text.Trim().Equals(""))
            {
                MessageBox.Show("Не все поля заполнены");

            }
            else
            {
                if (db != null)
                {
                    string sql = "UPDATE `deal` SET `end`=1 WHERE `id`=" + this.textBox1.Text.Trim();
                    int count = db.ExNonQuery(sql, "deal");
                    if (count > 0)
                        MessageBox.Show("Успешно");
                    else MessageBox.Show("Не удалось изменить");

                }
                else
                {
                    MessageBox.Show("Нужно подключиться к базе");
                }
            }
        }
    }
}
