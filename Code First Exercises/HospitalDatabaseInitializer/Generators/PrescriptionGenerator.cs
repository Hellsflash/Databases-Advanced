using System;
using System.Collections.Generic;
using System.Linq;
using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;

namespace HospitalDatabaseInitializer.Generators
{
    public class PrescriptionGenerator
    {
        internal static void InitialPrescriptionSeed(HospitalContext context)
        {
            var rnd = new Random();

            var allMedicamentIds = context.Medicaments.Select(d => d.MedicamentId).ToArray();

            var allPatientIds = context.Patients.Select(p => p.PatientId).ToArray();

            foreach (var patientId in allPatientIds)
            {
                var patientMedicamentsCount = rnd.Next(1, 4);

                var medicamentIds = new int[patientMedicamentsCount];

                for (var id = 0; id < patientMedicamentsCount; id++)
                {
                    var index = -1;

                    while (!allMedicamentIds.Contains(index) || medicamentIds.Contains(index))
                        index = rnd.Next(allMedicamentIds.Max());

                    medicamentIds[id] = index;
                }

                var prescriptions = new List<PatientMedicament>();
                foreach (var medicamentId in medicamentIds)
                {
                    var prescription = new PatientMedicament
                    {
                        PatientId = patientId,
                        MedicamentId = medicamentId
                    };

                    prescriptions.Add(prescription);
                }
                context.Patients.Find(patientId).Prescriptions = prescriptions;
            }

            context.SaveChanges();
        }

        public static void NewPrescription(int patientId, int medicamentId, HospitalContext context)
        {
            var prescription = new PatientMedicament
            {
                PatientId = patientId,
                MedicamentId = medicamentId
            };

            context.Patients.Find(patientId).Prescriptions.Add(prescription);
            context.SaveChanges();
        }

        public static void NewPrescription(Patient patient, Medicament medicament, HospitalContext context)
        {
            var prescription = new PatientMedicament
            {
                Patient = patient,
                Medicament = medicament
            };

            patient.Prescriptions.Add(prescription);
            context.SaveChanges();
        }
    }
}