/*
 *
 * NDbUnit
 * Copyright (C)2005
 * http://www.ndbunit.org
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

using System;
using System.Data;
using System.Collections;

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

    public abstract class DbOperation : IDbOperation
    {
        private string _quotePrefix = "";

        private string _quoteSuffix = "";

        public DbOperation()
        {
        }

        public string QuotePrefix
        {
            get
            {
                return _quotePrefix;
            }
            set
            {
                _quotePrefix = value;
            }
        }

        public string QuoteSuffix
        {
            get
            {
                return _quoteSuffix;
            }
            set
            {
                _quoteSuffix = value;
            }
        }

        protected DataRow CloneDataRow(DataTable dataTable, DataRow dataRow)
        {
            DataRow dataRowClone = dataTable.NewRow();
            IEnumerator enumerator = dataRow.ItemArray.GetEnumerator();
            for (int i = 0; i < dataRow.ItemArray.Length; ++i)
            {
                dataRowClone[i] = dataRow[i];
            }

            return dataRowClone;
        }

        protected bool IsPrimaryKeyValueEqual(DataRow dataRow1, DataRow dataRow2, DataColumn[] primaryKey)
        {
            if (primaryKey.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < primaryKey.Length; ++i)
            {
                DataColumn dataColumn = primaryKey[i];
                // Primary key column value is not equal.
                if (!dataRow1[dataColumn.ColumnName].Equals(dataRow2[dataColumn.ColumnName]))
                {
                    return false;
                }
            }

            return true;
        }

        protected abstract void OnDelete(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction);

        protected abstract void OnDeleteAll(IDbCommand dbCommand, IDbTransaction dbTransaction);

        protected abstract void OnInsert(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction);

        protected abstract void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction);

        protected abstract void OnRefresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, string tableName);

        protected abstract void OnUpdate(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, string tableName);

        private void deleteCommon(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, bool deleteAll)
        {
            Hashtable deletedTableColl = new Hashtable();

            DataSet dsSchema = dbCommandBuilder.GetSchema();

            DataSetTableIterator iterator = new DataSetTableIterator(dsSchema, true);

            foreach (DataTable dataTable in iterator)
            {
                deleteRecursive(ds, dataTable, dbCommandBuilder, dbTransaction, deletedTableColl, deleteAll);
            }
        }

        private void deleteRecursive(DataSet ds, DataTable dataTableSchema, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, Hashtable deletedTableColl, bool deleteAll)
        {
            // Table has already been deleted from.
            if (deletedTableColl.ContainsKey(dataTableSchema.TableName))
            {
                return;
            }

            // [20060724 - sdh] Move here (from end of method) to avoid infinite-loop when package has relation to itself
            // Table was deleted from in the database.
            deletedTableColl[dataTableSchema.TableName] = null;

            DataRelationCollection childRelations = dataTableSchema.ChildRelations;
            // The table has children.
            if (null != childRelations)
            {
                foreach (DataRelation childRelation in childRelations)
                {
                    // Must delete the child table first.
                    deleteRecursive(ds, childRelation.ChildTable, dbCommandBuilder, dbTransaction, deletedTableColl, deleteAll);
                }
            }

            if (deleteAll)
            {
                IDbCommand dbCommand = dbCommandBuilder.GetDeleteAllCommand(dataTableSchema.TableName);

                try
                {
                    OnDeleteAll(dbCommand, dbTransaction);
                }
                catch (DBConcurrencyException)
                {
                    // Swallow deletion of zero records.
                }
            }
            else
            {
                DataTable dataTable = ds.Tables[dataTableSchema.TableName];
                DataTable dataTableCopy = dataTable.Copy();
                dataTableCopy.AcceptChanges();

                foreach (DataRow dataRow in dataTableCopy.Rows)
                {
                    // Delete the row.
                    dataRow.Delete();
                }

                IDbCommand dbCommand = dbCommandBuilder.GetDeleteCommand(dataTableSchema.TableName);

                try
                {
                    OnDelete(dataTableCopy, dbCommand, dbTransaction);
                }
                catch (DBConcurrencyException)
                {
                    // Swallow deletion of zero records.
                }
            }
        }

        void IDbOperation.Delete(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            deleteCommon(ds, dbCommandBuilder, dbTransaction, false);
        }

        void IDbOperation.DeleteAll(IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            deleteCommon(null, dbCommandBuilder, dbTransaction, true);
        }

        void IDbOperation.Insert(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            insertCommon(ds, dbCommandBuilder, dbTransaction, false);
        }

        void IDbOperation.InsertIdentity(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            insertCommon(ds, dbCommandBuilder, dbTransaction, true);
        }

        void IDbOperation.Refresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {

            DataSetTableIterator iterator = new DataSetTableIterator(ds, false);

            foreach (DataTable dataTable in iterator)
            {
                OnRefresh(ds, dbCommandBuilder, dbTransaction, dataTable.TableName);
            }
        }

        void IDbOperation.Update(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction)
        {
            DataSet dsCopy = ds.Copy();
            dsCopy.AcceptChanges();

            DataSetTableIterator iterator = new DataSetTableIterator(dsCopy, true);

            foreach (DataTable dataTable in iterator)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    // Modify every table row.
                    dataRow.BeginEdit();
                    dataRow.EndEdit();
                }

                OnUpdate(dsCopy, dbCommandBuilder, dbTransaction, dataTable.TableName);
            }
        }

        private void insertCommon(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, bool insertIdentity)
        {
            Hashtable insertedTableColl = new Hashtable();

            DataSet dsSchema = dbCommandBuilder.GetSchema();

            DataSetTableIterator iterator = new DataSetTableIterator(dsSchema, true);

            foreach (DataTable dataTable in iterator)
            {
                insertRecursive(ds, dataTable, dbCommandBuilder, dbTransaction, insertedTableColl, insertIdentity);
            }
        }

        private void insertRecursive(DataSet ds, DataTable dataTableSchema, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, Hashtable insertedTableColl, bool insertIdentity)
        {
            // Table has already been inserted into.
            if (insertedTableColl.ContainsKey(dataTableSchema.TableName))
            {
                return;
            }
            // [20060724 - sdh] Move here (from end of method) to avoid infinite-loop when package has relation to itself
            // Table was inserted into in the database.
            insertedTableColl[dataTableSchema.TableName] = null;

            ConstraintCollection constraints = dataTableSchema.Constraints;
            if (null != constraints)
            {
                foreach (Constraint constraint in constraints)
                {
                    // The table has a foreign key constraint.
                    if (constraint.GetType() == typeof(ForeignKeyConstraint))
                    {
                        ForeignKeyConstraint fkConstraint = (ForeignKeyConstraint)constraint;
                        // Must insert parent table first.
                        insertRecursive(ds, fkConstraint.RelatedTable, dbCommandBuilder, dbTransaction, insertedTableColl, insertIdentity);
                    }
                }
            }
            // process parent tables first!
            DataRelationCollection parentRelations = dataTableSchema.ParentRelations;
            if (null != parentRelations)
            {
                foreach (DataRelation parentRelation in parentRelations)
                {
                    // Must insert parent table first.
                    insertRecursive(ds, parentRelation.ParentTable, dbCommandBuilder, dbTransaction, insertedTableColl, insertIdentity);
                }
            }

            DataRow dataRowClone = null;
            DataTable dataTable = ds.Tables[dataTableSchema.TableName];
            DataTable dataTableClone = dataTableSchema.Clone();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                // Insert as a new row.
                dataRowClone = CloneDataRow(dataTableClone, dataRow);
                dataTableClone.Rows.Add(dataRowClone);
            }

            if (insertIdentity)
            {
                IDbCommand dbCommand = dbCommandBuilder.GetInsertIdentityCommand(dataTableSchema.TableName);
                OnInsertIdentity(dataTableClone, dbCommand, dbTransaction);
            }
            else
            {
                IDbCommand dbCommand = dbCommandBuilder.GetInsertCommand(dataTableSchema.TableName);
                OnInsert(dataTableClone, dbCommand, dbTransaction);
            }
        }

    }
}
