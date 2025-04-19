namespace WebMessenger.Application.Common;

public static class ErrorMessages
{
  #region ValidationErrors

  public static string IsRequired => "IS_REQUIRED";
  public static string InvalidFormat => "INVALID_FORMAT";
  public static string MinSize(int minSize) => $"MIN_SIZE_{minSize}";
  public static string MaxSize(int minSize) => $"MAX_SIZE_{minSize}";
  public static string MustContainUppercaseLetter => "MUST_CONTAIN_UPPERCASE_LETTER";
  public static string MustContainNumber => "MUST_CONTAIN_NUMBER";
  public static string MustContainSpecialCharacter => "MUST_CONTAIN_SPECIAL_CHARACTER";

  #endregion
  
  #region RegisterErrors
  
  public static string EmailAlreadyInUse => "EMAIL_ALREADY_IN_USE";
  public static string UserNameAlreadyInUse => "USERNAME_ALREADY_IN_USE";
  
  #endregion
  
  #region SharedErrors
  
  public static string EntityNotFound(string nameEntity) => $"{nameEntity.ToUpper()}_NOT_FOUND";
  public static string TimeExpired => "TIME_EXPIRED";
  
  #endregion
}