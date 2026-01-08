using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management;
using System.Net.Quic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace BankApp
{
    internal class Functions
    {
        private SqlConnection Con;
        private SqlCommand Cmd;
        private DataTable dt;
        private SqlDataAdapter Sda;
        private String ConStr;

        public Functions()
        {
            ConStr = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\Adika\OneDrive\Documents\BankAppDb.mdf;Integrated Security=True;Connect Timeout=30";
            Con = new SqlConnection(ConStr);
            Cmd = new SqlCommand();
            Cmd.Connection = Con;
        }

        public DataTable GetData(string Query)
        {
            dt = new DataTable();
            Sda = new SqlDataAdapter(Query, ConStr);
            Sda.Fill(dt);
            return dt;
        }

        public int SetData(string Query)
        {
            int Cnt = 0;
            if (Con.State == ConnectionState.Closed)
            {
                Con.Open();
            }
            Cmd.CommandText = Query;
            Cnt = Cmd.ExecuteNonQuery();
            Con.Close();
            return Cnt;
        }

    }
}
