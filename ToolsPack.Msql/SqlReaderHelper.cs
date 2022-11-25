using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel;

namespace Toolspack.Msql
{
    /// <summary>
    /// Helper class for SqlDataReader to retrieve a nullable values in without type checking.
    /// </summary>
    public static class SqlReaderHelper
    {
        private static bool IsNullableType(Type valueType)
        {
            return (valueType.IsGenericType && valueType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        /// <summary>
        /// Returns the value, of type T, from the SqlDataReader, accounting for both generic and non-generic types.
        /// http://stackoverflow.com/a/18551053/347051
        /// </summary>
        /// <typeparam name="T">T, type applied</typeparam>
        /// <param name="reader">The SqlDataReader object that queried the database</param>
        /// <param name="columnName">The column of data to retrieve a value from</param>
        /// <returns>T, type applied; default value of type if database value is null</returns>
        [ObsoleteAttribute("This method is no longer neccessary in newer .NET version. Use GetFieldValue<T> instead.")]
        public static T GetValue<T>(this SqlDataReader reader, string columnName)
        {
            // Read the value out of the reader by string (column name); returns object
            object v = reader[columnName];

            // Cast to the generic type applied to this method (i.e. int?)
            Type valueType = typeof(T);

            // Check for null value from the database
            if (DBNull.Value != v)
            {
                // We have a null, do we have a nullable type for T?
                if (!IsNullableType(valueType))
                {
                    // No, this is not a nullable type so just change the value's type from object to T
                    return (T)Convert.ChangeType(v, valueType);
                }
                else
                {
                    // Yes, this is a nullable type so change the value's type from object to the underlying type of T
                    NullableConverter nullableConverter = new NullableConverter(valueType);

                    return (T)Convert.ChangeType(v, nullableConverter.UnderlyingType);
                }
            }

            // The value was null in the database, so return the default value for T; this will vary based on what T is (i.e. int has a default of 0)
            return default(T);
        }
        public static bool? GetBooleanNullable(this SqlDataReader reader, string colName) => reader.GetBooleanNullable(reader.GetOrdinal(colName));
        public static bool? GetBooleanNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetBoolean(index);
        }
        public static int? GetIntNullable(this SqlDataReader reader, string colName) => reader.GetIntNullable(reader.GetOrdinal(colName));
        public static int? GetIntNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetInt32(index);
        }
        public static long? GetLongNullable(this SqlDataReader reader, string colName) => reader.GetLongNullable(reader.GetOrdinal(colName));
        public static long? GetLongNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetInt64(index);
        }
        public static short? GetShortNullable(this SqlDataReader reader, string colName) => reader.GetShortNullable(reader.GetOrdinal(colName));
        public static short? GetShortNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetInt16(index);
        }
        public static float? GetFloatNullable(this SqlDataReader reader, string colName) => reader.GetFloatNullable(reader.GetOrdinal(colName));
        public static float? GetFloatNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetFloat(index);
        }
        public static decimal? GetDecimalNullable(this SqlDataReader reader, string colName) => reader.GetDecimalNullable(reader.GetOrdinal(colName));
        public static decimal? GetDecimalNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetDecimal(index);
        }
        public static DateTimeOffset? GetDateTimeOffsetNullable(this SqlDataReader reader, string colName) => reader.GetDateTimeOffsetNullable(reader.GetOrdinal(colName));
        public static DateTimeOffset? GetDateTimeOffsetNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetDateTimeOffset(index);
        }
        public static DateTime? GetDateTimeNullable(this SqlDataReader reader, string colName) => reader.GetDateTimeNullable(reader.GetOrdinal(colName));
        public static DateTime? GetDateTimeNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetDateTime(index);
        }
        public static byte? GetByteNullable(this SqlDataReader reader, string colName) => reader.GetByteNullable(reader.GetOrdinal(colName));
        public static byte? GetByteNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetByte(index);
        }
        public static double? GetDoubleNullable(this SqlDataReader reader, string colName) => reader.GetDoubleNullable(reader.GetOrdinal(colName));
        public static double? GetDoubleNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetDouble(index);
        }
        public static Guid? GetGuidNullable(this SqlDataReader reader, string colName) => reader.GetGuidNullable(reader.GetOrdinal(colName));
        public static Guid? GetGuidNullable(this SqlDataReader reader, int index)
        {
            if (reader.IsDBNull(index))
            {
                return null;
            }
            return reader.GetGuid(index);
        }
    }
}
