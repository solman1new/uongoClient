using System;
using System.IO;
using System.Collections;
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
    public partial class Form1 : Form
    {
        
        
        int idCurrent = -1;
        string dbCurrent = null;
        public DBUtils db = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void ПодключениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> arr = new Dictionary<string, string>();

                foreach (string line in File.ReadLines(Directory.GetCurrentDirectory()+"\\config.txt"))
                {
                    string[] temp = line.Split(':');
                    arr.Add(temp[0], temp[1]);
                }

                db = new DBUtils(arr["url"], Convert.ToInt32(arr["port"]), arr["namedb"], arr["username"], arr["password"]);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox1.Height = 21;
            this.searchButton.Height = 21;
            this.comboBox1.Height = 21;
        }

        void ПросмотрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (db != null)
            {
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Columns.Clear();

                this.dataGridView1.ColumnCount = 9;
                string[] enumColumns = new string[] { "id", "Время приема", "ФИО ребенка", "Д/р ребенка", "Адрес", "Обр. учреждение", "ФИО представителя", "Телефон", "Завершено" };
                this.comboBox1.Items.Clear();

                for (int i = 0; i < 9; i++)
                {   
                    if(i != 8)
                        this.comboBox1.Items.Add(enumColumns[i]);

                    this.dataGridView1.Columns[i].HeaderText = enumColumns[i];
                }

                dbCurrent = "deal";
                string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` ORDER BY `deal`.`id` DESC LIMIT 10";

                ArrayList arr = db.SqlQuery(sql, "deal");
                ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");
                

                foreach (string[] row in arr)
                {
                    idCurrent = Convert.ToInt32(row[0]);
                    
                    foreach (string[] rowObruch in obruch)
                        if (row[5].Equals(rowObruch[0]))
                            row[5] = rowObruch[1];
                        else if (row[5].Equals("0"))
                            row[5] = "Дошкольник";

                    if (row[8].Equals("1"))
                        row[8] = "Было";
                    else row[8] = "Актуально";

                    this.dataGridView1.Rows.Add(row);
                }
            } else
            {
                MessageBox.Show("Нужно подключиться к бд");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("До свидания");
        }

        private void ПросмотрToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (db != null)
            {
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Columns.Clear();

                this.dataGridView1.ColumnCount = 5;
                string[] enumColumns = new string[] { "id", "Наименование", "Адрес", "Город", "Телефон" };
                this.comboBox1.Items.Clear();


                for (int i = 0; i < 5; i++)
                {
                    
                    this.comboBox1.Items.Add(enumColumns[i]);
                    this.dataGridView1.Columns[i].HeaderText = enumColumns[i];
                }

                dbCurrent = "obruch";
                string sql = "SELECT * FROM `obruch` ORDER BY `id` DESC LIMIT 10";
                ArrayList arr = db.SqlQuery(sql, "obruch");

                

                foreach (string[] row in arr)
                {
                    idCurrent = Convert.ToInt32(row[0]);
                    
                    this.dataGridView1.Rows.Add(row);
                }
            }
            else
            {
                MessageBox.Show("Нужно подключиться к бд");
            }
        }

        private void ВремяНаЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ПросмотрToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (db != null)
            {
                this.dataGridView1.Rows.Clear();
                this.dataGridView1.Columns.Clear();

                this.dataGridView1.ColumnCount = 4;
                string[] enumColumns = new string[] { "id", "Дата и Время", "Использовано", "Для кого" };
                this.comboBox1.Items.Clear();

                for (int i = 0; i < 4; i++)
                {
                    if (!((i == 2) || (i == 3)))
                        this.comboBox1.Items.Add(enumColumns[i]);
                    

                    this.dataGridView1.Columns[i].HeaderText = enumColumns[i];
                }

                dbCurrent = "times";
                string sql = "SELECT * FROM `times` ORDER BY `id` DESC LIMIT 10";
                ArrayList arr = db.SqlQuery(sql, "times");


                
                foreach (string[] row in arr)
                {
                    idCurrent = Convert.ToInt32(row[0]);

                    if (row[2].Equals("1"))
                        row[2] = "Использовано";
                    else row[2] = "Свободно";
                    if (row[3].Equals("1"))
                        row[3] = "Школьник";
                    else row[3] = "Дошкольник";

                    this.dataGridView1.Rows.Add(row);
                }
            }
            else
            {
                MessageBox.Show("Нужно подключиться к бд");
            }
        }

        
        private void LoadMore_Click(object sender, EventArgs e)
        {
            if(db != null)
            {
                if(dbCurrent != null)
                {
                    if(idCurrent != -1)
                    {
                       string sql = null;
                        if (dbCurrent.Equals("obruch") || dbCurrent.Equals("times"))
                            sql = "SELECT * FROM `" + dbCurrent + "` WHERE `" + dbCurrent + "`.`id` < " + idCurrent + " ORDER BY `" + dbCurrent + "`.`id` DESC LIMIT 10";
                        else if (dbCurrent.Equals("deal"))
                            sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                " WHERE `deal`.`id` < " + idCurrent +" ORDER BY `deal`.`id` DESC LIMIT 10";
                        ArrayList arr = db.SqlQuery(sql, dbCurrent);

                        bool firstElement = false;
                        foreach (string[] row in arr)
                        {
                            if (!firstElement)
                            {
                                idCurrent = Convert.ToInt32(row[0]);
                                firstElement = true;
                            }

                            if (dbCurrent.Equals("times"))
                            {
                                if (row[2].Equals("1"))
                                    row[2] = "Использовано";
                                else row[2] = "Свободно";
                                if (row[3].Equals("1"))
                                    row[3] = "Школьник";
                                else row[3] = "Дошкольник";
                            }

                            if(dbCurrent.Equals("deal"))
                            {
                                ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                foreach (string[] rowObruch in obruch)
                                    if (row[5].Equals(rowObruch[0]))
                                        row[5] = rowObruch[1];
                                    else if (row[5].Equals("0"))
                                        row[5] = "Дошкольник";
                            }

                            this.dataGridView1.Rows.Add(row);
                        }
                    } else
                    {
                        MessageBox.Show("Нет базы данных, из которой нужно подгрузить");
                    }
                } else
                {
                    MessageBox.Show("Нет базы данных, из которой нужно подгрузить");
                }
            } else
            {
                MessageBox.Show("Нужно подключиться к бд");
            }
        }

        

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // MessageBox.Show(ColumnsCurrentTable[this.comboBox1.SelectedIndex]);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            switch(dbCurrent)
            {
                case "deal":
                    switch (this.comboBox1.SelectedIndex)
                    {
                            case 0:
                                if(db != null)
                                {
                                    if(dbCurrent != null)
                                    {
                                        this.dataGridView1.Rows.Clear();
                                    //string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obr`.`name`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                    //"INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                    //"INNER JOIN `obruch` AS obr ON `deal`.`obruch`=`obr`.`id` WHERE `deal`.`id`=" + this.textBox1.Text.Trim() + " ORDER BY `deal`.`id` DESC";
                                    //ArrayList arr = db.SqlQuery(sql, "deal");


                                        string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                                "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` WHERE `deal`.`id`= " + this.textBox1.Text.Trim() + " ORDER BY `deal`.`id` DESC";

                                        ArrayList arr = db.SqlQuery(sql, "deal");
                                        ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                        foreach (string[] row in arr)
                                        {
                                            idCurrent = Convert.ToInt32(row[0]);

                                            foreach (string[] rowObruch in obruch)
                                                if (row[5].Equals(rowObruch[0]))
                                                    row[5] = rowObruch[1];
                                                else if (row[5].Equals("0"))
                                                    row[5] = "Дошкольник";

                                            if (row[8].Equals("1"))
                                                row[8] = "Было";
                                            else row[8] = "Актуально";

                                            this.dataGridView1.Rows.Add(row);
                                        }

                                    } else
                                    {

                                    }
                                } else
                                {
                                    MessageBox.Show("Вы не подключились");
                                }
                                break;
                           case 1:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    //string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obr`.`name`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                    //    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                    //    "INNER JOIN `obruch` AS obr ON `deal`.`obruch`=`obr`.`id` WHERE `times`.`dt`='" + this.textBox1.Text.Trim() + "'ORDER BY `deal`.`id` DESC";
                                    //ArrayList arr = db.SqlQuery(sql, "deal");


                                    //foreach (string[] row in arr)
                                    //{
                                    //    this.dataGridView1.Rows.Add(row);
                                    //}

                                    string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                                    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` WHERE `times`.`dt` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";

                                    ArrayList arr = db.SqlQuery(sql, "deal");
                                    ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                    foreach (string[] row in arr)
                                    {
                                        idCurrent = Convert.ToInt32(row[0]);

                                        foreach (string[] rowObruch in obruch)
                                            if (row[5].Equals(rowObruch[0]))
                                                row[5] = rowObruch[1];
                                            else if (row[5].Equals("0"))
                                                row[5] = "Дошкольник";

                                        if (row[8].Equals("1"))
                                            row[8] = "Было";
                                        else row[8] = "Актуально";

                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 2:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();

                                    //string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obr`.`name`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                    //    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                    //    "INNER JOIN `obruch` AS obr ON `deal`.`obruch`=`obr`.`id` WHERE `deal`.`fiochild` LIKE % " + this.textBox1.Text.Trim() + " ORDER BY `deal`.`id` DESC";
                                    //ArrayList arr = db.SqlQuery(sql, "deal");


                                    //foreach (string[] row in arr)
                                    //{
                                    //    this.dataGridView1.Rows.Add(row);
                                    //}

                                    string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                                    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` WHERE `deal`.`childfio` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";

                                    ArrayList arr = db.SqlQuery(sql, "deal");
                                    ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                    foreach (string[] row in arr)
                                    {
                                        idCurrent = Convert.ToInt32(row[0]);

                                        foreach (string[] rowObruch in obruch)
                                            if (row[5].Equals(rowObruch[0]))
                                                row[5] = rowObruch[1];
                                            else if (row[5].Equals("0"))
                                                row[5] = "Дошкольник";

                                        if (row[8].Equals("1"))
                                            row[8] = "Было";
                                        else row[8] = "Актуально";

                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 3:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    //string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obr`.`name`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                    //    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                    //    "INNER JOIN `obruch` AS obr ON `deal`.`obruch`=`obr`.`id` WHERE `deal`.`birthday`='" + this.textBox1.Text.Trim() + "' ORDER BY `deal`.`id` DESC";
                                    //ArrayList arr = db.SqlQuery(sql, "deal");


                                    //foreach (string[] row in arr)
                                    //{
                                    //    this.dataGridView1.Rows.Add(row);
                                    //}

                                    string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                                    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` WHERE `deal`.`birthday` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";

                                    ArrayList arr = db.SqlQuery(sql, "deal");
                                    ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                    foreach (string[] row in arr)
                                    {
                                        idCurrent = Convert.ToInt32(row[0]);

                                        foreach (string[] rowObruch in obruch)
                                            if (row[5].Equals(rowObruch[0]))
                                                row[5] = rowObruch[1];
                                            else if (row[5].Equals("0"))
                                                row[5] = "Дошкольник";

                                        if (row[8].Equals("1"))
                                            row[8] = "Было";
                                        else row[8] = "Актуально";

                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 4:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    //string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obr`.`name`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                    //    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                    //    "INNER JOIN `obruch` AS obr ON `deal`.`obruch`=`obr`.`id` WHERE `deal`.`address` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";
                                    //ArrayList arr = db.SqlQuery(sql, "deal");


                                    //foreach (string[] row in arr)
                                    //{
                                    //    this.dataGridView1.Rows.Add(row);
                                    //}

                                    string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                                    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` WHERE `deal`.`address` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";

                                    ArrayList arr = db.SqlQuery(sql, "deal");
                                    ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                    foreach (string[] row in arr)
                                    {
                                        idCurrent = Convert.ToInt32(row[0]);

                                        foreach (string[] rowObruch in obruch)
                                            if (row[5].Equals(rowObruch[0]))
                                                row[5] = rowObruch[1];
                                            else if (row[5].Equals("0"))
                                                row[5] = "Дошкольник";

                                        if (row[8].Equals("1"))
                                            row[8] = "Было";
                                        else row[8] = "Актуально";

                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 5:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    //string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obr`.`name`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                    //    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                    //    "INNER JOIN `obruch` AS obr ON `deal`.`obruch`=`obr`.`id` WHERE `obr`.`name` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";
                                   
                                    string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                                    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` ORDER BY `deal`.`id` DESC";

                                    ArrayList arr = db.SqlQuery(sql, "deal");
                                    ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                    foreach (string[] row in arr)
                                    {
                                        idCurrent = Convert.ToInt32(row[0]);

                                        foreach (string[] rowObruch in obruch)
                                            if (row[5].Equals(rowObruch[0]))
                                                row[5] = rowObruch[1];
                                            else if (row[5].Equals("0"))
                                                row[5] = "Дошкольник";

                                        if (row[8].Equals("1"))
                                            row[8] = "Было";
                                        else row[8] = "Актуально";

                                        if(row[5].Contains(this.textBox1.Text.Trim()))
                                            this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 6:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    //string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obr`.`name`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                    //    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                    //    "INNER JOIN `obruch` AS obr ON `deal`.`obruch`=`obr`.`id` WHERE `deal`.`parent` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";
                                    //ArrayList arr = db.SqlQuery(sql, "deal");


                                    //foreach (string[] row in arr)
                                    //{
                                    //    this.dataGridView1.Rows.Add(row);
                                    //}

                                    string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                                    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` WHERE `deal`.`parent` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";

                                    ArrayList arr = db.SqlQuery(sql, "deal");
                                    ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                    foreach (string[] row in arr)
                                    {
                                        idCurrent = Convert.ToInt32(row[0]);

                                        foreach (string[] rowObruch in obruch)
                                            if (row[5].Equals(rowObruch[0]))
                                                row[5] = rowObruch[1];
                                            else if (row[5].Equals("0"))
                                                row[5] = "Дошкольник";

                                        if (row[8].Equals("1"))
                                            row[8] = "Было";
                                        else row[8] = "Актуально";

                                    } 

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 7:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    //string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obr`.`name`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                    //    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` " +
                                    //    "INNER JOIN `obruch` AS obr ON `deal`.`obruch`=`obr`.`id` WHERE `deal`.`phone` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";
                                    //ArrayList arr = db.SqlQuery(sql, "deal");


                                    //foreach (string[] row in arr)
                                    //{
                                    //    this.dataGridView1.Rows.Add(row);
                                    //}

                                    string sql = "SELECT `deal`.`id`, `times`.`dt`, `childfio`, `birthday`, `deal`.`address`, `obruch`, `parent`, `deal`.`phone`, `end` FROM `deal` " +
                                                    "INNER JOIN `times` ON `deal`.`dt`=`times`.`id` WHERE `deal`.`phone` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `deal`.`id` DESC";

                                    ArrayList arr = db.SqlQuery(sql, "deal");
                                    ArrayList obruch = db.SqlQuery("SELECT * FROM `obruch`", "obruch");

                                    foreach (string[] row in arr)
                                    {
                                        idCurrent = Convert.ToInt32(row[0]);

                                        foreach (string[] rowObruch in obruch)
                                            if (row[5].Equals(rowObruch[0]))
                                                row[5] = rowObruch[1];
                                            else if (row[5].Equals("0"))
                                                row[5] = "Дошкольник";

                                        if (row[8].Equals("1"))
                                            row[8] = "Было";
                                        else row[8] = "Актуально";

                                    }


                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        default:
                            MessageBox.Show("Возможно, вы не быбрали столбец");
                            break;
                    }
                    break;
                case "obruch":
                    switch (this.comboBox1.SelectedIndex)
                    {
                        case 0:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    string sql = "SELECT * FROM `obruch` WHERE `id`=" + this.textBox1.Text.Trim() + " ORDER BY `id` DESC";
                                    ArrayList arr = db.SqlQuery(sql, "obruch");


                                    foreach (string[] row in arr)
                                    {
                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 1:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    string sql = "SELECT * FROM `obruch` WHERE `name` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `id` DESC";
                                    ArrayList arr = db.SqlQuery(sql, "obruch");


                                    foreach (string[] row in arr)
                                    {
                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 2:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    string sql = "SELECT * FROM `obruch` WHERE `address` LIKE '%" + this.textBox1.Text.Trim() + "%'ORDER BY `id` DESC";
                                    ArrayList arr = db.SqlQuery(sql, "obruch");


                                    foreach (string[] row in arr)
                                    {
                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 3:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    string sql = "SELECT * FROM `obruch` WHERE `city` LIKE '%" + this.textBox1.Text.Trim() +"%'ORDER BY `id` DESC";
                                    ArrayList arr = db.SqlQuery(sql, "obruch");


                                    foreach (string[] row in arr)
                                    {
                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 4:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    string sql = "SELECT * FROM `obruch` WHERE `phone` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `id` DESC";
                                    ArrayList arr = db.SqlQuery(sql, "obruch");


                                    foreach (string[] row in arr)
                                    {
                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        default:
                            MessageBox.Show("Возможно, вы не быбрали столбец");
                            break;
                    }
                    break;
                case "times":
                    switch (this.comboBox1.SelectedIndex)
                    {
                        case 0:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    string sql = "SELECT * FROM `times` WHERE `id`=" + this.textBox1.Text.Trim() + " ORDER BY `id` DESC";
                                    ArrayList arr = db.SqlQuery(sql, "times");


                                    foreach (string[] row in arr)
                                    {
                                        if (row[2].Equals("0"))
                                            row[2] = "Свободно";
                                        else row[2] = "Занято";

                                        if (row[3].Equals("0"))
                                            row[3] = "Дошкольник";
                                        else row[3] = "Школьник";

                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        case 1:
                            if (db != null)
                            {
                                if (dbCurrent != null)
                                {
                                    this.dataGridView1.Rows.Clear();
                                    string sql = "SELECT * FROM `times` WHERE `dt` LIKE '%" + this.textBox1.Text.Trim() + "%' ORDER BY `id` DESC";
                                    ArrayList arr = db.SqlQuery(sql, "times");


                                    foreach (string[] row in arr)
                                    {
                                        if (row[2].Equals("0"))
                                            row[2] = "Свободно";
                                        else row[2] = "Занято";

                                        if (row[3].Equals("0"))
                                            row[3] = "Дошкольник";
                                        else row[3] = "Школьник";

                                        this.dataGridView1.Rows.Add(row);
                                    }

                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                MessageBox.Show("Вы не подключились");
                            }
                            break;
                        default:
                            MessageBox.Show("Возможно, вы не быбрали столбец");
                            break;
                    }
                    break;
                default:
                    MessageBox.Show("");
                    break;
            }
        }

        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.db != null)
            {
                if(this.dbCurrent != null)
                {
                    DeleteForm df = new DeleteForm(db, dbCurrent);
                    df.Show();
                } else
                {
                    MessageBox.Show("Нужно выбрать бд");
                }
            } else
            {
                MessageBox.Show("Вы не подключились");
            }
        }

        private void УдалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.db != null)
            {
                if (this.dbCurrent != null)
                {
                    DeleteForm df = new DeleteForm(db, dbCurrent);
                    df.Show();
                }
                else
                {
                    MessageBox.Show("Нужно выбрать бд");
                }
            }
            else
            {
                MessageBox.Show("Вы не подключились");
            }
        }

        private void УдалитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (this.db != null)
            {
                if (this.dbCurrent != null)
                {
                    DeleteForm df = new DeleteForm(db, dbCurrent);
                    df.Show();
                }
                else
                {
                    MessageBox.Show("Нужно выбрать бд");
                }
            }
            else
            {
                MessageBox.Show("Вы не подключились");
            }
        }

        private void ДобавитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            if (this.db != null)
            {
                if (this.dbCurrent != null)
                {
                    AddRowObruch df = new AddRowObruch(db, dbCurrent);
                    df.Show();
                }
                else
                {
                    MessageBox.Show("Нужно выбрать бд");
                }
            }
            else
            {
                MessageBox.Show("Вы не подключились");
            }
        }

        private void ДобавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.db != null)
            {
                if (this.dbCurrent != null)
                {
                    AddRowTimes df = new AddRowTimes(db, dbCurrent);
                    df.Show();
                    
                }
                else
                {
                    MessageBox.Show("Нужно выбрать бд");
                }
            }
            else
            {
                MessageBox.Show("Вы не подключились");
            }
        }

        private void ОтметитьСовершенноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.db != null)
            {
                if (this.dbCurrent != null)
                {
                    MarkAsFinished df = new MarkAsFinished(db);
                    df.Show();

                }
                else
                {
                    MessageBox.Show("Нужно выбрать бд");
                }
            }
            else
            {
                MessageBox.Show("Вы не подключились");
            }
        }
    }
}
