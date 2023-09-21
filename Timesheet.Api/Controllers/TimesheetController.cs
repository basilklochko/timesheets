using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Timesheet.Api.Models;
using Timesheet.Library.Model;
using Timesheet.Library.Repository;

namespace Timesheet.Api.Controllers
{
    [Auth]
    public class TimesheetController : ApiController
    {
        protected ITimesheetRepository TimesheetRepository;
        protected IEmailRepository EmailRepository;
        protected IUserRepository UserRepository;

        public TimesheetController(ITimesheetRepository timesheetRepository, IEmailRepository emailRepository, IUserRepository userRepository)
        {
            TimesheetRepository = timesheetRepository;
            EmailRepository = emailRepository;
            UserRepository = userRepository;
        }

        // GET api/timesheet?userId=userId&clientId=clientId
        public IEnumerable<Timesheet.Library.Model.Timesheet> Get(string userId, string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return TimesheetRepository.GetAllById(Int32.Parse(userId)) as IEnumerable<Timesheet.Library.Model.Timesheet>;
            }
            else
            {
                return TimesheetRepository.GetAllById(Int32.Parse(userId), Int32.Parse(clientId)) as IEnumerable<Timesheet.Library.Model.Timesheet>;
            }
        }

        // GET api/timesheet/id
        public Timesheet.Library.Model.Timesheet Get(int id)
        {
            return TimesheetRepository.Get(id) as Timesheet.Library.Model.Timesheet;
        }

