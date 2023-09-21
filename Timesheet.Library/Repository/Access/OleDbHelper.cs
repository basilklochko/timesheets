using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using Timesheet.Library.Model;

namespace Timesheet.Library
{
    public class OleDbHelper
    {
        public enum CommandType
        {
            SELECT,
            SELECT_WITH_WHERE,
            SELECT_DEFINED,
            INSERT,
            UPDATE,
            DELETE
        }

        public static void GetConnection(ref OleDbConnection connection)
        {
            if (connection == null)
            {
                connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["data"].ConnectionString);
            }
        }

        public static void OpenConnection(ref OleDbConnection connection)
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        public static int GetIdentity(ref object obj, OleDbConnection connection)
        {
            var model = (UpdatableModel)obj;

            if (model.id == 0)
            {
                var command = new OleDbCommand("SELECT @@IDENTITY", connection);
                model.id = Int32.Parse(command.ExecuteScalar().ToString());
            }

            return model.id;
        }

        public static IModel MapReaderRecord(IModel model, Type modelType, OleDbDataReader reader, string prefix = "")
        {
            foreach (var prop in modelType.GetProperties())
            {
                var propType = prop.PropertyType.UnderlyingSystemType;
                var value = reader.GetValue(reader.GetOrdinal(prefix + prop.Name));                

                if (prop.PropertyType.UnderlyingSystemType == typeof(UserType))
                {
                    prop.SetValue(model, (UserType)value);
                }
                else if (prop.PropertyType.UnderlyingSystemType == typeof(Guid))
                {
                    prop.SetValue(model, new Guid(value.ToString()));
                }
                else
                {
                    prop.SetValue(model, Convert.ChangeType(value, propType));
                }
            }

            return model;
        }

        public static string GetSQL(object value, string entity, CommandType commandType, string where = "")
        {
            StringBuilder command = new StringBuilder();
            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();

            switch (commandType)
            {
                case CommandType.SELECT_DEFINED:
                    command.AppendFormat("{0}", entity);
                    break;

                case CommandType.SELECT:
                    command.AppendFormat("SELECT * FROM [{0}] WHERE id = {1}", entity, value.GetType().GetProperty("id").GetValue(value));
                    break;

                case CommandType.SELECT_WITH_WHERE:
                    command.AppendFormat("SELECT * FROM [{0}] WHERE {1}", entity, where);
                    break;

                case CommandType.INSERT:
                    command.AppendFormat("INSERT INTO [{0}] ", entity);

                    foreach (var p in value.GetType().GetProperties())
                    {
                        if (p.Name.ToLower() == "id")
                        {
                            continue;
                        }

                        fields.AppendFormat("[{0}], ", p.Name);

                        if (p.PropertyType.UnderlyingSystemType == typeof(UserType))
                        {
                            values.AppendFormat("{0}, ", (int)(UserType)p.GetValue(value));
                        }
                        else if (p.PropertyType.UnderlyingSystemType == typeof(DateTime))                                               
                        {
                            if (p.Name == "CreatedDTS" || p.Name == "UpdatedDTS")
                            {
                                p.SetValue(value, DateTime.Now);
                            }

                            values.AppendFormat("{0}, ", "#" + DateTime.Parse(p.GetValue(value).ToString()).ToUniversalTime() + "#");
                        }
                        else
                        {
                            values.AppendFormat("{0}, ", p.PropertyType.UnderlyingSystemType == typeof(string) ||
                               p.PropertyType.UnderlyingSystemType == typeof(Guid) ?
                               "'" + p.GetValue(value) + "'" : p.GetValue(value));
                        }
                    }

                    command.AppendFormat("({0}) VALUES({1})", fields.ToString().Substring(0, fields.Length - 2), values.ToString().Substring(0, values.Length - 2));
                    break;

                case CommandType.UPDATE:
                    command.AppendFormat("UPDATE [{0}] SET ", entity);

                    foreach (var p in value.GetType().GetProperties())
                    {
                        if (p.Name.ToLower() == "id")
                        {
                            continue;
                        }

                        fields.AppendFormat("[{0}] = ", p.Name);

                        if (p.PropertyType.UnderlyingSystemType == typeof(UserType))
                        {
                            fields.AppendFormat("{0}, ", (int)(UserType)p.GetValue(value));
                        }
                        else if (p.PropertyType.UnderlyingSystemType == typeof(DateTime))
                        {
                            if (p.Name == "UpdatedDTS")
                            {
                                p.SetValue(value, DateTime.Now);
                            }

                            fields.AppendFormat("{0}, ", "#" + DateTime.Parse(p.GetValue(value).ToString()).ToUniversalTime() + "#");
                        }
                        else
                        {
                            fields.AppendFormat("{0}, ", p.PropertyType.UnderlyingSystemType == typeof(string) ||
                                p.PropertyType.UnderlyingSystemType == typeof(Guid) ?
                                "'" + p.GetValue(value) + "'" : p.GetValue(value));
                        }
                    }

                    command.AppendFormat("{0} WHERE id = {1}", fields.ToString().Substring(0, fields.Length - 2), value.GetType().GetProperty("id").GetValue(value));
                    break;

                case CommandType.DELETE:
                    command.AppendFormat("DELETE FROM [{0}] WHERE id = {1}", entity, value.GetType().GetProperty("id").GetValue(value));
                    break;
            }

            return command.ToString();
        }

        public static List<IModel> PopulateListModel(IModel model, OleDbCommand command)
        {
            var list = new List<IModel>();
            var modelType = model.GetType();

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model = MapReaderRecord(model, modelType, reader);
                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public static void PopulateListModel(ref List<IModel> list, IModel model, OleDbCommand command, string fields)
        {
            var modelType = model.GetType();

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model = MapReaderRecord(model, modelType, reader);

                        list.Add(model);
                    }
                }
            }
        }

        public static IModel PopulateModel(IModel model, OleDbCommand command)
        {
            var modelType = model.GetType();

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();

                    model = MapReaderRecord(model, modelType, reader);
                }
            }

            return model;
        }
    }
}
