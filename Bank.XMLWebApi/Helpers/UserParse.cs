using Bank.Entity.Concrete;
using System.Xml;

namespace Bank.XMLWebApi.Helpers
{
    public class UserParse 
    {
        public static (string FullName, string Email, string Phone) ParseUserInfoFromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string fullName = doc.SelectSingleNode("/User/FullName")?.InnerText ?? "";
            string email = doc.SelectSingleNode("/User/Email")?.InnerText ?? "";
            string phone = doc.SelectSingleNode("/User/Phone")?.InnerText ?? "";

            return (fullName, email, phone);
        }

    }
}
