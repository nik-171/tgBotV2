using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubBotTg.Presentation
{
    internal class BotService
    {
        public async Task HandleUpdateAsync()
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;

                if (callbackQuery.Data == "student_button_clicked")
                {
                    await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                }
            }

        }
    }
}
