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

using System.Text;

namespace NDbUnit.Core
{
    public static class TableNameHelper
    {
        public static string FormatTableName(string declaredTableName, string quotePrefix, string quoteSuffix)
        {
            StringBuilder result = new StringBuilder();

            var tableNameElements = declaredTableName.Split(".".ToCharArray());

            bool firstElement = true;

            foreach (string s in tableNameElements)
            {
                StringBuilder temp = new StringBuilder();
                if (s != string.Empty)
                {
                    if (!s.StartsWith(quotePrefix))
                        temp.Append(quotePrefix);

                    temp.Append(s);

                    if (!s.EndsWith(quoteSuffix))
                        temp.Append(quoteSuffix);

                    if (firstElement)
                        result.Append(temp.ToString());
                    else
                        result.Append("." + temp);

                    firstElement = false;
                }
            }

            return result.ToString();
        }
    }
}
