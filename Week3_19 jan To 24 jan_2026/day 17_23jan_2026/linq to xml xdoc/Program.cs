using System.Xml.Linq;

namespace linq_to_xml_xdoc
{
    class Program
    {
        static void Main()
        {
            Create();
            Read();
            Update();
            Delete();
        }

        static void Create()
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("Students",
                    new XElement("Student",
                        new XAttribute("Id", 1),
                        new XElement("Name", "Alice"),
                        new XElement("Age", 20)
                    ),
                    new XElement("Student",
                        new XAttribute("Id", 2),
                        new XElement("Name", "Bob"),
                        new XElement("Age", 22)
                    )
                )
            );

            doc.Save("studentsDoc.xml");
            Console.WriteLine("XML created.");
        }

        static void Read()
        {
            XDocument doc = XDocument.Load("studentsDoc.xml");

            var students = doc.Descendants("Student")
                              .Where(s => (int)s.Element("Age") > 20)
                              .Select(s => s.Element("Name")!.Value);

            Console.WriteLine("\nStudents older than 20:");
            foreach (var name in students)
                Console.WriteLine(name);
        }

        static void Update()
        {
            XDocument doc = XDocument.Load("studentsDoc.xml");

            var student = doc.Descendants("Student")
                             .FirstOrDefault(s => (int)s.Attribute("Id") == 1);

            if (student != null)
                student.Element("Age")!.Value = "21";

            doc.Save("studentsDoc.xml");
            Console.WriteLine("\nStudent updated.");
        }

        static void Delete()
        {
            XDocument doc = XDocument.Load("studentsDoc.xml");

            var student = doc.Descendants("Student")
                             .FirstOrDefault(s => (int)s.Attribute("Id") == 2);

            student?.Remove();

            doc.Save("studentsDoc.xml");
            Console.WriteLine("\nStudent deleted.");
        }
    }
}

