using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Windows.Forms;

namespace uongoClient
{
    public class DBUtils
    {
        MySqlConnection conn = null;
        public DBUtils(string host, int port, string database, string username, string password)
        {
            String connString = "Server=" + host + ";Database=" + database + ";port=" + port + "; User Id=" + username + ";password=" + password + ";SslMode=none";
            conn = new MySqlConnection(connString);
            try
            {
                conn.Open();
                System.Windows.Forms.MessageBox.Show("Объект создан");
                conn.Close();
            } catch(MySqlException exc)
            {
                System.Windows.Forms.MessageBox.Show("Не удалось подключиться: \n" + exc.ToString());
            }
            

        }

        public bool statusConnect()
        {
            if(conn != null)
            {
                return true;
            }
            return false;
        }

        public ArrayList SqlQuery(string query, string namedb)
        {
            ArrayList resultQuery = new ArrayList();

            try {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;

                MySqlDataReader rdr = cmd.ExecuteReader();

                int countColumn = 0;
                switch(namedb)
                {
                    case "deal":
                        countColumn = 9;
                        break;

                    case "times":
                        countColumn = 4;
                        break;

                    case "obruch":
                        countColumn = 5;
                        break;
                }
                while(rdr.Read())
                {
                    string[] str = new string[countColumn];
                    for (int i = 0; i < countColumn; i++)
                    {
                        str[i] = rdr[i].ToString();
                    }
                    resultQuery.Add(str);
                }
                rdr.Close();
                conn.Close();
                
            } catch(Exception exc)
            {
                MessageBox.Show("Ошибка при чтении...: " + exc.ToString());
            }

            return resultQuery;
        }

        public int ExNonQuery(string query, string namedb)
        {
            int count = 0;
            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;

                count = cmd.ExecuteNonQuery();


                conn.Close();

            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка при чтении...: " + exc.ToString());
            }
            return count;
        
        }
        
    }
}
