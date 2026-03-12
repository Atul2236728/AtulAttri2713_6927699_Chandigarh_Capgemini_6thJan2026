using System.Xml.Linq;

namespace linq_to_xml
{
    class Program
    {
        static void Main()
        {
            CreateXml();
            QueryXml();
            UpdateXml();
        }

        static void CreateXml()
        {
            XElement students = new XElement("Students",
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
            );

            students.Save("students.xml");
            Console.WriteLine("XML created.");
        }

        static void QueryXml()
        {
            XElement students = XElement.Load("students.xml");

            var names = students.Elements("Student")
                                .Where(s => (int)s.Element("Age") > 20)
                                .Select(s => s.Element("Name")!.Value);

            Console.WriteLine("\nStudents older than 20:");
            foreach (var name in names)
                Console.WriteLine(name);
        }

        static void UpdateXml()
        {
            XElement students = XElement.Load("students.xml");

            var student = students.Elements("Student")
                                  .FirstOrDefault(s => (int)s.Attribute("Id") == 1);

            if (student != null)
                student.Element("Age")!.Value = "21";

            students.Save("students.xml");
            Console.WriteLine("\nStudent updated.");
        }
    }
}
