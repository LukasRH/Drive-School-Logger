﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using MySql.Data.MySqlClient;

namespace DriveLogCode
{
    public static class MySql
    {
        private const string ConnectionString = "server=ds315e17.duckdns.org;port=50000;uid=DriveLog;pwd=#SWe2017;database=DriveLog";
        private static readonly  MySqlConnection Connection = new MySqlConnection(ConnectionString);

        private const string UserTable = "users";

        public static DataTable GetUser(string username, string table = UserTable)
        {
            if (!ExistUsername(username, table)) return null;

            var cmd = new MySqlCommand($"SELECT * FROM {table} WHERE username = '{username}' LIMIT 1");
            return SendQuery(cmd);
        }

        public static bool ExistEmail(string email, string table = UserTable)
        {
            return Exist("email", email, table);
        }

        public static bool ExistUsername(string username, string table = UserTable)
        {
            return Exist("username", username, table);
        }

        public static bool ExistCPR(string cpr, string table = UserTable)
        {
            return Exist("cpr", cpr, table);
        }

        private static bool Exist(string column, string value, string usertable = UserTable)
        {
            var cmd = new MySqlCommand($"SELECT 1 FROM {usertable} WHERE {column} = '{value}' LIMIT 1");

            var results = SendQuery(cmd);

            return results.Rows.Count == 1;

        }

        public static bool AddUser(string firstname, string lastname, string phone, string mail, string cpr, string address, 
            string zip, string city, string username, string password, string picture = null, string sysmin = "false", string usertable = UserTable)
        {
            var cmd = new MySqlCommand($"INSERT INTO {usertable} (" +
                                       $"firstname, lastname, phone, email, cpr, address, zip, city, username, `password`, picture, sysmin)" +
                                       $"VALUES (" +
                                       $"'{firstname}', '{lastname}', '{phone}', '{mail}', '{cpr}', '{address}', '{zip}', '{city}', '{username}', " +
                                       $"'{password}', '{picture}', '{sysmin}')");


            if (ExistTable(usertable)) return SendNonQuery(cmd);

            var tableCreated = CreateTable(usertable);

            if (tableCreated) return SendNonQuery(cmd);

            return false;
        }

        public static bool UpdateUser(string cpr, string firstname, string lastname, string phone, string mail, string address,
            string zip, string city, string username, string password, string picture = null, string sysmin = "false", string usertable = UserTable)
        {
            var cmd = new MySqlCommand($"UPDATE {usertable} SET " +
                                       $"`firstname` = '{firstname}', `lastname` = '{lastname}', `phone` = '{phone}'," +
                                       $"`email` = '{mail}', `address` = '{address}', `zip` = '{zip}', `city` = '{city}'," +
                                       $"`username` = '{username}', `password` = '{password}', `picture` = '{picture}', `sysmin` = '{sysmin}'" +
                                       $"WHERE `cpr` = '{cpr}'");

            return SendNonQuery(cmd);
        }

        private static bool ExistTable(string tablename)
        {
            var cmd = new MySqlCommand($"SELECT 1 FROM {tablename} LIMIT 1;");

            return SendNonQuery(cmd);
        }

        private static bool CreateTable(string tablename)
        {
            var query = $"CREATE TABLE `{tablename}` (" +
                        "`user_id`  int(11) NOT NULL AUTO_INCREMENT ," +
                        "`firstname`  varchar(255) NOT NULL ," +
                        "`lastname`  varchar(255) NOT NULL ," +
                        "`phone`  varchar(255) NOT NULL ," +
                        "`email`  varchar(255) NOT NULL ," +
                        "`cpr`  varchar(255) NOT NULL ," +
                        "`address`  varchar(255) NOT NULL ," +
                        "`zip`  varchar(255) NOT NULL ," +
                        "`city`  varchar(255) NOT NULL ," +
                        "`username`  varchar(255) NOT NULL ," +
                        "`password`  varchar(255) NOT NULL ," +
                        "`picture`  varchar(255) NULL ," +
                        "`sysmin`  enum('True','False') NOT NULL ," +
                        "PRIMARY KEY (`user_id`))" +
                        "ENGINE=InnoDB DEFAULT CHARACTER SET=utf8 COLLATE=utf8_danish_ci;";

            var cmd = new MySqlCommand(query);

            return SendNonQuery(cmd);
        }

        private static bool SendNonQuery(MySqlCommand cmd)
        {
            try
            {
                cmd.Connection = Connection;
                Connection.Open();
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (MySqlException ex)
            {
                //TODO: Do something with error
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }

        private static DataTable SendQuery(MySqlCommand cmd)
        {
            DataTable results = new DataTable();

            try
            {
                cmd.Connection = Connection;
                Connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    results.Load(reader);
                }
            }
            catch (MySqlException ex)
            {
                //TODO: Do something with error
                Console.WriteLine(ex.Message + ex.Number);
                return null;
            }
            finally
            {
                Connection.Close();
            }

            return results;

        }

    }
}
