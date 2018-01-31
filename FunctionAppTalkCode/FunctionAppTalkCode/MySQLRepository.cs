using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data.Common;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace FunctionAppTalkCode
{
    using Models;

    public static class MySQLRepository<T> where T : class
    {
        private static readonly string str = ConfigurationManager.ConnectionStrings["sqldb_connection"].ConnectionString;
        private static string CollectionId = "";

        public static void Initialize(string collectionId)
        {
            CollectionId = collectionId;
        }

        //public static async Task<T> GetItemAsync(string id)
        //{
        //    try
        //    {
        //        DbDataReader rows;

        //        using (MySqlConnection conn = new MySqlConnection(str))
        //        {
        //            conn.Open();
        //            var text = string.Format("SELECT thinkam.data.value from thinkam.data " +
        //                    "WHERE thinkam.data.collection = '{0}' AND thinkam.data.key = '{1}'", CollectionId, id);

        //            using (MySqlCommand cmd = new MySqlCommand(text, conn))
        //            {
        //                // Execute the command and log the # rows affected.
        //                rows = await cmd.ExecuteReaderAsync();

        //                while (rows.Read())
        //                {
        //                    var value = JsonConvert.DeserializeObject<T>(rows.GetFieldValue<String>(0));
        //                    return value;
        //                }
        //            }
        //        }

        //        return null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public static async Task<IEnumerable<T>> GetItemsAsync(string[] columns, string[] operators, string[] values)
        {
            try
            {
                DbDataReader rows;
                var list = new List<T>();

                using (MySqlConnection conn = new MySqlConnection(str))
                {
                    conn.Open();
                    var text = string.Format("SELECT talkcode.data.value from talkcode.data " +
                            "WHERE talkcode.data.collection = '{0}' ", CollectionId);

                    StringBuilder sb = new StringBuilder();

                    sb.Append(text);

                    for (var i = 0; i < columns.Length; i++)
                    {
                        sb.Append(string.Format(" AND JSON_EXTRACT(talkcode.data.value, '$.{0}') {1} {2}", columns[i], operators[i], values[i]));
                    }

                    using (MySqlCommand cmd = new MySqlCommand(sb.ToString(), conn))
                    {
                        // Execute the command and log the # rows affected.
                        rows = await cmd.ExecuteReaderAsync();

                        while (rows.Read())
                        {
                            var value = JsonConvert.DeserializeObject<T>(rows.GetFieldValue<String>(0));
                            list.Add(value);
                        }

                        return list;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Document CreateItem(Guid key, T item)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(str))
                {
                    conn.Open();
                    var text = string.Format("INSERT INTO talkcode.data (talkcode.data.key, talkcode.data.collection, talkcode.data.value) " +
                            " VALUES('{0}', '{1}', '{2}');", key, CollectionId, JsonConvert.SerializeObject(item));

                    using (MySqlCommand cmd = new MySqlCommand(text, conn))
                    {
                        // Execute the command and log the # rows affected.
                        cmd.ExecuteNonQuery();

                        return new Document()
                        {
                            Id = key.ToString(),
                            Data = JsonConvert.SerializeObject(item)
                        };
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public static Document UpdateItem(Guid key, T item)
        //{
        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(str))
        //        {
        //            conn.Open();
        //            var text = string.Format("UPDATE thinkam.data set thinkam.data.value = " +
        //                    "'{0}' WHERE thinkam.data.key = '{1}' and thinkam.data.collection = '{2}';", JsonConvert.SerializeObject(item), key, CollectionId);

        //            using (MySqlCommand cmd = new MySqlCommand(text, conn))
        //            {
        //                // Execute the command and log the # rows affected.
        //                cmd.ExecuteNonQuery();

        //                return new Document()
        //                {
        //                    Id = key.ToString(),
        //                    Data = JsonConvert.SerializeObject(item)
        //                };
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public static Document RemoveItem(Guid key, T item)
        //{
        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(str))
        //        {
        //            conn.Open();
        //            var text = string.Format("DELETE FROM thinkam.data " +
        //                    " WHERE thinkam.data.key = '{0}' and thinkam.data.collection = '{1}';", key, CollectionId);

        //            using (MySqlCommand cmd = new MySqlCommand(text, conn))
        //            {
        //                // Execute the command and log the # rows affected.
        //                cmd.ExecuteNonQuery();

        //                return new Document()
        //                {
        //                    Id = key.ToString(),
        //                    Data = JsonConvert.SerializeObject(item)
        //                };
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
