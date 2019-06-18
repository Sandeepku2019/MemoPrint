using MemoPrintingUtility.BO;
using MemoPrintingUtility.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MemoPrintingUtility.Controllers
{
    public class LongMemoController : Controller
    {
        // GET: LongMemo
        public ActionResult Index()
        {
            return View();
        }

        int PageBraker = 0;

        [HttpPost]
        public JsonResult GenerateLongmemo(string course)
        {
            if (course == "BA (L)")
            {
                return GenerateBA_L("BAL");
            }
            else if (course == "BCA(P)")
            {

                return GenerateReport(course);
            }
            else
            {
                return null;
            }
        }


        private JsonResult GenerateReport(string course)
        {
            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/TabularReport/");
                string fileNamedirectory = @"D:\LongMemo\";
                string filename = course + DateTime.Now.ToString("ddMMyyyy") + "_" + course + ".txt";

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


                // Create a new file   
                string hn = "";
                using (StreamWriter sw = fi.CreateText())
                {
                    //
                    var lstStudents = BoMemoService.GetLongMemoInstance().GetBCA_P_StudentDetailPRESBO();
                    var LstConStudents = BoMemoService.GetLongMemoInstance().GetBCA_P_StudentsConsDetailsBO();

                    var lstBCASubject = BoMemoService.getTabularReportInstance().GetBCAPSubjectInformation("BCA");

                    List<string> HallticketNumbers = LstConStudents.OrderBy(x => x.HTNO).Select(x => x.HTNO).Distinct().ToList<string>();
                    string Syear = string.Empty;
                    string Ssem = string.Empty;
                    int yr = 0;
                    int sm = 0;
                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {
                        hn = HallticketNumbers[i];

                        if (hn == "086165013")
                        {

                        }
                        int Psem = 0;
                        var lstStunsfirst = lstStudents.OrderBy(y => y.Order).Where(x => x.HallTicketNumber == HallticketNumbers[i]).ToList<StudentInformation>();
                        var lstStunsCons = LstConStudents.Where(x => x.HTNO == HallticketNumbers[i]).ToList<ConsDataEntity>();





                        int[] yrs = new int[] { 1, 2, 3 };
                        int[] sems = new int[] { 1, 2 };

                        List<StudentInformation> lstMemo = new List<StudentInformation>();

                        #region Clubing
                        for (int j = 0; j < yrs.Length; j++)
                        {
                            for (int z = 0; z < sems.Length; z++)
                            {

                                string consem = yrs[j].ToString() + sems[z].ToString();
                                var lstConData = lstStunsCons.Where(x => x.SEM == consem).ToList();

                                if (lstConData != null && lstConData.Count > 0)
                                {
                                    lstMemo = GetCondataFormat(lstConData[0].P1, lstConData[0].M1, lstConData[0].S1, lstConData[0].A1, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P2, lstConData[0].M2, lstConData[0].S2, lstConData[0].A2, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P3, lstConData[0].M3, lstConData[0].S3, lstConData[0].A3, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P4, lstConData[0].M4, lstConData[0].S4, lstConData[0].A4, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P5, lstConData[0].M5, lstConData[0].S5, lstConData[0].A5, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P6, lstConData[0].M6, lstConData[0].S6, lstConData[0].A6, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                    lstMemo = GetCondataFormat(lstConData[0].P7, lstConData[0].M7, lstConData[0].S7, lstConData[0].A7, lstMemo, lstBCASubject, yrs[j].ToString(), sems[z].ToString());
                                }
                                var lstpressData = lstStunsfirst.Where(x => x.Year == yrs[j].ToString() && x.Sem == sems[z].ToString()).ToList();
                                foreach (var Pres in lstpressData)
                                {
                                    var conUpdate = lstMemo.Where(x => x.SubjectCode == Pres.SubjectCode).ToList().FirstOrDefault();
                                    if (conUpdate != null)
                                    {
                                        if (conUpdate.ExernalMarks.ChangeINT() < Pres.ExernalMarks.ChangeINT())
                                        {
                                            conUpdate.ExernalMarks = Pres.ExernalMarks;
                                            conUpdate.InternalMarks = Pres.InternalMarks;
                                            conUpdate.AcadmicYear = "n18".ChangeToMonthandYear();
                                        }
                                    }


                                }
                            }
                        }


                        foreach (var merge in lstMemo)
                        {
                            int OCMark = merge.ExernalMarks.ChangeINT() + merge.InternalMarks.ChangeINT();
                            if (merge.MinMarks.ChangeINT() > OCMark)
                            {
                                merge.Status = "FAILED";
                            }
                            else if (OCMark > merge.MinMarks.ChangeINT())
                            {
                                merge.Status = "COMPLETED";
                            }

                        }
                        #endregion endclub

                        string Status = string.Empty;

                        if (lstMemo.Where(x => x.Status == "FAILED").ToList().Count() > 0)
                        {
                            Status = "FAILED";
                        }
                        else
                        {
                            Status = "COMPLETED";
                        }
























                        if (Status != "FAILED")
                        {

                            #region Sem declaration

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
                            #endregion

                            string HallTicket_1 = lstStunsfirst[0].HallTicketNumber == null ? "" : lstStunsfirst[0].HallTicketNumber;
                            string FN_1 = lstStunsfirst[0].FatherName == null ? "" : lstStunsfirst[0].FatherName;
                            string SN_1 = lstStunsfirst[0].StudentName == null ? "" : lstStunsfirst[0].StudentName;
                            string CC_1 = lstStunsfirst[0].collegecode == null ? "" : lstStunsfirst[0].collegecode;

                            string CourseDetails = GetSpaces(13) + course + " " + Syear + " YR " + Ssem + " SEM   NOV/DEC ." + (DateTime.Now.Year - 1).ToString();
                            CourseDetails = CourseDetails + GetSpaces(72 - CourseDetails.Length) + DateTime.Now.ToString("dd-MM-");





                            sw.WriteLine("");  //BCA Start Line 
                            sw.WriteLine("");
                            sw.WriteLine("             * * * Draft only.. * * *");

                            sw.WriteLine(GetSpaces(66) + CC_1);
                            string Examination = "B.C.A. ,  NOV., 2018";
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");

                            sw.WriteLine(GetSpaces(13) + Examination + GetSpaces(44 - Examination.Length) + DateTime.Now.ToString("dd-MM-yyyy"));
                            sw.WriteLine(GetSpaces(13) + SN_1 + GetSpaces(44 - SN_1.Length));
                            sw.WriteLine(GetSpaces(13) + FN_1 + GetSpaces(44 - FN_1.Length) + HallTicket_1);
                            sw.WriteLine("");
                            sw.WriteLine("");
                            sw.WriteLine("");
                            

                            int SubjectRowCount = 0;

                            int TotalMark = 0;
                            List<StudentInformation> lstMemoDetils = new List<StudentInformation>();
                            sw.WriteLine(""); SubjectRowCount++;

                            #region PRINTING
                            

                            for (int j = 0; j < yrs.Length; j++)
                            {
                                for (int z = 0; z < sems.Length; z++)
                                {

                                    var LstMemoSubjects = lstMemo.Where(x => x.Year == yrs[j].ToString() && x.Sem == sems[z].ToString()).ToList();
                                  

                                    if (j == 0 & z == 0)
                                    {
                                        StringBuilder builder = new StringBuilder();
                                        builder.Append("" + ((char)27) + ((char)71) + "I YEAR:" + ((char)27) + ((char)72));
                                        sw.WriteLine(builder.ToString());
                                    }

                                    if (j == 1 & z == 0)
                                    {
                                        sw.WriteLine("" + ((char)27) + ((char)71) + "II YEAR I SEMESTER:" + ((char)27) + ((char)72));
                                    }

                                    if (j == 1 & z == 1)
                                    {
                                        sw.WriteLine("" + ((char)27) + ((char)71) + "II YEAR II SEMESTER:" + ((char)27) + ((char)72));
                                    }


                                    if (j == 2 & z == 0)
                                    {
                                        sw.WriteLine("" + ((char)27) + ((char)71) + "III YEAR I SEMESTER:" + ((char)27) + ((char)72));
                                    }


                                    if (j == 2 & z == 1)
                                    {
                                        sw.WriteLine("" + ((char)27) + ((char)71) + "III YEAR II SEMESTER:" + ((char)27) + ((char)72));
                                    }

                                    string strTotMark = string.Empty;
                                    string strMinMark = string.Empty;
                                    string strSecuredMark = string.Empty;

                                    foreach (var merge in LstMemoSubjects)
                                    {
                                        if (merge.SubjectName != null && merge.SubjectCode.Length > 0 )
                                        {

                                            string Subject = merge.SubjectName.Length == 0 ? Convert.ToString(merge.SubjectCode) : merge.SubjectName;

                                            if (Subject == null)
                                            {
                                                Subject = "";
                                            }
                                            int totalMark = merge.SubjectExternalMarks.ChangeINT() + merge.SubjectInternalMarks.ChangeINT();

                                            if (totalMark.ToString().Length != 3)
                                            {
                                                strTotMark = totalMark.ToString("D" + 3);
                                            }
                                            else { strTotMark = totalMark.ToString(); }

                                            string MinMark = merge.MinMarks;

                                            if (MinMark.ToString().Length != 3)
                                            {
                                                strMinMark = int.Parse(MinMark).ToString("D" + 3);
                                            }
                                            bool chars2l = false;
                                            if (merge.ExernalMarks.Length == 2 && merge.ExernalMarks.Contains("AB") == false)
                                            {
                                                chars2l = true;
                                            }
                                            else { chars2l = false; }


                                            int OCMark = merge.ExernalMarks.ChangeINT() + merge.InternalMarks.ChangeINT();
                                            TotalMark = TotalMark + OCMark;

                                            if (chars2l = true)
                                            {
                                                strSecuredMark = OCMark.ToString("D" + 3);
                                            }
                                            else { strSecuredMark = OCMark.ToString(); }

                                            if (totalMark > 0)
                                            {
                                                sw.WriteLine(Subject + GetSpaces(43 - Subject.Length) + strTotMark + GetSpaces(6 - strTotMark.ToString().Length) + strMinMark + GetSpaces(6 - strMinMark.ToString().Length) + strSecuredMark + GetSpaces(5 - strSecuredMark.ToString().Length) + merge.AcadmicYear);
                                                SubjectRowCount++;
                                            }
                                        }

                                    }

                                    if (LstMemoSubjects.Count() > 0)
                                    {
                                        sw.WriteLine(""); SubjectRowCount++;
                                    }
                                   // sw.WriteLine(""); SubjectRowCount++;
                                }
                            }
                            #endregion


                            if (SubjectRowCount < 43)
                            {
                                int rCoun = 43 - SubjectRowCount;

                                for (int b = 0; b < rCoun; b++)
                                {
                                    sw.WriteLine("");
                                }

                            }

                            sw.WriteLine(GetSpaces(13) + TotalMark.ToString() + GetSpaces(5) + "(" + TotalMark.NumberToWords().ToUpper() + ")");

                            string Division = string.Empty;
                            if (TotalMark < 1500 && Status == "COMPLETED")
                            {
                                Division = "";
                            }

                            if ((TotalMark < 1799 && TotalMark > 1500) && Status == "COMPLETED")
                            {
                                Division = "SECOND DIVISION";
                            }

                            if ((TotalMark < 2099 && TotalMark > 1800) && Status == "COMPLETED")
                            {
                                Division = "FIRST DIVISION";
                            }


                            if ((TotalMark > 2100 && TotalMark < 3000) && Status == "COMPLETED")
                            {
                                Division = "FIRST DIVISION";
                            }

                            sw.WriteLine(GetSpaces(13) + Division);
                            //sw.WriteLine(GetSpaces(13) + Status + "/" + Division);
                            sw.WriteLine(" ");//65
                            sw.WriteLine(" ");//66
                            sw.WriteLine(" ");//67
                            sw.WriteLine(" ");//68
                            sw.WriteLine(GetSpaces(42) + "for");
                            sw.WriteLine(" ");//70
                            sw.WriteLine(" ");//71
                            sw.WriteLine(" ");//72
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
                return Json(Ex.Message, JsonRequestBehavior.AllowGet);
            }


        }

        private JsonResult GenerateBA_L(string BAL)
        {

            try
            {

                ///// Creting Notepad 
                //string fileNamedirectory = Server.MapPath("/TabularReport/");
                string fileNamedirectory = @"D:\LongMemo\";
                string filename = BAL + DateTime.Now.ToString("ddMMyyyy") + "_" + BAL + ".txt";

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


                // Create a new file   
                string hn = "";
                using (StreamWriter sw = fi.CreateText())
                {

                    var LstConStudents = BoMemoService.GetLongMemoInstance().GetBALConInformaionUpdated();

                    var lstBALPDCSubject = BoMemoService.getTabularReportInstance().GetBALSubjectInformation("BAL");

                    List<string> HallticketNumbers = LstConStudents.OrderBy(x => x.HTNO).Select(x => x.HTNO).Distinct().ToList<string>();

                    string Syear = string.Empty;
                    string Ssem = string.Empty;
                    int yr = 0;
                    int sm = 0;
                    for (int i = 0; i < HallticketNumbers.Count; i++)
                    {
                        hn = HallticketNumbers[i];

                        int Psem = 0;

                        var lstStunsCons = LstConStudents.Where(x => x.HTNO == HallticketNumbers[i]).ToList<BALConEntity>();



                        string HallTicket_1 = lstStunsCons[0].HTNO == null ? "" : lstStunsCons[0].HTNO;
                        string FN_1 = lstStunsCons[0].FatherName == null ? "" : lstStunsCons[0].FatherName;
                        string SN_1 = lstStunsCons[0].StudentName == null ? "" : lstStunsCons[0].StudentName;
                        string CC_1 = lstStunsCons[0].Code == null ? "" : lstStunsCons[0].Code;

                        var student = lstStunsCons[0];

                        sw.WriteLine(""); //BAL Start Line
                        sw.WriteLine("");
                        sw.WriteLine("");

                        sw.WriteLine(GetSpaces(66) + CC_1);
                        string Examination = "BA(L). ,  NOV., 2018";
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");

                        sw.WriteLine(GetSpaces(13) + Examination + GetSpaces(44 - Examination.Length) + DateTime.Now.ToString("dd-MM-yyyy"));
                        sw.WriteLine(GetSpaces(13) + SN_1 + GetSpaces(44 - SN_1.Length));
                        sw.WriteLine(GetSpaces(13) + FN_1 + GetSpaces(44 - FN_1.Length) + HallTicket_1);
                        sw.WriteLine("");
                        sw.WriteLine("");
                        sw.WriteLine("");
                        //sw.WriteLine("");
                        int[] yrs = new int[] { 1, 2, 3 };
                        //int[] sems = new int[] { 1, 2 };
                        int SubjectRowCount = 0;

                        int TotalMark = 0;
                        List<StudentInformation> lstMemoDetils = new List<StudentInformation>();
                        sw.WriteLine(""); SubjectRowCount++;




                        List<BALEntity> lstMemo = new List<BALEntity>();
                        #region condata formating
                        foreach (var con in lstStunsCons)
                        {
                            //1year
                            lstMemo = CnrEntityVertical(lstMemo, con.M11, con.Y11, con.HTNO, con.P11, "", "1", 1);
                            lstMemo = CnrEntityVertical(lstMemo, con.PM11, con.PA11, con.HTNO, con.P11, "", "1", 1, true);
                            lstMemo = CnrEntityVertical(lstMemo, con.M12, con.Y12, con.HTNO, con.P12, "", "1", 2);
                            lstMemo = CnrEntityVertical(lstMemo, con.M13, con.Y13, con.HTNO, con.P13, "", "1", 3);
                            lstMemo = CnrEntityVertical(lstMemo, con.M14, con.Y14, con.HTNO, con.P14, "", "1", 4);
                            lstMemo = CnrEntityVertical(lstMemo, con.M15, con.Y15, con.HTNO, con.P15, "", "1", 5);
                            lstMemo = CnrEntityVertical(lstMemo, con.M16, con.Y16, con.HTNO, con.P16, "", "1", 6);
                            lstMemo = CnrEntityVertical(lstMemo, con.PM16, con.PA16, con.HTNO, con.P16, "", "1", 6, true);

                            //2year
                            lstMemo = CnrEntityVertical(lstMemo, con.M21, con.Y21, con.HTNO, con.P21, "", "2", 1);
                            lstMemo = CnrEntityVertical(lstMemo, con.PM21, con.PA21, con.HTNO, con.P21, "", "2", 1, true);
                            lstMemo = CnrEntityVertical(lstMemo, con.M22, con.Y22, con.HTNO, con.P22, "", "2", 2);
                            lstMemo = CnrEntityVertical(lstMemo, con.M23, con.Y23, con.HTNO, con.P23, "", "2", 3);
                            lstMemo = CnrEntityVertical(lstMemo, con.M24, con.Y24, con.HTNO, con.P24, "", "2", 4);
                            lstMemo = CnrEntityVertical(lstMemo, con.M25, con.Y25, con.HTNO, con.P25, "", "2", 5);
                            lstMemo = CnrEntityVertical(lstMemo, con.M26, con.Y26, con.HTNO, con.P26, "", "2", 6);
                            lstMemo = CnrEntityVertical(lstMemo, con.PM26, con.PA26, con.HTNO, con.P26, "", "2", 6, true);

                            //3year
                            lstMemo = CnrEntityVertical(lstMemo, con.M31, con.Y31, con.HTNO, con.P31, "", "3", 1);
                            lstMemo = CnrEntityVertical(lstMemo, con.M32, con.Y32, con.HTNO, con.P32, "", "3", 2);
                            lstMemo = CnrEntityVertical(lstMemo, con.M33, con.Y33, con.HTNO, con.P33, "", "3", 3);
                            lstMemo = CnrEntityVertical(lstMemo, con.M34, con.Y34, con.HTNO, con.P34, "", "3", 4);
                            lstMemo = CnrEntityVertical(lstMemo, con.M35, con.Y35, con.HTNO, con.P35, "", "3", 5);
                            lstMemo = CnrEntityVertical(lstMemo, con.M36, con.Y36, con.HTNO, con.P36, "", "3", 6);
                            lstMemo = CnrEntityVertical(lstMemo, con.PM36, con.PA36, con.HTNO, con.P36, "", "3", 6, true);
                            lstMemo = CnrEntityVertical(lstMemo, con.M37, con.Y37, con.HTNO, con.P37, "", "3", 7);

                        }
                        #endregion


                        SubjectRowCount++;

                        string[] BALyrs = lstMemo.OrderBy(y => y.Year).Select(x => x.Year).Distinct().ToArray();

                        if (HallTicket_1 == "01412307") { }
                        foreach (string baly in BALyrs)
                        {
                            var memoEntity = lstMemo.Where(x => x.Year == baly).ToList();

                            string RYr = string.Empty;
                            if (baly == "1")
                            { RYr = "I"; }
                            if (baly == "2")
                            { RYr = "II"; }
                            if (baly == "3")
                            { RYr = "III"; }

                            sw.WriteLine("" + ((char)27) + ((char)71) + RYr + " Year:" + ((char)27) + ((char)72));
                            //sw.WriteLine(RYr + " Year:");
                            foreach (var merge in memoEntity)
                            {
                                var subjectinfo = lstBALPDCSubject.Where(x => x.SubjectCode == merge.subjectCode && x.Year == merge.Year).ToList().FirstOrDefault();
                                if (subjectinfo != null)
                                {

                                    string Subject = subjectinfo.SubjectName.Length == 0 ? Convert.ToString(subjectinfo.SubjectCode) : subjectinfo.SubjectName;

                                    if (Subject == null)
                                    {
                                        Subject = "";
                                    }
                                    string totmark = subjectinfo.MaxMark;
                                    int totalMark = subjectinfo.MaxMark.ChangeINT();//merge.SubjectExternalMarks.ChangeINT() + merge.SubjectInternalMarks.ChangeINT();
                                    string MinMark = subjectinfo.MinMark;

                                    int OCMark = merge.Marks.ChangeINT();
                                    TotalMark = TotalMark + OCMark;

                                    string SecuredMark = merge.Marks;
                                    
                                    //Subject + GetSpaces(42 - Subject.Length) + totalMark + GetSpaces(6 - totalMark.ToString().Length) + MinMark + GetSpaces(6 - MinMark.ToString().Length) + OCMark + GetSpaces(6 - OCMark.ToString().Length) + Academic != "" ? Academic.ChangeToMonthandYear() : ""


                                    string Academic = merge.Academic != null ? merge.Academic.ChangeToMonthandYear() : "";
                                    string subline = Subject + GetSpaces(42 - Subject.Length) + totmark + GetSpaces(6 - totmark.ToString().Length) + MinMark + GetSpaces(6 - MinMark.ToString().Length) + SecuredMark + GetSpaces(6 - SecuredMark.Length) + Academic;
                                    sw.WriteLine(subline);
                                    SubjectRowCount++;
                                }

                            }

                            var memoEntity_prac = lstMemo.Where(x => x.Year == baly && x.Practical == true).ToList();
                            foreach (var merge in memoEntity_prac)
                            {

                                var subjectinfo = lstBALPDCSubject.Where(x => x.SubjectCode == merge.subjectCode && x.Year == merge.Year).ToList().FirstOrDefault();
                                if (subjectinfo != null)
                                {

                                    string Subject = subjectinfo.SubjectName.Length == 0 ? Convert.ToString(subjectinfo.SubjectCode) : subjectinfo.SubjectName;

                                    if (Subject == null)
                                    {
                                        Subject = "";
                                    }
                                    else
                                    {
                                        Subject = Subject + " (P)";
                                    }

                                    string PtotMark = subjectinfo.PMaxMark;
                                    int totalMark = subjectinfo.PMaxMark.ChangeINT();//merge.SubjectExternalMarks.ChangeINT() + merge.SubjectInternalMarks.ChangeINT();
                                    string MinMark = subjectinfo.PMinMark;

                                    string SecuredPMark = string.Empty;
                                    int OCMark = merge.PMarks.ChangeINT();
                                    TotalMark = TotalMark + OCMark;

                                    SecuredPMark = merge.PMarks;


                                    if (totalMark > 0)
                                    {
                                        string Academic = merge.PYear != null ? merge.PYear.ChangeToMonthandYear() : "";
                                        string subline = Subject + GetSpaces(42 - Subject.Length) + PtotMark + GetSpaces(6 - PtotMark.Length) + MinMark + GetSpaces(6 - MinMark.ToString().Length) + SecuredPMark + GetSpaces(6 - SecuredPMark.Length) + Academic;
                                        sw.WriteLine(subline);
                                        SubjectRowCount++;


                                    }
                                }

                            }


                            sw.WriteLine(""); SubjectRowCount++;
                        }

                        //sw.WriteLine(""); SubjectRowCount++;


                        if (SubjectRowCount < 46)
                        {
                            int rCoun = 46 - SubjectRowCount;

                            for (int b = 0; b < rCoun; b++)
                            {
                                sw.WriteLine("");
                            }

                        }

                        TotalMark = lstStunsCons[0].Total.ChangeINT();
                        sw.WriteLine(GetSpaces(13) + TotalMark.ToString() + GetSpaces(5) + "(" + TotalMark.NumberToWords().ToUpper() + ")");
                        string Division = string.Empty;


                        if (TotalMark > 560 && TotalMark <= 767)
                        {

                            Division = "PASS";
                        }

                        if (TotalMark > 768 && TotalMark <= 959)
                        {

                            Division = "SECOND DIVISION";
                        }

                        if (TotalMark > 960 && TotalMark <= 1119)
                        {

                            Division = "FIRST  DIVISION";
                        }

                        if (TotalMark > 1120 && TotalMark <= 1160)
                        {

                            Division = "DSTIN. DIVISION";
                        }





                        sw.WriteLine(GetSpaces(13) + lstStunsCons[0].FinalResult + "/" + Division);
                        sw.WriteLine(" ");//65
                        sw.WriteLine(" ");//66
                        sw.WriteLine(" ");//67
                        sw.WriteLine(" ");//68
                        sw.WriteLine(GetSpaces(42) + "for");
                        sw.WriteLine(" ");//70
                        sw.WriteLine(" ");//71
                        sw.WriteLine(" ");//72
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



        private string GetSpaces(int totalLength)
        {
            string result = string.Empty;
            for (int i = 0; i < totalLength; i++)
            {
                result += " ";
            }
            return result;
        }



        private List<StudentInformation> GetCondataFormat(string SubCode, string MarksScored, string internals, string Academic, List<StudentInformation> lst, List<BCAPSubjectINformation> lstSubjectInfo, string yr, string sem)
        {
            var sub = lstSubjectInfo.Where(x => x.SubjectCode == SubCode && x.Year == yr && x.Sem == sem).ToList();
            string SbName = string.Empty;
            string MaxMarks = string.Empty;
            string MinMarks = string.Empty;
            string Occupined = string.Empty;
            string internalMarks = string.Empty;
            if (sub != null && sub.Count() > 0)
            {
                SbName = sub[0].SubjectName;
                MaxMarks = sub[0].MaxMark;
                MinMarks = sub[0].MinMark;
                internalMarks = sub[0].InternalMark;

            }

            lst.Add(new StudentInformation()
            {
                SubjectCode = SubCode,
                SubjectName = SbName,
                MinMarks = MinMarks,
                SubjectExternalMarks = MaxMarks,
                SubjectInternalMarks = internalMarks,
                ExernalMarks = MarksScored,
                InternalMarks = internals,
                AcadmicYear = Academic.ChangeToMonthandYear(),
                Year = yr,
                Sem = sem

            });

            return lst;
        }



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



    }


}
