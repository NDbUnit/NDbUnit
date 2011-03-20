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


using NDbUnit.Core;
using MbUnit.Framework;

namespace NDbUnit.Test
{
    [TestFixture]
    public class When_Initial_TableName_Contains_No_Escape_Characters_And_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = "schema.tablename";
            const string ESCAPED_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Single_Element_And_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = "tablename";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Single_Element_And_Extra_Leading_Delimeter
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = ".tablename";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Single_Element_And_Extra_Trailing_Delimeter
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = "tablename.";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Escape_Characters_And_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Original_TableName()
        {
            const string INITIAL_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }
    }



    [TestFixture]
    public class When_Initial_TableName_Contains_No_Escape_Characters_And_No_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Original_TableName()
        {
            const string INITIAL_TABLENAME = "schema.tablename";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, string.Empty, string.Empty));
        }
    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Escape_Characters_And_No_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Original_TableName()
        {
            const string INITIAL_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, string.Empty, string.Empty));
        }
    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Escape_Characters_For_A_Single_Element_And_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = "[schema].tablename";
            const string ESCAPED_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }
    }
}

