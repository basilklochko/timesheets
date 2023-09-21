
namespace Timesheet.Library.Repository
{
    public interface IEmailRepository
    {
        bool Send(string to, string subject, string body);
    }
}
