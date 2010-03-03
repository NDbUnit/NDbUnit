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

using System.IO;
using System;

namespace NDbUnit.Test
{
    public class XmlTestFiles
    {

        public abstract class XmlTestFilesBase
        {
            protected const string _defaultXmlFilename = "User.xml";
            protected const string _defaultXmlModFilename = "UserMod.xml";
            protected const string _defaultXmlRefreshFilename = "UserRefresh.xml";
            protected const string _defaultXmlSchemaFilename = "UserDS.xsd";
        }

        public class MySql : XmlTestFilesBase
        {
            private static string _xmlPath = @"..\..\Xml\MySql\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class SqlServer : XmlTestFilesBase
        {
            public static string XmlFileForSchemaPrefixTests
            {
                get { return Path.Combine(_xmlPath, "DataFileWithSchemaPrefixes.xml"); }
            }
            public static string XmlSchemaFileForSchemaPrefixTests
            {
                get { return Path.Combine(_xmlPath, "SchemaWithSchemaPrefixes.xsd"); }
            }
            private static string _xmlPath = @"..\..\Xml\SqlServer\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class SqlServerCe : XmlTestFilesBase
        {
            private static string _xmlPath = @"..\..\Xml\SqlServerCe\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class OleDb : XmlTestFilesBase
        {
            private static string _xmlPath = @"..\..\Xml\OleDb\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class Sqlite : XmlTestFilesBase
        {
            private static string _xmlPath = @"..\..\Xml\Sqlite\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class OracleClient : XmlTestFilesBase
        {
            private static string _xmlPath = @"..\..\Xml\OracleClient\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

    }
}
