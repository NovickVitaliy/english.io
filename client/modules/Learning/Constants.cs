namespace Learning;

public static class Constants
{
    public static class Localization
    {
        public const string AddDeckWordFormBaseName = "Learning.Resources.Features.Decks.Components.AddDeckWordModal";
        public const string ChangePasswordComponentBaseName = "Learning.Resources.Features.Settings.Components.SecurityComponent.ChangePasswordComponent";
    }

    public static class ValidationErrors
    {
        public static class AddDeckWord
        {
            public const string Required = "Required";
        }

        public static class ChangePasswordComponent
        {
            public const string OldPasswordsMustMatch = "Old_Passwords_Must_Match";
            public const string OldPasswordIsRequired = "Old_Password_Is_Required";
            public const string NewPasswordIsRequired = "New_Password_Is_Required";
            public const string OldPasswordConfirmIsRequired = "Old_Password_Confirm_Is_Required";
        }
    }
}
