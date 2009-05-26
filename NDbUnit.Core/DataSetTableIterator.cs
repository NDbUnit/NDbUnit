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

using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;

using System.Diagnostics;


namespace NDbUnit.Core
{

    /// <summary>
    /// Class that builds an iterator for tables.  The order of the tables returned by the iterator
    /// is determined by the foreign keys between tables.
    /// </summary>
    public class DataSetTableIterator : CollectionBase, IEnumerable<DataTable>, IEnumerator
    {
        private int _index = 0;
        private bool _iterateInReverse;


        /// <summary>
        /// Constructor that takes in a dataset to build iterator for the tables.
        /// </summary>
        /// <param name="dataSet">DataSet containing tables.</param>
        public DataSetTableIterator(DataSet dataSet)
        {
            _iterateInReverse = false;
            BuildTableList(dataSet);
        }

        /// <summary>
        /// Builds the table list.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        private void BuildTableList(DataSet dataSet)
        {
            AddTablesToList(dataSet.Tables);

            Trace.Assert(List.Count == dataSet.Tables.Count, "Dataset iterator did not add all tables to collection.");
        }


        /// <summary>
        /// Reverses the list if needed.
        /// </summary>
        private void ReverseListIfNeeded()
        {
            if (_iterateInReverse == true)
            {
                ArrayList tempList = new ArrayList();

                foreach (DataTable dataTable in List)
                {
                    tempList.Add(dataTable);
                }

                tempList.Reverse();

                List.Clear();

                foreach (DataTable dataTable in tempList)
                {
                    List.Add(dataTable);
                }

            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetTableIterator"/> class.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="iterateInReverse">if set to <c>true</c> [iterate in reverse].</param>
        public DataSetTableIterator(DataSet dataSet, bool iterateInReverse)
        {
            _iterateInReverse = iterateInReverse;
            BuildTableList(dataSet);

            ReverseListIfNeeded();
        }

        /// <summary>
        /// Iterate over tables in dataset and at them to the internal list.
        /// </summary>
        /// <param name="tables">Collection of tables.</param>
        private void AddTablesToList(DataTableCollection tables)
        {
            int count = 0;

            // Add tables with no parent keys
            foreach (DataTable table in tables)
            {
                if (ShouldAddToList(table))
                {
                    List.Add(table);
                    count++;
                }
            }

            if (count > 0)
            {
                AddTablesToList(tables);
            }

        }

        /// <summary>
        /// Adds a table to the list if
        ///     - It hasn't already been added
        ///     - It has no relations to parent tables where the parent table isn't in the list.
        /// </summary>
        /// <param name="table">DataTable to check if it meets conditions to add to list.</param>
        /// <returns>True if should add, otherwise false.</returns>
        private bool ShouldAddToList(DataTable table)
        {
            if (List.Contains(table))
            {
                return false;
            }

            foreach (Constraint constraint in table.Constraints)
            {
                ForeignKeyConstraint foreignKey = constraint as ForeignKeyConstraint;

                if (foreignKey != null)
                {
                    if (table.TableName.Equals(foreignKey.RelatedTable.TableName))
                    {
                        // do nothing because this is a self referencing parent
                    }
                    else if (List.Contains(foreignKey.RelatedTable))
                    {
                        // Because the parent has already been added, this constraint can be ignored.
                        // Don't return true as there can be other constraints that prevent this table from being added.
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///<returns>
        ///An IEnumerator that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<DataTable> IEnumerable<DataTable>.GetEnumerator()
        {
            foreach (DataTable table in InnerList)
            {
                yield return table;
            }
        }


        ///<summary>
        ///Advances the enumerator to the next element of the collection.
        ///</summary>
        ///<returns>
        ///true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        ///</returns>
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        ///<filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            if (_index < Count)
            {
                _index++;
                return true;
            }
            else
            {
                return false;
            }
        }

        ///<summary>
        ///Sets the enumerator to its initial position, which is before the first element in the collection.
        ///</summary>
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        ///<filterpriority>2</filterpriority>
        public void Reset()
        {
            _index = 0;
        }

        ///<summary>
        ///Gets the current element in the collection.
        ///</summary>
        ///<returns>
        ///The current element in the collection.
        ///</returns>
        ///<exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
        ///<filterpriority>2</filterpriority>
        object IEnumerator.Current
        {
            get { return List[_index]; }
        }
    }
}