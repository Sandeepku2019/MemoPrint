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
    }
}
