using System.Text.RegularExpressions;
using HubBotTg.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types.ReplyMarkups;

namespace HubBotTg.Core.Design
{
    public class SendableMessage 
    {
        public long ChatId { get; set; }
    }

    public class SendableMessageDesign
    {
        public required string Text { get; set; }

        public InlineKeyboardMarkup? Keyboard { get; set; }
    }

    public class User
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }   

        public long TelegramId { get; set; }
    }

    public static class MessageDesign
    {
        /// <summary>
        /// Пртветствевнное сообщение посмле первого /start
        /// </summary>
        public static SendableMessageDesign GetWelcomeMessageWithRoleSelection()
            => new SendableMessageDesign()
                   {
                       Text =  "Привветствую в ItHub Notification Bot!\nКто ты?",
                       Keyboard = new InlineKeyboardMarkup()
                           .AddNewRow().AddButton("Я - студент","i-student")
                           .AddNewRow().AddButton("Я - преподаватель","i-teacher")
                   };

        /// <summary>
        /// Сообщение с просмьбой выбрать группу для записи студенту
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetStudentGroupSelectionMessage(List<string> groups)
        {
            var design = new SendableMessageDesign()
            {
                Text = "Выбери свою группу: ",
                Keyboard = new InlineKeyboardMarkup()
            };

            var rows = groups.Select((value, index) => (value, index))
                             .GroupBy(x => x.index / 3)
                             .Select(g => g.Select(x => x.value));

            foreach (var row in rows)
            {
                design.Keyboard.AddNewRow();
                foreach (var col in row)
                    design.Keyboard.AddButton(col, $"student-group-select_{col}");
            }

            design.Keyboard.AddNewRow().AddButton("Я ошибся Т_Т - я преподаватель", "back_button");

            return design;
        }

        /// <summary>
        /// Сообщение с меню студента (с кнопкой изменения группы)
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetStudentMenuMessage(string studentGroupName)
            => new SendableMessageDesign()
            {
                Text = $"Отлично! Теперь тебе будут приходить сообщения для группы {studentGroupName}.",
                Keyboard = new InlineKeyboardMarkup()
                           .AddNewRow().AddButton("Перевыбрать группу", "i-student")
            };

        /// <summary>
        /// Сообщение после запроса на админство (с кнопкой отменить запрос и выбрать группу)
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminRequestSentMessage()
            => new SendableMessageDesign()
            {
                Text = $"Хорошо. Ожидайте подтверждения.",
                Keyboard = new InlineKeyboardMarkup()
                           .AddNewRow().AddButton("Я ошибся Т_Т - я студент", "back_button")
            };

        /// <summary>
        /// Сообщение пользователю, чей запрос на админство был удовлетворен  + админское меню
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminApprovalMessage()
            => new SendableMessageDesign()
            {
                Text = $"Ого! Да ты и в прямь преподаватель!\nЛови свою меню:",
                Keyboard = new InlineKeyboardMarkup()
                           .AddNewRow().AddButton("Список запросов на преподавателя", "admin-request")
                           .AddNewRow().AddButton("Создать рассылку", "create-broadcast")
            };


        /// <summary>
        /// Сообщение админского меню
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminMenuMessage()
            => new SendableMessageDesign()
            {
                Text = $"Преподавательское меню:",
                Keyboard = new InlineKeyboardMarkup()
                           .AddNewRow().AddButton("Список запросов на преподавателя", "admin-request")
                           .AddNewRow().AddButton("Создать рассылку", "create-broadcast")
            };

        /// <summary>
        /// Сообщение с предлоджением составить пост для рассылки
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetComposeBroadcastMessage()
            => new SendableMessageDesign()
            {
                Text = $"Составь сообщение для рассылки.",
                Keyboard = new InlineKeyboardMarkup().AddNewRow().AddButton("Назад", "back_button")
            };

        /// <summary>
        /// Сообщение утверждения дизайна сообщения
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetBroadcastVerificationMessage()
            => new SendableMessageDesign()
            {
                Text = $"Проверь, все ли окей.",
                Keyboard = new InlineKeyboardMarkup().AddNewRow().AddButton("Назад", "back_button")
            };

        /// <summary>
        /// Сообщение с выбором групп для рассылки
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetBroadcastGroupSelectionMessage(List<string> groups, List<string> deportaments)
        {
            var design = new SendableMessageDesign()
            {
                Text = "Выбери кафедру или группу получателейй. Если очень хочется, можно выбрать всех: ",
                Keyboard = new InlineKeyboardMarkup().AddNewRow()
            };

            foreach (var deportament in deportaments)
            {
                    design.Keyboard.AddButton(deportament, $"broadcast-deportament-select_{deportament}");
            }

            var rows = groups.Select((value, index) => (value, index))
                             .GroupBy(x => x.index / 3)
                             .Select(g => g.Select(x => x.value));

            foreach (var row in rows)
            {
                design.Keyboard.AddNewRow();
                foreach (var col in row)
                    design.Keyboard.AddButton(col, $"broadcast-group-select_{col}");
            }

            design.Keyboard.AddNewRow().AddButton("Разослать всем", "broadcast-group-select-all")
                           .AddNewRow().AddButton("Назад", "back_button");

            return design;
        }

        /// <summary>
        /// Сообщение успешной отправки рассылки
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetBroadcastCompletionMessage()
            => new SendableMessageDesign()
            {
                Text = $"Рассылка проведена успешно!:",
                Keyboard = new InlineKeyboardMarkup()
                           .AddNewRow().AddButton("Список запросов на преподавателя", "admin-request")
                           .AddNewRow().AddButton("Создать рассылку", "create-broadcast")
            };

        /// <summary>
        /// Сооббщение для вывода списка запросов на админство
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminRequestListMessage(List<User> requests)
        {
            var design = new SendableMessageDesign()
            {
                Text = "Эти граждане утверждают, что они преподаватели. Выбери тех, кто таковыми являются: ",
                Keyboard = new InlineKeyboardMarkup().AddNewRow()
            };

            foreach (var request in requests)
            {
                design.Keyboard.AddNewRow().AddButton($"{request.UserId}", $"admin-request-approved_{request.Id}");
            }


            design.Keyboard.AddNewRow().AddButton("Назад", "back_button");

            return design;
        }


        /// <summary>
        /// Сообщение успешного назначения админа
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminRequestApprovedMessage(User approvedUser)
            => new SendableMessageDesign()
            {
                Text = $"Пользователь {approvedUser.UserId} назначен преподавателем!",
                Keyboard = new InlineKeyboardMarkup()
                           .AddNewRow().AddButton("Список запросов на преподавателя", "admin-request")
                           .AddNewRow().AddButton("Создать рассылку", "create-broadcast")
            };
    }
}
