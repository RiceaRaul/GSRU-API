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
    }
}
