using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * Статический класс, определяющий методы сохранения/загрузки данных игрока в файл. 
 */

public static class ReadWriteAccountDateScript
{

    public static void WriteAccountData(string login)
    {
        if (login == string.Empty) return;
        if(Account.CurrentAccount.AccountName == String.Empty) return;
        using (var sw = new FileStream(string.Format(@"Account_{0}.xml", login), FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(sw, Account.CurrentAccount);
        }
    }


    public static void ReadAccountData(string login)
    {
        if (login == string.Empty) return;
        using (var sw = new FileStream(string.Format(@"Account_{0}.xml", login), FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            Account.CurrentAccount = formatter.Deserialize(sw) as Account;
        }
        Account.CurrentAccount.CheckAccountForErrors();
    }
}

