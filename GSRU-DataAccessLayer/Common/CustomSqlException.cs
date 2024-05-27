using System.ComponentModel;

namespace GSRU_DataAccessLayer.Common
{
    public enum CustomSqlException
    {
        DuplicateKey = 2627,
        CannotInsertNull = 515,
        ForeignKeyViolation = 547,
        UniqueKeyViolation = 2601,

        [Description("User does not exist")]
        UserNotExist = 500001,
        [Description("Password is incorrect")]
        PasswordIncorrect = 500002,
        [Description("Board does not exist")]
        BoardNotFound = 500003,
        [Description("Sprint does not exist")]
        SprintNotFound = 500004,
        [Description("Sprint already started")]
        SprintAlreadyStarted = 500005,
    }
}
