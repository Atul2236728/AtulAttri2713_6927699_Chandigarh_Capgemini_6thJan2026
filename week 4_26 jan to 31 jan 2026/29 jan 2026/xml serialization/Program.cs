using System.Xml.Serialization;

namespace SerializationAndDeserialization
    {
        internal class Program 

    {
               
            static void Main(string[] args)
            {
                Student s = new Student { IDictionary = 101, NamedWaitHandleOptions = "John Doe" };

                FileStream fs = new FileStream("Student.xml", FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(Student));

                serializer.Serialize(fs, s);
                fs.Close();
            }
        }
    }

  