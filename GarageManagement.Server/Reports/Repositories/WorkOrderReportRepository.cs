using GarageManagement.Server.Data;
using GarageManagement.Server.Reports.Models;
using GarageManagement.Server.Reports.RepoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
using System.Data;

namespace GarageManagement.Server.Reports.Repositories
{
    public class WorkOrderReportRepository:IWorkOrderReportRepository
    {
        private readonly ApplicationDbContext _context;
        public WorkOrderReportRepository(ApplicationDbContext context) { _context = context; }

        public async Task<byte[]> GenerateWorkOrderPdf(long jobCardId)
        {

            try
            {

                // WorkOrderDT

                var result = await _context.JobCards
                    .Where(b => b.Id == jobCardId)
                    .Select(a => new WorkOrderReportModel
                    {
                        JobCardNo = a.JobCardNo,
                        RegistrationNo = a.RegistrationNo,
                        OdometerIn = a.OdometerIn,
                        AvgKmsPerDay = a.AvgKmsPerDay,
                        Vin = a.Vin,
                        EngineNo = a.EngineNo,
                        VehicleColor = a.VehicleColor,
                        FuelType = a.FuelType,
                        ServiceType = a.ServiceType,
                        ServiceAdvisor = a.ServiceAdvisor,
                        Technician = a.Technician,
                        Vendor = a.Vendor,
                        IsApproved = true
                    })
                    .FirstOrDefaultAsync();

                if (result == null)
                    throw new Exception("No Job Card found");

        
                DataTable dt = new DataTable("WorkOrderDT");

                dt.Columns.Add("JobCardNo");
                dt.Columns.Add("RegistrationNo");
                dt.Columns.Add("OdometerIn");
                dt.Columns.Add("AvgKmsPerDay");
                dt.Columns.Add("Vin");
                dt.Columns.Add("EngineNo");
                dt.Columns.Add("VehicleColor");
                dt.Columns.Add("FuelType");
                dt.Columns.Add("ServiceType");
                dt.Columns.Add("ServiceAdvisor");
                dt.Columns.Add("Technician");
                dt.Columns.Add("Vendor");
                dt.Columns.Add("IsApproved", typeof(bool));

                dt.Rows.Add(
                    result.JobCardNo,
                    result.RegistrationNo,
                    result.OdometerIn,
                    result.AvgKmsPerDay,
                    result.Vin,
                    result.EngineNo,
                    result.VehicleColor,
                    result.FuelType,
                    result.ServiceType,
                    result.ServiceAdvisor,
                    result.Technician,
                    result.Vendor,
                    result.IsApproved
                );


                // Customer Voice

                var concerns = await _context.JobCardConcerns
                                                 .Where(a => a.JobCardId == jobCardId)
                                                 .Select(a => a.Text)
                                                 .ToListAsync();

                int totalRows = 10;
                DataTable dtCustomerVoice = new DataTable("CustomerVoiceDS");
                dtCustomerVoice.Columns.Add("Col1");
                dtCustomerVoice.Columns.Add("Col2");

                for (int i = 0; i < totalRows; i++)
                {
                    string c1 = i < concerns.Count ? $"{i + 1}. {concerns[i]}" : $"{i + 1}.";
                    int c2Index = i + totalRows;
                    string c2 = c2Index < concerns.Count ? $"{c2Index + 1}. {concerns[c2Index]}" : $"{c2Index + 1}.";

                    dtCustomerVoice.Rows.Add(c1, c2);
                }


                // Accessories

                DataTable dtAccessories = new DataTable("AccessoriesDS");
                dtAccessories.Columns.Add("Col1");
                dtAccessories.Columns.Add("Col2");

                // List of accessories and their checked state
                var accessories = new List<(string Name, bool IsChecked)>
                                    {
                                        ("Service booklet", true),
                                        ("Mud Flaps", false),
                                        ("Jack and Handle", true),
                                        ("Type condition", false),
                                        ("Tool kit", true),
                                        ("Battery Make", false),
                                        ("Floor mats", true),
                                        ("Battery No.", false),
                                        ("Spare Wheel", true),
                                        ("Stereo", false),
                                        ("Amplifier", true),
                                        ("DVD Player", false),
                                        ("LED TV", true),
                                        ("Speakers Qty", false),
                                        ("Cigarette Lighter", true)
                                    };

                int totalRowss = 10; // number of rows per column
                for (int i = 0; i < totalRowss; i++)
                {
                    // Left column
                    string left = i < accessories.Count
                        ? $"{(accessories[i].IsChecked ? "☑" : "☐")}    {accessories[i].Name}"
                        : "";

                    // Right column
                    int rightIndex = i + totalRowss;
                    string right = rightIndex < accessories.Count
                        ? $"{(accessories[rightIndex].IsChecked ? "☑" : "☐")}    {accessories[rightIndex].Name}"
                        : "";

                    dtAccessories.Rows.Add(left, right);
                }


                // Parts

                DataTable dtParts = new DataTable("PartsDS");

                dtParts.Columns.Add("SlNo", typeof(int));
                dtParts.Columns.Add("PartName", typeof(string));
                dtParts.Columns.Add("PartService", typeof(string));
                dtParts.Columns.Add("Quantity", typeof(int));
                dtParts.Columns.Add("Category", typeof(string));
                dtParts.Columns.Add("Technician", typeof(string));
                dtParts.Columns.Add("ServiceTimeDone", typeof(string));

                var partsList = new List<(string PartName, string PartService, int Quantity, string Category, string Technician, string ServiceTimeDone)>
                                        {
                                            ("Break", "Repair", 1, "Mechanical", "Ramesh", "10:00 AM"),
                                            ("Head Light", "Replace", 1, "Electrical", "Suresh", "10:30 AM"),
                                            ("Oil Filter", "Replace", 1, "Maintenance", "Ramesh", "11:00 AM")
                                        };

                int slNo = 1;
                foreach (var p in partsList)
                {
                    dtParts.Rows.Add(slNo++, p.PartName, p.PartService, p.Quantity, p.Category, p.Technician, p.ServiceTimeDone);
                }

                // Create Report

                LocalReport report = new LocalReport();

                report.ReportPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Reports", "RDLC", "WorkOrderReportDesigner.rdlc");

                report.EnableExternalImages = true;

                // Add DataSource

                report.DataSources.Clear();

                report.DataSources.Add(new ReportDataSource("WorkOrderDS", dt));
                report.DataSources.Add(new ReportDataSource("CustomerVoiceDS", dtCustomerVoice));
                report.DataSources.Add(new ReportDataSource("AccessoriesDS", dtAccessories));
                report.DataSources.Add(new ReportDataSource("PartsDS", dtParts));

                // Set Report Parameters

                var parameters = new List<ReportParameter>
                {
                    new ReportParameter("JobCardNo", result.JobCardNo ?? ""),
                    new ReportParameter("RegistrationNo", result.RegistrationNo ?? ""),
                    new ReportParameter("OdometerIn", result.OdometerIn?.ToString() ?? ""),
                    new ReportParameter("AvgKmsPerDay", result.AvgKmsPerDay?.ToString() ?? ""),
                    new ReportParameter("Vin", result.Vin ?? ""),
                    new ReportParameter("EngineNo", result.EngineNo ?? ""),
                    new ReportParameter("VehicleColor", result.VehicleColor ?? ""),
                    new ReportParameter("FuelType", result.FuelType ?? ""),
                    new ReportParameter("ServiceType", result.ServiceType ?? ""),
                    new ReportParameter("ServiceAdvisor", result.ServiceAdvisor ?? ""),
                    new ReportParameter("Technician", result.Technician ?? ""),
                    new ReportParameter("Vendor", result.Vendor ?? ""),
                    new ReportParameter("IsApproved", result.IsApproved.ToString())
                };

                report.SetParameters(parameters);

                // Render PDF

                return report.Render("PDF");
            }
            catch(Exception ex)
            {
                throw;
            }
          
        }


    }
}
