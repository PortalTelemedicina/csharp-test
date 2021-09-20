using System;

namespace Demo.DataExtensions.Sql
{
    public static class Extensions
    {
        #region Data

        public static T As<T>(this object source)
        {
            return (source == null || source == DBNull.Value) ? default : (T)source;
        }

        // Used to convert values going to the db
        public static object AsDbValue(this object source)
        {
            return source ?? DBNull.Value;
        }

        #endregion
    }
}
