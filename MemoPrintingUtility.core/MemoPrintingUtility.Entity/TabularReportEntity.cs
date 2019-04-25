using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoPrintingUtility.Entity
{
    public class TabularReportEntity
    {
        public string ReportTitle { get; set; }

        public List<StudentInformation> lstStudentInformaion { get; set; }
    }


    public class TotalsubjectRecord
    {
        public string subname { get; set; }
        public string status { get; set; } 

    }


    public class StudentInformation
    {
        public string LeterGrade { get; set; }


        public string Flotation { get; set;}

        public string SGPA { get; set; }

        public string collegecode { set; get; }

        public string BranchName { get; set; }

        public string HallTicketNumber { set; get; }

        public string StudentName { get; set; }

        public string FatherName { get; set; }

        public string SubjectCode { get; set; }

        public string Credits { get; set; }

        public string SubjectName { get; set; }

        public string InternalMarks { get; set; }

        public string ExernalMarks { get; set; }

        public string FinalResult { get; set; }

        public string Ei { get; set; }


        public string Gender { get; set; }

        public string caste { get; set; }

        public string PartI { get; set; }

        public int PartIMarks { get; set; }

        public string PartIStatus { get; set; }

        public string PartII { get; set; }

        public int PartIIMarks { get; set; }

        public string Status { get; set; }

        public int Order { get; set; }



    }


    public class ConsDataEntity
    {

        private string _CODE;

        private string _HTNO;

        private string _SEM;

        private string _P1;

        private string _M1;

        private string _S1;

        private string _R1;

        private string _A1;

        private string _P2;

        private string _M2;

        private string _S2;

        private string _R2;

        private string _A2;

        private string _P3;

        private string _M3;

        private string _S3;

        private string _R3;

        private string _A3;

        private string _P4;

        private string _M4;

        private string _S4;

        private string _R4;

        private string _A4;

        private string _P5;

        private string _M5;

        private string _S5;

        private string _R5;

        private string _A5;

        private string _P6;

        private string _M6;

        private string _S6;

        private string _R6;

        private string _A6;

        private string _P7;

        private string _M7;

        private string _S7;

        private string _R7;

        private string _A7;

        private string _P8;

        private string _M8;

        private string _S8;

        private string _R8;

        private string _A8;

        private string _P9;

        private string _M9;

        private string _S9;

        private string _R9;

        private string _A9;

        private string _P10;

        private string _M10;

        private string _S10;

        private string _R10;

        private string _A10;

        private string _RES;

        private string _SGPA;

        private string _EXAMMONT;

        public ConsDataEntity()
        {
        }


        public string CODE
        {
            get
            {
                return this._CODE;
            }
            set
            {
                if ((this._CODE != value))
                {
                    this._CODE = value;
                }
            }
        }


        public string HTNO
        {
            get
            {
                return this._HTNO;
            }
            set
            {
                if ((this._HTNO != value))
                {
                    this._HTNO = value;
                }
            }
        }


        public string SEM
        {
            get
            {
                return this._SEM;
            }
            set
            {
                if ((this._SEM != value))
                {
                    this._SEM = value;
                }
            }
        }


        public string P1
        {
            get
            {
                return this._P1;
            }
            set
            {
                if ((this._P1 != value))
                {
                    this._P1 = value;
                }
            }
        }


        public string M1
        {
            get
            {
                return this._M1;
            }
            set
            {
                if ((this._M1 != value))
                {
                    this._M1 = value;
                }
            }
        }


        public string S1
        {
            get
            {
                return this._S1;
            }
            set
            {
                if ((this._S1 != value))
                {
                    this._S1 = value;
                }
            }
        }


        public string R1
        {
            get
            {
                return this._R1;
            }
            set
            {
                if ((this._R1 != value))
                {
                    this._R1 = value;
                }
            }
        }


        public string A1
        {
            get
            {
                return this._A1;
            }
            set
            {
                if ((this._A1 != value))
                {
                    this._A1 = value;
                }
            }
        }


        public string P2
        {
            get
            {
                return this._P2;
            }
            set
            {
                if ((this._P2 != value))
                {
                    this._P2 = value;
                }
            }
        }

        public string M2
        {
            get
            {
                return this._M2;
            }
            set
            {
                if ((this._M2 != value))
                {
                    this._M2 = value;
                }
            }
        }


        public string S2
        {
            get
            {
                return this._S2;
            }
            set
            {
                if ((this._S2 != value))
                {
                    this._S2 = value;
                }
            }
        }


        public string R2
        {
            get
            {
                return this._R2;
            }
            set
            {
                if ((this._R2 != value))
                {
                    this._R2 = value;
                }
            }
        }


        public string A2
        {
            get
            {
                return this._A2;
            }
            set
            {
                if ((this._A2 != value))
                {
                    this._A2 = value;
                }
            }
        }

        public string P3
        {
            get
            {
                return this._P3;
            }
            set
            {
                if ((this._P3 != value))
                {
                    this._P3 = value;
                }
            }
        }

        public string M3
        {
            get
            {
                return this._M3;
            }
            set
            {
                if ((this._M3 != value))
                {
                    this._M3 = value;
                }
            }
        }

        public string S3
        {
            get
            {
                return this._S3;
            }
            set
            {
                if ((this._S3 != value))
                {
                    this._S3 = value;
                }
            }
        }

        public string R3
        {
            get
            {
                return this._R3;
            }
            set
            {
                if ((this._R3 != value))
                {
                    this._R3 = value;
                }
            }
        }

        public string A3
        {
            get
            {
                return this._A3;
            }
            set
            {
                if ((this._A3 != value))
                {
                    this._A3 = value;
                }
            }
        }


        public string P4
        {
            get
            {
                return this._P4;
            }
            set
            {
                if ((this._P4 != value))
                {
                    this._P4 = value;
                }
            }
        }


        public string M4
        {
            get
            {
                return this._M4;
            }
            set
            {
                if ((this._M4 != value))
                {
                    this._M4 = value;
                }
            }
        }


        public string S4
        {
            get
            {
                return this._S4;
            }
            set
            {
                if ((this._S4 != value))
                {
                    this._S4 = value;
                }
            }
        }


        public string R4
        {
            get
            {
                return this._R4;
            }
            set
            {
                if ((this._R4 != value))
                {
                    this._R4 = value;
                }
            }
        }


        public string A4
        {
            get
            {
                return this._A4;
            }
            set
            {
                if ((this._A4 != value))
                {
                    this._A4 = value;
                }
            }
        }

        public string P5
        {
            get
            {
                return this._P5;
            }
            set
            {
                if ((this._P5 != value))
                {
                    this._P5 = value;
                }
            }
        }


        public string M5
        {
            get
            {
                return this._M5;
            }
            set
            {
                if ((this._M5 != value))
                {
                    this._M5 = value;
                }
            }
        }

        public string S5
        {
            get
            {
                return this._S5;
            }
            set
            {
                if ((this._S5 != value))
                {
                    this._S5 = value;
                }
            }
        }

        
        public string R5
        {
            get
            {
                return this._R5;
            }
            set
            {
                if ((this._R5 != value))
                {
                    this._R5 = value;
                }
            }
        }

       
        public string A5
        {
            get
            {
                return this._A5;
            }
            set
            {
                if ((this._A5 != value))
                {
                    this._A5 = value;
                }
            }
        }

       
        public string P6
        {
            get
            {
                return this._P6;
            }
            set
            {
                if ((this._P6 != value))
                {
                    this._P6 = value;
                }
            }
        }

       
        public string M6
        {
            get
            {
                return this._M6;
            }
            set
            {
                if ((this._M6 != value))
                {
                    this._M6 = value;
                }
            }
        }

        
        public string S6
        {
            get
            {
                return this._S6;
            }
            set
            {
                if ((this._S6 != value))
                {
                    this._S6 = value;
                }
            }
        }

        
        public string R6
        {
            get
            {
                return this._R6;
            }
            set
            {
                if ((this._R6 != value))
                {
                    this._R6 = value;
                }
            }
        }

       
        public string A6
        {
            get
            {
                return this._A6;
            }
            set
            {
                if ((this._A6 != value))
                {
                    this._A6 = value;
                }
            }
        }

        
        public string P7
        {
            get
            {
                return this._P7;
            }
            set
            {
                if ((this._P7 != value))
                {
                    this._P7 = value;
                }
            }
        }

        
        public string M7
        {
            get
            {
                return this._M7;
            }
            set
            {
                if ((this._M7 != value))
                {
                    this._M7 = value;
                }
            }
        }

        
        public string S7
        {
            get
            {
                return this._S7;
            }
            set
            {
                if ((this._S7 != value))
                {
                    this._S7 = value;
                }
            }
        }

       
        public string R7
        {
            get
            {
                return this._R7;
            }
            set
            {
                if ((this._R7 != value))
                {
                    this._R7 = value;
                }
            }
        }

        
        public string A7
        {
            get
            {
                return this._A7;
            }
            set
            {
                if ((this._A7 != value))
                {
                    this._A7 = value;
                }
            }
        }

        
        public string P8
        {
            get
            {
                return this._P8;
            }
            set
            {
                if ((this._P8 != value))
                {
                    this._P8 = value;
                }
            }
        }

        
        public string M8
        {
            get
            {
                return this._M8;
            }
            set
            {
                if ((this._M8 != value))
                {
                    this._M8 = value;
                }
            }
        }

        
        public string S8
        {
            get
            {
                return this._S8;
            }
            set
            {
                if ((this._S8 != value))
                {
                    this._S8 = value;
                }
            }
        }

        
        public string R8
        {
            get
            {
                return this._R8;
            }
            set
            {
                if ((this._R8 != value))
                {
                    this._R8 = value;
                }
            }
        }

        
        public string A8
        {
            get
            {
                return this._A8;
            }
            set
            {
                if ((this._A8 != value))
                {
                    this._A8 = value;
                }
            }
        }

       
        public string P9
        {
            get
            {
                return this._P9;
            }
            set
            {
                if ((this._P9 != value))
                {
                    this._P9 = value;
                }
            }
        }

       
        public string M9
        {
            get
            {
                return this._M9;
            }
            set
            {
                if ((this._M9 != value))
                {
                    this._M9 = value;
                }
            }
        }

        
        public string S9
        {
            get
            {
                return this._S9;
            }
            set
            {
                if ((this._S9 != value))
                {
                    this._S9 = value;
                }
            }
        }

        
        public string R9
        {
            get
            {
                return this._R9;
            }
            set
            {
                if ((this._R9 != value))
                {
                    this._R9 = value;
                }
            }
        }

       
        public string A9
        {
            get
            {
                return this._A9;
            }
            set
            {
                if ((this._A9 != value))
                {
                    this._A9 = value;
                }
            }
        }

        
        public string P10
        {
            get
            {
                return this._P10;
            }
            set
            {
                if ((this._P10 != value))
                {
                    this._P10 = value;
                }
            }
        }

        
        public string M10
        {
            get
            {
                return this._M10;
            }
            set
            {
                if ((this._M10 != value))
                {
                    this._M10 = value;
                }
            }
        }

        
        public string S10
        {
            get
            {
                return this._S10;
            }
            set
            {
                if ((this._S10 != value))
                {
                    this._S10 = value;
                }
            }
        }

        
        public string R10
        {
            get
            {
                return this._R10;
            }
            set
            {
                if ((this._R10 != value))
                {
                    this._R10 = value;
                }
            }
        }
               
        public string A10
        {
            get
            {
                return this._A10;
            }
            set
            {
                if ((this._A10 != value))
                {
                    this._A10 = value;
                }
            }
        }

        public string RES
        {
            get
            {
                return this._RES;
            }
            set
            {
                if ((this._RES != value))
                {
                    this._RES = value;
                }
            }
        }
       
        public string SGPA
        {
            get
            {
                return this._SGPA;
            }
            set
            {
                if ((this._SGPA != value))
                {
                    this._SGPA = value;
                }
            }
        }
               
        public string EXAMMONT
        {
            get
            {
                return this._EXAMMONT;
            }
            set
            {
                if ((this._EXAMMONT != value))
                {
                    this._EXAMMONT = value;
                }
            }
        }
    }


}
