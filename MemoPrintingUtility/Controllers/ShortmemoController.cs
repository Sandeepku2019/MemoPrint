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
    public class ShortmemoController : Controller
    {
        // GET: Shortmemo
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GenerateTabularSemReport(int Psem, string course)
        {

            if (course == "BCA(P)")
            {
                return GenrateShortMemo_BCAP(Psem, "BCA");
            }
            else if (course == "BA (L)")
            {

                return GenrateShortMemo_BAL("BAL");
            }
            else
            {
                return GenrateShortMemo(Psem, course);
            }

        }



        private JsonResult GenrateShortMemo_BAL(string course)
        {


            string hn = "";
            try
            {

                string fileNamedirectory = @"D:\ShortMemo\";
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
                List<BALPresEntity> lstPreInformation = BoMemoService.getTabularReportInstance().GetBalPresInformation(course);


                List<BALSubjectInformation> LstBALSubjects = BoMemoService.getTabularReportInstance().GetBALSubjectInformation(course);
                string[] yrs = new string[] { "1", "2", "3" };


                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {


                    foreach (string y in yrs)
                    {
                        List<BALPresEntity> lstPre = lstPreInformation.Where(z => z.FK_YEAR == y).ToList();


                        List<string> HallticketNumbers = lstPre.OrderBy(x => x.HTNO).Select(x => x.HTNO).Distinct().ToList<string>();

                        // Making even number for  side by side print 
                        if (HallticketNumbers.Count % 2 == 0)
                        {

                        }
                        else
                        {
                            HallticketNumbers.Add("0");
                        }

                        for (int i = 0; i < HallticketNumbers.Count; i++)
                        {


                            hn = HallticketNumbers[i];
                            if (i == 0)
                            {
                                sw.WriteLine("");
                                sw.WriteLine("");
                                sw.WriteLine("");
                            }
                            var lstStunsfirst = lstPre.Where(x => x.HTNO == HallticketNumbers[i]).ToList<BALPresEntity>();
                            var lstStubScon = lstPre.Where(x => x.HTNO == HallticketNumbers[i + 1]).ToList<BALPresEntity>();







                            if (lstStunsfirst == null || lstStunsfirst.Count == 0)
                            { lstStunsfirst = new List<BALPresEntity>(); lstStunsfirst.Add(new BALPresEntity() { SubjectName = "", MaxMark = "", MinMark = "", MARKS = "", RESULT = "" }); }
                            if (lstStubScon == null || lstStubScon.Count == 0)
                            {
                                lstStubScon = new List<BALPresEntity>();
                                lstStubScon.Add(new BALPresEntity() { SubjectName = "", MaxMark = "", MinMark = "", MARKS = "", RESULT = "" });
                            }


                            foreach (var fs in lstStunsfirst)
                            {
                                var SubjectInformation1 = LstBALSubjects.Where(x => x.SubjectCode.Trim() == fs.SUB.Trim());


                                fs.SubjectName = SubjectInformation1.FirstOrDefault().SubjectName;
                                fs.SubjectCode = SubjectInformation1.FirstOrDefault().SubjectCode;
                                fs.MaxMark = SubjectInformation1.FirstOrDefault().MaxMark;
                                fs.MinMark = SubjectInformation1.FirstOrDefault().MinMark;

                                if (fs.RESULT == "F")
                                {
                                    fs.RESULT = "FAIL";
                                }

                                if (fs.RESULT == "P")
                                {
                                    fs.RESULT = "PASS";
                                }

                                if (fs.MARKS == "AB")
                                {
                                    fs.RESULT = "AB.";
                                }
                            }

                            foreach (var ss in lstStubScon)
                            {
                                if (ss.HTNO != null)
                                {
                                    var SubjectInformation2 = LstBALSubjects.Where(x => x.SubjectCode.Trim() == ss.SUB.Trim());
                                    ss.SubjectName = SubjectInformation2.FirstOrDefault().SubjectName;
                                    ss.SubjectCode = SubjectInformation2.FirstOrDefault().SubjectCode;
                                    ss.MaxMark = SubjectInformation2.FirstOrDefault().MaxMark;
                                    ss.MinMark = SubjectInformation2.FirstOrDefault().MinMark;
                                }
                                if (ss.RESULT == "F")
                                {
                                    ss.RESULT = "FAIL";
                                }

                                if (ss.RESULT == "")
                                {
                                    ss.RESULT = "PASS";
                                }

                                if (ss.MARKS == "AB")
                                {
                                    ss.RESULT = "AB.";
                                }
                            }

                            string gap = GetSpaces(5);
                            string HallTicket_1 = lstStunsfirst[0].HTNO == null ? "" : lstStunsfirst[0].HTNO;
                            string FN_1 = lstStunsfirst[0].FName == null ? "" : lstStunsfirst[0].FName;
                            string SN_1 = lstStunsfirst[0].FullName == null ? "" : lstStunsfirst[0].FullName;
                            string CC_1 = lstStunsfirst[0].FK_CLGCODE == null ? "" : lstStunsfirst[0].FK_CLGCODE;

                            if (course == "BAL")
                            {
                                course = "BA (L)";
                            }
                            string CourseDetails = GetSpaces(10) + course + " " + y + "  Year Examination   NOV/DEC ." + (DateTime.Now.Year - 1).ToString();
                            CourseDetails = CourseDetails + GetSpaces(55 - CourseDetails.Length) + DateTime.Now.ToString("dd-MM-yyyy");


                            string HallTicket_2 = lstStubScon[0].HTNO == null ? "" : lstStubScon[0].HTNO;
                            string FN_2 = lstStubScon[0].FName == null ? "" : lstStubScon[0].FName;
                            string SN_2 = lstStubScon[0].FullName == null ? "" : lstStubScon[0].FullName;
                            string CC_2 = lstStubScon[0].FK_CLGCODE == null ? "" : lstStubScon[0].FK_CLGCODE;

                            string CondidateRow = GetSpaces(10) + SN_1 + GetSpaces(55 - SN_1.Length) + gap + GetSpaces(10) + SN_2 + GetSpaces(55 - SN_2.Length);
                            string FatherRow = GetSpaces(10) + FN_1 + GetSpaces(45 - FN_1.Length) + HallTicket_1 + GetSpaces(10 - HallTicket_1.Length) + gap;
                            FatherRow = FatherRow + GetSpaces(10) + FN_2 + GetSpaces(45 - FN_2.Length) + HallTicket_2 + GetSpaces(10 - HallTicket_2.Length);

                            string ColCodeRow = CC_1 + GetSpaces(65 - CC_1.Length) + gap + CC_2 + GetSpaces(65 - CC_2.Length);




                            sw.WriteLine(ColCodeRow);
                            string CourseRows = CourseDetails + gap + CourseDetails;
                            sw.WriteLine(CourseRows);
                            sw.WriteLine(CondidateRow);
                            sw.WriteLine(FatherRow);


                            sw.WriteLine("");
                            sw.WriteLine("");

                            int rowCount = 0;
                            if (lstStunsfirst.Count > lstStubScon.Count)
                            {
                                rowCount = lstStunsfirst.Count;
                            }
                            else
                            {
                                rowCount = lstStubScon.Count;
                            }

                            int memorows = 0;
                            for (int j = 0; j < 19; j++)
                            {
                                if (memorows < 17)
                                {
                                    if (lstStunsfirst.Count - 1 < j)
                                    {
                                        lstStunsfirst.Add(new BALPresEntity() { SubjectName = "", MaxMark = "", MinMark = "", MARKS = "", RESULT = "" });
                                    }
                                    if (lstStubScon.Count - 1 < j)
                                    {
                                        lstStubScon.Add(new BALPresEntity() { SubjectName = "", MaxMark = "", MinMark = "", MARKS = "", RESULT = "" }); memorows++;
                                    }

                                    if (lstStunsfirst[j].SubjectName.Length < 45 && lstStubScon[j].SubjectName.Length < 45)
                                    {
                                        SingleLingSubjectBAL(sw, j, lstStunsfirst, lstStubScon);
                                    }
                                    else
                                    {
                                        TwolinesubjectBAL(sw, j, lstStunsfirst, lstStubScon);
                                        memorows++;
                                        memorows++;

                                    }
                                }

                            }

                            string SGPARow = string.Empty;
                            //first record
                            string FirstFinalResult = string.Empty;
                            if (lstStunsfirst.Where(x => x.RESULT == "F").ToList().Count() > 0)
                            {
                                FirstFinalResult = "FAILED";
                            }
                            else
                            {
                                FirstFinalResult = "PASSED";
                            }


                            if (FirstFinalResult == "PASSED" || FirstFinalResult == "COMPLETED" || FirstFinalResult == "PROMOTED")
                            {
                                string sg = lstStunsfirst[0].TOTAL_MARKS == null ? "***" : lstStunsfirst[0].TOTAL_MARKS;
                                SGPARow = GetSpaces(10) + sg + GetSpaces(55 - sg.Length);
                            }
                            else
                            {
                                SGPARow = GetSpaces(10) + "***" + GetSpaces(65 - "***".Length);
                            }
                            SGPARow = SGPARow + gap;


                            //Second recod
                            string SecondFinalResult = string.Empty;
                            if (lstStubScon.Where(x => x.RESULT == "F").ToList().Count() > 0)
                            {
                                SecondFinalResult = "FAILED";
                            }
                            else
                            {
                                SecondFinalResult = "PASSED";
                            }

                            if (SecondFinalResult == "PASSED" || SecondFinalResult == "COMPLETED" || SecondFinalResult == "PROMOTED")
                            {
                                string sg1 = lstStubScon[0].TOTAL_MARKS == null ? "***" : lstStubScon[0].TOTAL_MARKS;
                                SGPARow = SGPARow + GetSpaces(10) + sg1 + GetSpaces(55 - sg1.Length);
                            }
                            else
                            {
                                SGPARow = SGPARow + GetSpaces(10) + "***" + GetSpaces(65 - "***".Length);
                            }

                            sw.WriteLine(SGPARow);



                            string statusrow = GetSpaces(10) + FirstFinalResult + GetSpaces(55 - FirstFinalResult.Length) + gap;
                            statusrow = statusrow + GetSpaces(10) + SecondFinalResult + GetSpaces(55 - SecondFinalResult.Length);

                            sw.WriteLine(statusrow);
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");


                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            i++;


                        }
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
                if (hn != "0")
                {
                }
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }
        }



        private JsonResult GenrateShortMemo_BCAP(int Psem, string course)
        {


            string hn = "";
            try
            {

                ///// Creting Notepad 

                string fileNamedirectory = @"D:\ShortMemo\";
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

                string Syear = string.Empty;
                string Ssem = string.Empty;
                int yr = 0;
                int sm = 0;
                if (Psem == 11)
                {
                    Syear = "I";
                    yr = 1;
                    sm = 1;
                    Ssem = "I";

                }

                if (Psem == 12)
                {
                    Syear = "I";
                    yr = 1;
                    sm = 2;
                    Ssem = "II";

                }

                if (Psem == 21)
                {
                    Syear = "II";
                    yr = 2;
                    sm = 1;
                    Ssem = "I";

                }

                if (Psem == 22)
                {
                    Syear = "II";
                    yr = 2;
                    sm = 1;
                    Ssem = "II";

                }

                if (Psem == 31)
                {
                    Syear = "III";
                    yr = 3;
                    Ssem = "I";
                    sm = 1;
                }

                if (Psem == 32)
                {
                    Syear = "III";
                    yr = 3;
                    Ssem = "II";
                    sm = 1;
                }

                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {
                    //
                    var lstStudents = BoMemoService.getTabularReportInstance().GetBCA_P_StudentDetailPRES(sm.ToString(), yr.ToString());
                    var lstBCASubject = BoMemoService.getTabularReportInstance().GetBCAPSubjectInformation("BCA");

                    List<string> HallticketNumbers = lstStudents.OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();

                    List<string> MallPactMember = BoMemoService.getTabularReportInstance().GetMallPractHtno(course, Psem, sm, yr).Select(x => x.HallTicketNumber).Distinct().ToList<string>();

                    foreach (var malstuden in MallPactMember)
                    {
                        HallticketNumbers.Remove(malstuden);
                    }


                    // Making even number for  side by side print 


                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {


                        hn = HallticketNumbers[i];

                        if (hn == "086155038")
                        {

                        }
                        if (i == 0)
                        {
                            //sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                        }
                        var lstStunsfirst = lstStudents.OrderBy(y => y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();




                        string HallTicket_1 = lstStunsfirst[0].HallTicketNumber == null ? "" : lstStunsfirst[0].HallTicketNumber;
                        string FN_1 = lstStunsfirst[0].FatherName == null ? "" : lstStunsfirst[0].FatherName;
                        string SN_1 = lstStunsfirst[0].StudentName == null ? "" : lstStunsfirst[0].StudentName;
                        string CC_1 = lstStunsfirst[0].collegecode == null ? "" : lstStunsfirst[0].collegecode;

                        string CourseDetails = GetSpaces(12) + course + " " + Syear + " YR " + Ssem + " SEM   NOV/DEC ." + (DateTime.Now.Year - 1).ToString();
                        CourseDetails = CourseDetails + GetSpaces(73 - CourseDetails.Length) + DateTime.Now.ToString("dd-MM-yyyy");




                        string CondidateRow = GetSpaces(12) + SN_1 + GetSpaces(61 - SN_1.Length);
                        string FatherRow = GetSpaces(12) + FN_1;
                        FatherRow = FatherRow + GetSpaces(73 - FatherRow.Length) + HallTicket_1 + GetSpaces(10 - HallTicket_1.Length);


                        //string ColCodeRow = CC_1 + GetSpaces(65 - CC_1.Length) + gap + CC_1 + GetSpaces(65 - CC_1.Length);




                        //sw.WriteLine(ColCodeRow);

                        sw.WriteLine(CourseDetails);
                        sw.WriteLine(CondidateRow);
                        sw.WriteLine(FatherRow);


                        sw.WriteLine("");
                        sw.WriteLine("");

                        sw.WriteLine("");
                        //sw.WriteLine("");

                        int rowCount = 0;


                        int memorows = 0;
                        for (int j = 0; j < 28; j++)
                        {
                            if (memorows < 28)
                            {
                                if (lstStunsfirst.Count - 1 < j)
                                {
                                    lstStunsfirst.Add(new StudentInformation() { SubjectName = "", InternalMarks = "", ExernalMarks = "", SubjectExternalMarks = "", SubjectInternalMarks = "", FinalResult = "" });
                                }


                                if (lstStunsfirst[j].SubjectName.Length <= 39)
                                {
                                    SingleLingSubjectBCP(sw, j, lstStunsfirst, lstBCASubject);
                                }

                            }

                        }

                        int tM = Convert.ToInt32(lstStunsfirst[0].TotalMarks);
                        string totalMarks = GetSpaces(12) + tM.ToString() + "(" + tM.NumberToWords().ToUpper() + ")";
                        sw.WriteLine(totalMarks);
                        sw.WriteLine(GetSpaces(12) + lstStunsfirst[0].FinalResult);





                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");


                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        //i++;


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
                if (hn != "0")
                {
                }
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        private void SingleLingSubjectBCP(StreamWriter sw, int i, List<StudentInformation> lstStunsfirst, List<BCAPSubjectINformation> lstBCASUbject)
        {

            bool flag1 = false;
            bool flag2 = false;
            string space = " ";
            string dotedrow = "- -  - -  - -  - -  - -  - -  - -";

            if (lstStunsfirst[i].SubjectName.Length < 51 && lstStunsfirst[i].SubjectName.Length > 0)
            {

                string MinMarksToPass = "";
                var subinfo = lstBCASUbject.Where(x => x.Year == lstStunsfirst[i].Year && x.Sem == lstStunsfirst[i].Sem && x.SubjectCode == lstStunsfirst[i].SubjectCode).ToList();
                if (subinfo.Count() > 0)
                {
                    MinMarksToPass = subinfo[0].MinMark;
                }

                string SubjectAndGrades = string.Empty;
                if (i < lstStunsfirst.Count)
                {
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].SubjectName + GetSpaces(50 - lstStunsfirst[i].SubjectName.Length);

                    string SubExternal = string.Empty;
                    if (lstStunsfirst[i].SubjectExternalMarks != null)
                    {
                        SubExternal = lstStunsfirst[i].SubjectExternalMarks;
                    }
                    string External = string.Empty;
                    if (lstStunsfirst[i].ExernalMarks != null)
                    {
                        External = lstStunsfirst[i].ExernalMarks;
                    }
                    SubjectAndGrades = SubjectAndGrades + SubExternal.ChangeINT().ToString("D"+3) + GetSpaces(4 - SubExternal.Length);
                    SubjectAndGrades = SubjectAndGrades + External + GetSpaces(4 - External.Length);



                    string Subinternal = string.Empty;
                    if (lstStunsfirst[i].SubjectInternalMarks != null)
                    {
                        Subinternal = lstStunsfirst[i].SubjectInternalMarks.ChangeINT().ToString("D" + 3);
                    }
                    else
                    { Subinternal = "- -"; }


                    string inernal = string.Empty;
                    if (lstStunsfirst[i].InternalMarks != null)
                    {
                        inernal = lstStunsfirst[i].InternalMarks;
                    }
                    else
                    { inernal = "- -"; }

                    SubjectAndGrades = SubjectAndGrades + Subinternal + GetSpaces(4 - Subinternal.Length);
                    SubjectAndGrades = SubjectAndGrades + inernal + GetSpaces(5 - inernal.Length);

                    int totalMarks = Convert.ToInt32(SubExternal.ChangeINT()) + Convert.ToInt32(Subinternal.ChangeINT());
                    int OccupiedMarks = Convert.ToInt32(External.ChangeINT()) + Convert.ToInt32(inernal.ChangeINT());

                    string TM = "";
                    //if (totalMarks == 0)
                    //{
                    //    TM = "";
                    //}
                    //else
                    //{
                    if (Subinternal == "- -")
                    { TM = SubExternal; }
                    else
                    {
                        totalMarks.ToString("D" + 3);
                    }
                    //}


                    SubjectAndGrades = SubjectAndGrades + TM + GetSpaces(5 - TM.Length);
                    SubjectAndGrades = SubjectAndGrades + OccupiedMarks.ToString("D" + 3).Truncate(3) + GetSpaces(6 - OccupiedMarks.ToString("D" + 3).Truncate(3).Length);

                    string SubjectResult = string.Empty;

                    if (External.ChangeINT() > 0)
                    {
                        if (Convert.ToInt32(External.ChangeINT()) > Convert.ToInt32(MinMarksToPass))
                        {
                            SubjectResult = "PASS";

                        }
                        else
                        {
                            SubjectResult = "FAIL";
                        }
                    }
                    else
                    {
                        SubjectResult = "FAIL";
                    }


                    SubjectAndGrades = SubjectAndGrades + SubjectResult.ToString() + GetSpaces(4 - SubjectResult.Length);

                }
                else
                {
                    if (flag1 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(50 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);


                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);


                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);

                        flag1 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(100);
                    }
                }

                sw.WriteLine(SubjectAndGrades);
            }
            else
            {
                sw.WriteLine("");
            }


        }

        private JsonResult GenrateShortMemo(int Psem, string course)
        {


            string hn = "";
            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/Tab");
                string fileNamedirectory = @"D:\ShortMemo\";
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

                string Syear = string.Empty;
                string Ssem = string.Empty;
                int yr = 0;
                int sm = 0;
                if (Psem == 11)
                {
                    Syear = "I";
                    yr = 1;
                    sm = 1;
                    Ssem = "I";

                }
                if (Psem == 21)
                {
                    Syear = "II";
                    yr = 2;
                    sm = 1;
                    Ssem = "I";

                }

                if (Psem == 31)
                {
                    Syear = "III";
                    yr = 3;
                    Ssem = "I";
                    sm = 1;
                }

                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {
                    //
                    var lstStudents = BoMemoService.getTabularReportInstance().GetStudentDetail(course, Psem, sm, yr);
                    //var LstConStudents = BoMemoService.getTabularReportInstance().GetStudentsConsDetails(course, Psem);

                    int TotalSubjectAttemt = 0;
                    int TotalSubjectpass = 0;
                    int rowcount = 0;
                    List<string> HallticketNumbers = lstStudents.OrderBy(x => x.HallTicketNumber).Select(x => x.HallTicketNumber).Distinct().ToList<string>();

                    List<string> MallPactMember = BoMemoService.getTabularReportInstance().GetMallPractHtno(course, Psem, sm, yr).Select(x => x.HallTicketNumber).Distinct().ToList<string>();

                    foreach (var malstuden in MallPactMember)
                    {
                        HallticketNumbers.Remove(malstuden);
                    }


                    // Making even number for  side by side print 
                    if (HallticketNumbers.Count % 2 == 0)
                    {

                    }
                    else
                    {
                        HallticketNumbers.Add("0");
                    }

                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {


                        hn = HallticketNumbers[i];
                        if (i == 0)
                        {
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                        }
                        var lstStunsfirst = lstStudents.OrderBy(y => y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                        var lstStubScon = lstStudents.OrderBy(y => y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i + 1]).ToList<StudentInformation>();


                        if (lstStunsfirst == null || lstStunsfirst.Count == 0)
                        { lstStunsfirst = new List<StudentInformation>(); lstStunsfirst.Add(new StudentInformation() { SubjectName = "", Credits = "", Status = "", SGPA = "", LeterGrade = "" }); }
                        if (lstStubScon == null || lstStubScon.Count == 0)
                        {
                            lstStubScon = new List<StudentInformation>();
                            lstStubScon.Add(new StudentInformation() { SubjectName = "", Credits = "", Status = "", SGPA = "", LeterGrade = "", FinalResult = "" });
                        }


                        string gap = GetSpaces(5);
                        string HallTicket_1 = lstStunsfirst[0].HallTicketNumber == null ? "" : lstStunsfirst[0].HallTicketNumber;
                        string FN_1 = lstStunsfirst[0].FatherName == null ? "" : lstStunsfirst[0].FatherName;
                        string SN_1 = lstStunsfirst[0].StudentName == null ? "" : lstStunsfirst[0].StudentName;
                        string CC_1 = lstStunsfirst[0].collegecode == null ? "" : lstStunsfirst[0].collegecode;

                        string CourseDetails = GetSpaces(10) + course + " " + Syear + "YR / " + Ssem + "SEM   NOV/DEC ." + (DateTime.Now.Year - 1).ToString();
                        CourseDetails = CourseDetails + GetSpaces(55 - CourseDetails.Length) + DateTime.Now.ToString("dd-MM-yyyy");


                        string HallTicket_2 = lstStubScon[0].HallTicketNumber == null ? "" : lstStubScon[0].HallTicketNumber;
                        string FN_2 = lstStubScon[0].FatherName == null ? "" : lstStubScon[0].FatherName;
                        string SN_2 = lstStubScon[0].StudentName == null ? "" : lstStubScon[0].StudentName;
                        string CC_2 = lstStubScon[0].collegecode == null ? "" : lstStubScon[0].collegecode;

                        string CondidateRow = GetSpaces(10) + SN_1 + GetSpaces(55 - SN_1.Length) + gap + GetSpaces(10) + SN_2 + GetSpaces(55 - SN_2.Length);
                        string FatherRow = GetSpaces(10) + FN_1 + GetSpaces(45 - FN_1.Length) + HallTicket_1 + GetSpaces(10 - HallTicket_1.Length) + gap;
                        FatherRow = FatherRow + GetSpaces(10) + FN_2 + GetSpaces(45 - FN_2.Length) + HallTicket_2 + GetSpaces(10 - HallTicket_2.Length);

                        string ColCodeRow = CC_1 + GetSpaces(65 - CC_1.Length) + gap + CC_2 + GetSpaces(65 - CC_2.Length);




                        sw.WriteLine(ColCodeRow);
                        string CourseRows = CourseDetails + gap + CourseDetails;
                        sw.WriteLine(CourseRows);
                        sw.WriteLine(CondidateRow);
                        sw.WriteLine(FatherRow);


                        sw.WriteLine("");
                        sw.WriteLine("");

                        int rowCount = 0;
                        if (lstStunsfirst.Count > lstStubScon.Count)
                        {
                            rowCount = lstStunsfirst.Count;
                        }
                        else
                        {
                            rowCount = lstStubScon.Count;
                        }

                        int memorows = 0;
                        for (int j = 0; j < 19; j++)
                        {
                            if (memorows < 17)
                            {
                                if (lstStunsfirst.Count - 1 < j)
                                {
                                    lstStunsfirst.Add(new StudentInformation() { SubjectName = "", Credits = "", Status = "", SGPA = "", LeterGrade = "" });
                                }
                                if (lstStubScon.Count - 1 < j)
                                {
                                    lstStubScon.Add(new StudentInformation() { SubjectName = "", Credits = "", Status = "", SGPA = "", LeterGrade = "" }); memorows++;
                                }

                                if (lstStunsfirst[j].SubjectName.Length < 55 && lstStubScon[j].SubjectName.Length < 55)
                                {
                                    SingleLingSubject(sw, j, lstStunsfirst, lstStubScon);
                                }
                                else
                                {
                                    Twolinesubject(sw, j, lstStunsfirst, lstStubScon);
                                    memorows++;
                                    memorows++;

                                }
                            }

                        }

                        string SGPARow = string.Empty;
                        //first record
                        string FirstFinalResult = string.Empty;
                        if (lstStunsfirst[0].Ei == "E" && lstStunsfirst[0].FinalResult == "PROMOTED")
                        {
                            FirstFinalResult = "FAILED";
                        }
                        else
                        {
                            FirstFinalResult = lstStunsfirst[0].FinalResult;
                        }


                        if (FirstFinalResult == "PASSED" || FirstFinalResult == "COMPLETED" || FirstFinalResult == "PROMOTED")
                        {
                            string sg = lstStunsfirst[0].SGPA == null ? "***" : lstStunsfirst[0].SGPA;
                            SGPARow = GetSpaces(10) + sg + GetSpaces(55 - sg.Length);
                        }
                        else
                        {
                            SGPARow = GetSpaces(10) + "***" + GetSpaces(65 - "***".Length);
                        }
                        SGPARow = SGPARow + gap;


                        //Second recod
                        string SecondFinalResult = string.Empty;
                        if (lstStubScon[0].Ei == "E" && lstStubScon[0].FinalResult == "PROMOTED")
                        {
                            SecondFinalResult = "FAILED";
                        }
                        else
                        {
                            SecondFinalResult = lstStubScon[0].FinalResult;
                        }

                        if (SecondFinalResult == "PASSED" || SecondFinalResult == "COMPLETED" || SecondFinalResult == "PROMOTED")
                        {
                            string sg1 = lstStubScon[0].SGPA == null ? "***" : lstStubScon[0].SGPA;
                            SGPARow = SGPARow + GetSpaces(10) + sg1 + GetSpaces(55 - sg1.Length);
                        }
                        else
                        {
                            SGPARow = SGPARow + GetSpaces(10) + "***" + GetSpaces(65 - "***".Length);
                        }

                        sw.WriteLine(SGPARow);



                        string statusrow = GetSpaces(10) + FirstFinalResult + GetSpaces(55 - FirstFinalResult.Length) + gap;
                        statusrow = statusrow + GetSpaces(10) + SecondFinalResult + GetSpaces(55 - SecondFinalResult.Length);

                        sw.WriteLine(statusrow);
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");


                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        i++;


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
                if (hn != "0")
                {
                }
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
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
                string sub = subject + " " + yearPass.CheckNumeric(2);
                appenedstring = appenedstring + sub + GetSpaces(10 - sub.Length);
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
                appenedstring = appenedstring + sub + GetSpaces(10 - sub.Length);
                return appenedstring;
            }
            else
            {
                return appenedstring;
            }

        }

        private int addHeaderFooter(StreamWriter sw, int rowcount)
        {
            if (rowcount == 0)
            {



                sw.WriteLine(" ");
                sw.WriteLine(" ");
                rowcount = 1;


            }
            else if (rowcount == 29)
            {
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");


                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");
                sw.WriteLine(" ");

                rowcount = 1;


            }
            else { rowcount++; }
            return rowcount;
        }

        private void SingleLingSubject(StreamWriter sw, int i, List<StudentInformation> lstStunsfirst, List<StudentInformation> lstStubScon)
        {
            bool flag1 = false;
            bool flag2 = false;
            string space = " ";
            string dotedrow = "- -  - -  - -  - -  - -  - -  - -";

            if (lstStunsfirst[i].SubjectName.Length < 55 && lstStubScon[i].SubjectName.Length < 55)
            {

                string SubjectAndGrades = string.Empty;
                if (i < lstStunsfirst.Count)
                {
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].SubjectName + GetSpaces(55 - lstStunsfirst[i].SubjectName.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length);

                }
                else
                {
                    if (flag1 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(55 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(6 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        flag1 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(66);
                    }
                }
                SubjectAndGrades = SubjectAndGrades + GetSpaces(5);
                if (i < lstStubScon.Count)
                {

                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].SubjectName + GetSpaces(55 - lstStubScon[i].SubjectName.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
                }
                else
                {
                    if (flag2 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(55 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(6 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        flag2 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(66);
                    }
                }
                sw.WriteLine(SubjectAndGrades);

            }

        }

        private void Twolinesubject(StreamWriter sw, int i, List<StudentInformation> lstStunsfirst, List<StudentInformation> lstStubScon)
        {

            string subject1 = "";
            subject1 = lstStunsfirst[i].SubjectName;
            string s1sp1 = "";
            string s1sp2 = "";

            if (subject1.Length > 55)
            {
                s1sp1 = subject1.Substring(0, 48);
                s1sp2 = subject1.Substring(48);
            }
            else
            {
                s1sp1 = subject1.Substring(0, 50);
                s1sp2 = "";
            }


            string subject2 = "";
            subject2 = lstStubScon[i].SubjectName;
            string s2sp1 = "";
            string s2sp2 = "";
            if (subject2.Length > 55)
            {
                s2sp1 = subject2.Substring(0, 48);
                s2sp2 = subject2.Substring(48);
            }
            else
            {
                s2sp1 = subject2.Substring(0, 48);
                s2sp2 = "";

            }

            ///scenario 1
            if (subject1.Length > 55 && subject2.Length > 55)
            {
                string s1 = s1sp1 + GetSpaces(55 - s1sp1.Length) + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length) + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length) + GetSpaces(5);
                s1 = s1 + s2sp1 + GetSpaces(55 - s2sp1.Length) + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length) + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
                sw.WriteLine(s1);

                string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


                sw.WriteLine(sbuid);
            }

            //Scenario 2
            ///scenario 1
            if (subject1.Length > 55 && subject2.Length < 55)
            {
                string s1 = s1sp1 + GetSpaces(55 - s1sp1.Length) + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length) + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length) + GetSpaces(5);
                s1 = s1 + s2sp1 + GetSpaces(55 - s2sp1.Length) + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length) + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
                sw.WriteLine(s1);

                string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


                sw.WriteLine(sbuid);
            }
            //scenario 3
            if (subject1.Length < 55 && subject2.Length > 55)
            {
                string s1 = s1sp1 + GetSpaces(55 - s1sp1.Length) + lstStunsfirst[i].Credits + GetSpaces(6 - lstStunsfirst[i].Credits.Length) + lstStunsfirst[i].LeterGrade + GetSpaces(4 - lstStunsfirst[i].LeterGrade.Length) + GetSpaces(5);
                s1 = s1 + s2sp1 + GetSpaces(55 - s2sp1.Length) + lstStubScon[i].Credits + GetSpaces(6 - lstStubScon[i].Credits.Length) + lstStubScon[i].LeterGrade + GetSpaces(4 - lstStubScon[i].LeterGrade.Length);
                sw.WriteLine(s1);

                string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


                sw.WriteLine(sbuid);
            }

        }



        private void SingleLingSubjectBAL(StreamWriter sw, int i, List<BALPresEntity> lstStunsfirst, List<BALPresEntity> lstStubScon)
        {
            bool flag1 = false;
            bool flag2 = false;
            string space = " ";
            string dotedrow = "- -  - -  - -  - -  - -  - -  - -";

            if (lstStunsfirst[i].SubjectName.Length < 45 && lstStubScon[i].SubjectName.Length < 45)
            {

                string SubjectAndGrades = string.Empty;
                if (i < lstStunsfirst.Count)
                {
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].SubjectName + GetSpaces(43 - lstStunsfirst[i].SubjectName.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].MaxMark + GetSpaces(6 - lstStunsfirst[i].MaxMark.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].MinMark + GetSpaces(6 - lstStunsfirst[i].MinMark.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].MARKS + GetSpaces(6 - lstStunsfirst[i].MARKS.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStunsfirst[i].RESULT + GetSpaces(4 - lstStunsfirst[i].RESULT.Length);

                }
                else
                {
                    if (flag1 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(41 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(6 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        flag1 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(66);
                    }
                }
                SubjectAndGrades = SubjectAndGrades + GetSpaces(5);
                if (i < lstStubScon.Count)
                {

                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].SubjectName + GetSpaces(43 - lstStubScon[i].SubjectName.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].MaxMark + GetSpaces(6 - lstStubScon[i].MaxMark.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].MinMark + GetSpaces(6 - lstStubScon[i].MinMark.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].MARKS + GetSpaces(6 - lstStubScon[i].MARKS.Length);
                    SubjectAndGrades = SubjectAndGrades + lstStubScon[i].RESULT + GetSpaces(4 - lstStubScon[i].RESULT.Length);
                }
                else
                {
                    if (flag2 == false)
                    {
                        SubjectAndGrades = SubjectAndGrades + dotedrow + GetSpaces(55 - dotedrow.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(6 - space.Length);
                        SubjectAndGrades = SubjectAndGrades + space + GetSpaces(4 - space.Length);
                        flag2 = true;
                    }
                    else
                    {
                        SubjectAndGrades = GetSpaces(66);
                    }
                }
                sw.WriteLine(SubjectAndGrades);

            }

        }

        private void TwolinesubjectBAL(StreamWriter sw, int i, List<BALPresEntity> lstStunsfirst, List<BALPresEntity> lstStubScon)
        {

            string subject1 = "";
            subject1 = lstStunsfirst[i].SubjectName;
            string s1sp1 = "";
            string s1sp2 = "";

            if (subject1.Length > 55)
            {
                s1sp1 = subject1.Substring(0, 48);
                s1sp2 = subject1.Substring(48);
            }
            else
            {
                s1sp1 = subject1.Substring(0, 50);
                s1sp2 = "";
            }


            string subject2 = "";
            subject2 = lstStubScon[i].SubjectName;
            string s2sp1 = "";
            string s2sp2 = "";
            if (subject2.Length > 55)
            {
                s2sp1 = subject2.Substring(0, 48);
                s2sp2 = subject2.Substring(48);
            }
            else
            {
                s2sp1 = subject2.Substring(0, 48);
                s2sp2 = "";

            }

            ///scenario 1
            if (subject1.Length > 43 && subject2.Length > 43)
            {
                string s1 = s1sp1 + GetSpaces(43 - s1sp1.Length) + lstStunsfirst[i].MaxMark + GetSpaces(3 - lstStunsfirst[i].MaxMark.Length) + lstStunsfirst[i].MinMark + GetSpaces(3 - lstStunsfirst[i].MinMark.Length) + lstStunsfirst[i].MARKS + GetSpaces(3 - lstStunsfirst[i].MARKS.Length) + lstStunsfirst[i].RESULT + GetSpaces(4 - lstStunsfirst[i].RESULT.Length) + GetSpaces(5);
                s1 = s1 + s2sp1 + GetSpaces(43 - s2sp1.Length) + lstStubScon[i].MaxMark + GetSpaces(3 - lstStubScon[i].MaxMark.Length) + lstStubScon[i].MinMark + GetSpaces(3 - lstStubScon[i].MinMark.Length) + lstStubScon[i].MARKS + GetSpaces(3 - lstStubScon[i].MARKS.Length) + lstStubScon[i].RESULT + GetSpaces(4 - lstStubScon[i].RESULT.Length);
                sw.WriteLine(s1);

                string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


                sw.WriteLine(sbuid);
            }

            //Scenario 2
            ///scenario 1
            if (subject1.Length > 43 && subject2.Length < 43)
            {
                string s1 = s1sp1 + GetSpaces(43 - s1sp1.Length) + lstStunsfirst[i].MaxMark + GetSpaces(3 - lstStunsfirst[i].MaxMark.Length) + lstStunsfirst[i].MinMark + GetSpaces(3 - lstStunsfirst[i].MinMark.Length) + lstStunsfirst[i].MARKS + GetSpaces(3 - lstStunsfirst[i].MARKS.Length) + lstStunsfirst[i].RESULT + GetSpaces(4 - lstStunsfirst[i].RESULT.Length) + GetSpaces(5);
                s1 = s1 + s2sp1 + GetSpaces(43 - s2sp1.Length) + lstStubScon[i].MaxMark + GetSpaces(3 - lstStubScon[i].MaxMark.Length) + lstStubScon[i].MinMark + GetSpaces(3 - lstStubScon[i].MinMark.Length) + lstStubScon[i].MARKS + GetSpaces(3 - lstStubScon[i].MARKS.Length) + lstStubScon[i].RESULT + GetSpaces(4 - lstStubScon[i].RESULT.Length);
                sw.WriteLine(s1);

                string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


                sw.WriteLine(sbuid);
            }
            //scenario 3
            if (subject1.Length < 43 && subject2.Length > 43)
            {
                string s1 = s1sp1 + GetSpaces(43 - s1sp1.Length) + lstStunsfirst[i].MaxMark + GetSpaces(3 - lstStunsfirst[i].MaxMark.Length) + lstStunsfirst[i].MinMark + GetSpaces(3 - lstStunsfirst[i].MinMark.Length) + lstStunsfirst[i].MARKS + GetSpaces(3 - lstStunsfirst[i].MARKS.Length) + lstStunsfirst[i].RESULT + GetSpaces(4 - lstStunsfirst[i].RESULT.Length) + GetSpaces(5);
                s1 = s1 + s2sp1 + GetSpaces(43 - s2sp1.Length) + lstStubScon[i].MaxMark + GetSpaces(3 - lstStubScon[i].MaxMark.Length) + lstStubScon[i].MinMark + GetSpaces(3 - lstStubScon[i].MinMark.Length) + lstStubScon[i].MARKS + GetSpaces(6 - lstStubScon[i].MARKS.Length) + lstStubScon[i].RESULT + GetSpaces(4 - lstStubScon[i].RESULT.Length);
                sw.WriteLine(s1);

                string sbuid = s1sp2 + GetSpaces(65 - s1sp2.Length) + GetSpaces(5) + s2sp2 + GetSpaces(65 - s2sp2.Length);


                sw.WriteLine(sbuid);
            }

        }


        private static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Paisa " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }

        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }   
    }
}

