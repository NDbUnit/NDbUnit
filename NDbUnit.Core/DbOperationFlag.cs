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