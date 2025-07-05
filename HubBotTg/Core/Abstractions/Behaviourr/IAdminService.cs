//Для задач админа
public interface IAdminService
{
    Task HandleAdminRequestAsync(long userId); // ставит админ-реквест в бд
    Task SendGroupSelectionAsync(long chatId, List<Group> groups); //показывает клавиатуру с группами (на моменте когда пользователь выбирает группы для рассылки).
    Task SendAdminMainMenuAsync(long chatId);   // высылает майн меню админа с калбек кнопками
    Task SendAdminMSGPreview(long chatId);      // высылает превью поста с каблек кнопкой подтверждения
    Task SendAdminRequestList(long chatId);     // высылает нумерованный список реквестов из бд и калбек кнопка "назад"
}

//Для чего: Обрабатывает административные функции: заявки на админство, создание постов, выбор групп для рассылки.
