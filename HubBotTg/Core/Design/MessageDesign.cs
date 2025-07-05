using HubBotTg.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace HubBotTg.Core.Design
{
    public class SendableMessage
    {
        public long ChatId { get; set; }
    }

    public class SendableMessageDesign
    {
        public string Text { get; set; }

        public InlineKeyboardMarkup Keyboard { get; set; }
    }

    public static class MessageDesign
    {
        /// <summary>
        /// Пртветствевнное сообщение посмле первого /start
        /// </summary>
        public static SendableMessageDesign GetWelcomeMessageWithRoleSelection()
        {

        }

        /// <summary>
        /// Сообщение с просмьбой выбрать группу для записи студенту
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetStudentGroupSelectionMessage(List<string> groups)
        {

        }

        /// <summary>
        /// Сообщение с меню студента (с кнопкой изменения группы)
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetStudentMenuMessage()
        {

        }

        /// <summary>
        /// Сообщение после запроса на админство (с кнопкой отменить запрос и выбрать группу)
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminRequestSentMessage()
        {

        }

        
        /// <summary>
        /// Сообщение пользователю, чей заппрос на аадминство был удовлетворен  + админское меню
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminApprovalMessage()
        {

        }

        /// <summary>
        /// Сообщение админского меню
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminMenuMessage()
        {

        }

        /// <summary>
        /// Сообщение с предлоджением составить пост для рассылки
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetComposeBroadcastMessage()
        {

        }

        /// <summary>
        /// Сообщение утверждения дизайна сообщения
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetBroadcastVerificationMessage()
        {

        }

        /// <summary>
        /// Сообщение с выбором групп для рассылки
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetBroadcastGroupSelectionMessage(List<string> groups, List<string> deportament)
        {

        }

        /// <summary>
        /// Сообщение успешной отправки рассылки
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetBroadcastCompletionMessage()
        {

        }

        /// <summary>
        /// Сооббщение для вывода списка запросов на админство
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminRequestListMessage(List<User> requests)
        {

        }


        /// <summary>
        /// Сообщение успешного назначения админа
        /// </summary>
        /// <returns></returns>
        public static SendableMessageDesign GetAdminRequestApprovedMessage()
        {

        }
    }
}