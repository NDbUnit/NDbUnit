/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2011
 * http://code.google.com/p/ndbunit
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System.Data;
using System.IO;

namespace NDbUnit.Core
{
    public interface IDbCommandBuilder
    {
        int CommandTimeOutSeconds { get; set; }
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