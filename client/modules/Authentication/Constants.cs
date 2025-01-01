namespace Authentication;

public static class Constants
{
    public static class Localization
    {
        public const string LoginFormBaseName = "Authentication.Resources.Features.Login.Components.LoginForm";
        public const string RegisterFormBaseName = "Authentication.Resources.Features.Register.Components.RegisterForm";
    }
    
    public static class ValidationErrorsName
    {
        public static class Login
        {
            public const string EmailIsRequired = "Email_Required";
            public const string EmailMustBeInValidFormat = "Email_Format";
            public const string PasswordIsRequired = "Password_Required";
        }
        
        public static class Register    
        {
            public const string EmailIsRequired = "Email_Required";
            public const string EmailMustBeInValidFormat = "Email_Format";
            public const string PasswordIsRequired = "Password_Required";
            public const string ConfirmPasswordIsRequired = "ConfirmPassword_Required";
            public const string PasswordAndConfirmPasswordMustBeEqual = "Passwords_Must_Be_Equal";
        }
    }
}