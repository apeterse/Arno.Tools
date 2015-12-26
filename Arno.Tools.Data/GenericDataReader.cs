namespace Arno.Tools.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    /*
    <copyright file="GenericDataReader.cs" >        
           This source is a Public Domain Dedication.
           Attribution is appreciated
           </copyright> 
    <author>Arno Petersen</author>
    <date>26.12.2015</date>
    <summary>
        Generic Class to access IEnumarables with the IDataReaderInterface
        Inspired by the ObjectDataReader from Sky Sanders (http://stackoverflow.com/questions/2258310/get-an-idatareader-from-a-typed-list)
    </summary>
    */
    public class GenericDataReader<T> : IDataReader
    {
        private readonly IEnumerator<T> enumerator;

        private List<PropertyInfo> properties;

        private T current;

        private bool closed = false;

        public GenericDataReader(IEnumerable<T> list)
        {
            Type theType = typeof(T);
            this.properties = theType.GetProperties().ToList();
            this.enumerator = list.GetEnumerator();
        }

        private void CheckIndex(int i)
        {
            if (i < 0 || i >= this.properties.Count)
            {
                throw new IndexOutOfRangeException();
            }
        }

        public void Dispose()
        {
            this.properties = null;
            this.enumerator.Dispose();
        }

        public string GetName(int i)
        {
            this.CheckIndex(i);
            return this.properties[i].Name;
        }

        public string GetDataTypeName(int i)
        {
            this.CheckIndex(i);
            return this.properties[i].PropertyType.ToString();
        }

        public Type GetFieldType(int i)
        {
            this.CheckIndex(i);
            return this.properties[i].PropertyType;
        }

        public object GetValue(int i)
        {
            this.CheckIndex(i);
            return this.properties[i].GetValue(this.current, null);
        }

        public int GetValues(object[] values)
        {
            int i = 0;

            foreach (var propertyInfo in this.properties)
            {
                if (i >= values.Length)
                {
                    break;
                }

                values[i] = propertyInfo.GetValue(this.current, null);

                i++;
            }

            return i;

        }

        public int GetOrdinal(string name)
        {
            return this.properties.FindIndex(test => test.Name == name);
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            this.CheckIndex(i);
            return (char)this.properties[i].GetValue(this.current, null);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            this.CheckIndex(i);
            return (Guid)this.properties[i].GetValue(this.current, null);
        }

        public short GetInt16(int i)
        {
            this.CheckIndex(i);
            return (short)this.properties[i].GetValue(this.current, null);
        }

        public int GetInt32(int i)
        {
            this.CheckIndex(i);
            return (int)this.properties[i].GetValue(this.current, null);
        }

        public long GetInt64(int i)
        {
            this.CheckIndex(i);
            return (long)this.properties[i].GetValue(this.current, null);
        }

        public float GetFloat(int i)
        {
            this.CheckIndex(i);
            return (float)this.properties[i].GetValue(this.current, null);
        }

        public double GetDouble(int i)
        {
            this.CheckIndex(i);
            return (double)this.properties[i].GetValue(this.current, null);
        }

        public string GetString(int i)
        {
            this.CheckIndex(i);
            return (string)this.properties[i].GetValue(this.current, null);
        }

        public decimal GetDecimal(int i)
        {
            this.CheckIndex(i);
            return (decimal)this.properties[i].GetValue(this.current, null);
        }

        public DateTime GetDateTime(int i)
        {
            this.CheckIndex(i);
            return (DateTime)this.properties[i].GetValue(this.current, null);
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            this.CheckIndex(i);
            return this.properties[i].GetValue(this.current, null) == null;
            throw new NotImplementedException();
        }

        public int FieldCount
        {
            get
            {
                return this.properties.Count;
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object IDataRecord.this[string name]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Close()
        {
            this.closed = true;
        }

        public DataTable GetSchemaTable()
        {
            var table = new DataTable();
            foreach (var propertyInfo in this.properties)
            {
                table.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }

            return table;
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public bool Read()
        {
            bool returnValue = this.enumerator.MoveNext();
            this.current = returnValue ? this.enumerator.Current : default(T);
            return returnValue;
        }

        public int Depth { get; }

        public bool IsClosed
        {
            get
            {
                return this.closed;
            }
        }

        public int RecordsAffected { get; }
    }
}