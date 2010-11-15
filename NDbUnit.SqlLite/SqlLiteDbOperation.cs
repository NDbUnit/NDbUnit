/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2010
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
using System.Data.Common;
using System.Data.SQLite;

namespace NDbUnit.Core.SqlLite
{
    public class SqlLiteDbOperation : DbOperation
    {
        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new SQLiteDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new SQLiteCommand(cmdText);
        }

        /// <summary>
        /// SQLite doesn't need any changes to insert PK values.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="dbCommand"></param>
        /// <param name="dbTransaction"></param>
        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            OnInsert(dataTable, dbCommand, dbTransaction);
        }
    }
}
