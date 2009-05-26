/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2009
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

using System.Data;
using System.IO;

namespace NDbUnit.Core
{
    public interface IDbCommandBuilder
    {
        string QuotePrefix
        {
            get;
        }

        string QuoteSuffix
        {
            get;
        }

        IDbConnection Connection
        {
            get;
        }

        DataSet GetSchema();
        void BuildCommands(string xmlSchemaFile);
        void BuildCommands(Stream xmlSchema);
        IDbCommand GetSelectCommand(string tableName);
        IDbCommand GetInsertCommand(string tableName);
        IDbCommand GetInsertIdentityCommand(string tableName);
        IDbCommand GetDeleteCommand(string tableName);
        IDbCommand GetDeleteAllCommand(string tableName);
        IDbCommand GetUpdateCommand(string tableName);
    }
}