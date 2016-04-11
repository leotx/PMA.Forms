using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using ModernHttpClient;
using Newtonsoft.Json.Linq;
using PMA.Forms.Helpers;
using PMA.Forms.Models;
using PMA.Forms.Resources;

namespace PMA.Forms.Services
{
    public class AppointmentService
    {
        private string Token { get; }
        private HttpClient HttpClient { get; }
        public DateTime DateOfAppointment { private get; set; }

        public AppointmentService(string token)
        {
            Token = token;
            DateOfAppointment = DateTime.Now;
            HttpClient = new HttpClient(new NativeMessageHandler());
        }

        public string StartAppointment(TimeSpan startTime)
        {
            var dailyAppointment = new
            {
                token = Token,
                data = $"{DateOfAppointment:yyyy-MM-dd}",
                inicio = $"{startTime.RoundToNearest(5):HH:mm}",
                intervalo = "00:00",
                fim = "21:00"
            };

            return CreateDailyAppointment(dailyAppointment);
        }

        public string IntervalAppointment(TimeSpan intervalTime)
        {
            var appointment = FindDailyAppointment();

            var totalIntervalTime = intervalTime + appointment.IntervalTime;

            var dailyAppointment = new
            {
                token = Token,
                data = $"{DateOfAppointment:yyyy-MM-dd}",
                inicio = appointment.StartTime.ToString(),
                intervalo = $"{totalIntervalTime.RoundToNearest(5):HH:mm}",
                fim = "21:00"
            };

            return CreateDailyAppointment(dailyAppointment);
        }

        public string EndAppointment(TimeSpan endTime)
        {
            var appointment = FindDailyAppointment();

            var dailyAppointment = new
            {
                token = Token,
                data = $"{DateOfAppointment:yyyy-MM-dd}",
                inicio = appointment.StartTime.ToString(),
                intervalo = appointment.IntervalTime.ToString(),
                fim = $"{endTime.RoundToNearest(5):HH:mm}"
            };

            return CreateDailyAppointment(dailyAppointment);
        }

        private string CreateDailyAppointment(object dailyAppointment)
        {
            var jsonAppointment = JObject.FromObject(dailyAppointment).ToString();

            var response = HttpClient.PostAsync(Url.DailyAppointment,
                new StringContent(jsonAppointment, Encoding.UTF8, "application/json")).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        private DailyAppointment FindDailyAppointment()
        {
            var dailyAppointment = new
            {
                token = Token,
                dataInicial = $"{DateOfAppointment:yyyy-MM-dd}",
                dataFinal = $"{DateOfAppointment:yyyy-MM-dd}"
            };

            var jsonAppointment = JObject.FromObject(dailyAppointment).ToString();

            var response = HttpClient.PostAsync(Url.DailyAppointmentList,
                new StringContent(jsonAppointment, Encoding.UTF8, "application/json")).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;

            return PopulateDailyAppointment(responseString)?[0];
        }

        private static List<DailyAppointment> PopulateDailyAppointment(string response)
        {
            return XDocument.Parse(response).Descendants("apontamentoDiario").Select(DailyAppointment.CreateDailyAppointment).ToList();
        }
    }
}