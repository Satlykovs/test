public class EmailAnswerPatterns
{
    public readonly Dictionary<string, string> emailAnswerPatterns = new Dictionary<string, string>()
    {
        {
            "Регистрация", "Ваша почта была использована при создании"+
            "аккаунта на платформе KOD&PUZZLE. Если это были не вы,"+
            " обратитесь сюда:________________"
        },

        {
            "Вход в аккаунт", "Был выполнен вход в ваш аккаунт на платформе"+
            " KOD&PUZZLE. Если это были не вы, обратитесь сюда:____________"
        },
        {
            "Изменены личные данные", "Ваши личные данные были изменены."+
            " Если это были не вы, обратитесь сюда:_____"
        }
        
    };
}