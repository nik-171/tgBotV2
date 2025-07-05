//Управление состояниями пользователей
public interface IUserStateService
{
    string GetState(long userId); //возвращает текущее состояние пользователя (например, WaitingForRole).
    void SetState(long userId, string state); // устанавливает новое состояние (например, после выбора роли).
    void SetTempData(long userId, string key, object value); // временно сохраняет данные (например, черновик поста).
    T GetTempData<T>(long userId, string key); // полчучает временные данные.
    void ClearState(long userId); // сбрасывает состояние при завершении диалога.
}

//Для чего: Отслеживает текущее состояние пользователя в диалоге с ботом (например, ожидает выбора группы или подтверждения поста).