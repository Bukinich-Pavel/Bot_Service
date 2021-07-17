using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace Bot
{
    partial class Service2 : ServiceBase
    {
        BotKsySend ksysend;
        public Service2()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Добавьте код для запуска службы.
            ksysend = new BotKsySend();
            Thread thread = new Thread(new ThreadStart(ksysend.Start));
            thread.Start();

        }

        protected override void OnStop()
        {
            // TODO: Добавьте код, выполняющий подготовку к остановке службы.
            ksysend.Stop();
            Thread.Sleep(1000);

        }
    }

    class BotKsySend
    {
        static List<string> love = new List<string> { };
        static bool bl = true;
        bool enabled = true;
        string token;
        WebClient wc;
        int update_id;
        string startUrl;

        public BotKsySend()
        {
            token = "1793664423:AAEht_3gBVX7H8hyFACErB1MjxjIpg6AzO4"; //тоекен MyLOveKseniya_Bot
            //string token = "1815629190:AAF7uqO106uo1DNsXxqNlY_XeMfKKZN03Og"; //токен(адрес) бота

            wc = new WebClient() { Encoding = Encoding.UTF8 };

            update_id = 0;


            startUrl = $@"https://api.telegram.org/bot{token}/"; // создает полный адрес бота

        }

        public void Start()
        {
            enabled = true;
            while (enabled)
            {

                string url = $"{startUrl}getUpdates?offset={update_id}"; //прочитать сообщение
                var r = wc.DownloadString(url); // скачать строку

                var msgs = JObject.Parse(r)["result"].ToArray(); // я хуй его знает что оно делает

                string userIdMy = "866008227"; // это адрес моего акк в телеге
                string userId = "840306162"; // это адрес ксюши акк в телеге


                DateTime dt = DateTime.Now.ToLocalTime(); // узнаем сколько сейчас времени

                Random random = new Random();


                if (dt.Hour == 7 && bl)
                {
                    #region считывает из файла и записывает в Love

                    string json = File.ReadAllText(@"D:\Unity\Bot\Bot\bin\Debug\LoveString.json");
                    love = JsonConvert.DeserializeObject<List<string>>(json);

                    #endregion

                    string message = love[random.Next(0, love.Count)];

                    url = $"{startUrl}sendMessage?chat_id={userId}&text={message}";
                    Console.WriteLine(wc.DownloadString(url));
                    bl = false;
                }
                if (dt.Hour >= 8 && dt.Hour <= 13)
                {
                    bl = true;
                }

                if (dt.Hour == 14 && bl)
                {
                    #region считывает из файла и записывает в Love

                    string json = File.ReadAllText(@"D:\Unity\Bot\Bot\bin\Debug\LoveString.json");
                    love = JsonConvert.DeserializeObject<List<string>>(json);

                    #endregion

                    string message = love[random.Next(0, love.Count)];

                    url = $"{startUrl}sendMessage?chat_id={userId}&text={message}";
                    Console.WriteLine(wc.DownloadString(url));
                    bl = false;
                }
                if (dt.Hour >= 15 && dt.Hour <= 20)
                {
                    bl = true;
                }

                if (dt.Hour == 21 && bl)
                {
                    #region считывает из файла и записывает в Love

                    string json = File.ReadAllText(@"D:\Unity\Bot\Bot\bin\Debug\LoveString.json");
                    love = JsonConvert.DeserializeObject<List<string>>(json);

                    #endregion

                    string message = love[random.Next(0, love.Count)];

                    url = $"{startUrl}sendMessage?chat_id={userId}&text={message}";
                    wc.DownloadString(url);

                    //url = $"{startUrl}sendMessage?chat_id={userIdMy}&text={message}";
                    //wc.DownloadString(url);

                    bl = false;
                }
                if (dt.Hour >= 23 && dt.Hour >= 0 && dt.Hour >= 6)
                {
                    bl = true;
                }

                Thread.Sleep(1800000);

            }

        }

        public void Stop()
        {
            enabled = false;

        }

    }
}
