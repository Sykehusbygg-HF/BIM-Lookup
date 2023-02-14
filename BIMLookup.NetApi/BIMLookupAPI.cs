using BIMLookup.NetApi.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BIMLookup.NetApi
{
    public class BIMLookupAPI
    {
        static string _uri = "https://bimlookup.azurewebsites.net/api/";
        static string _username;
        static string _password;
        static string _token;
        /// <summary>
        /// Token Valid for 2hours
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public BIMLookupAPI(string uri, string username, string password)
        {
            _uri = uri;
            _username = username;
            _password = password;
            _token = AuthenticateGetToken(_username, _password);
        }
        public BIMLookupAPI(string uri)
        {
            _uri = uri;
        }
        public BIMLookupAPI()
            {

        }
        public string Uri { get { return _uri; } set { _uri = value; } }
        public string UserName { get { return _username; } set { _username = value; } }
        public string Password { get { return _password; } set { _password = value; } }
        public string Token { get { return _token; } set { _token = value; } }

        public string GetCurrentToken()
        {
            return _token;
        }
        public long GetTokenExpirationTime()
        {
            if (string.IsNullOrEmpty(_token))
                return -1;
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(_token);
            string tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            long ticks = long.Parse(tokenExp);
            return ticks;

        }
        public bool CheckTokenIsValid()
        {
            var tokenTicks = GetTokenExpirationTime();
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }
        public bool ConnectionOK()
        {
            if(!CheckTokenIsValid())
            {
                _token = AuthenticateGetToken(_username, _password);
            }
            return !string.IsNullOrEmpty(_token);
        }
        public bool ConnectionNoAuthOK()
        {
            if (this.GetConnectionOKNoAuth())
                return true;
            return false;
        }
        public async Task<string> AuthenticateGetTokenAsync(string username, string password)
        {
            RestClientOptions opt = new RestClientOptions(_uri) { Encoding = Encoding.UTF8 };
            RestClient client = new RestClient(opt);
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.Resource = $"Authentication/Authenticate";
            string body = $"{{ \"userName\": \"{_username}\", \"password\": \"{_password}\" }}";
            request.AddBody(body);


            RestResponse response = await client.ExecuteAsync(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return response?.Content?.Substring(1, response.Content.Length - 2);

            }
            return string.Empty;
        }
        public string AuthenticateGetToken(string username, string password)
        {
            RestClientOptions opt = new RestClientOptions(_uri) { Encoding = Encoding.UTF8 };
            RestClient client = new RestClient(opt);
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.Resource = $"Authentication/Authenticate";
            string body = $"{{ \"userName\": \"{_username}\", \"password\": \"{_password}\" }}";
            request.AddBody(body);


            RestResponse response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return response?.Content?.Substring(1, response.Content.Length - 2);

            }
            return string.Empty;
        }
        internal async Task<string> GetInternalAsync(string resource, object body = null, bool anonymously = false)
        {
            if (!anonymously)
            {
                if (!ConnectionOK())
                    return null;
            }
            RestClientOptions opt = new RestClientOptions(_uri) { Encoding = Encoding.UTF8 };
            RestClient client = new RestClient(opt);
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.Resource = $"{resource}";
            string auth = $"Bearer {_token}";
            request.AddHeader("Authorization", auth);
            //request.AddHeader("Content-Type", "application/json");

            if (body != null)
                request.AddBody(body);

            RestResponse response = await client.ExecuteAsync(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content;

            }
            return string.Empty;
        }
        internal string GetInternal(string resource, object body = null, bool anonymously = false)
        {
            if (!anonymously)
            {
                if (!ConnectionOK())
                    return null;
            }
            RestClientOptions opt = new RestClientOptions(_uri) { Encoding = Encoding.UTF8 };
            RestClient client = new RestClient(opt);
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.RequestFormat = RestSharp.DataFormat.Json;


            request.Resource = $"{resource}";
            string auth = $"Bearer {_token}";
            request.AddHeader("Authorization", auth);
            //request.AddHeader("Content-Type", "application/json");

            if (body != null)
                request.AddBody(body);

            RestResponse response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content;

            }
            return string.Empty;
        }
        internal async Task<string> GetAllInternalAsync(string resource, object body=null)
        {
            if (!ConnectionOK())
                return null;
            RestClientOptions opt = new RestClientOptions(_uri) { Encoding = Encoding.UTF8 };
            RestClient client = new RestClient(opt);
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.Resource = $"odata/{resource}";
            string auth = $"Bearer {_token}";
            request.AddHeader("Authorization", auth);
            if(body != null)
                request.AddBody(body);

            RestResponse response = await client.ExecuteAsync(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return response.Content;

            }
            return string.Empty;
        }
        internal string GetAllInternal(string resource, object body = null)
        {
            if (!ConnectionOK())
                return null;
            RestClientOptions opt = new RestClientOptions(_uri) { Encoding = Encoding.UTF8 };
            RestClient client = new RestClient(opt);
            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.Resource = $"odata/{resource}";
            string auth = $"Bearer {_token}";
            request.AddHeader("Authorization", auth);
            if (body != null)
                request.AddBody(body);

            RestResponse response = client.Execute(request);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return response.Content;

            }
            return string.Empty;
        }
        internal async Task<string> GetAllProjectsInternalAsync()
        {
            return await GetAllInternalAsync("Project");
        }
        internal string GetAllProjectsInternal()
        {
            return GetAllInternal("Project");
        }
        internal string GetAllDisciplinesInternal()
        {
            return GetAllInternal("Discipline");
            
        }
        internal async Task<string> GetAllDisciplinesInternalAsync()
        {
            return await GetAllInternalAsync("Discipline");

        }
        internal async Task<string> GetAllProjectsNoAuthInternalAsync()
        {
            return await GetInternalAsync("NoAuthProject",null, true);
        }
        internal string GetAllProjectsNoAuthInternal()
        {
            return GetInternal("NoAuthProject", null, true);
        }
        internal string GetConnectionOKNoAuthInternal()
        {
            return GetInternal("NoAuthConnectionOK", null, true);
        }
        internal string GetAllDisciplinesNoAuthInternal()
        {
            return GetInternal("NoAuthDiscipline", null, true);
        }
        internal async Task<string> GetAllDisciplinesNoAuthInternalAsync()
        {
            return await GetInternalAsync("NoAuthDiscipline", null, true);

        }
        internal async Task<string> GetPropertyInstancesInternalAsync(string ProjectCode, string Phase, string DisciplineCode, object body, bool anonymously = false)
        {
            return await GetInternalAsync($"PropertyInstanceCustom/ByCodes?ProjectCode={ProjectCode}&Phase={Phase}&DisciplineCode={DisciplineCode}", body, anonymously);
        }
        internal string GetPropertyInstancesInternal(string ProjectCode, string Phase, string DisciplineCode, object body, bool anonymously = false)
        {
            return GetInternal($"PropertyInstanceCustom/ByCodes?ProjectCode={ProjectCode}&Phase={Phase}&DisciplineCode={DisciplineCode}", body, anonymously);
        }
        internal async Task<string> GetMasterkravProjectViewInternalAsync(string ProjectCode, string Phase, string DisciplineCode, object body, bool anonymously = false)
        {
            return await GetInternalAsync($"MasterkravProjectViewCustom/ByCodes?ProjectCode={ProjectCode}&Phase={Phase}&DisciplineCode={DisciplineCode}",body, anonymously);
        }
        internal string GetMasterkravProjectViewInternal(string ProjectCode, string Phase, string DisciplineCode, object body, bool anonymously = false)
        {
            return GetInternal($"MasterkravProjectViewCustom/ByCodes?ProjectCode={ProjectCode}&Phase={Phase}&DisciplineCode={DisciplineCode}",body, anonymously);
        }
        public List<Project> GetAllProjects()
        {
            string json = GetAllProjectsInternal();
            return ProjectResponse.FromJson(json);
        }
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            string json = await GetAllProjectsInternalAsync();
            return ProjectResponse.FromJson(json);
        }
        public List<Discipline> GetAllDisciplines()
        {
            string json = GetAllDisciplinesInternal();
            return DisciplineResponse.FromJson(json);
        }
        public async Task<List<Discipline>> GetAllDisciplinesAsync()
        {
            string json = await GetAllDisciplinesInternalAsync();
            return DisciplineResponse.FromJson(json);
        }

        public bool GetConnectionOKNoAuth()
        {
            string json = GetConnectionOKNoAuthInternal();
            if (json.ToLower().Contains("true"))
                return true;
            return false;
        }
        public List<Project> GetAllProjectsNoAuth()
        {
            string json = GetAllProjectsNoAuthInternal();
            return Project.FromJson(json);
        }
        public async Task<List<Project>> GetAllProjectsNoAuthAsync()
        {
            string json = await GetAllProjectsNoAuthInternalAsync();
            return Project.FromJson(json);
        }
        public List<Discipline> GetAllDisciplinesNoAuth()
        {
            string json = GetAllDisciplinesNoAuthInternal();
            return Discipline.FromJson(json);
        }
        public async Task<List<Discipline>> GetAllDisciplinesNoAuthAsync()
        {
            string json = await GetAllDisciplinesNoAuthInternalAsync();
            return Discipline.FromJson(json);
        }
        public async Task<List<PropertyInstance>> GetProprtyInstancesAsync(string ProjectCode, string Phase, string DisciplineCode, bool anonymously = false)
        {
            string json = await GetPropertyInstancesInternalAsync(ProjectCode, Phase, DisciplineCode, anonymously);
            return PropertyInstanceResponse.FromJson2(json);
        }
        public List<PropertyInstance> GetPropertyInstances(string ProjectCode, string Phase, string DisciplineCode, bool anonymously = false)
        {
            string json = GetPropertyInstancesInternal(ProjectCode, Phase, DisciplineCode, anonymously);
            return PropertyInstanceResponse.FromJson2(json);
        }
        public async Task<List<MasterkravProjectView>> GetMasterkravProjectViewAsync(string ProjectCode, string Phase, string DisciplineCode, bool anonymously = false)
        {
            string json = await GetMasterkravProjectViewInternalAsync(ProjectCode, Phase, DisciplineCode, null , anonymously);
            return MasterkravProjectView.FromJson(json);
        }
        public List<MasterkravProjectView> GetMasterkravProjectView(string ProjectCode, string Phase, string DisciplineCode, bool anonymously = false)
        {
            string json = GetMasterkravProjectViewInternal(ProjectCode, Phase, DisciplineCode, null , anonymously);
            return MasterkravProjectView.FromJson(json);
        }
        
    }
}
