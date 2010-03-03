/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2010
 * http://code.google.com/p/ndbunit
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */

using System.Configuration;
namespace NDbUnit.Test
{
    public class DbConnection
    {
        public static string MySqlConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString; }
        }

        public static string OleDbConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["OleDbConnectionString"].ConnectionString; }
        }

        public static string SqlCeConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SqlCeConnectionString"].ConnectionString; }
        }

        public static string SqlConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString; }
        }

        public static string SqlLiteConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SqlLiteConnectionString"].ConnectionString; }
        }

        public static string SqlLiteInMemConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SqlLiteInMemConnectionString"].ConnectionString; }
        }

        public static string OracleClientConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["OracleClientConnectionString"].ConnectionString; }
        }
    }
}
