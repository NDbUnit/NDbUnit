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

using System.Data;

namespace NDbUnit.Core
{
    public interface IDbOperation
    {
        void Insert(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void InsertIdentity(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void Delete(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void DeleteAll(IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void Update(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void Refresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
    }
}