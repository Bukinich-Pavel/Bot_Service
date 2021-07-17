using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace Bot
{
    public partial class Service1 : ServiceBase
    {

        TelegrammBotKsy botKsy;

        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            botKsy = new TelegrammBotKsy();
            Thread thread = new Thread(new ThreadStart(botKsy.Start));
            thread.Start();
        }

        protected override void OnStop()
        {
            botKsy.Stop();
            Thread.Sleep(1000);
        }
    }

    class TelegrammBotKsy
    {
        static ITelegramBotClient bot;
        static List<string> love = new List<string> { };
        bool enabled = true;
        string patch = @"D:\Unity\Bot\Bot\bin\Debug\LoveString.json";
        public TelegrammBotKsy()
        {
            #region считывает из файла и записывает в Love
            //if (!File.Exists("LoveString.json"))
            //{
            //    return;
            //}

            string json = File.ReadAllText(patch);
            love = JsonConvert.DeserializeObject<List<string>>(json);

            #endregion


            bot = new TelegramBotClient("1815629190:AAF7uqO106uo1DNsXxqNlY_XeMfKKZN03Og"); // BoolaBot

            bot.OnMessage += AddStringLove;



        }

        public void Start()
        {
            bot.StartReceiving();

            while (enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            bot.StopReceiving();
            enabled = false;
        }
        
        // обработка события
        public async static void AddStringLove(object sender, MessageEventArgs e)
        {
            // выводит список признаний
            #region выводит список признаний
            Console.WriteLine(e.Message.Chat.Id);
            if (e.Message.Text == "/список")
            {
                for (int i = 0; i < love.Count; i++)
                {
                    await bot.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: $"{i}. {love[i]}");
                }

                foreach (var item in love)
                {
                    Console.WriteLine(item);
                }

                return;
            }
            #endregion

            // Удалить
            #region Удаляет выбранное по индексу пожелание
            string str = e.Message.Text;
            string number;
            if (str.Contains("/удалить"))
            {
                number = str.Substring(8);
                number = number.Trim();
                int num;
                try
                {
                    num = Convert.ToInt32(number);
                    love.RemoveAt(num);
                    string json = JsonConvert.SerializeObject(love);
                    File.WriteAllText("LoveString.json", json);

                }
                catch (Exception)
                {
                    await bot.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: "Ошибка удаления!");
                    return;
                }

                return;
            }
            #endregion

            // Принимает сообщение только от меня и сохраняет в список признаний
            #region Добавление признания
            if (e.Message.Text != null)
            {
                love.Add(e.Message.Text);
            }

            if (e.Message.Chat.Id.ToString() == "866008227")
            {
                string json = JsonConvert.SerializeObject(love);
                File.WriteAllText("LoveString.json", json);

                await bot.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: $"Добавлено: {e.Message.Text}");
                Console.WriteLine();
                Console.WriteLine($"Добавлено: {e.Message.Text}\n");

            }
            #endregion

        }

    }



}
