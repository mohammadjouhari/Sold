using System.Data;

namespace ShopexCoreV2.Data
{
    public static class SqlDataReaderExtensions
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader, Func<IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }
}
