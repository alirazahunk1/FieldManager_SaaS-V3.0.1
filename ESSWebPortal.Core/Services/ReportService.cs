using ClosedXML.Excel;
using ESSDataAccess.DbContext;
using ESSDataAccess.Enum;
using ESSDataAccess.Identity;
using ESSDataAccess.Models;
using ESSWebPortal.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESSWebPortal.Core.Services
{
    public class ReportService : IReport
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateTimeLineReport(string filePath, DateTime monthYear)
        {
            var now = DateTime.Now;

            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            var fileName = "TimeLineReport_" + monthYear.ToString("MMM/yyyy") + $"_On_{now.ToString("HH_mm_ss_tt")}.xlsx";

            var dates = DateHelper.GetDates(monthYear.Year, monthYear.Month);

            var teams = await _context.Teams
            .Select(x => new TeamModel
            {
                Id = x.Id,
                Name = x.Name,
            }).AsNoTracking()
            .ToListAsync();

            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            var employees = users.Where(x => x.ParentId != null && x.TeamId != null && x.ScheduleId != null)
                .ToList();

            var attendances = await _context.Attendances
                .Where(x => x.CreatedAt.Month == monthYear.Month && x.CreatedAt.Year == monthYear.Year)
                .AsNoTracking()
                .ToListAsync();

            var schedules = await _context.Schedules
                .AsNoTracking()
                .ToListAsync();

            var titleString = "TimeLine Attendance Report " + dates.FirstOrDefault().ToString("MMMM dd yyyy") + " to " + dates.LastOrDefault().ToString("MMMM dd yyyy");

            var preparedOn = "Prepared on : " + now.ToString("dd/MM/yyyy HH:mm tt");

            var employeesCountString = "Employees Count : " + employees.Count();

            var wb = new XLWorkbook();

            //Summary Sheet
            var wsSummary = wb.Worksheets.Add("Summary");

            wsSummary.Cell("A1").Value = titleString;
            wsSummary.Cell("A1").Style.Font.FontSize = 14;
            wsSummary.Cell("A1").Style.Font.Bold = true;

            var rngMain = wsSummary.Range("A1:C1");
            rngMain.Row(1).Merge();

            wsSummary.Cell("A3").Value = preparedOn;
            wsSummary.Cell("A4").Value = employeesCountString;

            //Setting Headers
            wsSummary.Cell("A6").Value = "Manager Name";
            wsSummary.Cell("B6").Value = "Employee Name";
            wsSummary.Cell("C6").Value = "Designation";
            wsSummary.Cell("D6").Value = "Unique ID";
            wsSummary.Cell("E6").Value = "Team";
            wsSummary.Cell("F6").Value = "Present (Days)";
            wsSummary.Cell("G6").Value = "Half (Days)";
            wsSummary.Cell("H6").Value = "Absent (Days)";
            wsSummary.Cell("I6").Value = "Weekly Off (Days)";
            wsSummary.Cell("J6").Value = "On Leave (Days)";
            wsSummary.Cell("K6").Value = "Penalty (Days)";
            wsSummary.Cell("L6").Value = "Total (Days)";
            wsSummary.Cell("M6").Value = "In Late (Days)";
            wsSummary.Cell("N6").Value = "Out Early (Days)";
            wsSummary.Cell("O6").Value = "Total Attendance Time";

            //Adding Employees data
            int row = 7;
            foreach (var employee in employees)
            {
                var manager = users.FirstOrDefault(x => x.Id == employee.ParentId);
                var team = teams.FirstOrDefault(x => x.Id == employee.TeamId);
                var schedule = schedules.FirstOrDefault(x => x.Id == employee.ScheduleId);
                var employeeAttendances = attendances.Where(x => x.EmployeeId == employee.Id);

                TimeSpan shiftTimeHalf = (schedule.EndTime - schedule.StartTime) / 2;

                var presentDays = 0;
                var halfDays = 0;
                var absentDays = 0;
                var weeklyOffDays = 0;
                var onLeaveDays = 0;
                var penaltyDays = 0;

                var inLateDays = 0;
                var outLateDays = 0;
                var totalHours = 0d;
                var totalMinutes = 0d;

                foreach (var date in dates)
                {
                    var attendance = employeeAttendances.FirstOrDefault(x => x.CreatedAt.Date == date.Date);

                    if (attendance == null)
                    {
                        if (!IsWeekOff(schedule, date))
                        {
                            absentDays++;
                        }
                        else
                        {
                            weeklyOffDays++;
                        }
                    }
                    else
                    {
                        presentDays++;


                        if (attendance.CheckInTime.TimeOfDay < schedule.StartTime.TimeOfDay)
                        {
                            inLateDays++;
                        }

                        if (!attendance.CheckOutTime.HasValue)
                        {
                            var lastData = await _context.Trackings
                                .Where(x => x.AttendanceId == attendance.Id)
                                .OrderByDescending(x => x.CreatedAt)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

                            if (lastData != null)
                            {
                                var data = lastData.CreatedAt - attendance.CheckInTime;
                                totalHours += data.Hours;
                                totalMinutes += data.Minutes;
                            }
                        }
                        else
                        {
                            var data = attendance.CheckOutTime.Value - attendance.CheckInTime;

                            totalHours += data.Hours;
                            totalMinutes += data.Minutes;

                            if (attendance.CheckOutTime.Value.TimeOfDay <= shiftTimeHalf)
                            {
                                halfDays++;
                            }
                            else if (attendance.CheckOutTime.Value.TimeOfDay > schedule.EndTime.TimeOfDay)
                            {
                                outLateDays++;
                            }
                        }
                    }
                }

                wsSummary.Cell($"A{row}").Value = manager == null ? string.Empty : manager.GetFullName();
                wsSummary.Cell($"B{row}").Value = employee.GetFullName();
                wsSummary.Cell($"C{row}").Value = employee.Designation;
                wsSummary.Cell($"D{row}").Value = "E" + employee.Id;
                wsSummary.Cell($"E{row}").Value = team == null ? string.Empty : team.Name;
                wsSummary.Cell($"F{row}").Value = presentDays;
                wsSummary.Cell($"G{row}").Value = halfDays;
                wsSummary.Cell($"H{row}").Value = absentDays;
                wsSummary.Cell($"I{row}").Value = weeklyOffDays;
                wsSummary.Cell($"J{row}").Value = onLeaveDays;
                wsSummary.Cell($"K{row}").Value = penaltyDays;
                wsSummary.Cell($"L{row}").Value = presentDays + halfDays + absentDays + weeklyOffDays + onLeaveDays;
                wsSummary.Cell($"M{row}").Value = inLateDays;
                wsSummary.Cell($"N{row}").Value = outLateDays;


                double number = totalMinutes / 60;

                int left = 0;
                int right = 0;

                TimeHelper.Split(number, 1, out left, out right);

                wsSummary.Cell($"O{row}").Value = $"{totalHours + left} Hours {right} Minutes";
                wsSummary.Columns(1, 16).AdjustToContents();

                row++;
            }

            var rngTable = wsSummary.Range($"A6:O{row}");

            var rngHeader = rngTable.Range("A6:O6");
            rngHeader.Style.Font.Bold = true;
            rngHeader.Style.Font.FontSize = 12;

            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //Summary Sheet Ends

            //Quick Sheet Start
            var wsQuick = wb.Worksheets.Add("Quick");

            wsQuick.Cell("A1").Value = titleString;
            wsQuick.Cell("A1").Style.Font.FontSize = 14;
            wsQuick.Cell("A1").Style.Font.Bold = true;

            wsQuick.Cell("A3").Value = preparedOn;
            wsQuick.Cell("A4").Value = employeesCountString;

            //Setting headers
            wsQuick.Cell("A6").Value = "Date || Employee Name";
            wsQuick.Cell("A6").Style.Font.FontSize = 12;
            wsQuick.Cell("A6").Style.Font.Bold = true;
            wsQuick.Cell("A7").Value = "";

            wsQuick.SheetView.FreezeColumns(1);
            wsQuick.SheetView.FreezeRows(7);

            row = 8;
            foreach (var date in dates)
            {
                wsQuick.Cell($"A{row}").Value = date.ToString("dd/MM/yyyy");
                row++;
            }

            //B-G 4 columns for each user
            int col = 2;
            foreach (var employee in employees)
            {
                var team = teams.FirstOrDefault(x => x.Id == employee.TeamId);
                var employeeAttendances = attendances.Where(x => x.EmployeeId == employee.Id);
                var schedule = schedules.FirstOrDefault(x => x.Id == employee.ScheduleId);

                var lcol = col;
                //Setting the header
                wsQuick.Cell(6, lcol).Value = employee.GetFullName();
                wsQuick.Cell(6, lcol).Style.Font.Bold = true;
                wsQuick.Cell(6, lcol).Style.Font.FontSize = 12;
                var rngName = wsQuick.Range(6, lcol, 6, lcol + 4);
                rngName.Row(1).Merge();

                wsQuick.Cell(7, lcol).Value = "Check In At";
                wsQuick.Cell(7, lcol).Style.Font.FontSize = 12;
                wsQuick.Cell(7, lcol).Style.Font.Bold = true;
                lcol++;
                wsQuick.Cell(7, lcol).Value = "Check Out At";
                wsQuick.Cell(7, lcol).Style.Font.FontSize = 12;
                wsQuick.Cell(7, lcol).Style.Font.Bold = true;
                lcol++;
                wsQuick.Cell(7, lcol).Value = "Time On Duty";
                wsQuick.Cell(7, lcol).Style.Font.FontSize = 12;
                wsQuick.Cell(7, lcol).Style.Font.Bold = true;
                lcol++;
                wsQuick.Cell(7, lcol).Value = "Status";
                wsQuick.Cell(7, lcol).Style.Font.FontSize = 12;
                wsQuick.Cell(7, lcol).Style.Font.Bold = true;
                lcol++;
                wsQuick.Cell(7, lcol).Value = "Teams";
                wsQuick.Cell(7, lcol).Style.Font.FontSize = 12;
                wsQuick.Cell(7, lcol).Style.Font.Bold = true;
                lcol++;
                var lRow = 8;




                foreach (var date in dates)
                {
                    string inTime = string.Empty;
                    string outTime = string.Empty;
                    string status = string.Empty;
                    string timeOfDuty = string.Empty;

                    var attendance = employeeAttendances.FirstOrDefault(x => x.CreatedAt.Date == date.Date);

                    if (attendance == null)
                    {
                        if (!IsWeekOff(schedule, date))
                        {
                            status = "Absent";
                        }
                        else
                        {
                            status = "Weekly Off";
                        }
                    }
                    else
                    {
                        if (!attendance.CheckOutTime.HasValue)
                        {
                            var lastData = await _context.Trackings
                                .Where(x => x.AttendanceId == attendance.Id)
                                .OrderByDescending(x => x.CreatedAt)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

                            if (lastData != null)
                            {
                                attendance.CheckOutTime = lastData.CreatedAt;
                            }
                        }

                        var time = attendance.CheckOutTime.Value - attendance.CheckInTime;

                        //Present Case
                        inTime = attendance.CheckInTime.ToString("hh:mm tt");
                        outTime = attendance.CheckOutTime.Value.ToString("hh:mm tt");
                        timeOfDuty = $"{time.Hours} Hours {time.Minutes} minutes";
                        status = "Present";
                    }
                    var lCol = col;
                    //Setting Values
                    wsQuick.Cell(lRow, lCol).Value = !string.IsNullOrEmpty(inTime) ? $"[{inTime}]" : string.Empty;
                    lCol++;
                    wsQuick.Cell(lRow, lCol).Value = !string.IsNullOrEmpty(outTime) ? $"[{outTime}]" : string.Empty;
                    lCol++;
                    wsQuick.Cell(lRow, lCol).Value = timeOfDuty;
                    lCol++;
                    wsQuick.Cell(lRow, lCol).Value = status;
                    lCol++;
                    wsQuick.Cell(lRow, lCol).Value = team.Name;
                    lCol++;


                    lRow++;
                }

                col += 5;
                row = lRow;
            }

            wsQuick.Row(1).Merge();

            var rngQuick = wsQuick.Range(6, 1, row, col);
            rngQuick.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rngQuick.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngQuick.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            wsQuick.Columns(1, col).AdjustToContents();
            //Quick Sheet Ends
            var wsDetail = wb.Worksheets.Add("Detail");

            var wsOverview = wb.Worksheets.Add("Overview");

            var finalPath = filePath + fileName;

            wb.SaveAs(finalPath);

            return finalPath;
        }

        public async Task<string> GenerateAttendanceReport(string filePath, string type, DateTime monthYear, List<string> ids, bool isAll = false)
        {
            var now = DateTime.Now.ToString("HH_mm_ss_tt");

            var fileName = monthYear.ToString("MMM/yyyy") + $"_On_{now}.xlsx";

            //Teams
            if (type.Equals("0"))
            {
                var finalName = "TeamWiseReport_" + fileName;

                var result = await GenAttendanceReport(monthYear, isAll ? new List<string>() : ids, true, filePath, finalName);

                if (!result) return string.Empty;

                return finalName;
            }
            else
            {
                var finalName = "UserWiseReport_" + fileName;
                var result = await GenAttendanceReport(monthYear, isAll ? new List<string>() : ids, false, filePath, finalName);

                if (!result) return string.Empty;

                return finalName;

            }

        }

        public async Task<string> GenerateVisitReport(string filePath, string type, DateTime monthYear, List<string> ids, bool isAll = false)
        {
            var now = DateTime.Now.ToString("HH_mm_ss_tt");

            var fileName = monthYear.ToString("MMM/yyyy") + $"_On_{now}.xlsx";

            //Teams
            if (type.Equals("1"))
            {
                var finalName = "TeamWiseReport_" + fileName;

                var result = await GetVisitReport(monthYear, isAll ? new List<string>() : ids, true, filePath, finalName);

                if (!result) return string.Empty;

                return finalName;
            }
            else if (type.Equals("2"))
            {
                var finalName = "UserWiseReport_" + fileName;
                var result = await GetVisitReport(monthYear, isAll ? new List<string>() : ids, false, filePath, finalName);

                if (!result) return string.Empty;

                return finalName;

            }
            else return string.Empty;

        }


        private async Task<bool> GenAttendanceReport(DateTime date, List<string> ids, bool isTeamWise, string filePath, string fileName)
        {
            List<TeamModel> teams = new List<TeamModel>();

            var dates = DateHelper.GetDates(date.Year, date.Month);

            var workbook = new XLWorkbook();

            var users = await _context.Users
                .Include(x => x.Schedule)
                .ToListAsync();

            var attendances = await _context.Attendances!
                .Where(x => x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year)
                .Include(x => x.Trackings)
                .ToListAsync();

            if (isTeamWise)
            {
                if (ids.Count() == 0)
                {
                    teams = await _context.Teams
                        .ToListAsync();
                }
                else
                {
                    teams = await _context.Teams
                       .Where(x => ids.Contains(x.Id.ToString()))
                       .ToListAsync();
                }

                //Creating sheets by team
                foreach (var team in teams)
                {
                    var usersInTeam = users.Where(x => x.TeamId == team.Id).ToList();

                    var ws = workbook.Worksheets.Add(team.Name.ToUpper());

                    int row = 2;
                    int begRow = 2;
                    //Generating report by user
                    for (int i = 0; i < usersInTeam.Count(); i++)
                    {
                        var user = usersInTeam[i];

                        var attendance = attendances
                            .Where(x => x.EmployeeId == user.Id)
                            .ToList();

                        ws.Cell(row, 1).Value = user.GetFullName() + " (User ID: " + user.Id + ")";

                        //For new line 
                        row++;

                        ws.Cell($"A{row}").Value = "S.No";
                        ws.Cell($"B{row}").Value = "DATE";
                        ws.Cell($"C{row}").Value = "P/A";
                        ws.Cell($"D{row}").Value = "In/Out";
                        ws.Cell($"E{row}").Value = "WORKING HRS";
                        ws.Cell($"F{row}").Value = "REMARKS";

                        ws.Cell($"A{row}").Style.Font.Bold = true;
                        ws.Cell($"B{row}").Style.Font.Bold = true;
                        ws.Cell($"C{row}").Style.Font.Bold = true;
                        ws.Cell($"D{row}").Style.Font.Bold = true;
                        ws.Cell($"E{row}").Style.Font.Bold = true;
                        ws.Cell($"F{row}").Style.Font.Bold = true;

                        //Setting the value for every days in the month and year

                        for (int j = 0; j < dates.Count(); j++)
                        {
                            //increment row by one to enter on next line
                            row++;

                            var dt = dates[j];

                            var att = attendance.FirstOrDefault(x => x.CreatedAt.Date == dt.Date);

                            string workingHours, status, remarks, @in, @out;
                            @in = @out = workingHours = status = remarks = string.Empty;

                            DateTime? checkOut = null;
                            if (att is null)
                            {
                                bool isWeekOff = IsWeekOff(user.Schedule, dt);

                                status = isWeekOff ? "P/A" : "A";
                                remarks = isWeekOff ? "Week Off" : "";
                            }
                            else
                            {
                                status = "P";

                                if (att.CheckOutTime.HasValue)
                                {
                                    checkOut = att.CheckOutTime;
                                }
                                else
                                {
                                    checkOut = att.Trackings
                                    .OrderByDescending(x => x.CreatedAt)
                                    .Select(x => x.CreatedAt)
                                    .FirstOrDefault();
                                }

                                workingHours = $"{(checkOut - att.CheckInTime).Value.Hours} Hours {(checkOut - att.CheckInTime).Value.Minutes} Minutes";

                                @in = att.CheckInTime.ToString("hh:mm tt");

                                @out = checkOut.Value.ToString("hh:mm tt");
                            }

                            //Set sl.no
                            ws.Cell($"A{row}").Value = (j + 1).ToString();
                            ws.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            //Set Date
                            ws.Cell($"B{row}").Value = dt.ToString("dd-MM-yyyy");
                            ws.Cell($"B{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            //Set Present / Abs
                            ws.Cell($"C{row}").Value = status;
                            ws.Cell($"C{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            //In/Out
                            ws.Cell($"D{row}").SetDataType(XLDataType.Text);
                            ws.Cell($"D{row}").Value = $"{@in} - {@out}";
                            ws.Cell($"D{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            //Working Hours
                            ws.Cell($"E{row}").Value = workingHours;
                            ws.Cell($"E{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            //Remarks
                            ws.Cell($"F{row}").Value = remarks;
                            ws.Cell($"F{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                        }

                        //Total Table range
                        var rngTable = ws.Range($"A{begRow}:F{row}");
                        rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        //Format title cell
                        rngTable.Cell(1, 1).Style.Font.Bold = true;
                        rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                        rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        //Merge title cells
                        rngTable.Row(1).Merge(); // We could've also used: rngTable.Range("A1:E1").Merge()

                        //Add thick borders
                        rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        //Setting the header
                        /*     var rngHeader = rngTable.Range($"A{row}:D{row}");
                            rngHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            rngHeader.Style.Font.Bold = true;
                            rngHeader.Style.Fill.BackgroundColor = XLColor.Aqua;*/


                        row += 4;
                        begRow = row;
                    }

                    ws.Columns(1, 7).AdjustToContents();
                }
            }

            workbook.SaveAs(filePath + fileName);
            return true;
        }

        private async Task<bool> GetVisitReport(DateTime date, List<string> ids, bool isTeamWise, string filePath, string fileName)
        {
            List<TeamModel> teams = new List<TeamModel>();

            List<AppUser> userlist = new List<AppUser>();

            var dates = DateHelper.GetDates(date.Year, date.Month);

            var workbook = new XLWorkbook();

            var users = await _context.Users
                .Where(x => x.Status == UserStatus.Active)
                .Include(x => x.Schedule)
                .Include(x => x.Attendances)!.ThenInclude(x => x.Visits)
                .ToListAsync();

            var attendances = await _context.Attendances!
                .Where(x => x.CreatedAt.Month == date.Month && x.CreatedAt.Year == date.Year)
                .Include(x => x.Trackings)
                .Include(x => x.Visits)!.ThenInclude(x => x.Client)
                .ToListAsync();

            /*var visits = await _context.Visits
                .Where(x => x.AttendanceId)*/

            if (isTeamWise)
            {
                if (ids.Count() == 0)
                {
                    teams = await _context.Teams
                        .ToListAsync();
                }
                else
                {
                    teams = await _context.Teams
                       .Where(x => ids.Contains(x.Id.ToString()))
                       .ToListAsync();
                }

                //Creating sheets by team
                foreach (var team in teams)
                {
                    var usersInTeam = users.Where(x => x.TeamId == team.Id).ToList();

                    var ws = workbook.Worksheets.Add(team.Name.ToUpper());

                    int row = 2;
                    int begRow = 2;
                    //Generating report by user
                    for (int i = 0; i < usersInTeam.Count(); i++)
                    {
                        var user = usersInTeam[i];

                        var attendance = attendances
                            .Where(x => x.EmployeeId == user.Id)
                            .ToList();

                        ws.Cell(row, 1).Value = user.GetFullName() + " (User ID: " + user.Id + ")";

                        //For new line 
                        row++;

                        ws.Cell($"A{row}").Value = "S.No";
                        ws.Cell($"B{row}").Value = "Client";
                        ws.Cell($"C{row}").Value = "Date";
                        ws.Cell($"D{row}").Value = "Time";
                        ws.Cell($"E{row}").Value = "Address";
                        ws.Cell($"F{row}").Value = "REMARKS";

                        ws.Cell($"A{row}").Style.Font.Bold = true;
                        ws.Cell($"B{row}").Style.Font.Bold = true;
                        ws.Cell($"C{row}").Style.Font.Bold = true;
                        ws.Cell($"D{row}").Style.Font.Bold = true;
                        ws.Cell($"E{row}").Style.Font.Bold = true;
                        ws.Cell($"F{row}").Style.Font.Bold = true;

                        //Setting the value for every days in the month and year

                        for (int j = 0; j < dates.Count(); j++)
                        {
                            var dt = dates[j];

                            var att = attendance.FirstOrDefault(x => x.CreatedAt.Date == dt.Date);

                            if (att == null) continue;

                            for (int k = 0; k < att.Visits.Count(); k++)
                            {
                                //increment row by one to enter on next line
                                row++;

                                string address, status, remarks, client, time;
                                client = address = status = remarks = time = string.Empty;

                                DateTime? checkOut = null;
                                if (att is null)
                                {
                                    bool isWeekOff = IsWeekOff(user.Schedule, dt);

                                    //status = isWeekOff ? "P/A" : "A";
                                    remarks = isWeekOff ? "Week Off" : "";
                                }
                                else
                                {
                                    status = "P";

                                    if (att.CheckOutTime.HasValue)
                                    {
                                        checkOut = att.CheckOutTime;
                                    }
                                    else
                                    {
                                        checkOut = att.Trackings
                                        .OrderByDescending(x => x.CreatedAt)
                                        .Select(x => x.CreatedAt)
                                        .FirstOrDefault();
                                    }

                                    client = att.Visits[k].Client.Name;

                                    address = att.Visits[k].Address;

                                    remarks = att.Visits[k].Remarks;

                                    time = att.Visits[k].CreatedAt.ToString("hh:mm tt");

                                    /*@in = att.CheckInTime.ToString("hh:mm tt");

                                    @out = checkOut.Value.ToString("hh:mm tt");*/
                                }

                                //Set sl.no
                                ws.Cell($"A{row}").Value = (k + 1).ToString();
                                ws.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //Client name
                                ws.Cell($"B{row}").Value = client;
                                ws.Cell($"B{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //Date
                                ws.Cell($"C{row}").Value = dt.ToString("dd-MM-yyyy");
                                ws.Cell($"C{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //Time
                                ws.Cell($"D{row}").SetDataType(XLDataType.Text);
                                ws.Cell($"D{row}").Value = $"[{time}]";
                                ws.Cell($"D{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //address
                                ws.Cell($"E{row}").SetDataType(XLDataType.Text);
                                ws.Cell($"E{row}").Value = address;
                                ws.Cell($"E{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //Working Hours
                                ws.Cell($"F{row}").Value = remarks;
                                ws.Cell($"F{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            }

                        }

                        //Total Table range
                        var rngTable = ws.Range($"A{begRow}:F{row}");
                        rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        //Format title cell
                        rngTable.Cell(1, 1).Style.Font.Bold = true;
                        rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                        rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        //Merge title cells
                        rngTable.Row(1).Merge(); // We could've also used: rngTable.Range("A1:E1").Merge()

                        //Add thick borders
                        rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        //Setting the header
                        /*     var rngHeader = rngTable.Range($"A{row}:D{row}");
                            rngHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            rngHeader.Style.Font.Bold = true;
                            rngHeader.Style.Fill.BackgroundColor = XLColor.Aqua;*/


                        row += 4;
                        begRow = row;
                    }

                    ws.Columns(1, 7).AdjustToContents();
                }
            }
            else
            {
                if (ids.Count() == 0)
                {
                    userlist = await _context.Users
                        .ToListAsync();
                }
                else
                {
                    userlist = await _context.Users
                       .Where(x => ids.Contains(x.Id.ToString()))
                       .ToListAsync();
                }

                var ws = workbook.Worksheets.Add("User Report");

                foreach (var user in userlist)
                {
                    //var usersInTeam = users.Where(x => x.TeamId == team.Id).ToList();



                    int row = 2;
                    int begRow = 2;
                    //Generating report by user
                    for (int i = 0; i < userlist.Count(); i++)
                    {
                        var usr = userlist[i];

                        var attendance = attendances
                            .Where(x => x.EmployeeId == usr.Id)
                            .ToList();

                        ws.Cell(row, 1).Value = user.GetFullName() + " (User ID: " + user.Id + ")";

                        //For new line 
                        row++;

                        ws.Cell($"A{row}").Value = "S.No";
                        ws.Cell($"B{row}").Value = "Client";
                        ws.Cell($"C{row}").Value = "Date";
                        ws.Cell($"D{row}").Value = "Time";
                        ws.Cell($"E{row}").Value = "Address";
                        ws.Cell($"F{row}").Value = "REMARKS";

                        ws.Cell($"A{row}").Style.Font.Bold = true;
                        ws.Cell($"B{row}").Style.Font.Bold = true;
                        ws.Cell($"C{row}").Style.Font.Bold = true;
                        ws.Cell($"D{row}").Style.Font.Bold = true;
                        ws.Cell($"E{row}").Style.Font.Bold = true;
                        ws.Cell($"F{row}").Style.Font.Bold = true;

                        //Setting the value for every days in the month and year

                        for (int j = 0; j < dates.Count(); j++)
                        {
                            var dt = dates[j];

                            var att = attendance.FirstOrDefault(x => x.CreatedAt.Date == dt.Date);

                            if (att == null) continue;

                            for (int k = 0; k < att.Visits.Count(); k++)
                            {
                                //increment row by one to enter on next line
                                row++;

                                string address, status, remarks, client, time;
                                client = address = status = remarks = time = string.Empty;

                                DateTime? checkOut = null;
                                if (att is null)
                                {
                                    bool isWeekOff = IsWeekOff(user.Schedule, dt);

                                    //status = isWeekOff ? "P/A" : "A";
                                    remarks = isWeekOff ? "Week Off" : "";
                                }
                                else
                                {
                                    status = "P";

                                    if (att.CheckOutTime.HasValue)
                                    {
                                        checkOut = att.CheckOutTime;
                                    }
                                    else
                                    {
                                        checkOut = att.Trackings
                                        .OrderByDescending(x => x.CreatedAt)
                                        .Select(x => x.CreatedAt)
                                        .FirstOrDefault();
                                    }

                                    client = att.Visits[k].Client.Name;

                                    address = att.Visits[k].Address;

                                    remarks = att.Visits[k].Remarks;

                                    time = att.Visits[k].CreatedAt.ToString("h:mm tt");

                                    //time = tt.Hour + ":" + tt.Minute;

                                    /*@in = att.CheckInTime.ToString("hh:mm tt");

                                    @out = checkOut.Value.ToString("hh:mm tt");*/
                                }

                                //Set sl.no
                                ws.Cell($"A{row}").Value = (k + 1).ToString();
                                ws.Cell($"A{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //Set Date
                                ws.Cell($"B{row}").Value = client;
                                ws.Cell($"B{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //Client name
                                ws.Cell($"C{row}").Value = dt.ToString("dd-MM-yyyy");
                                ws.Cell($"C{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //Client name
                                ws.Cell($"D{row}").SetDataType(XLDataType.Text);
                                ws.Cell($"D{row}").Value = time;
                                ws.Cell($"D{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //address
                                ws.Cell($"E{row}").SetDataType(XLDataType.Text);
                                ws.Cell($"E{row}").Value = address;
                                ws.Cell($"E{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                                //Working Hours
                                ws.Cell($"F{row}").Value = remarks;
                                ws.Cell($"F{row}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            }

                        }

                        //Total Table range
                        var rngTable = ws.Range($"A{begRow}:F{row}");
                        rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                        //Format title cell
                        rngTable.Cell(1, 1).Style.Font.Bold = true;
                        rngTable.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                        rngTable.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        //Merge title cells
                        rngTable.Row(1).Merge(); // We could've also used: rngTable.Range("A1:E1").Merge()

                        //Add thick borders
                        rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        //Setting the header
                        /*     var rngHeader = rngTable.Range($"A{row}:D{row}");
                            rngHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            rngHeader.Style.Font.Bold = true;
                            rngHeader.Style.Fill.BackgroundColor = XLColor.Aqua;*/


                        row += 4;
                        begRow = row;
                    }

                    ws.Columns(1, 7).AdjustToContents();
                }

            }

            workbook.SaveAs(filePath + fileName);
            return true;
        }

        private static bool IsWeekOff(ScheduleModel schedule, DateTime date)
        {
            if (schedule == null) return false;

            var week = date.DayOfWeek;
            switch (week)
            {
                case DayOfWeek.Sunday:
                    return !schedule.Sunday;
                case DayOfWeek.Monday:
                    return !schedule.Monday;
                case DayOfWeek.Tuesday:
                    return !schedule.Tuesday;
                case DayOfWeek.Wednesday:
                    return !schedule.Wednesday;
                case DayOfWeek.Thursday:
                    return !schedule.Thursday;
                case DayOfWeek.Friday:
                    return !schedule.Friday;
                case DayOfWeek.Saturday:
                    return !schedule.Saturday;
                default:
                    break;
            }
            return false;
        }


    }
}
