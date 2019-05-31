using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.BO
{
    public interface ILongMemoService
    {
        List<ConsDataEntity> GetBCA_P_StudentsConsDetailsBO();

        List<StudentInformation> GetBCA_P_StudentDetailPRESBO();
    }
}
