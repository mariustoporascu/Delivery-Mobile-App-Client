﻿using LivroApp.Constants;
using LivroApp.Models.AuthModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LivroApp.ViewModels.UserVModels
{
    public class AskHelpPopUpVM
    {
        public HelpMe Help { get; set; }
        public AskHelpPopUpVM()
        {
            Help = new HelpMe();
            if (App.IsLoggedIn)
            {
                Help.Email = App.UserInfo.Email;
                Help.Name = App.UserInfo.FullName;
                Help.TelNo = App.UserInfo.PhoneNumber;
            }
        }
        public async Task<bool> SendMsg()
        {
            try
            {
                HttpClient client = new HttpClient();
                Uri uri = new Uri($"{ServerConstants.BaseUrl}/foodapp/askhelp");
                var json = JsonConvert.SerializeObject(Help);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = await client.PostAsync(uri, data);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var respInfo = await httpResponseMessage.Content.ReadAsStringAsync();
                    if (respInfo.Contains("Message sent!"))
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
