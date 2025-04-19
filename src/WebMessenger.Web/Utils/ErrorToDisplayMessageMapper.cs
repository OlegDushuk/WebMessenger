namespace WebMessenger.Web.Utils;

public static class ErrorToDisplayMessageMapper
{
  public static string ToDisplayMessage(string error)
  {
    return error switch
    {
      "IS_REQUIRED" => "Це поле обов'язкове",
      "INVALID_FORMAT" => "Не вірний формат",
      "MUST_CONTAIN_UPPERCASE_LETTER" => "Має містити хоча б одну велику літеру",
      "MUST_CONTAIN_NUMBER" => "Має містити хоча б одну цифру",
      "MUST_CONTAIN_SPECIAL_CHARACTER" => "Має містити хоча б один спец. символ",
      "EMAIL_ALREADY_IN_USE" => "Ця ел. пошта вже використовується",
      "USERNAME_ALREADY_IN_USE" => "Це ім'я користувача вже використовується",
      "USER_BY_THIS_EMAIL_NOT_FOUND" => "Обліковий запис з такою ел. поштою не знайдено",
      "ACCOUNT_NOT_ACTIVE" => "Обліковий запис з такою ел. поштою не активований, будь ласка верифікуйте ел. пошту",
      "PASSWORD_INVALID" => "Невірний пароль",
      "USER_IS_ACTIVE" => "Обліковий запис вже активований",
      "TOKEN_NOT_FOUND" => "Посилання більше не дійсне",
      "USER_BY_THIS_TOKEN_NOT_FOUND" => "Посилання більше не дійсне",
      "TypeError: Failed to fetch" => "На разі сервер недоступний, спробуйте пізніше",
      _ => "Невідома помилка"
    };
  }
}