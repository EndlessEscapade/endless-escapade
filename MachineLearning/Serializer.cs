using log4net;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;
using Terraria;
using Terraria.ModLoader;

namespace EEMod.MachineLearning
{
    public class Serializers
    {
        public void SerializeToXML<T>(T objects, string fileName)
        {

            XmlSerializer xs = new XmlSerializer(typeof(T));

            TextWriter txtWriter = new StreamWriter(fileName);

            xs.Serialize(txtWriter, objects);

            txtWriter.Close();
        }
        public void Serialize(object t, string path)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, t);
            }
        }
        public void SerializeToJson(object t, string path)
        {
           // var json = new JavaScriptSerializer().Serialize(t);
        }
        //Could explicitly return 2d array, 
        //or be casted from an object to be more dynamic
        public T Deserialize<T>(string path)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                return (T)bformatter.Deserialize(stream);
            }
        }
        public void SerializeToXMLMDA<T>(T[,] objects, string fileName)
        {
            T[] To1DArray(T[,] input)
            {
                int size = input.Length;
                T[] result = new T[size];

                int write = 0;
                for (int i = 0; i <= input.GetUpperBound(0); i++)
                {
                    for (int z = 0; z <= input.GetUpperBound(1); z++)
                    {
                        result[write++] = input[i, z];
                    }
                }
                return result;
            }
            T[] flattenedArray = To1DArray(objects);
            XmlSerializer xs = new XmlSerializer(typeof(T[]));

            TextWriter txtWriter = new StreamWriter(fileName);

            xs.Serialize(txtWriter, flattenedArray);

            txtWriter.Close();
        }


        public T DeserializeToObject<T>(string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(fileName))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }
}
