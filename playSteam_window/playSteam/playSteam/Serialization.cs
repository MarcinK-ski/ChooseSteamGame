using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kalikowo
{
    static class Serialization
    {
        private const string DEFAULT_FILE_NAME = "file.bin";
        public static string _fileName = DEFAULT_FILE_NAME;
        private static FileStream _stream;

        public static void Serialize(object objectToSerialize, string customFileName = null)
        {
            setCorrectFileName(customFileName);

            if (!isFileExists(_fileName))
                _stream = File.Create(_fileName);
            else
                _stream = File.OpenWrite(_fileName);

            Console.WriteLine("SERIALIZACJA...");
            var formatter = new BinaryFormatter();

            formatter.Serialize(_stream, objectToSerialize);

            _stream.Close();
        }

        public static object Deserialize(string customFileName = null)
        {
            setCorrectFileName(customFileName);

            if (!isFileExists(_fileName))
            {
                Console.WriteLine("Plik nie istnieje!");
                return null;
            }

            _stream = File.OpenRead(_fileName);

            Console.WriteLine("DESERIALIZACJA...");
            var formatter = new BinaryFormatter();
            //TODO: spr czy plik nie jest pusty!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            var res = formatter.Deserialize(_stream);

            _stream.Close();

            return res;
        }

        private static void setCorrectFileName(string customFileName)
        {
            if (customFileName != null)
                _fileName = customFileName;

        }

        public static bool isFileExists(string customFileName = DEFAULT_FILE_NAME)
        {
            if (File.Exists(customFileName))
                return true;

            return false;
        }
        
    }
}
