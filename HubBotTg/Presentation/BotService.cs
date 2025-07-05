using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubBotTg.Core.Models;


namespace HubBotTg.Presentation
{
    internal class BotService
    {
        public async Task HandleUpdateAsync()
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;

                if (callbackQuery.Data == Callback.StudentButton())
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                } else if (callbackQuery.Data == Callback.AdminButton())
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                } else if (callbackQuery.Data == Callback.NewPostButton())
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                } else if (callbackQuery.Data == Callback.AddAdminButton())
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                } else if (callbackQuery.Data == Callback.BackButton())
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                } else if (callbackQuery.Data == Callback.ApprovePostButton())
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                } else if (callbackQuery.Data == Callback.StudentGroup())
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                } else if (callbackQuery.Data == Callback.AdminGroup())
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                    
                }
            }

        }
    }
}
