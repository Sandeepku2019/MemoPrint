using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.DA
{
    public interface ILongMemoDAService
    {
        List<ConsDataEntity> GetBCA_P_StudentsConsDetails();

        List<StudentInformation> GetBCA_P_StudentDetailPRES();


    }
}
