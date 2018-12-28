using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

using RubyClient.Models;

namespace RubyClient.Controller
{

    public class RAController
    {
        private const string NAME = "token";
        private const string DOMAIN = @"https://localhost:44353/";

        private static RAController _instance;
        private static object syncRoot = new Object();

        private RAController() { }

        public static RAController GetInstance()
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                        _instance = new RAController();
                }
            }
            return _instance;
        }

        public async Task<List<NoteModel>> GetNotesAsync()
        {
            string content = string.Empty;

            Uri baseAddress = new Uri(DOMAIN);
            CookieContainer cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    cookieContainer.Add(baseAddress, new Cookie(NAME, App.UserToken));

                    var answer = await client.GetAsync(DOMAIN + "Notes");

                    answer.EnsureSuccessStatusCode();

                    if (answer.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent con = answer.Content;
                        content = await con.ReadAsStringAsync();
                    }
                }
            }

            List<NoteModel> result = null;

            if (content != "")
                result = JsonConvert.DeserializeObject<List<NoteModel>>(content);

            return await Task.FromResult(result);
        }

        public async Task<bool> AddNewNoteAsync(NoteModel model)
        {
            bool answer = false;
            string content = string.Empty;
            string json = JsonConvert.SerializeObject(model);

            Uri baseAddress = new Uri(DOMAIN);
            CookieContainer cookieContainer = new CookieContainer();
            HttpRequestMessage request = new HttpRequestMessage();
            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    cookieContainer.Add(baseAddress, new Cookie(NAME, App.UserToken));

                    var result = await client.PostAsync(DOMAIN + "Add", httpContent);

                    result.EnsureSuccessStatusCode();

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent con = result.Content;
                        content = await con.ReadAsStringAsync();
                        answer = true;
                    }
                }
            }

            return await Task.FromResult(answer);
        }

        public async Task<bool> UpdateDataNoteAsync(NoteModel model)
        {
            string content = string.Empty;
            string json = JsonConvert.SerializeObject(model);

            Uri baseAddress = new Uri(DOMAIN);
            CookieContainer cookieContainer = new CookieContainer();
            HttpRequestMessage request = new HttpRequestMessage();
            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    cookieContainer.Add(baseAddress, new Cookie(NAME, App.UserToken));

                    var result = await client.PutAsync(DOMAIN + "Put", httpContent);

                    result.EnsureSuccessStatusCode();

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent con = result.Content;
                        content = await con.ReadAsStringAsync();
                    }
                }
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> CheckConnective()
        {
            string content = string.Empty;

            Uri baseAddress = new Uri(DOMAIN);
            CookieContainer cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    cookieContainer.Add(baseAddress, new Cookie(NAME, App.UserToken));

                    var answer = await client.GetAsync(DOMAIN + "Auth");

                    answer.EnsureSuccessStatusCode();

                    if (answer.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent con = answer.Content;
                        content = await con.ReadAsStringAsync();
                    }
                }
            }

            return content.Equals("true") ? true : false;
        }
    }
}
