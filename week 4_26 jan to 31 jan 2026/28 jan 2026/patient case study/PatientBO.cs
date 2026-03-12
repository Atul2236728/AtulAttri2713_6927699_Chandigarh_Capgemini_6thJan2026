using System;
using System.Collections.Generic;
using System.Text;

namespace patient_case_study
{
    internal class PatientBO
    {
        public void DisplayPatientDetails(List<Patient> patientList, string name)
        {
            List<Patient> result = (from p in patientList
                                    where p.Name == name
                                    select p).ToList();

            if (result.Count == 0)
            {
                Console.WriteLine("Patient named {0} not found", name);
            }
            else
            {
                Console.WriteLine("Name                 Age   Illness          City");
                foreach (Patient p in result)
                {
                    Console.WriteLine(p.ToString());
                }
            }
        }

        public void DisplayYoungestPatientDetails(List<Patient> patientList)
        {
            int minAge = (from p in patientList select p.Age).Min();

            var youngest = from p in patientList
                           where p.Age == minAge
                           select p;

            Console.WriteLine("Name                 Age   Illness          City");
            foreach (Patient p in youngest)
            {
                Console.WriteLine(p.ToString());
            }
        }

        public void displayPatientsFromCity(List<Patient> patientList, string cName)
        {
            List<Patient> result = (from p in patientList
                                    where p.City == cName
                                    select p).ToList();

            if (result.Count == 0)
            {
                Console.WriteLine("City named {0} not found", cName);
            }
            else
            {
                Console.WriteLine("Name                 Age   Illness          City");
                foreach (Patient p in result)
                {
                    Console.WriteLine(p.ToString());
                }
            }
        }
    }
}
