using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.DA
{
   public interface ISubjectRangeGenerateDA
    {
         List<SubjectRangeEntity> GetSubjectDetailsForRange();

        void InsertSubjectCode(SubjectRangeEntity LstsubjectCode);
    }
}
