/*
 *
 * NDbUnit
 * Copyright (C)2005
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

using System.IO;

namespace NDbUnit.Test
{
	public class XmlTestFiles
	{

        public class MySqlTestFiles
        {
            private static string _xmlPath = @"..\..\Xml\MySql\";

            public static string XmlSchemaFile
            {
                get
                {
                    return Path.Combine(_xmlPath, "UserDS4MySql.xsd");
                }
            }


            public static string XmlFile
            {
                get
                {
                    return Path.Combine(_xmlPath, "User.xml");
                }
            }

            public static string XmlModFile
            {
                get
                {
                    return Path.Combine(_xmlPath, "UserMod.xml");
                }
            }

            public static string XmlRefreshFile
            {
                get
                {
                    return Path.Combine(_xmlPath, "UserRefresh.xml");
                }
            }
        }
		// expect to be run from the bin/Debug or bin/Release directory
		private static string _xmlPath = @"..\..\Xml\";

	    public static string XmlSchemaFile
		{
			get
			{
				return Path.Combine(_xmlPath, "UserDS.xsd");
			}
		}
        public static string XmlSchemaFileForSqlServerCe
        {
            get
            {
                return Path.Combine(_xmlPath, "SqlServerCeDataSet.xsd");
            }
        }
		public static string xmlSchemaFileForSQLite
		{
			get
			{
				return Path.Combine(_xmlPath, "UserDS4SQLite.xsd");
			}
		}

		public static string XmlFile
		{
			get
			{
				return Path.Combine(_xmlPath, "User.xml");
			}
		}

		public static string XmlModFile
		{
			get
			{
				return Path.Combine(_xmlPath, "UserMod.xml");
			}
		}

		public static string XmlRefreshFile
		{
			get
			{
				return Path.Combine(_xmlPath, "UserRefresh.xml");
			}
		}
	}
}
