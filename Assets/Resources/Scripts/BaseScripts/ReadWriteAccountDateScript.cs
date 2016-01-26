using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

/*
 * Статический класс, определяющий методы сохранения/загрузки данных игрока в файл. 
 */
public static class ReadWriteAccountDateScript{

    public static void WriteAccountData(string login)
    {
        if (login == string.Empty) return;
        using(var sw = new StreamWriter(string.Format(@"Account_{0}.xml", login), false))
        {
            var formatter = new XmlSerializer(typeof (Account));
            formatter.Serialize(sw, Account.CurrentAccount);
        }
    }


    public static void ReadAccountData(string login)
    {
        if (login == string.Empty) return;
        using (var sw = new StreamReader(string.Format(@"Account_{0}.xml", login), false))
        {
            var formatter = new XmlSerializer(typeof(Account));
            Account.CurrentAccount = formatter.Deserialize(sw) as Account;
        }
    }
}
