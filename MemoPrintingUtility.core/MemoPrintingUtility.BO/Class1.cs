using KU_BO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UnivPortal.Reports
{
    public partial class aspReportInterface : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void brnGenerate_Click(object sender, EventArgs e)
        {
            try
            {






                ///// Creting Notepad 
                string fileNamedirectory = @"D:\TabularReport\";
                string filename = "BA" + DateTime.Now.ToString("ddMMyyyy") + ".txt";

                // check for Directory
                if (!Directory.Exists(fileNamedirectory))  // if it doesn't exist, create
                {
                    Directory.CreateDirectory(fileNamedirectory);
                }


                // file information 
                FileInfo fi = new FileInfo(fileNamedirectory + filename);
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileNamedirectory + filename))
                {
                    File.Delete(fileNamedirectory + filename);
                }



                ////Get the Data
                TabularReportEntity objTabularReport = new TabularReportEntity();
                objTabularReport = GettheDataForTabularReport();


                // Create a new file     
                using (StreamWriter sw = fi.CreateText())
                {
                    int PageNumber = 1;
                    int SubjectIndex = 0;
                    int Studentindex = 0;
                    int i = 0;
                    string[] AcademicYrs = new string[] { "I", "II", "III" };

                    while (objTabularReport.lstStudentInformaion != null && Studentindex <= objTabularReport.lstStudentInformaion.Count)
                    {

                        //sw.WriteLine(" ");
                        //sw.WriteLine(" ");
                        //sw.WriteLine(" ");

                        //sw.WriteLine(("                                  " + objTabularReport.ReportTitle + "                                 ").Truncate(132));
                        sw.WriteLine(("PAGE NO:" + PageNumber + "========================================================================================================Date:" + DateTime.Now.ToString("dd-MMM-yyyy") + "===").Truncate(132));


                        string HallTicket = objTabularReport.lstStudentInformaion[Studentindex].HallTicketNumber;
                        string FN = objTabularReport.lstStudentInformaion[Studentindex].FatherName;
                        string SN = objTabularReport.lstStudentInformaion[Studentindex].StudentName;
                        string CC = objTabularReport.lstStudentInformaion[Studentindex].collegecode;
                        string status = objTabularReport.lstStudentInformaion[Studentindex].Status;
                        int PreMarks = 0;
                        string Stuindex = (Studentindex + 1) + ")";
                        int PartIMarks = 0;
                        int PartIIMarks = 0;

                        List<SubjectMarksDetails> lstSubjectsMarks = objTabularReport.lstStudentInformaion[Studentindex].lstSubjectsandMarks.Where(X => X.HallTicketNumber == HallTicket).ToList<SubjectMarksDetails>();



                        sw.WriteLine(Stuindex + GetSpaces(5 - Stuindex.Length) + CC + GetSpaces(5 - CC.Length) + HallTicket + GetSpaces(15 - HallTicket.Length) + SN + GetSpaces(45 - SN.Length) + FN + GetSpaces(45 - FN.Length) + HallTicket + GetSpaces(17 - HallTicket.Length));


                        foreach (string Acyr in AcademicYrs)
                        {

                            #region  Continoues
                            List<SubjectMarksDetails> lstCON = lstSubjectsMarks.Where(x => x.AcademicYr == Acyr && x.AcademicType == "CON").OrderBy(X => X.order).ToList<SubjectMarksDetails>();
                            string subjectstring = string.Empty;
                            string Markformat = GetSpaces(10);
                            if (lstCON.Count > 0)
                            {
                                subjectstring = lstCON[0].AcademicType + " " + Acyr + GetSpaces(6 - Acyr.Length);
                                foreach (var sub in lstCON)
                                {

                                    string subjectformt = "";

                                    //subject header formatting
                                    if (sub.IsPracticals)
                                    {
                                        subjectformt = sub.SubjectName + " " + sub.ExamType + sub.ExamYearPassout + " " + sub.PracticleName + sub.PracticleYrPassout + "   ";
                                    }
                                    else
                                    {
                                        subjectformt = sub.SubjectName + " " + sub.ExamType + sub.ExamYearPassout + "       ";
                                    }
                                    subjectstring = subjectstring + subjectformt;


                                    //subject marks formating
                                    if (sub.IsPracticals)
                                    {
                                        Markformat = Markformat + sub.TheoryMarks.ToString("D3") + GetSpaces(5) + sub.PracticleMarks.ToString("D3") + GetSpaces(3);
                                    }
                                    else
                                    {
                                        Markformat = Markformat + sub.TheoryMarks.ToString("D3") + GetSpaces(11);
                                    }
                                }

                                sw.WriteLine(subjectstring);
                                sw.WriteLine(Markformat);
                            }

                            #endregion

                            #region Present
                            List<SubjectMarksDetails> lstPRE = lstSubjectsMarks.Where(x => x.AcademicYr == Acyr && x.AcademicType == "PRE").OrderBy(X => X.order).ToList<SubjectMarksDetails>();
                            string subjectstringPRE = string.Empty;
                            string MarkformatPRE = GetSpaces(10);
                            string Aademicstatus = string.Empty;
                            if (lstPRE.Count > 0)
                            {
                                subjectstringPRE = lstPRE[0].AcademicType + " " + Acyr + GetSpaces(6 - Acyr.Length);

                                foreach (var sub in lstPRE)
                                {
                                    string subjectformt = "";

                                    //subject header formatting
                                    if (sub.IsPracticals)
                                    {
                                        subjectformt = GetSpaces((sub.order - 1) * 14) + sub.SubjectName + "    " + " " + sub.PracticleName + sub.PracticleYrPassout + "   ";
                                    }
                                    else
                                    {
                                        subjectformt = GetSpaces((sub.order - 1) * 14) + sub.SubjectName + " " + sub.ExamType + sub.ExamYearPassout + "       ";
                                    }
                                    subjectstringPRE = subjectstringPRE + subjectformt;


                                    if (sub.Presence != "AB")
                                    {
                                        //subject marks formating
                                        if (sub.IsPracticals)
                                        {
                                            MarkformatPRE = GetSpaces((sub.order - 1) * 14) + MarkformatPRE + sub.TheoryMarks.ToString("D3") + GetSpaces(5) + sub.PracticleMarks.ToString("D3") + GetSpaces(3);
                                        }
                                        else
                                        {
                                            MarkformatPRE = GetSpaces((sub.order - 1) * 14) + MarkformatPRE + sub.TheoryMarks.ToString("D3") + GetSpaces(11);
                                        }
                                    }
                                    else
                                    {
                                        //subject marks formating
                                        if (sub.IsPracticals)
                                        {
                                            PreMarks = PreMarks + sub.TheoryMarks + sub.PracticleMarks;
                                            MarkformatPRE = GetSpaces((sub.order - 1) * 14) + MarkformatPRE + "AB" + GetSpaces(6) + "AB" + GetSpaces(4);
                                        }
                                        else
                                        {
                                            PreMarks = PreMarks + sub.TheoryMarks;
                                            MarkformatPRE = GetSpaces((sub.order - 1) * 14) + MarkformatPRE + "AB AB" + GetSpaces(9);
                                        }
                                    }

                                    Aademicstatus = GetSpaces(102) + sub.AcademicStatus + GetSpaces(15 - sub.AcademicStatus.Length) + sub.TheoryMarks + GetSpaces(15 - sub.TheoryMarks.ToString().Length);
                                }


                                sw.WriteLine(subjectstringPRE);
                                sw.WriteLine(MarkformatPRE);

                            }

                            #endregion

                            #region Status
                            if (Acyr != "III")
                            {
                                if (Aademicstatus.Trim().Length > 0)
                                {
                                    sw.WriteLine(Aademicstatus);
                                }
                                sw.WriteLine(GetSpaces(10) + "..........................................................................................................................");
                            }
                            else
                            {

                                sw.WriteLine(GetSpaces(10) + "...........................................................................                    " + status + GetSpaces(15 - status.Length) + PreMarks + GetSpaces(15 - PreMarks.ToString().Length));
                            }
                            #endregion

                            #region Part I and Part II Calculation

                            if (Acyr != "III")
                            {
                                var lstPart1 = lstSubjectsMarks.Where(x => x.AcademicYr == Acyr && (x.SubjectName == "ENS")).ToList<SubjectMarksDetails>();
                                if (lstPart1.Count > 0)
                                {
                                    PartIMarks = PartIMarks + lstPart1.Select(x => x.TheoryMarks).ToArray().Max();
                                    PartIMarks = PartIMarks + lstPart1.Select(x => x.PracticleMarks).ToArray().Max();
                                }


                                var lstPartssub2 = lstSubjectsMarks.Where(x => x.AcademicYr == Acyr && (x.SubjectName == "TEL")).ToList<SubjectMarksDetails>();
                                if (lstPartssub2.Count > 0)
                                {
                                    PartIMarks = PartIMarks + lstPartssub2.Select(x => x.TheoryMarks).ToArray().Max();
                                    PartIMarks = PartIMarks + lstPartssub2.Select(x => x.PracticleMarks).ToArray().Max();
                                }


                                string[] distrinctSubject = lstSubjectsMarks.Where(x => x.AcademicYr == Acyr).Select(y => y.SubjectName).ToArray();

                                foreach (string subpartII in distrinctSubject)
                                {

                                    PartIIMarks = PartIIMarks + lstSubjectsMarks.Where(x => x.AcademicYr == Acyr && x.SubjectName == subpartII).Select(y => y.TheoryMarks).ToArray().Max();
                                    PartIIMarks = PartIIMarks + lstSubjectsMarks.Where(x => x.AcademicYr == Acyr && x.SubjectName == subpartII).Select(y => y.PracticleMarks).ToArray().Max();
                                }
                            }

                        }
                        sw.WriteLine(GetSpaces(12) + "******PART-I  " + PartIMarks + " " + GetSpaces(15) + "PART II  " + PartIIMarks + GetSpaces(21 - PartIIMarks.ToString().Length) + "******");

                        #endregion


                        sw.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

                        Studentindex++;
                    }


                }





                // Write file contents on console.     
                using (StreamReader sr = File.OpenText(fileNamedirectory + filename))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
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


        private TabularReportEntity GettheDataForTabularReport()
        {
            TabularReportEntity objTabulrReportData = new TabularReportEntity();

            objTabulrReportData.ReportTitle = "TABULATION LIST OF B.Com SUPPL. EXAMINATIONS HELD IN SEP/OCT -2018";


            objTabulrReportData.lstStudentInformaion = new List<StudentInformation>();


            objTabulrReportData.lstStudentInformaion.Add(

                new StudentInformation()
                {
                    collegecode = "007",
                    HallTicketNumber = "007122071",
                    StudentName = "KANUGANTI RAKESH",
                    FatherName = "KANUGANTI HINA YAKAIAH",
                    PartI = "Part-I",
                    PartIMarks = 169,
                    PartIStatus = "PASS",
                    PartII = "PART-II",
                    PartIIMarks = 750,
                    Status = "FAILED"
                });


            #region con I
            objTabulrReportData.lstStudentInformaion[0].lstSubjectsandMarks = new List<SubjectMarksDetails>();
            objTabulrReportData.lstStudentInformaion[0].lstSubjectsandMarks.AddRange(new List<SubjectMarksDetails>() {
                new SubjectMarksDetails()
                {
                    SubjectName = "ENS",
                    TheoryMarks = 25,
                    ExamType = "S",
                    AcademicType = "CON",
                    AcademicYr = "I",
                    ExamYearPassout = 13,
                    HallTicketNumber = "007122071",
                    IsPracticals = true,
                    PracticleYrPassout = 12,
                    PracticleName = "A",
                    PracticleMarks = 18,
                    order = 1,


                },
                new SubjectMarksDetails()
                {
                    SubjectName = "TEL",
                    TheoryMarks = 40,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "I",
                    ExamYearPassout = 12,
                    HallTicketNumber = "007122071",
                    IsPracticals = false,
                    order = 2
                },
                new SubjectMarksDetails()
                {
                    SubjectName = "CIE",
                    TheoryMarks = 21,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "I",
                    ExamYearPassout = 12,
                    HallTicketNumber = "007122071",
                    IsPracticals = false,
                    order = 3
                },
                 new SubjectMarksDetails()
                 {
                     SubjectName = "BES",
                     TheoryMarks = 37,
                     ExamType = "S",
                     AcademicType = "CON",
                     AcademicYr = "I",
                     ExamYearPassout = 12,
                     HallTicketNumber = "007122071",
                     IsPracticals = false,
                     order = 4
                 },
                  new SubjectMarksDetails()
                  {
                      SubjectName = "F1A",
                      TheoryMarks = 32,
                      ExamType = "S",
                      AcademicType = "CON",
                      AcademicYr = "I",
                      ExamYearPassout = 13,
                      HallTicketNumber = "007122071",
                      IsPracticals = true,
                      PracticleYrPassout = 14,
                      PracticleName = "A",
                      PracticleMarks = 18,
                      order = 5
                  },

                   new SubjectMarksDetails()
                   {
                       SubjectName = "BOM",
                       TheoryMarks = 32,
                       ExamType = "S",
                       AcademicType = "CON",
                       AcademicYr = "I",
                       ExamYearPassout = 12,
                       HallTicketNumber = "007122071",
                       IsPracticals = true,
                       PracticleYrPassout = 12,
                       PracticleName = "A",
                       PracticleMarks = 25,
                       order = 6
                   },
                    new SubjectMarksDetails()
                    {
                        SubjectName = "FIT ",
                        TheoryMarks = 25,
                        ExamType = "A",
                        AcademicType = "CON",
                        AcademicYr = "I",
                        ExamYearPassout = 12,
                        HallTicketNumber = "007122071",
                        IsPracticals = true,
                        PracticleYrPassout = 12,
                        PracticleName = "A",
                        PracticleMarks = 16,
                        order = 7
                    }});


            #endregion


            #region con II



            objTabulrReportData.lstStudentInformaion[0].lstSubjectsandMarks.AddRange(new List<SubjectMarksDetails>() {
                new SubjectMarksDetails()
                {
                    SubjectName = "ENS",
                    TheoryMarks = 29,
                    ExamType = "S",
                    AcademicType = "CON",
                    AcademicYr = "II",
                    ExamYearPassout = 17,
                    HallTicketNumber = "007122071",
                    IsPracticals = true,
                    PracticleYrPassout = 13,
                    PracticleName = "A",
                    PracticleMarks = 11,
                    order = 1,


                },
                new SubjectMarksDetails()
                {
                    SubjectName = "TEL",
                    TheoryMarks = 46,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "II",
                    ExamYearPassout = 13,
                    HallTicketNumber = "007122071",
                    IsPracticals = false,
                    order = 2
                },
                new SubjectMarksDetails()
                {
                    SubjectName = "EST",
                    TheoryMarks = 74,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "II",
                    ExamYearPassout = 13,
                    HallTicketNumber = "007122071",
                    IsPracticals = false,
                    order = 3
                },
                 new SubjectMarksDetails()
                 {
                     SubjectName = "FBI",
                     TheoryMarks = 25,
                     ExamType = "A",
                     AcademicType = "CON",
                     AcademicYr = "II",
                     ExamYearPassout = 13,
                     HallTicketNumber = "007122071",
                     IsPracticals = true,
                     PracticleYrPassout = 13,
                    PracticleName = "A",
                    PracticleMarks = 25,
                     order = 4
                 },
                  new SubjectMarksDetails()
                  {
                      SubjectName = "AAC",
                      TheoryMarks = 31,
                      ExamType = "S",
                      AcademicType = "CON",
                      AcademicYr = "II",
                      ExamYearPassout = 13,
                      HallTicketNumber = "007122071",
                      IsPracticals = true,
                      PracticleYrPassout = 13,
                      PracticleName = "A",
                      PracticleMarks = 22,
                      order = 5
                  },

                   new SubjectMarksDetails()
                   {
                       SubjectName = "BSS",
                       TheoryMarks = 32,
                       ExamType = "S",
                       AcademicType = "CON",
                       AcademicYr = "II",
                       ExamYearPassout = 13,
                       HallTicketNumber = "007122071",
                       IsPracticals = true,
                       PracticleYrPassout = 13,
                       PracticleName = "A",
                       PracticleMarks = 23,
                       order = 6
                   },
                    new SubjectMarksDetails()
                    {
                        SubjectName = "TX2",
                        TheoryMarks = 37,
                        ExamType = "S",
                        AcademicType = "CON",
                        AcademicYr = "II",
                        ExamYearPassout = 13,
                        HallTicketNumber = "007122071",
                        IsPracticals = true,
                        PracticleYrPassout = 13,
                        PracticleName = "A",
                        PracticleMarks = 18,
                        order = 7
                    },
                     new SubjectMarksDetails()
                {
                    SubjectName = "OAT",
                    TheoryMarks = 63,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "II",
                    ExamYearPassout = 13,
                    HallTicketNumber = "007122071",
                    IsPracticals = false, order = 8}

            });


            #endregion


            #region con III
            objTabulrReportData.lstStudentInformaion[0].lstSubjectsandMarks.AddRange(new List<SubjectMarksDetails>() {
                new SubjectMarksDetails()
                {
                    SubjectName = "BSL",
                    TheoryMarks = 27,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122071",
                    IsPracticals = true,
                    PracticleYrPassout = 14,
                    PracticleName = "A",
                    PracticleMarks = 25,
                    order = 1,


                },
                new SubjectMarksDetails()
                {
                    SubjectName = "CRA",
                    TheoryMarks = 18,
                    ExamType = "S",
                    AcademicType = "CON",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122071",
                    IsPracticals = true,
                    PracticleYrPassout = 14,
                    PracticleName = "A",
                    PracticleMarks = 24,
                    order = 2,
                },
                new SubjectMarksDetails()
                {
                    SubjectName = "CAM",
                    TheoryMarks = 38,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122071",
                    IsPracticals = true,
                    PracticleYrPassout = 14,
                    PracticleName = "A",
                    PracticleMarks = 25,
                    order = 3,
                },
                 new SubjectMarksDetails()
                 {
                    SubjectName = "ADG",
                    TheoryMarks = 25,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122071",
                    IsPracticals = true,
                    PracticleYrPassout = 14,
                    PracticleName = "A",
                    PracticleMarks = 26,
                    order = 4,
                 },
                  new SubjectMarksDetails()
                  {
                      SubjectName = "ACA",
                      TheoryMarks = 25,
                      ExamType = "A",
                      AcademicType = "CON",
                      AcademicYr = "III",
                      ExamYearPassout = 14,
                      HallTicketNumber = "007122071",
                      IsPracticals = true,
                      PracticleYrPassout = 14,
                      PracticleName = "A",
                      PracticleMarks = 26,
                      order = 5
                  },

                   new SubjectMarksDetails()
                   {
                       SubjectName = "ADA",
                       TheoryMarks = 56,
                       ExamType = "A",
                       AcademicType = "CON",
                       AcademicYr = "III",
                       ExamYearPassout = 14,
                       HallTicketNumber = "007122071",
                       IsPracticals = false,

                       order = 6
                   },
                    new SubjectMarksDetails()
                    {
                        SubjectName = "BCN",
                        TheoryMarks = 25,
                        ExamType = "A",
                        AcademicType = "CON",
                        AcademicYr = "III",
                        ExamYearPassout = 14,
                        HallTicketNumber = "007122071",
                        IsPracticals = false,


                        order = 7,
                    }, new SubjectMarksDetails()
                    {
                        SubjectName = "SCN",
                        TheoryMarks = 25,
                        ExamType = "A",
                        AcademicType = "CON",
                        AcademicYr = "III",
                        ExamYearPassout = 14,
                        HallTicketNumber = "007122071",
                        IsPracticals = false,

                        order = 8
                    }

            });







            #endregion

            #region PRE III
            objTabulrReportData.lstStudentInformaion[0].lstSubjectsandMarks.AddRange(new List<SubjectMarksDetails>() {

                new SubjectMarksDetails()
                {
                    SubjectName = "CRA",
                    ExamType = "S",
                    AcademicType = "PRE",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122071",
                    order = 2,
                    Presence= "AB",
                    TheoryMarks=0,
                    AcademicStatus= "AB"
                },

            });







            #endregion



            objTabulrReportData.lstStudentInformaion.Add(

               new StudentInformation()
               {
                   collegecode = "007",
                   HallTicketNumber = "007122072",
                   StudentName = "TINGILIKARI SANDEEP",
                   FatherName = "TINGILIKARI VIDYASAGAR",
                   PartI = "Part-I",
                   PartIMarks = 169,
                   PartIStatus = "PASS",
                   PartII = "PART-II",
                   PartIIMarks = 750,
                   Status = "FAILED"
               });


            #region con I
            objTabulrReportData.lstStudentInformaion[1].lstSubjectsandMarks = new List<SubjectMarksDetails>();
            objTabulrReportData.lstStudentInformaion[1].lstSubjectsandMarks.AddRange(new List<SubjectMarksDetails>() {
                new SubjectMarksDetails()
                {
                    SubjectName = "ENS",
                    TheoryMarks = 25,
                    ExamType = "S",
                    AcademicType = "CON",
                    AcademicYr = "I",
                    ExamYearPassout = 13,
                    HallTicketNumber = "007122072",
                    IsPracticals = true,
                    PracticleYrPassout = 12,
                    PracticleName = "A",
                    PracticleMarks = 18,
                    order = 1,


                },
                new SubjectMarksDetails()
                {
                    SubjectName = "TEL",
                    TheoryMarks = 40,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "I",
                    ExamYearPassout = 12,
                    HallTicketNumber = "007122072",
                    IsPracticals = false,
                    order = 2
                },
                new SubjectMarksDetails()
                {
                    SubjectName = "CIE",
                    TheoryMarks = 21,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "I",
                    ExamYearPassout = 12,
                    HallTicketNumber = "007122072",
                    IsPracticals = false,
                    order = 3
                },
                 new SubjectMarksDetails()
                 {
                     SubjectName = "BES",
                     TheoryMarks = 37,
                     ExamType = "S",
                     AcademicType = "CON",
                     AcademicYr = "I",
                     ExamYearPassout = 12,
                     HallTicketNumber = "007122072",
                     IsPracticals = false,
                     order = 4
                 },
                  new SubjectMarksDetails()
                  {
                      SubjectName = "F1A",
                      TheoryMarks = 32,
                      ExamType = "S",
                      AcademicType = "CON",
                      AcademicYr = "I",
                      ExamYearPassout = 13,
                      HallTicketNumber = "007122072",
                      IsPracticals = true,
                      PracticleYrPassout = 14,
                      PracticleName = "A",
                      PracticleMarks = 18,
                      order = 5
                  },

                   new SubjectMarksDetails()
                   {
                       SubjectName = "BOM",
                       TheoryMarks = 32,
                       ExamType = "S",
                       AcademicType = "CON",
                       AcademicYr = "I",
                       ExamYearPassout = 12,
                       HallTicketNumber = "007122072",
                       IsPracticals = true,
                       PracticleYrPassout = 12,
                       PracticleName = "A",
                       PracticleMarks = 25,
                       order = 6
                   },
                    new SubjectMarksDetails()
                    {
                        SubjectName = "FIT ",
                        TheoryMarks = 25,
                        ExamType = "A",
                        AcademicType = "CON",
                        AcademicYr = "I",
                        ExamYearPassout = 12,
                        HallTicketNumber = "007122072",
                        IsPracticals = true,
                        PracticleYrPassout = 12,
                        PracticleName = "A",
                        PracticleMarks = 16,
                        order = 7
                    }});


            #endregion


            #region con II



            objTabulrReportData.lstStudentInformaion[1].lstSubjectsandMarks.AddRange(new List<SubjectMarksDetails>() {
                new SubjectMarksDetails()
                {
                    SubjectName = "ENS",
                    TheoryMarks = 29,
                    ExamType = "S",
                    AcademicType = "CON",
                    AcademicYr = "II",
                    ExamYearPassout = 17,
                    HallTicketNumber = "007122072",
                    IsPracticals = true,
                    PracticleYrPassout = 13,
                    PracticleName = "A",
                    PracticleMarks = 11,
                    order = 1,


                },
                new SubjectMarksDetails()
                {
                    SubjectName = "TEL",
                    TheoryMarks = 46,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "II",
                    ExamYearPassout = 13,
                    HallTicketNumber = "007122072",
                    IsPracticals = false,
                    order = 2
                },
                new SubjectMarksDetails()
                {
                    SubjectName = "EST",
                    TheoryMarks = 74,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "II",
                    ExamYearPassout = 13,
                    HallTicketNumber = "007122072",
                    IsPracticals = false,
                    order = 3
                },
                 new SubjectMarksDetails()
                 {
                     SubjectName = "FBI",
                     TheoryMarks = 25,
                     ExamType = "A",
                     AcademicType = "CON",
                     AcademicYr = "II",
                     ExamYearPassout = 13,
                     HallTicketNumber = "007122072",
                     IsPracticals = true,
                     PracticleYrPassout = 13,
                    PracticleName = "A",
                    PracticleMarks = 25,
                     order = 4
                 },
                  new SubjectMarksDetails()
                  {
                      SubjectName = "AAC",
                      TheoryMarks = 31,
                      ExamType = "S",
                      AcademicType = "CON",
                      AcademicYr = "II",
                      ExamYearPassout = 13,
                      HallTicketNumber = "007122072",
                      IsPracticals = true,
                      PracticleYrPassout = 13,
                      PracticleName = "A",
                      PracticleMarks = 22,
                      order = 5
                  },

                   new SubjectMarksDetails()
                   {
                       SubjectName = "BSS",
                       TheoryMarks = 32,
                       ExamType = "S",
                       AcademicType = "CON",
                       AcademicYr = "II",
                       ExamYearPassout = 13,
                       HallTicketNumber = "007122072",
                       IsPracticals = true,
                       PracticleYrPassout = 13,
                       PracticleName = "A",
                       PracticleMarks = 23,
                       order = 6
                   },
                    new SubjectMarksDetails()
                    {
                        SubjectName = "TX2",
                        TheoryMarks = 37,
                        ExamType = "S",
                        AcademicType = "CON",
                        AcademicYr = "II",
                        ExamYearPassout = 13,
                        HallTicketNumber = "007122072",
                        IsPracticals = true,
                        PracticleYrPassout = 13,
                        PracticleName = "A",
                        PracticleMarks = 18,
                        order = 7
                    },
                     new SubjectMarksDetails()
                {
                    SubjectName = "OAT",
                    TheoryMarks = 63,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "II",
                    ExamYearPassout = 13,
                    HallTicketNumber = "007122072",
                    IsPracticals = false, order = 8}

            });


            #endregion


            #region con III
            objTabulrReportData.lstStudentInformaion[1].lstSubjectsandMarks.AddRange(new List<SubjectMarksDetails>() {
                new SubjectMarksDetails()
                {
                    SubjectName = "BSL",
                    TheoryMarks = 27,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122072",
                    IsPracticals = true,
                    PracticleYrPassout = 14,
                    PracticleName = "A",
                    PracticleMarks = 25,
                    order = 1,


                },
                new SubjectMarksDetails()
                {
                    SubjectName = "CRA",
                    TheoryMarks = 18,
                    ExamType = "S",
                    AcademicType = "CON",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122072",
                    IsPracticals = true,
                    PracticleYrPassout = 14,
                    PracticleName = "A",
                    PracticleMarks = 24,
                    order = 2,
                },
                new SubjectMarksDetails()
                {
                    SubjectName = "CAM",
                    TheoryMarks = 38,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122072",
                    IsPracticals = true,
                    PracticleYrPassout = 14,
                    PracticleName = "A",
                    PracticleMarks = 25,
                    order = 3,
                },
                 new SubjectMarksDetails()
                 {
                    SubjectName = "ADG",
                    TheoryMarks = 25,
                    ExamType = "A",
                    AcademicType = "CON",
                    AcademicYr = "III",
                    ExamYearPassout = 14,
                    HallTicketNumber = "007122072",
                    IsPracticals = true,
                    PracticleYrPassout = 14,
                    PracticleName = "A",
                    PracticleMarks = 26,
                    order = 4,
                 },
                  new SubjectMarksDetails()
                  {
                      SubjectName = "ACA",
                      TheoryMarks = 25,
                      ExamType = "A",
                      AcademicType = "CON",
                      AcademicYr = "III",
                      ExamYearPassout = 14,
                      HallTicketNumber = "007122072",
                      IsPracticals = true,
                      PracticleYrPassout = 14,
                      PracticleName = "A",
                      PracticleMarks = 26,
                      order = 5
                  },

                   new SubjectMarksDetails()
                   {
                       SubjectName = "ADA",
                       TheoryMarks = 56,
                       ExamType = "A",
                       AcademicType = "CON",
                       AcademicYr = "III",
                       ExamYearPassout = 14,
                       HallTicketNumber = "007122072",
                       IsPracticals = false,

                       order = 6
                   },
                    new SubjectMarksDetails()
                    {
                        SubjectName = "BCN",
                        TheoryMarks = 25,
                        ExamType = "A",
                        AcademicType = "CON",
                        AcademicYr = "III",
                        ExamYearPassout = 14,
                        HallTicketNumber = "007122072",
                        IsPracticals = false,


                        order = 7,
                    }, new SubjectMarksDetails()
                    {
                        SubjectName = "SCN",
                        TheoryMarks = 25,
                        ExamType = "A",
                        AcademicType = "CON",
                        AcademicYr = "III",
                        ExamYearPassout = 14,
                        HallTicketNumber = "007122072",
                        IsPracticals = false,

                        order = 8
                    }

            });







            #endregion

            #region PRE I
            objTabulrReportData.lstStudentInformaion[1].lstSubjectsandMarks.AddRange(new List<SubjectMarksDetails>() {

                new SubjectMarksDetails()
                {
                    SubjectName = "ENG",
                    ExamType = "S",
                    AcademicType = "PRE",
                    AcademicYr = "I",
                    ExamYearPassout = 18,
                    HallTicketNumber = "007122072",
                    order = 1,
                    Presence= "",
                    TheoryMarks=30,
                    AcademicStatus= "PASS"
                },

            });







            #endregion


            return objTabulrReportData;

        }
    }
}