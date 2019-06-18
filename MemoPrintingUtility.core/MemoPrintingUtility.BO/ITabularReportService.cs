using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.BO
{
    public interface ITabularReportService
    {
        List<StudentInformation> GetStudentDetail(string Course, int Semister, int sem, int year);

        List<ConsDataEntity> GetStudentsConsDetails(string Course, int Semister);

        List<StudentInformation> GetMallPractHtno(string Course, int Semister, int sem, int year);
       List<TotalsubjectRecord> getTotalandPassed(string Course, int year);

        List<BALPresEntity> GetBalPresInformation(string course);
        List<BALConEntity> GetBALConInformaion();

        List<StudentInformation> GetBCA_P_StudentDetailPRES(string sem, string year);

        List<ConsDataEntity> GetBCA_P_StudentsConsDetails(string Semister);

        List<BALSubjectInformation> GetBALSubjectInformation(string CourseName);

        List<BCAPSubjectINformation> GetBCAPSubjectInformation(string CourseName);



        // vertical Tr
        List<StudentInformation> GetStudentDetailVR(string Course);

        List<ConsDataEntity> GetStudentsConsDetailsVR(string Course);

        List<CollegeDetails> GetCollegeDetails();

    }
}
