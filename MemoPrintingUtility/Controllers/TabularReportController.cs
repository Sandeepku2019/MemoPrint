using MemoPrintingUtility.BO;

using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemoPrintingUtility.Controllers
{
    public class TabularReportController : Controller
    {
        int PageNumber = 1;
        // GET: TabularReport
        public ActionResult Index()
        {
            return View();
        }
        int PageBraker = 0;

        [HttpPost]
        public JsonResult GenerateTabularSemReport(int Psem, string course)
        {
            if (course == "BA (L)")
            {
                return GenerateBA_L("BAL");
            }
            else if (course == "BCA(P)")
            {
                return GenerateReport_BC_P(Psem, "BCA");
            }
            else
            {

                return GenerateReport(Psem, course);
            }

        }

        private JsonResult GenerateReport(int Psem, string course)
        {
            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/TabularReport/");
                string fileNamedirectory = @"D:\TabularReport\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_" + Psem + ".txt";

                // check for Directory
                if (!Directory.Exists(fileNamedirectory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(fileNamedirectory);
                }


                // file information 
                FileInfo fi = new FileInfo(fileNamedirectory + filename);
                // Check if file already exists. If yes, delete it.     
                if (System.IO.File.Exists(fileNamedirectory + filename))
                {
                    System.IO.File.Delete(fileNamedirectory + filename);
                }

                MemoPrintService BoMemoService = new MemoPrintService();

                int year = 0;
                int sem = 0;
                if (Psem == 11)
                {
                    year = 1;
                    sem = 1;

                }
                if (Psem == 21)
                {
                    year = 2;
                    sem = 1;

                }

                if (Psem == 31)
                {
                    year = 3;
                    sem = 1;

                }

                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {
                    //
                    var lstStudents = BoMemoService.getTabularReportInstance().GetStudentDetail(course, Psem, sem, year);
                    var LstConStudents = BoMemoService.getTabularReportInstance().GetStudentsConsDetails(course, Psem);
                    var lsttotalpassed = BoMemoService.getTabularReportInstance().getTotalandPassed(course, year);

                    int TotalSubjectAttemt = 0;
                    int TotalSubjectpass = 0;
                    string PrevColcode = "";
                    bool ColPagebrk = false;

                    List<string> lstColCodes = lstStudents.OrderBy(x => x.collegecode.Trim()).Select(y => y.collegecode.Trim()).Distinct().ToList<string>();


                    int series = 0;
                    foreach (var colcode in lstColCodes)
                    {
                        List<string> HallticketNumbers = lstStudents.Where(y => y.collegecode.Trim() == colcode.Trim()).OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();
                        bool isExstudent = false;

                        ColPagebrk = true;
                        #region Htnonumbers
                        for (int i = 0; i < HallticketNumbers.Count; i++)
                        {


                            series++;
                            int totalSUbjects = 0;
                            int passedSubject = 0;
                            bool detained = false;
                            int rowcount = 0;
                            var lstStuns = lstStudents.Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                            var StudentConsInformatio = LstConStudents.Where(x => x.HTNO == HallticketNumbers[i]).ToList<ConsDataEntity>();

                            string HallTicket = lstStuns[0].HallTicketNumber;
                            var objtotalaspassed = lsttotalpassed.Where(x => x.Htno.Trim() == HallTicket.Trim()).ToList();

                            if (HallTicket == "131191022")
                            {

                            }

                            string FN = lstStuns[0].FatherName == null ? "" : lstStuns[0].FatherName;
                            string SN = lstStuns[0].StudentName == null ? "" : lstStuns[0].StudentName;
                            string CC = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                            string EI = lstStuns[0].Ei == null ? "" : lstStuns[0].Ei;
                            string CollegeCode = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                            string nameformat = series + GetSpaces(5 - series.ToString().Length) + GetSpaces(1) + CollegeCode + GetSpaces(5 - CollegeCode.Length) + HallTicket + GetSpaces(12 - HallTicket.Length) + SN + GetSpaces(4 - SN.Length) + FN + GetSpaces(45 - FN.Length);
                            if (EI == "E")
                            {
                                isExstudent = true;
                            }
                            else
                            {
                                isExstudent = false;
                            }




                            if (lstStudents != null && lstStudents.Count > 0)
                            {
                                if (StudentConsInformatio != null && StudentConsInformatio.Count > 0)
                                {
                                    rowcount = rowcount + 2;
                                }
                                rowcount = rowcount + 6;

                            }
                            if ((PageBraker + rowcount) > 72)
                            {
                                int differ = 72 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }


                                PageBraker = addHeaderFooter(sw, 72, course, year.ToString(), sem.ToString());
                            }

                            if (PrevColcode != "" && PrevColcode.Trim() != colcode.Trim() && ColPagebrk == true)
                            {
                                ColPagebrk = false;
                                int differ = 73 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }


                                PageBraker = addHeaderFooter(sw, 72, course, year.ToString(), sem.ToString());

                            }


                            if (PrevColcode == "")
                            {
                                PrevColcode = colcode;
                            }

                            if (i == 0)
                            {
                                PageBraker = addHeaderFooter(sw, PageBraker, course, year.ToString(), sem.ToString());
                                sw.WriteLine("");
                                sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            }
                            else
                            {
                                sw.WriteLine("");
                                sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            }
                            List<TotalsubjectRecord> lstts = new List<TotalsubjectRecord>();

                            #region Cons
                            if (StudentConsInformatio != null & StudentConsInformatio.Count > 0)
                            {

                                var ConsDetails = StudentConsInformatio.FirstOrDefault();
                                //string Attemps = CalucateTotalAttemps(ConsDetails);

                                string subjectstringCONS = string.Empty;
                                string SubjectCONSMarks_U = string.Empty;
                                string subjectsMarksCONS_U = string.Empty;
                                string MarkformatCONS = GetSpaces(10);

                                string SubjectCONSMarks_S = string.Empty;
                                string subjectsMarksCONS_S = string.Empty;

                                string subjectCONSformt = "";

                                subjectstringCONS = "CONS." + GetSpaces(5 - "CONS.".Length);
                                SubjectCONSMarks_U = "   U" + GetSpaces(5 - "CONS.".Length);
                                SubjectCONSMarks_S = "   S" + GetSpaces(5 - "CONS.".Length) + GetSpaces(2);

                                #region Subject title 


                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P1, ConsDetails.A1);

                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P2, ConsDetails.A2);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P3, ConsDetails.A3);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P4, ConsDetails.A4);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P5, ConsDetails.A5);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P6, ConsDetails.A6);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P7, ConsDetails.A7);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P8, ConsDetails.A8);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P9, ConsDetails.A9);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P10, ConsDetails.A10);

                                if (ConsDetails.P1 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P1, status = ConsDetails.R1 });
                                if (ConsDetails.P2 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P2, status = ConsDetails.R2 });
                                if (ConsDetails.P3 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P3, status = ConsDetails.R3 });
                                if (ConsDetails.P4 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P4, status = ConsDetails.R4 });
                                if (ConsDetails.P5 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P5, status = ConsDetails.R5 });
                                if (ConsDetails.P6 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P6, status = ConsDetails.R6 });
                                if (ConsDetails.P7 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P7, status = ConsDetails.R7 });
                                if (ConsDetails.P8 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P8, status = ConsDetails.R8 });
                                if (ConsDetails.P9 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P9, status = ConsDetails.R9 });
                                if (ConsDetails.P10 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P10, status = ConsDetails.R10 });


                                subjectstringCONS = "CONS." + GetSpaces(5 - "CONS.".Length) + GetSpaces(1) + subjectCONSformt;
                                sw.WriteLine(subjectstringCONS + GetSpaces(113 - subjectstringCONS.Length) + ConsDetails.RES + "   " + ConsDetails.SGPA);

                                if (ConsDetails.RES == "L")
                                {
                                    detained = true;
                                }
                                #endregion


                                #region Subject External Mark 
                                string SubjectExtCons = string.Empty;
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M1, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M2, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M3, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M4, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M5, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M6, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M7, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M8, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M9, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M10, string.Empty);
                                SubjectCONSMarks_U = SubjectCONSMarks_U + GetSpaces(2) + SubjectExtCons;
                                sw.WriteLine(SubjectCONSMarks_U);

                                #endregion


                                #region Subject Internal Mark 
                                string SubjectintCons = string.Empty;

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S1, ConsDetails.R1);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S2, ConsDetails.R2);


                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S3, ConsDetails.R3);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S4, ConsDetails.R4);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S5, ConsDetails.R5);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S6, ConsDetails.R6);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S7, ConsDetails.R7);

                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S8, ConsDetails.R8);


                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S9, ConsDetails.R9);


                                SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S10, ConsDetails.R10);

                                SubjectCONSMarks_S = SubjectCONSMarks_S + SubjectintCons;

                                sw.WriteLine(SubjectCONSMarks_S);

                                #endregion

                            }
                            #endregion


                            #region  Pre
                            string subjectstringPRE = string.Empty;
                            string SubjectMarks_U = string.Empty;
                            string subjectsMarksPRE_U = string.Empty;
                            string MarkformatPRE = GetSpaces(10);
                            string Aademicstatus = string.Empty;



                            string SubjectMarks_S = string.Empty;
                            string subjectsMarksPRE_S = string.Empty;


                            if (lstStuns.Count > 0)
                            {
                                string subjectformt = "";
                                subjectstringPRE = "PRES." + GetSpaces(5 - "PRES.".Length) + GetSpaces(1);
                                subjectsMarksPRE_U = "   U" + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                if (!isExstudent)
                                {
                                    subjectsMarksPRE_S = "   S" + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                }
                                else
                                {
                                    subjectsMarksPRE_S = GetSpaces(5) + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                }
                                int subord = 1;

                                int count = 0;

                                //totalSUbjects = totalSUbjects + lstStuns.Count;
                                //passedSubject = passedSubject + lstStuns.Where(x => x.Status == "P").ToList<StudentInformation>().Count();



                                foreach (StudentInformation studentM in lstStuns.OrderBy(X => X.Order))
                                {

                                    #region total subs and paassed subs

                                    TotalsubjectRecord obj = new TotalsubjectRecord() { subname = studentM.SubjectCode, status = studentM.Status };
                                    if (lstts != null && lstts.Count > 0)
                                    {
                                        if (lstts.Where(x => x.subname.ToLower() == studentM.SubjectCode.ToLower()).ToList().Count > 0)
                                        {
                                            foreach (var t in lstts)
                                            {
                                                if (t.subname == obj.subname)
                                                {
                                                    t.status = studentM.Status;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lstts.Add(obj);
                                        }
                                    }
                                    else
                                    {
                                        lstts.Add(obj);

                                    }
                                    #endregion
                                    string spaces = "";
                                    string umakspace = "";
                                    string intmarkspae = "";
                                    int ordr = studentM.Order;
                                    spaces = GetSpaces((ordr - subord) * 8);
                                    umakspace = GetSpaces((ordr - subord) * 8);
                                    intmarkspae = GetSpaces((ordr - subord) * 8);
                                    string subPReF = studentM.LeterGrade;
                                    if (studentM.LeterGrade == "AB")
                                    {
                                        subPReF = "F";
                                    }
                                    string intmark = string.Empty;
                                    if (isExstudent == false)
                                    {
                                        intmark = Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks) + GetSpaces(3 - Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks).Length) + " " + subPReF;
                                    }

                                    if (count > 0)
                                    {




                                        spaces = GetSpaces(spaces.Length - studentM.SubjectCode.Length);


                                        if (isExstudent == false)
                                        {
                                            umakspace = GetSpaces(umakspace.Length - studentM.ExernalMarks.ToString().Length);

                                            intmarkspae = GetSpaces(intmarkspae.Length - intmark.Length);
                                        }
                                        else
                                        {
                                            if (studentM.ExernalMarks.ToString() == "AB.")
                                            {
                                                umakspace = GetSpaces(umakspace.Length + 1 - (studentM.ExernalMarks.ToString() + " " + studentM.LeterGrade).Length);
                                            }
                                            else
                                            {
                                                umakspace = GetSpaces(umakspace.Length - (studentM.ExernalMarks.ToString().Truncate(3) + " " + studentM.LeterGrade).Length);
                                            }

                                        }
                                    }



                                    subjectformt = spaces + studentM.SubjectCode;
                                    subjectstringPRE = subjectstringPRE + subjectformt;

                                    if (isExstudent == false)
                                    {
                                        SubjectMarks_U = umakspace + studentM.ExernalMarks.ToString().Truncate(3);
                                    }
                                    else
                                    {
                                        SubjectMarks_U = umakspace + studentM.ExernalMarks.ToString().Truncate(4) + " " + subPReF;

                                    }

                                    subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;


                                    if (isExstudent == false)
                                    {
                                        intmark = intmarkspae + intmark;// + GetSpaces(7 - intmark.Length); ;
                                    }
                                    SubjectMarks_S = intmark;
                                    subjectsMarksPRE_S = subjectsMarksPRE_S + SubjectMarks_S;

                                    count++;
                                    subord = ordr;
                                }

                                string subjectsPF = string.Empty;
                                if (objtotalaspassed == null || objtotalaspassed.Count == 0)
                                {
                                    objtotalaspassed = new List<TotalsubjectRecord>();
                                    objtotalaspassed.Add(new TotalsubjectRecord() { TotalSubs = 0, PassedSubs = 0 });
                                }
                                var aa = objtotalaspassed.Sum(x => x.PassedSubs) + lstts.Where(x => x.status != "F").ToList().Count;
                                var bb = objtotalaspassed.Sum(x => x.TotalSubs);

                                var Paa = lstts.Where(x => x.status != "F" && x.subname != null).ToList().Count;
                                var Pbb = lstts.Where(x => x.subname != null).ToList().Count;

                                var expaa = lstStuns.Where(x => x.Status != "F").ToList().Count;

                                if (isExstudent == false)
                                {
                                    subjectsPF = "<" + Paa + "/" + Pbb + ">" + "<" + (aa).ToString().Truncate(2) + "/" + (bb + lstts.Count).ToString().Truncate(2) + ">";
                                }
                                else
                                {
                                    subjectsPF = "<" + Paa + "/" + Pbb + ">" + "<" + (expaa + objtotalaspassed.Sum(x => x.PassedSubs)) + "/" + bb + ">";
                                }

                                int external = lstStuns.Sum(x => x.ExernalMarks.ChangeINT());
                                int TotalMark = external + lstStuns.Sum(x => x.InternalMarks.ChangeINT());
                                sw.WriteLine(subjectstringPRE + GetSpaces(113 - subjectsMarksPRE_U.Length) + "  " + subjectsPF + " " + TotalMark);
                                string finalR = lstStuns[0].FinalResult;
                                if (finalR != "MALL.PRACT")
                                {
                                    if (detained == true && TotalMark > 0)
                                    {
                                        finalR = "PROMOTED";

                                    }
                                    else
                                    {
                                        finalR = lstStuns[0].FinalResult;

                                    }
                                }
                                else
                                {
                                    finalR = "MP";

                                }

                                string Flotation = string.Empty;

                                //lstStuns.Where(x => x.Flotation.ToString() == "FL").ToList().Count > 0 ? "FL" : "";



                                if (lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count > 0)
                                {
                                    Flotation = "FL AC";
                                }
                                else
                                if (lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count == 0)
                                {
                                    Flotation = "FL";

                                }
                                else
                                if (lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count == 0)
                                {
                                    Flotation = "AC";

                                }

                                sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length) + "  " + finalR + "  " + lstStuns[0].SGPA + " " + Flotation);
                                if (isExstudent == false)
                                {
                                    sw.WriteLine(subjectsMarksPRE_S + GetSpaces(113 - subjectsMarksPRE_U.Length));

                                }

                                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

                            }




                            #endregion

                            PageBraker = PageBraker + rowcount;

                        }

                        #endregion
                    }

                }

                // Write file contents on console.     
                using (StreamReader sr = System.IO.File.OpenText(fileNamedirectory + filename))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                return Json(fileNamedirectory + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }


        }




        private JsonResult GenerateReport_BC_P(int Psem, string course)
        {
            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/TabularReport/");
                string fileNamedirectory = @"D:\TabularReport\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_" + Psem + ".txt";

                // check for Directory
                if (!Directory.Exists(fileNamedirectory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(fileNamedirectory);
                }


                // file information 
                FileInfo fi = new FileInfo(fileNamedirectory + filename);
                // Check if file already exists. If yes, delete it.     
                if (System.IO.File.Exists(fileNamedirectory + filename))
                {
                    System.IO.File.Delete(fileNamedirectory + filename);
                }

                MemoPrintService BoMemoService = new MemoPrintService();

                int year = 0;
                int sem = 0;
                if (Psem == 11)
                {
                    year = 1;
                    sem = 1;

                }

                if (Psem == 12)
                {
                    year = 1;
                    sem = 2;

                }

                if (Psem == 21)
                {
                    year = 2;
                    sem = 1;

                }

                if (Psem == 22)
                {
                    year = 2;
                    sem = 2;

                }

                if (Psem == 31)
                {
                    year = 3;
                    sem = 1;

                }
                if (Psem == 32)
                {
                    year = 3;
                    sem = 2;

                }

                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {
                    //
                    var lstStudents = BoMemoService.getTabularReportInstance().GetBCA_P_StudentDetailPRES(sem.ToString(), year.ToString());
                    var LstConStudents = BoMemoService.getTabularReportInstance().GetBCA_P_StudentsConsDetails(Psem.ToString());
                    //var lsttotalpassed = BoMemoService.getTabularReportInstance().getTotalandPassed(course, year);


                    string PrevColcode = "";
                    bool ColPagebrk = false;

                    List<string> lstColCodes = lstStudents.OrderBy(x => x.collegecode.Trim()).Select(y => y.collegecode.Trim()).Distinct().ToList<string>();


                    int series = 0;
                    foreach (var colcode in lstColCodes)
                    {
                        List<string> HallticketNumbers = lstStudents.Where(y => y.collegecode.Trim() == colcode.Trim()).OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();
                        bool isExstudent = false;

                        ColPagebrk = true;
                        #region Htnonumbers
                        for (int i = 0; i < HallticketNumbers.Count; i++)
                        {


                            series++;
                            int totalSUbjects = 0;
                            int passedSubject = 0;
                            bool detained = false;
                            int rowcount = 0;
                            var lstStuns = lstStudents.Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                            var StudentConsInformatio = LstConStudents.Where(x => x.HTNO == HallticketNumbers[i]).ToList<ConsDataEntity>();

                            string HallTicket = lstStuns[0].HallTicketNumber;


                            string FN = lstStuns[0].FatherName == null ? "" : lstStuns[0].FatherName;
                            string SN = lstStuns[0].StudentName == null ? "" : lstStuns[0].StudentName;
                            string CC = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                            string EI = lstStuns[0].Ei == null ? "" : lstStuns[0].Ei;
                            string CollegeCode = lstStuns[0].collegecode == null ? "" : lstStuns[0].collegecode;
                            string nameformat = series + GetSpaces(5 - series.ToString().Length) + GetSpaces(1) + CollegeCode + GetSpaces(5 - CollegeCode.Length) + HallTicket + GetSpaces(12 - HallTicket.Length) + SN + GetSpaces(40 - SN.Length) + FN + GetSpaces(45 - FN.Length);
                            if (EI == "E")
                            {
                                isExstudent = true;
                            }
                            else
                            {
                                isExstudent = false;
                            }




                            if (lstStudents != null && lstStudents.Count > 0)
                            {
                                if (StudentConsInformatio != null && StudentConsInformatio.Count > 0)
                                {
                                    rowcount = rowcount + 1;
                                }
                                rowcount = rowcount + 5;

                            }
                            if ((PageBraker + rowcount) > 72)
                            {
                                int differ = 73 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }


                                PageBraker = addHeaderFooter(sw, 72, course, year.ToString(), sem.ToString());
                            }

                            if (PrevColcode != "" && PrevColcode.Trim() != colcode.Trim() && ColPagebrk == true)
                            {
                                ColPagebrk = false;
                                int differ = 73 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }


                                PageBraker = addHeaderFooter(sw, 72, course, year.ToString(), sem.ToString());

                            }


                            if (PrevColcode == "")
                            {
                                PrevColcode = colcode;
                            }

                            if (i == 0)
                            {
                                PageBraker = addHeaderFooter(sw, PageBraker, course, year.ToString(), sem.ToString());

                                sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            }
                            else
                            {

                                sw.WriteLine(nameformat.Truncate(113) + EI + GetSpaces(7 - EI.Length) + HallTicket + GetSpaces(10 - HallTicket.Length));

                            }
                            //List<TotalsubjectRecord> lstts = new List<TotalsubjectRecord>();

                            #region Cons
                            if (StudentConsInformatio != null & StudentConsInformatio.Count > 0)
                            {

                                var ConsDetails = StudentConsInformatio.FirstOrDefault();
                                //string Attemps = CalucateTotalAttemps(ConsDetails);

                                string subjectstringCONS = string.Empty;
                                string SubjectCONSMarks_U = string.Empty;
                                string subjectsMarksCONS_U = string.Empty;
                                string MarkformatCONS = GetSpaces(10);

                                string SubjectCONSMarks_S = string.Empty;
                                string subjectsMarksCONS_S = string.Empty;

                                string subjectCONSformt = "";

                                subjectstringCONS = "CONS." + GetSpaces(5 - "CONS.".Length);
                                SubjectCONSMarks_U = "   U" + GetSpaces(5 - "CONS.".Length);
                                SubjectCONSMarks_S = "   S" + GetSpaces(5 - "CONS.".Length) + GetSpaces(2);

                                #region Subject title 


                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P1, ConsDetails.A1);

                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P2, ConsDetails.A2);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P3, ConsDetails.A3);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P4, ConsDetails.A4);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P5, ConsDetails.A5);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P6, ConsDetails.A6);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P7, ConsDetails.A7);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P8, ConsDetails.A8);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P9, ConsDetails.A9);
                                subjectCONSformt = ConcatenateSubject(subjectCONSformt, ConsDetails.P10, ConsDetails.A10);

                                //if (ConsDetails.P1 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P1, status = ConsDetails.R1 });
                                //if (ConsDetails.P2 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P2, status = ConsDetails.R2 });
                                //if (ConsDetails.P3 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P3, status = ConsDetails.R3 });
                                //if (ConsDetails.P4 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P4, status = ConsDetails.R4 });
                                //if (ConsDetails.P5 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P5, status = ConsDetails.R5 });
                                //if (ConsDetails.P6 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P6, status = ConsDetails.R6 });
                                //if (ConsDetails.P7 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P7, status = ConsDetails.R7 });
                                //if (ConsDetails.P8 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P8, status = ConsDetails.R8 });
                                //if (ConsDetails.P9 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P9, status = ConsDetails.R9 });
                                //if (ConsDetails.P10 != null) lstts.Add(new TotalsubjectRecord() { subname = ConsDetails.P10, status = ConsDetails.R10 });


                                subjectstringCONS = "CONS." + GetSpaces(5 - "CONS.".Length) + GetSpaces(1) + subjectCONSformt;
                                sw.WriteLine(subjectstringCONS + GetSpaces(113 - subjectstringCONS.Length) + ConsDetails.RES + "   " + ConsDetails.SGPA);

                                if (ConsDetails.RES == "L")
                                {
                                    detained = true;
                                }
                                #endregion


                                #region Subject External Mark 
                                string SubjectExtCons = string.Empty;
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M1, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M2, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M3, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M4, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M5, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M6, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M7, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M8, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M9, string.Empty);
                                SubjectExtCons = ConcatenateSubjectMarks(SubjectExtCons, ConsDetails.M10, string.Empty);
                                SubjectCONSMarks_U = SubjectCONSMarks_U + GetSpaces(2) + SubjectExtCons;
                                sw.WriteLine(SubjectCONSMarks_U);

                                #endregion


                                //#region Subject Internal Mark 
                                //string SubjectintCons = string.Empty;

                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S1, ConsDetails.R1);

                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S2, ConsDetails.R2);


                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S3, ConsDetails.R3);

                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S4, ConsDetails.R4);

                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S5, ConsDetails.R5);

                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S6, ConsDetails.R6);

                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S7, ConsDetails.R7);

                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S8, ConsDetails.R8);


                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S9, ConsDetails.R9);


                                //SubjectintCons = ConcatenateSubjectMarks(SubjectintCons, ConsDetails.S10, ConsDetails.R10);

                                //SubjectCONSMarks_S = SubjectCONSMarks_S + SubjectintCons;

                                //sw.WriteLine(SubjectCONSMarks_S);

                                //#endregion

                            }
                            #endregion


                            #region  Pre
                            string subjectstringPRE = string.Empty;
                            string SubjectMarks_U = string.Empty;
                            string subjectsMarksPRE_U = string.Empty;
                            string MarkformatPRE = GetSpaces(10);
                            string Aademicstatus = string.Empty;



                            string SubjectMarks_S = string.Empty;
                            string subjectsMarksPRE_S = string.Empty;


                            if (lstStuns.Count > 0)
                            {
                                string subjectformt = "";
                                subjectstringPRE = "PRES." + GetSpaces(5 - "PRES.".Length) + GetSpaces(1);
                                subjectsMarksPRE_U = "   U" + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                if (!isExstudent)
                                {
                                    subjectsMarksPRE_S = "   S" + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                }
                                else
                                {
                                    subjectsMarksPRE_S = GetSpaces(5) + GetSpaces(5 - "PRES.".Length) + GetSpaces(2);
                                }
                                int subord = 1;

                                int count = 0;

                                //totalSUbjects = totalSUbjects + lstStuns.Count;
                                //passedSubject = passedSubject + lstStuns.Where(x => x.Status == "P").ToList<StudentInformation>().Count();



                                foreach (StudentInformation studentM in lstStuns.OrderBy(X => X.Order))
                                {

                                    #region total subs and paassed subs

                                    //TotalsubjectRecord obj = new TotalsubjectRecord() { subname = studentM.SubjectCode, status = studentM.Status };
                                    //if (lstts != null && lstts.Count > 0)
                                    //{
                                    //    if (lstts.Where(x => x.subname.ToLower() == studentM.SubjectCode.ToLower()).ToList().Count > 0)
                                    //    {
                                    //        foreach (var t in lstts)
                                    //        {
                                    //            if (t.subname == obj.subname)
                                    //            {
                                    //                t.status = studentM.Status;
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        lstts.Add(obj);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    lstts.Add(obj);

                                    //}
                                    #endregion
                                    string spaces = "";
                                    string umakspace = "";
                                    string intmarkspae = "";
                                    int ordr = studentM.Order;
                                    spaces = GetSpaces((ordr - subord) * 8);
                                    umakspace = GetSpaces((ordr - subord) * 8);
                                    intmarkspae = GetSpaces((ordr - subord) * 8);
                                    string subPReF = studentM.LeterGrade;
                                    if (studentM.LeterGrade == "AB")
                                    {
                                        subPReF = "F";
                                    }
                                    string intmark = string.Empty;
                                    if (isExstudent == false)
                                    {
                                        intmark = Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks) + GetSpaces(3 - Convert.ToString(studentM.InternalMarks == null ? "" : studentM.InternalMarks).Length) + " " + subPReF;
                                    }

                                    if (count > 0)
                                    {




                                        spaces = GetSpaces(spaces.Length - studentM.SubjectCode.Length);


                                        if (isExstudent == false)
                                        {
                                            umakspace = GetSpaces(umakspace.Length - studentM.ExernalMarks.ToString().Length);

                                            intmarkspae = GetSpaces(intmarkspae.Length - intmark.Length);
                                        }
                                        else
                                        {
                                            if (studentM.ExernalMarks.ToString() == "AB.")
                                            {
                                                umakspace = GetSpaces(umakspace.Length + 1 - (studentM.ExernalMarks.ToString() + " " + studentM.LeterGrade).Length);
                                            }
                                            else
                                            {
                                                umakspace = GetSpaces(umakspace.Length - (studentM.ExernalMarks.ToString().Truncate(3) + " " + studentM.LeterGrade).Length);
                                            }

                                        }
                                    }



                                    subjectformt = spaces + studentM.SubjectCode;
                                    subjectstringPRE = subjectstringPRE + subjectformt;

                                    if (isExstudent == false)
                                    {
                                        SubjectMarks_U = umakspace + studentM.ExernalMarks.ToString().Truncate(3);
                                    }
                                    else
                                    {
                                        SubjectMarks_U = umakspace + studentM.ExernalMarks.ToString().Truncate(4) + " " + subPReF;

                                    }

                                    subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;


                                    if (isExstudent == false)
                                    {
                                        intmark = intmarkspae + intmark;// + GetSpaces(7 - intmark.Length); ;
                                    }
                                    SubjectMarks_S = intmark;
                                    subjectsMarksPRE_S = subjectsMarksPRE_S + SubjectMarks_S;

                                    count++;
                                    subord = ordr;
                                }

                                string subjectsPF = string.Empty;
                                //if (objtotalaspassed == null || objtotalaspassed.Count == 0)
                                //{
                                //    objtotalaspassed = new List<TotalsubjectRecord>();
                                //    objtotalaspassed.Add(new TotalsubjectRecord() { TotalSubs = 0, PassedSubs = 0 });
                                //}
                                //var aa = objtotalaspassed.Sum(x => x.PassedSubs) + lstts.Where(x => x.status != "F").ToList().Count;
                                //var bb = objtotalaspassed.Sum(x => x.TotalSubs);

                                //var Paa = lstts.Where(x => x.status != "F" && x.subname != null).ToList().Count;
                                //var Pbb = lstts.Where(x => x.subname != null).ToList().Count;

                                //var expaa = lstStuns.Where(x => x.Status != "F").ToList().Count;

                                //if (isExstudent == false)
                                //{
                                //    subjectsPF = "<" + Paa + "/" + Pbb + ">" + "<" + (aa).ToString().Truncate(2) + "/" + (bb + lstts.Count).ToString().Truncate(2) + ">";
                                //}
                                //else
                                //{
                                //    subjectsPF = "<" + Paa + "/" + Pbb + ">" + "<" + (expaa + objtotalaspassed.Sum(x => x.PassedSubs)) + "/" + bb + ">";
                                //}

                                int external = lstStuns.Sum(x => x.ExernalMarks.ChangeINT());
                                int TotalMark = external + lstStuns.Sum(x => x.InternalMarks.ChangeINT());
                                sw.WriteLine(subjectstringPRE + GetSpaces(113 - subjectsMarksPRE_U.Length) + "  " + subjectsPF + " " + TotalMark);
                                string finalR = lstStuns[0].FinalResult;
                                if (finalR != "MALL.PRACT")
                                {
                                    if (detained == true && TotalMark > 0)
                                    {
                                        finalR = "PROMOTED";

                                    }
                                    else
                                    {
                                        finalR = lstStuns[0].FinalResult;

                                    }
                                }
                                else
                                {
                                    finalR = "MP";

                                }

                                if (finalR == "PROMOTED")
                                {
                                    finalR = "FAILED";
                                }

                                string Flotation = string.Empty;

                                //lstStuns.Where(x => x.Flotation.ToString() == "FL").ToList().Count > 0 ? "FL" : "";



                                if (lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count > 0)
                                {
                                    Flotation = "FL AC";
                                }
                                else
                                if (lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count == 0)
                                {
                                    Flotation = "FL";

                                }
                                else
                                if (lstStuns.Where(x => x.GRACE_MARKS2 != null).ToList().Count > 0 && lstStuns.Where(x => x.GRACE_MARKS != null).ToList().Count == 0)
                                {
                                    Flotation = "AC";

                                }

                                sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length) + "  " + finalR + "  " + lstStuns[0].SGPA + " " + Flotation);
                                if (isExstudent == false)
                                {
                                    sw.WriteLine(subjectsMarksPRE_S + GetSpaces(113 - subjectsMarksPRE_U.Length));

                                }

                                sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

                            }




                            #endregion

                            PageBraker = PageBraker + rowcount;

                        }

                        #endregion
                    }

                }

                // Write file contents on console.     
                using (StreamReader sr = System.IO.File.OpenText(fileNamedirectory + filename))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

                return Json(fileNamedirectory + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }


        }


        private JsonResult GenerateBA_L(string course)
        {

            try
            {
                string fileNamedirectory = @"D:\TabularReport\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + ".txt";

                // check for Directory
                if (!Directory.Exists(fileNamedirectory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(fileNamedirectory);
                }


                // file information 
                FileInfo fi = new FileInfo(fileNamedirectory + filename);
                // Check if file already exists. If yes, delete it.     
                if (System.IO.File.Exists(fileNamedirectory + filename))
                {
                    System.IO.File.Delete(fileNamedirectory + filename);
                }

                MemoPrintService BoMemoService = new MemoPrintService();

                List<BALConEntity> lstConInforamton = BoMemoService.getTabularReportInstance().GetBALConInformaion();
                List<BALPresEntity> lsiPreInformation = BoMemoService.getTabularReportInstance().GetBalPresInformation(course);

             


                #region Formatting CON Data


                #endregion

                var lstColCodes = lsiPreInformation.OrderBy(x => x.FK_CLGCODE.Trim()).Select(y => y.FK_CLGCODE.Trim()).Distinct().ToList<string>();
                using (StreamWriter sw = fi.CreateText())
                {
                    int series = 0;
                    string PrevColcode = "";
                    bool ColPagebrk = false;
                    foreach (var colcode in lstColCodes)
                    {
                        ColPagebrk = true;
                        List<string> HallticketNumbers = lsiPreInformation.Where(y => y.FK_CLGCODE.Trim() == colcode.Trim()).OrderBy(x => x.HTNO).Select(x => x.HTNO).Distinct().ToList<string>();
                        HallticketNumbers.Remove("015162606");
                        for (int i = 0; i < HallticketNumbers.Count; i++)
                        {
                            var lstPREStuns = lsiPreInformation.Where(x => x.HTNO == HallticketNumbers[i]).ToList<BALPresEntity>();
                            var lstcon = lstConInforamton.Where(x => x.HTNO == HallticketNumbers[i]).ToList<BALConEntity>();
                            string TotalMark = "";
                            string FinalResult = "";
                            int rowcount = 0;
                            //int PresTotal = lstPREStuns.Sum(x => x.MARKS.ChangeINT());
                            //string PreRsult = "";

                            //if (lstPREStuns.Where(x => x.RESULT == "F").ToList().Count > 0)
                            //{
                            //    PreRsult = "FAILED";
                            //}
                            //else
                            //{
                            //    PreRsult = "PASSED";
                            //}

                            string HallTIcket = HallticketNumbers[i];
                            List<BALEntity> lstEntity = new List<BALEntity>();
                            FinalResult = lstcon[0].FinalResult;
                            TotalMark = lstcon[0].Total;
                            #region condata formating
                            foreach (var con in lstcon)
                            {
                                //1year
                                lstEntity = CnrEntityVertical(lstEntity, con.M11, con.Y11, con.HTNO, con.P11, "", "I", 1);
                                lstEntity = CnrEntityVertical(lstEntity, con.PM11, con.PA11, con.HTNO, con.P11, "", "I", 1, true);
                                lstEntity = CnrEntityVertical(lstEntity, con.M12, con.Y12, con.HTNO, con.P12, "", "I", 2);
                                lstEntity = CnrEntityVertical(lstEntity, con.M13, con.Y13, con.HTNO, con.P13, "", "I", 3);
                                lstEntity = CnrEntityVertical(lstEntity, con.M14, con.Y14, con.HTNO, con.P14, "", "I", 4);
                                lstEntity = CnrEntityVertical(lstEntity, con.M15, con.Y15, con.HTNO, con.P15, "", "I", 5);
                                lstEntity = CnrEntityVertical(lstEntity, con.M16, con.Y16, con.HTNO, con.P16, "", "I", 6);
                                lstEntity = CnrEntityVertical(lstEntity, con.PM16, con.PA16, con.HTNO, con.P16, "", "I", 6, true);

                                //2year
                                lstEntity = CnrEntityVertical(lstEntity, con.M21, con.Y21, con.HTNO, con.P21, "", "II", 1);
                                lstEntity = CnrEntityVertical(lstEntity, con.PM21, con.PA21, con.HTNO, con.P21, "", "II", 1, true);
                                lstEntity = CnrEntityVertical(lstEntity, con.M22, con.Y22, con.HTNO, con.P22, "", "II", 2);
                                lstEntity = CnrEntityVertical(lstEntity, con.M23, con.Y23, con.HTNO, con.P23, "", "II", 3);
                                lstEntity = CnrEntityVertical(lstEntity, con.M24, con.Y24, con.HTNO, con.P24, "", "II", 4);
                                lstEntity = CnrEntityVertical(lstEntity, con.M25, con.Y25, con.HTNO, con.P25, "", "II", 5);
                                lstEntity = CnrEntityVertical(lstEntity, con.M26, con.Y26, con.HTNO, con.P26, "", "II", 6);
                                lstEntity = CnrEntityVertical(lstEntity, con.PM26, con.PA26, con.HTNO, con.P26, "", "II", 6, true);

                                //3year
                                lstEntity = CnrEntityVertical(lstEntity, con.M31, con.Y21, con.HTNO, con.P31, "", "III", 1);
                                lstEntity = CnrEntityVertical(lstEntity, con.M32, con.Y22, con.HTNO, con.P32, "", "III", 2);
                                lstEntity = CnrEntityVertical(lstEntity, con.M33, con.Y23, con.HTNO, con.P33, "", "III", 3);
                                lstEntity = CnrEntityVertical(lstEntity, con.M34, con.Y24, con.HTNO, con.P34, "", "III", 4);
                                lstEntity = CnrEntityVertical(lstEntity, con.M35, con.Y25, con.HTNO, con.P35, "", "III", 5);
                                lstEntity = CnrEntityVertical(lstEntity, con.M36, con.Y26, con.HTNO, con.P36, "", "III", 6);
                                lstEntity = CnrEntityVertical(lstEntity, con.PM36, con.PA26, con.HTNO, con.P36, "", "III", 6, true);
                                lstEntity = CnrEntityVertical(lstEntity, con.M37, con.Y27, con.HTNO, con.P37, "", "III", 7);

                            }
                            #endregion
                            lstEntity = CnrEntityVertical(lstEntity, lstPREStuns);

                          
                            series++;
                            int totalSUbjects = 0;
                            int passedSubject = 0;
                            bool detained = false;

                            string[] yrs = new string[] { "I", "II", "III" };

                            string FN = lstPREStuns[0].FName == null ? "" : lstPREStuns[0].FName;
                            string SN = lstPREStuns[0].FullName == null ? "" : lstPREStuns[0].FullName;
                            string CC = lstPREStuns[0].FK_COURSEID == null ? "" : lstPREStuns[0].FK_CLGCODE;
                            string EI = lstPREStuns[0].EI == null ? "" : lstPREStuns[0].EI;
                            string nameformat = series + GetSpaces(5 - series.ToString().Length) + GetSpaces(1) + colcode + GetSpaces(5 - CC.Length) + HallTIcket + GetSpaces(12 - HallTIcket.Length) + SN + GetSpaces(45 - SN.Length) + FN + GetSpaces(45 - FN.Length) + EI + " ";

                            #region Page break and Col break set up
                            string[] Types = new string[] { "CONS.", "PRES." };
                            foreach (string y in yrs)
                            {
                                foreach (var s in Types)
                                {
                                    if (lstEntity.Where(x => x.Year == y && x.Type == s).ToList().Count > 0)
                                    {
                                        rowcount = rowcount + 3;
                                    }
                                }

                            }
                            rowcount = rowcount + 3;


                            if ((PageBraker + rowcount) > 72)
                            {
                                int differ = 72 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }


                                yraddHeaderFooter(sw, 72, "BA (L)", "I,II,III", "");
                            }

                            if (PrevColcode != "" && PrevColcode.Trim() != colcode.Trim() && ColPagebrk == true)
                            {
                                ColPagebrk = false;
                                int differ = 72 - PageBraker;

                                for (int h = 0; h < differ; h++)
                                {
                                    sw.WriteLine(" ");
                                }


                                PageBraker = yraddHeaderFooter(sw, 72, "BA (L)", "I,II,III", "");

                            }


                            if (PrevColcode == "")
                            {
                                PrevColcode = colcode;
                            }
                            #endregion

                            if (series == 1)
                            {
                                PageBraker = yraddHeaderFooter(sw, 0, "BA (L)", "I,II,III", "");
                                sw.WriteLine(nameformat.Truncate(113) + GetSpaces(7 - EI.Length) + HallTIcket + GetSpaces(10 - HallTIcket.Length));
                            }
                            else
                            {
                                sw.WriteLine(nameformat.Truncate(113) + GetSpaces(7 - EI.Length) + HallTIcket + GetSpaces(10 - HallTIcket.Length));
                            }




                            foreach (string yr in yrs)
                            {
                                foreach (string ty in Types)
                                {

                                    int subord = 0;
                                    int count = 0;
                                    var lstpopulaateData = lstEntity.Where(x => x.Year == yr && x.Type == ty).ToList().OrderBy(y => y.Type);




                                    string subjectstring = string.Empty;
                                    string SubjectMarks_U = GetSpaces(10);
                                    string subjectsMarksPRE_U = string.Empty;
                                    string MarkformatPRE = GetSpaces(10);
                                    string Aademicstatus = string.Empty;
                                    subjectsMarksPRE_U = subjectsMarksPRE_U + MarkformatPRE;

                                    string SubjectMarks_S = string.Empty;
                                    string subjectsMarksPRE_S = string.Empty;
                                    string subjectformt = "";

                                    if (lstpopulaateData.Count() > 0)
                                    {
                                        subjectstring = ty + yr + GetSpaces(10 - (ty + yr).Length);
                                    }

                                    bool flag = false;
                                    bool abflag = false;

                                    if (lstpopulaateData.Count() > 0)
                                    {
                                        if (lstpopulaateData.ToList()[0].Order > 1 && lstpopulaateData.ToList()[0].Type == "PRES.")
                                        {

                                            flag = true;

                                            if (lstpopulaateData.Where(z => z.Marks == "AB").ToList().Count > 0)
                                            {
                                                abflag = true;
                                            }
                                        }
                                    }

                                    foreach (var sd in lstpopulaateData.OrderBy(z => z.Order))
                                    {
                                        string PA = string.Empty;
                                        string PM = string.Empty;

                                        if (sd.Practical == true)
                                        {
                                            if (sd.PMarks != null)
                                            {
                                                PM = sd.PMarks;
                                                PM.Truncate(3);
                                            }
                                            else
                                            {
                                                PM = GetSpaces(3);
                                            }


                                            if (sd.PYear != null)
                                            {
                                                PA = sd.PYear;
                                            }
                                            else
                                            {
                                                PA = GetSpaces(3);
                                            }

                                        }

                                        PM.Truncate(3);
                                        PA.Truncate(3);

                                        string spaces = "";
                                        string umakspace = "";

                                        int ordr = sd.Order;
                                        spaces = GetSpaces((ordr - subord) * 10);
                                        if (sd.Practical == true)
                                        {
                                            umakspace = GetSpaces((ordr - subord) * 10);
                                        }
                                        else
                                        { umakspace = GetSpaces(((ordr - subord) * 10) - 3); }



                                        if (flag == true)
                                        {
                                            //spaces = spaces + GetSpaces(4);
                                            ////umakspace = umakspace + GetSpaces(4);

                                            flag = false;

                                            if (abflag == true)
                                            {

                                                umakspace = umakspace + GetSpaces(1);
                                                abflag = false;
                                            }
                                        }


                                        string ADC = string.Empty;
                                        if (sd.Academic != null)
                                        {
                                            ADC = sd.Academic;
                                        }
                                        else
                                        {
                                            ADC = GetSpaces(3);
                                        }
                                        ADC.Truncate(3);

                                        string SDC = string.Empty;
                                        if (sd.subjectCode != null)
                                        {
                                            SDC = sd.subjectCode;
                                        }
                                        else
                                        {

                                            SDC = GetSpaces(3);
                                        }


                                        string re = string.Empty;
                                        if (sd.Result != null)
                                        {
                                            re = sd.Result;
                                            re = re + GetSpaces(3 - re.Length);

                                        }
                                        else
                                        {
                                            re = GetSpaces(3);
                                        }



                                        string Marks = string.Empty;
                                        if (sd.Marks != null)
                                        {
                                            if (sd.Marks == "AB")
                                            {
                                                sd.Marks = "AB.";
                                            }
                                            Marks = sd.Marks;

                                        }
                                        else
                                        {
                                            Marks = GetSpaces(3);
                                        }
                                        Marks.Truncate(3);



                                        string SubCode = SDC + " " + ADC.Truncate(3) + " " + PA.Truncate(3);





                                        string M = Marks.ToString() + " " + re + " " + PM;
                                        if (ty == "PRES." && ordr > 1 && lstpopulaateData.Count() == 1)
                                        {

                                            umakspace = GetSpaces(spaces.Length - M.Length + 1);
                                            subjectformt = GetSpaces(spaces.Length - SubCode.Length + 1) + SubCode;
                                        }
                                        else
                                        {
                                            umakspace = GetSpaces(spaces.Length - M.Length);
                                            subjectformt = GetSpaces(spaces.Length - SubCode.Length) + SubCode;
                                        }

                                        subjectstring = subjectstring + subjectformt;
                                        SubjectMarks_U = umakspace + M;

                                        subjectsMarksPRE_U = subjectsMarksPRE_U + SubjectMarks_U;
                                        subord = ordr;

                                    }
                                    if (lstpopulaateData.Count() > 0)
                                    {
                                        if (ty == "PRES.")
                                        {
                                            int PresTotal = lstpopulaateData.Sum(x => x.Marks.ChangeINT());
                                            string PreRsult = "";

                                            if (lstpopulaateData.Where(x => x.Result == "F").ToList().Count > 0)
                                            {
                                                PreRsult = "FAILED";
                                            }
                                            else
                                            {
                                                PreRsult = "PASSED";
                                            }
                                            sw.WriteLine(subjectstring + GetSpaces(110 - subjectstring.Length) + PresTotal + GetSpaces(5) + PreRsult);
                                        }
                                        else
                                        {
                                            sw.WriteLine(subjectstring + GetSpaces(113 - subjectstring.Length));
                                        }
                                        sw.WriteLine(subjectsMarksPRE_U + GetSpaces(113 - subjectsMarksPRE_U.Length));
                                        if (ty == "PRES." && yr == "III")
                                        {
                                            sw.WriteLine("         ............................................................................................");
                                        }
                                        else
                                        {
                                            
                                            sw.WriteLine("        ...........................................................................................................................");
                                        }
                                    }
                                }


                            }
                            sw.WriteLine("            " + GetSpaces(25) + "********* Part 2: " + TotalMark + GetSpaces(20) + FinalResult);
                            sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");


                            PageBraker = PageBraker + rowcount;
                        }

                    }

                }



                return Json(fileNamedirectory + filename, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public void ShowFile(string filename)
        {
            //Clears all content output from Buffer Stream
            Response.ClearContent();
            //Clears all headers from Buffer Stream
            Response.ClearHeaders();
            //Adds an HTTP header to the output stream
            Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
            //Gets or Sets the HTTP MIME type of the output stream
            Response.ContentType = "application/pdf";
            //Writes the content of the specified file directory to an HTTP response output stream as a file block
            Response.WriteFile(filename);
            //sends all currently buffered output to the client
            Response.Flush();
            //Clears all content output from Buffer Stream
            Response.Clear();
        }

        private string GetSpaces(int totalLength)
        {
            string result = string.Empty;
            for (int i = 0; i < totalLength; i++)
            {
                result += " ";
            }
            return result;
        }


        [HttpPost]
        public JsonResult GetCourseDetials()
        {
            //get the course details
            MemoPrintService MemoPrintService = new MemoPrintService();
            var lstCourses = MemoPrintService.GetCourseInstance().GetCourseDetails();
            return Json(lstCourses, JsonRequestBehavior.AllowGet);

        }


        private string ConcatenateSubject(string appenedstring, string subject, string yearPass)
        {
            if (subject != null && subject.Length > 0)
            {
                string sub = subject + " " + yearPass;//.CheckNumeric(2);
                appenedstring = appenedstring + sub + GetSpaces(8 - sub.Length);
                return appenedstring;
            }
            else
            {
                return appenedstring;
            }
        }

        private string ConcatenateSubjectMarks(string appenedstring, string subjectMarks, string Result = "")
        {
            if (subjectMarks != null && subjectMarks.Length > 0)
            {
                string sub = subjectMarks + " " + Result;
                appenedstring = appenedstring + sub + GetSpaces(8 - sub.Length);
                return appenedstring;
            }
            else
            {
                return appenedstring;
            }

        }




        private int addHeaderFooter(StreamWriter sw, int rowcount, string course, string year, string sem, bool cchange = false)
        {
            if (rowcount == 0)
            {

                //sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM.  EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(43 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(44 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = PageBraker + 8;

            }
            else if (rowcount == 72)
            {

                rowcount = 0;
                PageNumber++;
                //sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                // sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM.  EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = 8;
                //addHeaderFooter(sw, rowcount, course, year, sem);


            }
            else { PageBraker++; }
            return PageBraker;
        }


        private int yraddHeaderFooter(StreamWriter sw, int rowcount, string course, string year, string sem, bool cchange = false)
        {
            if (rowcount == 0)
            {

                //sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR SUPPL. EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(43 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(44 - "FATHER'S NAME".Length) + "TOTAL    RESULT   ");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = PageBraker + 8;

            }
            else if (rowcount == 72)
            {

                rowcount = 0;
                PageNumber++;
                //sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                // sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
                sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR SUPPL. EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
                sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "TOTAL    RESULT   ");
                sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

                PageBraker = 8;
                //addHeaderFooter(sw, rowcount, course, year, sem);


            }
            else { PageBraker++; }
            return PageBraker;
        }


        //private int addHeaderFooter(StreamWriter sw, int rowcount, string course, string year, string sem, bool cchange= false)
        //{
        //    if (rowcount == 0)
        //    {


        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");

        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");
        //        sw.WriteLine(("                                  TABULATION lIST OF " + course + " " + year + " YEAR " + sem + " SEM.  EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
        //        sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(43 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(44 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
        //        sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

        //        PageBraker = PageBraker + 9;

        //    }
        //    else if (rowcount == 72)
        //    {

        //        rowcount = 0;
        //        PageNumber++;

        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");

        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");
        //        sw.WriteLine(" ");
        //        // sw.WriteLine(("                                  TABULATION LIST OF " + course + " " + year + " YEAR " + sem + " SEM                              ").Truncate(132));
        //        sw.WriteLine(("                                  TABULATION lIST OF " + course + " " + year + " YEAR " + sem + " SEM.  EXAMINATIONS HELD IN Nov., " + (DateTime.Now.Year - 1).ToString() + "                           ").Truncate(132));
        //        sw.WriteLine("SLNO." + GetSpaces(5) + "HTNO" + GetSpaces(15 - "HTNO".Length) + "NAME" + GetSpaces(45 - "NAME".Length) + "FATHER'S NAME" + GetSpaces(45 - "FATHER'S NAME".Length) + "RES/TOT. SGPA/CGPA");
        //        sw.WriteLine(("PAGE NO:" + PageNumber + "-------------------------------------------------------------------------------------------------------Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));

        //        PageBraker = 9;
        //        //addHeaderFooter(sw, rowcount, course, year, sem);


        //    }
        //    else { PageBraker++; }
        //    return PageBraker;
        //}



        private List<BALEntity> CnrEntityVertical(List<BALEntity> lstEntity, string Marks, string Academic, string HTNO, string SubjectCode, string EI, string year, int order, bool isPractical = false)
        {
            if (isPractical == false)
            {
                lstEntity.Add(new BALEntity() { HTNO = HTNO, Marks = Marks, subjectCode = SubjectCode, Academic = Academic, Order = order, Year = year, Type = "CONS." });
                return lstEntity;
            }
            else
            {
                var itm = lstEntity.Where(x => x.Year == year && x.Order == order).ToList();
                if (itm != null && itm.Count > 0)
                {
                    itm[0].Practical = true;
                    itm[0].PMarks = Marks;
                    itm[0].PYear = Academic;
                    itm[0].Order = order;
                }

                return lstEntity;
            }
        }

        private List<BALEntity> CnrEntityVertical(List<BALEntity> lstEntity, List<BALPresEntity> lstPRES)
        {
            foreach (var pre in lstPRES)
            {

                string yr = "";
                if (pre.FK_YEAR == "1")
                { yr = "I"; }

                if (pre.FK_YEAR == "2")
                { yr = "II"; }


                if (pre.FK_YEAR == "3")
                { yr = "III"; }


                var ent = lstEntity.Where(x => x.subjectCode == pre.SUB && x.HTNO == pre.HTNO && x.Year == yr).ToList();
                int order = 0;
                bool ispracical = false;
                if (ent != null && ent.Count > 0)
                {
                    order = ent[0].Order;
                    ispracical = ent[0].Practical;
                }

                lstEntity.Add(new BALEntity()
                {
                    Practical = ispracical,
                    HTNO = pre.HTNO,
                    Marks = pre.MARKS,
                    Academic = pre.Academic,
                    subjectCode = pre.SUB,
                    Order = order,
                    Year = yr,
                    Type = "PRES.",
                    EI = pre.EI,
                    Result = pre.RESULT,
                    PTotal = pre.TOTAL_MARKS

                });
            }
            return lstEntity;
        }

    }
}