        // POST api/timesheet
        public HttpResponseMessage Post(Timesheet.Library.Model.Timesheet timesheet)
        {
            var result = TimesheetRepository.Save(timesheet);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result.ToString())
            };

            return response;
        }

        // POST api/timesheet/id&status=status
        public HttpResponseMessage Post(int id, int status)
        {
            var result = TimesheetRepository.ChangeStatus(id, (Library.Model.TimesheetStatus)status);

            if (result)
            {
                var timesheet = TimesheetRepository.Get(id) as Timesheet.Library.Model.Timesheet;

                var client = timesheet.Client;
                var vendor = timesheet.Vendor;
                var consultant = timesheet.Consultant;

                var token = new Token()
                {
                    UserId = client.id,
                    Guid = Guid.NewGuid()
                };

                UserRepository.StoreToken(token);

                var url = HttpUtility.HtmlEncode(String.Format("http://{0}/timesheets/#timesheet/{1}?token={2}", Request.RequestUri.Host, id, token.Guid.ToString()));

                switch ((Library.Model.TimesheetStatus)status)
                { 
                    case Library.Model.TimesheetStatus.Submitted:
                        // vendor
                        EmailRepository.Send(vendor.Email, "Timesheet Submitted", string.Format("Consultant <b>{0}</b> with client <b>{1}</b> has submitted timesheet.<br/><br/>Please use this link to open it.<br/>{2}", consultant.UserName, client.UserName, url));
                        // client
                        EmailRepository.Send(client.Email, "Timesheet Submitted", string.Format(@"Consultant <b>{0}</b> has submitted timesheet.<br/><br/>Please use this link to open it.<br/>{1}", consultant.UserName, url));
                        break;

                    case Library.Model.TimesheetStatus.Rejected:
                        // vendor
                        EmailRepository.Send(vendor.Email, "Timesheet Rejected", string.Format(@"Client <b>{0}</b> has rejected timesheet of consultant <b>{1}</b>.<br/><br/>Please use this link to open it.<br/>{2}", client.UserName, consultant.UserName, url));
                        // consultant
                        EmailRepository.Send(consultant.Email, "Timesheet Rejected", string.Format(@"Client <b>{0}</b> has rejected timesheet of consultant <b>{1}</b>.<br/><br/>Please use this link to open it.<br/>{2}", client.UserName, consultant.UserName, url));
                        break;

                    case Library.Model.TimesheetStatus.Approved:
                        // vendor
                        EmailRepository.Send(vendor.Email, "Timesheet Approved", string.Format(@"Client <b>{0}</b> has approved timesheet of consultant <b>{1}</b>.Please use this link to open it.<br/>{2}", client.UserName, consultant.UserName, url));
                        // consultant
                        EmailRepository.Send(consultant.Email, "Timesheet Approved", string.Format(@"Client <b>{0}</b> has approved timesheet of consultant <b>{1}</b>.Please use this link to open it.<br/>{2}", client.UserName, consultant.UserName, url));
                        break;

                    case Library.Model.TimesheetStatus.Processed:
                        // consultant
                        EmailRepository.Send(consultant.Email, "Timesheet Processed", string.Format("Vendor <b>{0}</b> has processed timesheet of consultant <b>{1}</b>.Please use this link to open it.<br/>{2}", vendor.UserName, consultant.UserName, url));
                        break;
                }
            }

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result.ToString())
            };

            return response;
        }

        // DELETE api/timesheet/5
        public HttpResponseMessage Delete(int id)
        {
            var result = TimesheetRepository.Delete(id);

            var response = new HttpResponseMessage()
            {
                Content = new StringContent(result ? 1.ToString() : 0.ToString())
            };

            return response;
        }

        [HttpGet]
        public HttpResponseMessage Get(int id, string type)
        {
            var timesheet = TimesheetRepository.Get(id) as Timesheet.Library.Model.Timesheet;

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Timesheet " + id;
            document.Info.Author = "Timesheets";

            // Create new page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont f_large = new XFont("Verdana", 14, XFontStyle.Bold);
            XFont f_norm = new XFont("Verdana", 10, XFontStyle.Regular);
            XFont fb_norm = new XFont("Verdana", 10, XFontStyle.Bold);

            XPen pen = new XPen(XColor.FromName("black"));

            var startX = 80;

            gfx.DrawString(timesheet.Vendor.UserName, f_large, XBrushes.Black, startX, 40);
            gfx.DrawString(timesheet.Vendor.Address, f_norm, XBrushes.Black, startX, 60);

            gfx.DrawString(string.Format("Consultant Name: {0}", timesheet.Consultant.UserName), f_norm, XBrushes.Black, startX, 100);
            gfx.DrawString(string.Format("Email: {0}", timesheet.Consultant.Email), f_norm, XBrushes.Black, startX, 120);
            gfx.DrawString(string.Format("Client Site: {0}", timesheet.Client.UserName), f_norm, XBrushes.Black, startX, 160);

            gfx.DrawString(string.Format("From: {0}", timesheet.StartDate.ToString("MM/dd/yyyy")), f_norm, XBrushes.Black, 400, 100);
            gfx.DrawString(string.Format("To:     {0}", timesheet.EndDate.ToString("MM/dd/yyyy")), f_norm, XBrushes.Black, 400, 120);

            gfx.DrawLine(pen, startX, 180, 520, 180);
            gfx.DrawLine(pen, 180, 180, 180, 200);
            gfx.DrawLine(pen, 180, 200, 180, 220);
            gfx.DrawLine(pen, 180, 220, 180, 240);

            decimal total = 0;
            var lineY = 200;

            gfx.DrawString("Date", fb_norm, XBrushes.Black, startX, lineY);
            gfx.DrawString("Hours", fb_norm, XBrushes.Black, 200, lineY);
            lineY += 20;

            gfx.DrawLine(pen, startX, lineY, 520, lineY);
            lineY += 20;

            foreach (var day in timesheet.Days)
            {
                gfx.DrawString(string.Format("{0}", day.Day.ToString("MM/dd/yyyy, ddd")), f_norm, XBrushes.Black, startX, lineY);
                gfx.DrawLine(pen, 180, lineY, 180, lineY + 20);
                gfx.DrawString(string.Format("{0}", Math.Round(day.Worked, 1)), f_norm, XBrushes.Black, 200, lineY);

                total += day.Worked;

                lineY += 20;
            }

            gfx.DrawLine(pen, startX, lineY, 520, lineY);
            gfx.DrawLine(pen, 180, lineY, 180, lineY + 20);

            lineY += 20;
            gfx.DrawString("Total: ", f_norm, XBrushes.Black, startX, lineY);
            gfx.DrawString(string.Format("{0}", Math.Round(total, 1)), fb_norm, XBrushes.Black, 200, lineY);
            gfx.DrawLine(pen, 180, lineY, 180, lineY + 20);

            lineY += 20;
            gfx.DrawLine(pen, startX, lineY, 520, lineY);

            lineY += 40;

            if (timesheet.TimesheetStatus == TimesheetStatus.Approved.ToString())
            {
                gfx.DrawString(string.Format("Approved By: {0}", timesheet.Client.Contact), f_norm, XBrushes.Black,
                    startX, lineY);
            }

            lineY += 40;
            gfx.DrawString(string.Format("Attn: {0}", timesheet.Vendor.Contact), f_norm, XBrushes.Black, startX, lineY);
            lineY += 20;
            gfx.DrawString(string.Format("Phone #: {0}", timesheet.Vendor.Phone), f_norm, XBrushes.Black, startX, lineY);
            lineY += 20;
            gfx.DrawString(string.Format("Fax #: {0}", timesheet.Vendor.Fax), f_norm, XBrushes.Black, startX, lineY);
            lineY += 20;
            gfx.DrawString(string.Format("Email: {0}", timesheet.Vendor.Email), f_norm, XBrushes.Black, startX, lineY);

            MemoryStream output = new MemoryStream();

            document.Save(output, false);

            var sr = new StreamReader(output);
            string sb = sr.ReadToEnd();

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StringContent(sb);

            //a text file is actually an octet-stream (pdf, etc)
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            //we used attachment to force download
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "timesheet_" + id + ".pdf";

            return result;
        }
    }
}
