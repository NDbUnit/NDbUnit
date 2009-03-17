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

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using NDbUnit.Core;

namespace NDbUnit.Test
{
    [TestFixture]
    public class WhenInitialTableNameContainsNoEscapeCharactersAndEscapeCharactersAreProvided
    {
        [Test]
        public void ReturnsProperlyEscapedTableName()
        {
            const string INITIAL_TABLENAME = "schema.tablename";
            const string ESCAPED_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class WhenInitialTableNameContainsSingleElementAndEscapeCharactersAreProvided
    {
        [Test]
        public void ReturnsProperlyEscapedTableName()
        {
            const string INITIAL_TABLENAME = "tablename";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class WhenInitialTableNameContainsSingleElementAndExtraLeadingDelimeter
    {
        [Test]
        public void ReturnsProperlyEscapedTableName()
        {
            const string INITIAL_TABLENAME = ".tablename";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class WhenInitialTableNameContainsSingleElementAndExtraTrailingDelimeter
    {
        [Test]
        public void ReturnsProperlyEscapedTableName()
        {
            const string INITIAL_TABLENAME = "tablename.";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class WhenInitialTableNameContainsEscapeCharactersAndEscapeCharactersAreProvided
    {
        [Test]
        public void ReturnsOriginalTableName()
        {
            const string INITIAL_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }
    }



    [TestFixture]
    public class WhenInitialTableNameContainsNoEscapeCharactersAndNoEscapeCharactersAreProvided
    {
        [Test]
        public void ReturnsOriginalTableName()
        {
            const string INITIAL_TABLENAME = "schema.tablename";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, string.Empty, string.Empty));
        }
    }

    [TestFixture]
    public class WhenInitialTableNameContainsEscapeCharactersAndNoEscapeCharactersAreProvided
    {
        [Test]
        public void ReturnsOriginalTableName()
        {
            const string INITIAL_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, string.Empty, string.Empty));
        }
    }

    [TestFixture]
    public class WhenInitialTableNameContainsEscapeCharactersForASingleElementAndEscapeCharactersAreProvided
    {
        [Test]
        public void ReturnsProperlyEscapedTableName()
        {
            const string INITIAL_TABLENAME = "[schema].tablename";
            const string ESCAPED_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }
    }
}

