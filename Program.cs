using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WeatherApp
{
    public class Program
    {
        static TelegramBotClient bot = new TelegramBotClient("token");
        
        private static string run = " ";
        private static Weather weather = null;

        static async Task Main(string[] args)
        {
            var receiveOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage
                }
            };

            bot.StartReceiving(UpdateHandler, ErrorHandler, new ReceiverOptions());

            Console.ReadLine();
        }

        private static Task ErrorHandler(ITelegramBotClient bot, Exception ex, CancellationToken token)
        {
            //throw new NotImplementedException(ex.Message);
            return null;
        }


        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken token)
        {
            if (update.Type == UpdateType.Message)
            {
                try
                {
                    long _id = update.Message.Chat.Id;
                    Console.WriteLine($"Id: {_id} " +
                                      $"| Name: {update.Message.Chat.FirstName} {update.Message.Chat.LastName} " +
                                      $"| Text: {update.Message.Text}");

                    if (update.Message.Type == MessageType.Text && update.Message.Text.ToLower() == "/start")
                    {
                        run = "City";

                        ReplyKeyboardMarkup replyKeyboard = new(
                                        new[]
                                        {
                                            new KeyboardButton("Kyiv"),
                                            new KeyboardButton("Wroclaw"),
                                            new KeyboardButton("London"),
                                            new KeyboardButton("Paris")
                                        })
                        { ResizeKeyboard = true };

                        await bot.SendTextMessageAsync(_id, $"Привет, {update.Message.Chat.FirstName}" +
                                                             "\nНапиши мне город для прогноза погоды." +
                                                             "\nПожалуйста, на английском, тошо я Дурбецало", replyMarkup: replyKeyboard);
                    }

                    else if (update.Message.Type == MessageType.Text && run == "City")
                    {
                        run = "Days";
                        Weather.City = update.Message.Text;

                        ReplyKeyboardMarkup replyKeyboard = new(
                                        new[]
                                        {
                                            new KeyboardButton("1"),
                                            new KeyboardButton("3"),
                                            new KeyboardButton("7"),
                                            new KeyboardButton("14")
                                        })
                        { ResizeKeyboard = true };

                        await bot.SendTextMessageAsync(_id, "Напиши на сколько дней прогноз.", replyMarkup: replyKeyboard);
                    }

                    else if (update.Message.Type == MessageType.Text && run == "Days")
                    {
                        weather = new Weather();
                        run = " ";

                        await bot.SendTextMessageAsync(_id, await weather.GetStringWeatherToBot(int.Parse(update.Message.Text)));

                        await Task.Delay(500);

                        await bot.SendTextMessageAsync(update.Message.Chat.Id, "Спасибо, что дал мне работёнку:)", replyMarkup: new ReplyKeyboardRemove());
                    }
                    
                    else
                    {
                        ReplyKeyboardMarkup replyKeyboard = new(
                                        new[]
                                        {
                                        new KeyboardButton("/start")
                                        })
                        { ResizeKeyboard = true };

                        await bot.SendTextMessageAsync(update.Message.Chat.Id, "Отправь мне /start или нажми на кнопку ниже", replyMarkup: replyKeyboard);
                    }
                }

                catch (Exception)
                {
                    run = " ";
                    await bot.SendTextMessageAsync(update.Message.Chat.Id, "Шото не так, попробуй ещё раз!", replyMarkup: new ReplyKeyboardRemove());
                }
            }
        }
    }
}