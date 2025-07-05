using HubBotTg.Core.Design;
using HubBotTg.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HubBotTg.Presentation
{
    /// <summary>
    /// Сервис для обработки команд бота и оркестрации бизнес-логики.
    /// Не отправляет сообщения напрямую, а только возвращает данные для последующей обработки.
    /// </summary>
    public class CommandService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        /// <summary>
        /// Конструктор с внедрением зависимостей
        /// </summary>
        /// <param name="userRepository">Репозиторий для работы с пользователями</param>
        /// <param name="groupRepository">Репозиторий для работы с группами</param>
        public CommandService(IUserRepository userRepository, IGroupRepository groupRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        /// <summary>
        /// Обработка команды /start или начального состояния пользователя
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <returns>Данные для отображения соответствующего меню</returns>
        public async Task<UserStateData> HandleStartCommand(long userId)
        {
            // Получаем информацию о пользователе из базы данных
            var user = await _userRepository.GetByIdAsync(userId);
            
            // Если пользователь новый (нет в базе)
            if (user == null)
            {
                return new UserStateData(
                    State.StateStart,
                    null,
                    MessageDesign.GetWelcomeMessageWithRoleSelection() // Показываем меню выбора роли
                );
            }
            
            // Если пользователь является администратором
            if (user.IsAdmin)
            {
                return new UserStateData(
                    State.StateStart,
                    null,
                    MessageDesign.GetAdminMenuMessage() // Показываем админ-меню
                );
            }
            
            // Если пользователь уже выбрал группу (обычный студент)
            if (user.Group != null)
            {
                return new UserStateData(
                    State.StateStart,
                    null,
                    MessageDesign.GetStudentMenuMessage(user.Group.Name) // Показываем меню студента
                );
            }

            // Во всех остальных случаях показываем меню выбора роли
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetWelcomeMessageWithRoleSelection()
            );
        }

        /// <summary>
        /// Обработка выбора роли "Студент"
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <returns>Данные для отображения списка групп</returns>
        public async Task<UserStateData> HandleStudentRoleSelection(long userId)
        {
            // Получаем все группы из базы данных
            var groups = await _groupRepository.GetAllGroupsAsync();
            var groupNames = groups.Select(g => g.Name).ToList();
            
            // Возвращаем сообщение с выбором группы
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetStudentGroupSelectionMessage(groupNames)
            );
        }

        /// <summary>
        /// Обработка выбора роли "Преподаватель"
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <returns>Данные для отображения соответствующего сообщения</returns>
        public async Task<UserStateData> HandleTeacherRoleSelection(long userId)
        {
            // Получаем информацию о пользователе
            var user = await _userRepository.GetByIdAsync(userId);
            
            // Если пользователь новый
            if (user == null)
            {
                // Создаем нового пользователя с запросом на админство
                user = new User { Id = userId, AdminRoleRequest = true };
                await _userRepository.UpsertUserAsync(user, null);
            }
            else if (!user.IsAdmin)
            {
                // Если пользователь уже существует, но не админ - обновляем запрос
                user.AdminRoleRequest = true;
                await _userRepository.UpdateUserAsync(user);
            }

            // Если пользователь уже является админом
            if (user.IsAdmin)
            {
                return new UserStateData(
                    State.StateStart,
                    null,
                    MessageDesign.GetAdminMenuMessage() // Показываем админ-меню
                );
            }
            
            // Если запрос на админство отправлен
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetAdminRequestSentMessage() // Сообщение об отправке запроса
            );
        }

        /// <summary>
        /// Обработка выбора группы студентом
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <param name="groupName">Название выбранной группы</param>
        /// <returns>Данные для отображения меню студента</returns>
        public async Task<UserStateData> HandleStudentGroupSelection(long userId, string groupName)
        {
            // Получаем информацию о пользователе
            var user = await _userRepository.GetByIdAsync(userId);
            
            // Если пользователь новый (маловероятно, но на всякий случай)
            if (user == null)
            {
                user = new User { Id = userId };
            }
            
            // Обновляем информацию о пользователе (добавляем/изменяем группу)
            await _userRepository.UpsertUserAsync(user, groupName);
            
            // Возвращаем меню студента с подтверждением выбора группы
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetStudentMenuMessage(groupName)
            );
        }

        /// <summary>
        /// Обработка отмены запроса на админство
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <returns>Данные для отображения начального меню</returns>
        public async Task<UserStateData> HandleCancelAdminRequest(long userId)
        {
            // Получаем информацию о пользователе
            var user = await _userRepository.GetByIdAsync(userId);
            
            // Если пользователь существует
            if (user != null)
            {
                // Снимаем запрос на админство
                user.AdminRoleRequest = false;
                await _userRepository.UpdateUserAsync(user);
            }
            
            // Возвращаем пользователя к начальному меню выбора роли
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetWelcomeMessageWithRoleSelection()
            );
        }

        /// <summary>
        /// Обработка запроса на отображение админ-меню
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <returns>Данные для отображения админ-меню</returns>
        public async Task<UserStateData> HandleAdminMenu(long userId)
        {
            // Просто возвращаем стандартное админ-меню
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetAdminMenuMessage()
            );
        }

        /// <summary>
        /// Обработка запроса на создание рассылки
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <returns>Данные для отображения сообщения о создании поста</returns>
        public async Task<UserStateData> HandleCreateBroadcast(long userId)
        {
            // Переводим пользователя в состояние создания поста
            return new UserStateData(
                State.StatePostCreating,
                null,
                MessageDesign.GetComposeBroadcastMessage()
            );
        }

        /// <summary>
        /// Обработка созданного поста (после того как пользователь отправил контент)
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <param name="postContent">Содержимое поста (текст, медиа и т.д.)</param>
        /// <returns>Данные для отображения сообщения подтверждения</returns>
        public async Task<UserStateData> HandlePostCreated(long userId, object postContent)
        {
            // Переводим пользователя в состояние подтверждения поста
            return new UserStateData(
                State.StatePostConfirming,
                postContent, // Сохраняем содержимое поста для последующего использования
                MessageDesign.GetBroadcastVerificationMessage()
            );
        }

        /// <summary>
        /// Обработка подтверждения поста (после того как пользователь проверил содержимое)
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <param name="postContent">Содержимое поста (текст, медиа и т.д.)</param>
        /// <returns>Данные для отображения выбора групп рассылки</returns>
        public async Task<UserStateData> HandlePostConfirmed(long userId, object postContent)
        {
            // Получаем все группы из базы данных
            var groups = await _groupRepository.GetAllGroupsAsync();
            var groupNames = groups.Select(g => g.Name).ToList();
            
            // Извлекаем названия кафедр из имен групп (первые 2 символа)
            var departments = groupNames
                .Select(g => g.Substring(0, 2)) // Берем первые 2 символа как код кафедры
                .Distinct() // Убираем дубликаты
                .ToList();
            
            // Переводим пользователя в состояние выбора групп для рассылки
            return new UserStateData(
                State.StateGroupChosingForPostSanding,
                postContent, // Сохраняем содержимое поста
                MessageDesign.GetBroadcastGroupSelectionMessage(groupNames, departments)
            );
        }

        /// <summary>
        /// Обработка запроса на просмотр списка запросов на админство
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <returns>Данные для отображения списка запросов</returns>
        public async Task<UserStateData> HandleAdminRequestList(long userId)
        {
            // Получаем всех пользователей с активными запросами на админство
            var requests = await _userRepository.GetUsersByAdminRequestAsync(true);
            
            // Преобразуем в формат, ожидаемый MessageDesign
            var requestUsers = requests.Select(u => new User 
            { 
                FirstName = u.UserId, // Предполагаем, что UserId содержит имя
                LastName = null, // Фамилия не используется (можно адаптировать)
                TelegramId = u.Id 
            }).ToList();
            
            // Возвращаем сообщение со списком запросов
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetAdminRequestListMessage(requestUsers)
            );
        }

        /// <summary>
        /// Обработка подтверждения админских прав для пользователя
        /// </summary>
        /// <param name="userId">ID администратора, который подтверждает</param>
        /// <param name="approvedUserId">ID пользователя, которого делают админом</param>
        /// <returns>Данные для отображения сообщения об успешном назначении</returns>
        public async Task<UserStateData> HandleAdminApproved(long userId, long approvedUserId)
        {
            // Получаем информацию о пользователе, которого назначают админом
            var approvedUser = await _userRepository.GetByIdAsync(approvedUserId);
            
            if (approvedUser != null)
            {
                // Назначаем админские права и снимаем запрос
                approvedUser.IsAdmin = true;
                approvedUser.AdminRoleRequest = false;
                await _userRepository.UpdateUserAsync(approvedUser);
            }
            
            // Создаем объект User для MessageDesign
            var user = new User 
            { 
                FirstName = approvedUser?.UserId, // Предполагаем, что UserId содержит имя
                LastName = null, // Фамилия не используется (можно адаптировать)
                TelegramId = approvedUserId 
            };
            
            // Возвращаем сообщение об успешном назначении
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetAdminRequestApprovedMessage(user)
            );
        }

        /// <summary>
        /// Обработка завершения процесса рассылки
        /// </summary>
        /// <param name="userId">ID пользователя в Telegram</param>
        /// <param name="postContent">Содержимое поста</param>
        /// <param name="selectedGroups">Список выбранных групп для рассылки</param>
        /// <returns>Данные для отображения сообщения об успешной рассылке</returns>
        public async Task<UserStateData> HandleBroadcastCompletion(long userId, object postContent, List<string> selectedGroups)
        {
            // В реальной реализации здесь должна быть логика рассылки сообщения выбранным группам
            // Но в нашем случае мы просто возвращаем сообщение об успехе
            
            return new UserStateData(
                State.StateStart,
                null,
                MessageDesign.GetBroadcastCompletionMessage()
            );
        }
    }
}
