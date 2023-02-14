using BIMLookup.NetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BimLookup.NetTest
{
    internal class Program
    {
        private static string _uri = "https://localhost:44318/api/";//"https://bimlookup.azurewebsites.net/api/";//"https://localhost:44318/api/";//"https://bimlookup.azurewebsites.net/api/";

        private static string _username=$"APIServiceUser";
        private static string _password= $"hA7T0sZluP6d56Sh1IW5";
        static void Main(string[] args)
        {
            //BIMLookupAPI api = new BIMLookupAPI(_uri, _username, _password);
            //string token = api.GetCurrentToken();  
            //bool connected = api.ConnectionOK();

            //var test = api.GetAllProjectsAsync().GetAwaiter().GetResult();
            //var test2 = api.GetAllDisciplinesAsync().GetAwaiter().GetResult();
            //var test3 = api.GetProprtyInstancesAsync("SNR", "Skisseprosjekt", "ARK", false).GetAwaiter().GetResult();


            BIMLookupAPI api2 = new BIMLookupAPI(_uri);
            bool ok = api2.ConnectionNoAuthOK();
            var test5 = api2.GetAllDisciplinesNoAuth();
            var test6 = api2.GetAllProjectsNoAuth();
           
            var test4 = api2.GetMasterkravProjectViewAsync("SNR", "Skisseprosjekt", "ARK", true).GetAwaiter().GetResult();

        }
    }
}
