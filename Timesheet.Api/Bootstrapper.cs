using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using System.Web.Http;
using Timesheet.Library.Repository;
using Timesheet.Library.Repository.Mssql;
using Timesheet.Library.Repository.Email;

namespace Timesheet.Api
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            IUnityContainer container = BuildUnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            UnityContainer container = new UnityContainer();
            //container.AddNewExtension();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType();    
            RegisterTypes(container);

            return container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IVendorClientRepository, VendorClientRepository>();
            container.RegisterType<IVendorConsultantRepository, VendorConsultantRepository>();
            container.RegisterType<IClientConsultantRepository, ClientConsultantRepository>();
            container.RegisterType<ITimesheetRepository, TimesheetRepository>();
            container.RegisterType<IEmailRepository, EmailRepository>();
        }
    }
}