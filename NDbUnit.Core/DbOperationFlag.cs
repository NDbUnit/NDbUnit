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

namespace NDbUnit.Core
{
    /// <summary>
    ///	The database operation to perform.
    /// </summary>
    public enum DbOperationFlag
    {
        /// <summary>No operation.</summary>
        None,
        /// <summary>Insert rows into a set of database tables.</summary>
        Insert,
        /// <summary>Insert rows into a set of database tables.  Allow identity 
        /// inserts to occur.</summary>
        InsertIdentity,
        /// <summary>Delete rows from a set of database tables.</summary>
        Delete,
        /// <summary>Delete all rows from a set of database tables.</summary>
        DeleteAll,
        /// <summary>Update rows in a set of database tables.</summary>
        Update,
        /// <summary>Refresh rows in a set of database tables.  Rows that exist 
        /// in the database are updated.  Rows that don't exist are inserted.</summary>
        Refresh,
        /// <summary>Composite operation of DeleteAll and Insert.</summary>
        CleanInsert,
        /// <summary>Composite operation of DeleteAll and InsertIdentity.</summary>
        CleanInsertIdentity
    }
}