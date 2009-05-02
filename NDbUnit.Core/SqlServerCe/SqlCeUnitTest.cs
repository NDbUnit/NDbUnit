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

using System.Data.Common;
using System.Data.SqlServerCe;
using System.Data;

namespace NDbUnit.Core.SqlServerCe
{
    public class SqlCeUnitTest : NDbUnitTest
    {
        public SqlCeUnitTest(string connectionString) : base(connectionString)
        {
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(string connectionString)
        {
            return new SqlCeDbCommandBuilder(connectionString);
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new SqlCeDbOperation();
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SqlCeDataAdapter((SqlCeCommand) command);
        }
    }
}
